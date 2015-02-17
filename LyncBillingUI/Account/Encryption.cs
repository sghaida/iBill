using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LyncBillingUI.Account
{
    /// <summary>
    /// AES Rijndael Managed Encryption
    /// Source: http://www.codeproject.com/Tips/704372/How-to-use-Rijndael-ManagedEncryption-with-Csharp
    /// </summary>
    public class Encryption
    {
        #region Consts
        internal const string GUID = "7E16F828-D633-4A71-A66B-32BA8F44311E-64E08645-1B51-4896-9A0C-98DD2B8D726F";

        internal const string SALT = "24 54 6d 73 73 64 32 62 2b 63 24 32 25 53 78 43 67 5f 3f 45 21 36 25 6e 3d 6d 3f 72 39 21 25 58 23 4d 75 56 56 48 71 " + 
            "74 61 48 58 40 64 47 39 39 4d 3d 36 70 78 2d 3d 53 3f 51 4d 42 61 76 2d 66 41 74 7a 43 55 4e 39 45 62 78 50 4b 44 46 6b 76 77 47 73 6d 25 6e 33 79 56 38 57 64 44 55 74 46 " + 
            "4b 78 75 39 43 36 36 43 70 25 44 24 66 36 57 71 2a 3f 39 45 54 38 4d 46 59 62 75 79 45 39 39 78 70 54 47 47 4e 55 2b 72 38 6e 50 2d 2b 2b 4a 77 44 4b 74 61 59 40 24 38 70 " + 
            "52 26 55 6d 40 4a 43 35 58 45 66 79 65 52 39 4d 5f 37 68 3d 58 41 6e 51 61 47 36 4a 36 77 74 79 77 45 39 73 77 73 43 72 4b 63 78 37 53 41 39 39 68 34 61 25 59 6b 57 51 38 " + 
            "50 40 67 24 51 37 4b 53 2a 44 36 21 73 32 76 4c 62 48 6b 65 66 63 68 40 64 2a 68 37 4b 7a 25 42 37 64 35 76 64 72 38 5a 7a 66 2a 3f 72 61 50 6b 36 72 76 59 50 41 39 5e 37 " + 
            "57 78 76 32 32 73 37 47 66 21 4c 54 4b 4c 71 56 35 2a 68 36 25 2a 42 2b 73 75 70 21 68 71 68 57 25 4e 47 78 57 53 70 78 68 2d 5a 23 23 78 4c 62 55 51 3f 2a 75 5f 26 74 41 " + 
            "74 66 4a 72 6e 5f 39 48 33 21 76 44 52 21 4c 53 77 3f 46 61 74 77 25 64 4b 23 40 59 72 77 24 70 23 68 54 78 66 6a 44 33 62 3f 76 37 38 6a 6d 4d 77 45 67 32 43 79 47 77 3f " + 
            "68 41 2b 78 41 23 5a 73 24 4c 2a 70 53 40 39 34 4b 71 6e 41 6d 46 6e 2d 3d 6e 62 55 2d 6b 64 3f 41 2d 79 38 55 57 43 25 35 25 39 76 65 55 54 2d 39 50 64 75 46 58 4d 24 5f " + 
            "48 40 4a 72 5e 3d 74 4b 24 73 46 79 38 3f 26 26 74 57 4b 67 4d 39 71 73 46 4a 2d 39 64 5f 2d 73 2a 3d 59 4d 42 50 68 72 71 4e 43 43 64 5e 47 34 43 2b 70 35 76 4b 67 4b 38 " + 
            "4b 42 72 73 78 55 62 32 2b 25 2d 52 32 7a 54 77 33 21 47 52 45 26 25 62 62 34 4d 51 44 3d 66 2a 56 74 21 44 41 50 59 23 4e 57 6d 3d 4c 73 43 75 44 36 23 53 43 4d 3f 2a 56 " + 
            "32 52 43 57 6a 2b 25 5f 26 64 46 66 24 34 36 74 2a 52 58 50 25 40 33 35 67 3d 37 21 7a 2a 34 4b 55 73 40 40 56 61 34 2a 75 63 75 66 71 33 35 50 23 24 37 2a 3d 33 5f 4b 50 " + 
            "2d 35 4c 46 52 71 47 71 2b 23 3d 47 34 5e 75 23 79 21 79 6a 6d 75 38 32 35 68 4e 73 51 2b 65 24 59 71 59 2a 36 3f 24 33 33 59 56 78 32 70 79 3d 26 71 6e 5e 36 63 42 5a 55 " + 
            "44 3d 25 40 79 66 54 33 66 5a 32 39 4d 65 48 4d 46 4c 4a 63 38 77 4b 57 56 61 34 5f 43 66 2a 5f 40 47 46 76 53 21 2a 45 48 48 25 75 70 24 45 4b 4a 56 6d 56 40 5f 32 44 53 " + 
            "6d 2a 6d 71 40 40 70 5a 72 24 25 48 5f 34 70 24 41 53 43 44 6d 62 4c 76 3d 66 2b 2a 56 43 67 5e 3d 79 38 5a 2d 25 63 6d 74 67 63 72 32 5e 42 4a 61 7a 79 66 57 4c 24 38 26 " + 
            "59 57 25 76 6a 32 55 34 43 56 68 2d 72 66 48 38 5f 65 40 67 23 43 76 5a 65 5a 4c 55 45 46 4b 48 33 45 26 45 5e 6d 72 71 34 73 58 47 4a 33 74 26 59 6d 2b 33 68 21 38 4a 67 " + 
            "52 61 62 6b 4b 66 67 5a 63 41 53 47 54 2d 64 6b 66 4c 7a 4e 36 77 32 48 35 43 6e 72 65 6e 6b 74 6b 52 37 76 46 5f 21 35 66 55 46 34 5a 46 58 3d 48 57 5f 48 36 52 2b 76 5e " + 
            "2a 6d 46 65 46 51 54 38 41 65 4e 74 5f 4b 70 25 40 64 78 3f 53 58 45 3f 3f 2a 4a 6a 40 78 42 65 5a 77 73 70 4e 56 66 4c 5a 5f 78 42 71 35 2a 70 52 46 78 25 47 2d 2a 50 21 " + 
            "71 5e 52 45 2a 54 4c 72 58 79 71 47 24 75 63 53 3f 23 40 41 57 71 57 5e 51 63 64 5f 50 45 34 4e 41 46 45 23 58 37 70 58 23 3f 58 74 73 4c 56 4b 41 5e 36 56 5a 4c 36 4c 6a " + 
            "6d 46 6a 3f 24 38 71 23 65 2d 4e 61 6d 44 54 77";

        private static RijndaelManaged AESALG { get; set; }
        #endregion

        #region CONSTRUCTOR
        public Encryption()
        {
            if (AESALG == null)
            {
                AESALG = NewRijndaelManaged(SALT);
            }
        }
        #endregion


        #region NewRijndaelManaged
        /// <summary>
        /// Create a new RijndaelManaged class and initialize it
        /// </summary>
        /// <param name="salt" />The pasword salt
        /// <returns>
        private static RijndaelManaged NewRijndaelManaged(string salt)
        {
            if (salt == null) throw new ArgumentNullException("salt");
            var saltBytes = Encoding.ASCII.GetBytes(salt);
            var key = new Rfc2898DeriveBytes(GUID, saltBytes);

            var aesAlg = new RijndaelManaged();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

            return aesAlg;
        }
        #endregion


        #region Key and Salt Management
        private static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private static byte[] CreateKey(string password, int keyBytes = 32)
        {
            const int Iterations = 300;
            var keyGenerator = new Rfc2898DeriveBytes(password, StringToByteArray(SALT), Iterations);
            return keyGenerator.GetBytes(keyBytes);
        }
        #endregion


        #region Rijndael Encryption
        /// <summary>
        /// Encrypt the given text and give the byte array back as a BASE64 string
        /// </summary>
        /// <param name="text" />The text to encrypt
        /// <returns>The encrypted text</returns>
        public string EncryptRijndael(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("text");

            if(AESALG == null)
                AESALG = NewRijndaelManaged(SALT);

            var encryptor = AESALG.CreateEncryptor(AESALG.Key, AESALG.IV);
            var msEncrypt = new MemoryStream();

            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }
        #endregion


        #region Rijndael Decryption
        /// <summary>
        /// Checks if a string is base64 encoded
        /// </summary>
        /// <param name="base64String" />The base64 encoded string
        /// <returns>
        public bool IsBase64String(string base64String)
        {
            base64String = base64String.Trim();
            
            bool isBase64 = (base64String.Length % 4 == 0) 
                && Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

            return isBase64;
        }

        /// <summary>
        /// Decrypts the given text
        /// </summary>
        /// <param name="cipherText" />The encrypted BASE64 text
        /// <returns>De gedecrypte text</returns>
        public string DecryptRijndael(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");

            if (!IsBase64String(cipherText))
                throw new Exception("The cipherText input parameter is not base64 encoded");

            string text;

            if (AESALG == null)
                AESALG = NewRijndaelManaged(SALT);

            var decryptor = AESALG.CreateDecryptor(AESALG.Key, AESALG.IV);
            var cipher = Convert.FromBase64String(cipherText);

            using (var msDecrypt = new MemoryStream(cipher))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        text = srDecrypt.ReadToEnd();
                    }
                }
            }
            return text;
        }
        #endregion

    }

}