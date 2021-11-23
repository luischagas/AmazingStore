﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AmazingStore.Domain.Shared.Utils
{
    public static class Util
    {
        #region Public Methods

        public static string Encrypt(string value, string keyValue)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var key = Encoding.UTF8.GetBytes(keyValue);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(value);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        var str = Convert.ToBase64String(result);
                        var fullCipher = Convert.FromBase64String(str);
                        return str;
                    }
                }
            }
        }

        public static string Decrypt(string value, string keyValue)
        {
            if (string.IsNullOrEmpty(value)) 
                return value;

            value = value.Replace(" ", "+");
            var fullCipher = Convert.FromBase64String(value);

            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);

            var key = Encoding.UTF8.GetBytes(keyValue);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        #endregion Public Methods
    }
}