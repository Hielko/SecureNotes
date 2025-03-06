namespace Simple.Encryption
{
    using Microsoft.Extensions.Options;
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public class Encryption : IEncryption
    {
        private readonly EncryptionOptions _encryptionOptions;

        public Encryption(IOptions<EncryptionOptions> options)
        {
            _encryptionOptions = options.Value;
            if (String.IsNullOrEmpty(_encryptionOptions.Key))
            {
                throw new Exception("Encryption: Key is empty");
            }
        }

        //private const string VoorbeeldKey = "E546C8DF278CD5931069B522E695D4F2"; // 32 Tekens

        private string? GetKey() => _encryptionOptions.Key;

        private const int AES_KEY_SIZE = 256;
        private const int AES_BLOCK_SIZE = 128;

        /// <summary>
        /// Encrypt plaintext met key
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private string? Encrypt(string? plaintext, string? key)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            byte[] keyBytes = Convert.FromBase64String(key);
            byte[] iv = GenerateRandomIV();

            using (var aesAlg = Aes.Create())
            {
                aesAlg.KeySize = AES_KEY_SIZE;
                aesAlg.BlockSize = AES_BLOCK_SIZE;
                aesAlg.Key = keyBytes;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plaintext);
                    }

                    byte[] encryptedBytes = msEncrypt.ToArray();

                    // Combine IV and encrypted data for storage or transmission
                    byte[] combinedData = new byte[iv.Length + encryptedBytes.Length];
                    Buffer.BlockCopy(iv, 0, combinedData, 0, iv.Length);
                    Buffer.BlockCopy(encryptedBytes, 0, combinedData, iv.Length, encryptedBytes.Length);

                    return Convert.ToBase64String(combinedData);
                }
            }
        }

        /// <summary>
        /// Encrypt met gebruik making van de MASTERKEY
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public string? Encrypt(string? plaintext) => Encrypt(plaintext, GetKey());

        /// <summary>
        /// Decrypt ciphertext met key
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private string? Decrypt(string? ciphertext, string? key)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (ciphertext is null)
            {
                throw new ArgumentNullException(nameof(ciphertext));
            }

            byte[] keyBytes = Convert.FromBase64String(key);
            byte[] combinedData = Convert.FromBase64String(ciphertext);

            byte[] iv = new byte[AES_BLOCK_SIZE / 8];
            byte[] encryptedBytes = new byte[combinedData.Length - iv.Length];

            Buffer.BlockCopy(combinedData, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(combinedData, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

            using (var aesAlg = Aes.Create())
            {
                aesAlg.KeySize = AES_KEY_SIZE;
                aesAlg.BlockSize = AES_BLOCK_SIZE;
                aesAlg.Key = keyBytes;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(encryptedBytes))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Decrypt met gebruik making van de MASTERKEY
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public string? Decrypt(string? plaintext) => Decrypt(plaintext, GetKey());

        private byte[] GenerateRandomIV()
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.GenerateIV();
                return aesAlg.IV;
            }
        }
    }
}