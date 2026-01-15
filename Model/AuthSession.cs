using System;
using System.Security.Cryptography;

namespace LicenseService.Model;

public record AuthSession(byte[] serverDhPub, byte[] appDhPub, byte[] appSignPub, ECDiffieHellman serverDh, DateTime expiresAt);
