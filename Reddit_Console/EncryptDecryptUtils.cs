 
using System.Security.Cryptography;
using System.Text;

namespace Reddit_Console
{
    public static class EncryptDecryptUtils
    {
        private static byte[] initVector = new byte[16];
        public static string AesEncrypt(string textToCrypt, string key)
        {
            try
            {
                var cryptkey = Encoding.ASCII.GetBytes(key);

                using (var rijndaelManaged = new RijndaelManaged { Key = cryptkey, IV = initVector, Mode = CipherMode.CBC })
                using (var memoryStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(cryptkey, initVector), CryptoStreamMode.Write))
                {
                    using (var ws = new StreamWriter(cryptoStream))
                    {
                        ws.Write(textToCrypt);
                    }
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public static string AesDecrypt(string cipherData, string key)
        {
            try
            {
                var cryptkey = Encoding.ASCII.GetBytes(key);

                using (var rijndaelManaged = new RijndaelManaged { Key = cryptkey, IV = initVector, Mode = CipherMode.CBC })
                using (var memoryStream = new MemoryStream(Convert.FromBase64String(cipherData)))
                using (var cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(cryptkey, initVector), CryptoStreamMode.Read))
                {
                    return new StreamReader(cryptoStream).ReadToEnd();
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
