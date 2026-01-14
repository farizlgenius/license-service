using System;
using LicenseService.Data;
using LicenseService.Helper;
using LicenseService.Model;
using Microsoft.EntityFrameworkCore;

namespace LicenseService.Service.Impl;

public sealed class AuthService(AppDbContext context) : IAuthService
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

            var dataToSign = serverDhPub.Concat(appDhPub).ToArray();
            var signature = EncryptHelper.SignData(signer,dataToSign);

            return new ExchangeResponse(
                  Convert.ToBase64String(serverDhPub),
                  Convert.ToBase64String(serverSignPub),
                  Convert.ToBase64String(signature)
            );
      }


      public async Task<bool> VerifyAsync(VerifyRequest request)
      {
            
      }
}
