using System.Security.Cryptography;
using System.Text;

namespace InventoryManagementSystem
{
    public class EncryptDecrypt
    {
        public static String EncryptPassword(String password, String salt)
        {
            String encryptedPassword = "";
            String saltedPassword = password + salt;

            HashAlgorithm algorithm = new SHA512Managed();

            byte[] hash = algorithm.ComputeHash(Encoding.Unicode.GetBytes(saltedPassword));

            foreach (byte b in hash)
            {
                encryptedPassword += b.ToString("X2");
            }

            return encryptedPassword;
        }
    }
}
