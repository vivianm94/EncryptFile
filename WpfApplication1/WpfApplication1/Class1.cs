using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public sealed class Class1
    {
        Class1()
        {

        }
        private static readonly object padlock = new object();
        private static Class1 instance = null;
        public static Class1 Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new Class1();
                        }
                    }
                }
                return instance;
            }
        }

        public bool EncryptFile(string path)
        {
            string ext = Path.GetExtension(path).Replace(".","");
            string filename = Path.GetFileNameWithoutExtension(path);

            byte[] b= System.Text.ASCIIEncoding.ASCII.GetBytes(ext);
            string encrypted = Convert.ToBase64String(b);
            var newPath = path.Replace(filename+"."+ext, filename+"_"+ encrypted + ".vik");

            try
            {
                string password = @"vik12389"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = newPath;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

   
                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(path, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);
                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
                File.Delete(path);

                return true;
                
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool DecryptFile(string path)
        {
            string ext = Path.GetExtension(path).Replace(".", "");
            string filename = Path.GetFileNameWithoutExtension(path);

            string[] splitFilename = filename.Split('_');

            byte[] b = Convert.FromBase64String(splitFilename[1]);

            string decryptedfileext = System.Text.ASCIIEncoding.ASCII.GetString(b);

            var newPath = path.Replace(filename + ".vik", splitFilename[0] + '.'+ decryptedfileext);
            try
            {
                string password = @"vik12389"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

      
                FileStream fsCrypt = new FileStream(path, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(newPath, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

                return true;

            }
            catch (Exception e)
            {
                return false;
            }

        }



    }
}
