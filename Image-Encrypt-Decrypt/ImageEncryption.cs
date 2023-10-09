using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Image_Encrypt_Decrypt
{

    
    public class ImageEncryption
    {
        public ImageEncryption()
        {
        }

        public KeyIvResult GenerateKeyIV(int size = 256)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = size;
                aesAlg.GenerateKey();
                aesAlg.GenerateIV();

                byte[] keyBytes = aesAlg.Key;
                byte[] ivBytes = aesAlg.IV;

                string keyBase64 = Convert.ToBase64String(keyBytes);
                string ivBase64 = Convert.ToBase64String(ivBytes);
                return new KeyIvResult {KeyBase64 = keyBase64, IVBase64 = ivBase64 };
            }
        }

        public string EncryptImage(byte[] imageData,string key,string iv)
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Convert.FromBase64String(key);
                aesAlg.IV = Convert.FromBase64String(iv);
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(imageData, 0, imageData.Length);
                        csEncrypt.FlushFinalBlock();

                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }   
            }
        }

        public byte[] DecryptImage(string encryptedData, string key, string iv)
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Convert.FromBase64String(key);
                aesAlg.IV = Convert.FromBase64String(iv);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedData)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream msDecryptedImage = new MemoryStream())
                        {
                            csDecrypt.CopyTo(msDecryptedImage);
                            return msDecryptedImage.ToArray();
                        }     
                    } 
                }
            } 
        }

        public class KeyIvResult
        {
            public string KeyBase64 { get; set; }
            public string IVBase64 { get; set; }
        }
    }
}
