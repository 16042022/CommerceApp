using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommerceUtils.Utils
{
    public static class GenerateSecrutiyKey
    {
        public static void GenerateKey()
        {
            // Create folder
            string keyStorePath = Directory.GetCurrentDirectory() + @"\\keyStore";
            if (!Directory.Exists(keyStorePath))
            {
                Directory.CreateDirectory(keyStorePath);
            }

            // Get RSA key
            var rsaSeed = RSA.Create();

            var privateKey = rsaSeed.ToXmlString(true);
            var publicKey = rsaSeed.ToXmlString(false);

            using var publicKeyFile = File.Create(Path.Combine(keyStorePath, "publicKey.xml"));
            using var privateKeyFile = File.Create(Path.Combine(keyStorePath, "privateKey.xml"));

            publicKeyFile.Write(Encoding.UTF8.GetBytes(publicKey));
            privateKeyFile.Write(Encoding.UTF8.GetBytes(privateKey));
        }
    }
}
