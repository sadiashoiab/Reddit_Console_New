using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 
using System.IO;
using System.Text.RegularExpressions; 
using System.Globalization; 
using System.Security.Cryptography; 
namespace Encrypt_decript
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                textBox2.Text = AesEncrypt(textBox1.Text, "Ibdd7eIcddowEDKs"); 
        }
        private static byte[] initVector = new byte[16];
        public  string AesEncrypt(string textToCrypt, string key)
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

        /// <summary>
        /// Aes Decryption
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public  string AesDecrypt(string cipherData, string key)
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

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = AesDecrypt(textBox1.Text, "Ibdd7eIcddowEDKs");
        }
    }
}
