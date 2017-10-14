using System;
using System.Security.Cryptography;
using System.Web;
using System.Text;
using System.Linq;
using System.Web.Configuration;

namespace BOSoundShare
{
    public static class Encrypt
    {
        public static byte[] ConvertToByteArray(string text, int number_base)
        {
            return Enumerable.Range(0, text.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(text.Substring(x, 2), number_base))
                             .ToArray();
        }

        public static string Pwd_Encode(string vPwd)
        {

            //encrypts & hashes the given password
            MachineKeySection machineKey;
            HMACSHA512 objHash;

            machineKey = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath).GetSection("system.web/machineKey") as MachineKeySection;

            objHash = new HMACSHA512();
            objHash.Key = ConvertToByteArray(machineKey.ValidationKey, 16);

            return Convert.ToBase64String(objHash.ComputeHash(Encoding.Unicode.GetBytes(vPwd)));
        }

        public static bool CompareWithPass(String plain, String encrypted)
        {
            return Pwd_Encode(plain).Equals(encrypted);
        }

    }
}