using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Image_Encrypt_Decrypt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[] imageData = File.ReadAllBytes("img.png");

            ImageEncryption imageEncryption = new ImageEncryption();
            var keyIv = imageEncryption.GenerateKeyIV();

            string encryptedImage = imageEncryption.EncryptImage(imageData,keyIv.KeyBase64 , keyIv.IVBase64);
            Console.WriteLine($"Image Encrypted!.");

            File.WriteAllBytes("encrypted_image.png", Convert.FromBase64String(encryptedImage));

            byte[] decryptedImageData = imageEncryption.DecryptImage(encryptedImage, keyIv.KeyBase64, keyIv.IVBase64);

            File.WriteAllBytes("decrypted_image.png", decryptedImageData);
            Console.WriteLine($"Image Decrypted!.");

            Console.ReadLine();
        }
    }
}
