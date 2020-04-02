using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public sealed class EncryptDecrypt
    {
        EncryptDecrypt()
        {

        }      
        private static readonly Lazy<EncryptDecrypt> classInstance = new Lazy<EncryptDecrypt>(() => new EncryptDecrypt());
        public static EncryptDecrypt Instance => classInstance.Value;

        public bool EncryptFile(string path)
        {
            string extension = Path.GetExtension(path).Replace(".", string.Empty);
            string fileName = Path.GetFileNameWithoutExtension(path);

            byte[] encodeExtension = Encoding.ASCII.GetBytes(extension);
            string encrypted = Convert.ToBase64String(encodeExtension);
            string newPath = path.Replace(Path.ChangeExtension(fileName, extension), fileName + "_" + encrypted + ".vik");

            try
            {
                string password = @"vik12389"; // Your Key Here
                byte[] key = Encoding.Unicode.GetBytes(password);

                string cryptFile = newPath;
                using (FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create))
                using (RijndaelManaged RMCrypto = new RijndaelManaged())
                using (ICryptoTransform ct = RMCrypto.CreateEncryptor(key, key))
                using (CryptoStream cs = new CryptoStream(fsCrypt,
                    ct,
                    CryptoStreamMode.Write))
                using (FileStream fsIn = new FileStream(path, FileMode.Open))
                {
                    fsIn.CopyTo(cs);
                }

                File.Delete(path);

                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DecryptFile(string path)
        {
            string extension = Path.GetExtension(path).Replace(".", string.Empty);
            string fileName = Path.GetFileNameWithoutExtension(path);

            string[] splitFilename = fileName.Split('_');

            byte[] encodeExtension = Convert.FromBase64String(splitFilename[1]);

            string decryptedFileExt = Encoding.ASCII.GetString(encodeExtension);

            string newPath = path.Replace(Path.ChangeExtension(fileName, "vik"), splitFilename[0] + '.' + decryptedFileExt);
            try
            {
                string password = @"vik12389"; // Your Key Here
                byte[] key = Encoding.Unicode.GetBytes(password);
                using (FileStream fsCrypt = new FileStream(path, FileMode.Open))
                using (RijndaelManaged RMCrypto = new RijndaelManaged())
                using (ICryptoTransform ct = RMCrypto.CreateDecryptor(key, key))
                using (CryptoStream cs = new CryptoStream(fsCrypt,ct,CryptoStreamMode.Read))
                using (FileStream fsOut = new FileStream(newPath, FileMode.Create))
                {
                    cs.CopyTo(fsOut);
                }

                return true;

            }
            catch (Exception e)
            {
                return false;
            }

        }

    }
}
