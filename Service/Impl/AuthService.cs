using System;
using LicenseService.Data;
using LicenseService.Helper;
using LicenseService.Model;
using Microsoft.EntityFrameworkCore;

namespace LicenseService.Service.Impl;

public sealed class AuthService(AppDbContext context, RedisService redis) : IAuthService
{

      public async Task<ExchangeResponse> ExchangeAsync(ExchangeRequest request)
      {
            // Step 1 : Get Client Public Keys
            var appDhPub = Convert.FromBase64String(request.dhPub);
            var appSignPub = Convert.FromBase64String(request.signPub);

            // Step 2 : Get Signer from database
            var sign = await context.sign_key
            .AsNoTracking()
            .OrderByDescending(s => s.created_date)
            .FirstOrDefaultAsync(s => s.is_revoked == false);

            if (sign is null)
            {
                  return null;
            }

            var serverSignPri = sign.sign_priv;
            var serverSignPub = sign.sign_pub;
            var signer = EncryptHelper.LoadSignerPrivateKey(serverSignPri);

            // Step 3 : Create Server Key Pair
            var serverDh = EncryptHelper.CreateDh();
            var serverDhPub = EncryptHelper.ExportDhPublicKey(serverDh);


            // Step 4 : Store the ServerDh Private Key in Redis with short expiry
            var redisKey = Guid.NewGuid().ToString();
            var authSession = new AuthSession(
                  serverDhPub,
                  appDhPub,
                  appSignPub,
                  serverDh,
                  DateTime.UtcNow.AddMinutes(5)
            );
            var json = System.Text.Json.JsonSerializer.Serialize(authSession);
            await redis.SetAsync(redisKey, json, TimeSpan.FromMinutes(5));

            var dataToSign = serverDhPub.Concat(appDhPub).ToArray();
            var signature = EncryptHelper.SignData(signer, dataToSign);

            return new ExchangeResponse(
                  redisKey,
                  Convert.ToBase64String(serverDhPub),
                  Convert.ToBase64String(serverSignPub),
                  Convert.ToBase64String(signature)
            );
      }


      public async Task<bool> VerifyAsync(VerifyRequest request)
      {
            // Step 1 : Get Auth Session from Redis
            var authSessionJson = await redis.GetAsync(request.sessionId);
            if (authSessionJson is null) return false;
            var authSession = System.Text.Json.JsonSerializer.Deserialize<AuthSession>(authSessionJson);
            if (authSession is null) return false;

            // Step 2 : Verify Signature
            var dataToVerify = authSession.serverDhPub.Concat(authSession.appDhPub).ToArray();
            var appSignPub = EncryptHelper.LoadVerifierPublicKey(authSession.appSignPub);
            return EncryptHelper.VerifyData(dataToVerify, appSignPub, Convert.FromBase64String(request.signature));
      }
}
