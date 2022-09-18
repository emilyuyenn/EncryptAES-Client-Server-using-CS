using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace Test1
{
    class TestServer
    {
        public static class GlobalKey
        {
            public const String STRING_PERMUTATION = "sinhnx.dev";
            public const Int32 BYTE_PERMUTATION_1 = 0x19;
            public const Int32 BYTE_PERMUTATION_2 = 0x59;
            public const Int32 BYTE_PERMUTATION_3 = 0x17;
            public const Int32 BYTE_PERMUTATION_4 = 0x41;
        }
        public static string Encrypt(string strData)
        {
            return Convert.ToBase64String(EncryptByte(Encoding.UTF8.GetBytes(strData)));
        }
        public static byte[] EncryptByte(byte[] strData)
        {
            try
            {
                PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(GlobalKey.STRING_PERMUTATION,
            new byte[] { GlobalKey.BYTE_PERMUTATION_1,
                         GlobalKey.BYTE_PERMUTATION_2,
                         GlobalKey.BYTE_PERMUTATION_3,
                         GlobalKey.BYTE_PERMUTATION_4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }

        public static string Decrypt(string strData)
        {
            return Encoding.UTF8.GetString(DecryptByte(Convert.FromBase64String(strData)));
        }


        public static byte[] DecryptByte(byte[] strData)
        {
            try
            {
                PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(GlobalKey.STRING_PERMUTATION,
            new byte[] { GlobalKey.BYTE_PERMUTATION_1,
                         GlobalKey.BYTE_PERMUTATION_2,
                         GlobalKey.BYTE_PERMUTATION_3,
                         GlobalKey.BYTE_PERMUTATION_4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }

        public static byte[] shiftArray(byte[] arr)
        {
            byte[] data = new byte[2048];
            for (int i = 0; i < arr.Length; i++) 
            {
                data[i+1] = arr[i];
            }

            return data;
        }
        static void Main(string[] args)
        {
            //1. Create
            TcpClient client = new TcpClient();
            //2. Connect
            client.Connect("127.0.0.1", 8888);
            //3. Get Stream
            NetworkStream stream = client.GetStream();
            Console.Write("Your name: ");
            string name = Console.ReadLine();
            byte[] data = Encoding.ASCII.GetBytes(name);



{
    //lets take a new CSP with a new 2048 bit rsa key pair
            // var csp = new RSACryptoServiceProvider(2048);

            // //how to get the private key
            // var privKey = csp.ExportParameters(true);

            // //and the public key ...
            // var pubKey = csp.ExportParameters(false);

            // //converting the public key into a string representation
            // string pubKeyString;
            // {
            //     //we need some buffer
            //     var sw = new System.IO.StringWriter();
            //     //we need a serializer
            //     var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //     //serialize the key into the stream
            //     xs.Serialize(sw, pubKey);
            //     //get the string from the stream
            //     pubKeyString = sw.ToString();
            // }

            // //converting it back
            // {
            //     //get a stream from the string
            //     var sr = new System.IO.StringReader(pubKeyString);
            //     //we need a deserializer
            //     var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //     //get the object back from the stream
            //     pubKey = (RSAParameters)xs.Deserialize(sr);
            // }

            // //conversion for the private key is no black magic either ... omitted
            // //we have a public key ... let's get a new csp and load that key
            // csp = new RSACryptoServiceProvider();
            // csp.ImportParameters(pubKey);

            // //input a message
            // Console.Write("input a message: ");
            // var plainTextData = Console.ReadLine();

            // //for encryption, always handle bytes...
            // var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(plainTextData);

            // //apply pkcs#1.5 padding and encrypt our data 
            // byte[] bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

            // //we might want a string representation of our cypher text... base64 will do
            // var cypherText = Convert.ToBase64String(bytesCypherText); // encrypt done
            // Console.WriteLine("encrypted message: " + cypherText); 

            //first, get our bytes back from the base64 string ...
            // bytesCypherText = Convert.FromBase64String(cypherText);

            // //we want to decrypt, therefore we need a csp and load our private key
            // csp = new RSACryptoServiceProvider();
            // csp.ImportParameters(privKey);

            // //decrypt and strip pkcs#1.5 padding
            // bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

            // //get our original plainText back...
            // plainTextData = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);
            // Console.WriteLine("decrypted message: " + plainTextData);


            

}

            
            
            //4. Send data
            byte[] encrypted = EncryptByte(data);
            byte[] shift_encrypted = shiftArray(encrypted);
            shift_encrypted[0] = (byte)encrypted.Length;
            //Console.WriteLine("coi xem sao: {0}", encrypted);
            // // Encrypt data
            //byte[] encrypted = EncryptByte(bytesCypherText);
            // byte[] shift_encrypted = shiftArray(bytesCypherText);
            // shift_encrypted[0] = (byte)bytesCypherText.Length;
            //string strEncrypted = (Encrypt(name));
            //Console.WriteLine("Encypted: " + string.Join(", ", shift_encrypted));
            //byte[] data = Encoding.ASCII.GetBytes(encrypted);
            stream.Write(shift_encrypted, 0, shift_encrypted.Length);
            //stream.Write(data,0,data.Length);
            //5. Receive Data
            //byte[] dataReceive = new byte[1024];
            //stream.Read(dataReceive, 0, 1024);
            //Console.WriteLine("Server return: \"" + DecryptTextFromMemory(dataReceive, key, IV) + "\"");
            //6. Close
            stream.Close();
            client.Close();
        }
    }
}
