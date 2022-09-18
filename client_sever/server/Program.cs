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
            //byte[] data = Convert.FromBase64String(strData);
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

        public static byte[] shiftArray(byte[] arr, int len_data)
        {
            byte[] data = new byte[len_data];
            for (int i = 0; i < len_data; i++) 
            {
                data[i] = arr[i+1];
            }

            return data;
        }
        static void Main(string[] args)
        {
            //1. Listen
            IPAddress address = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(address, 8888);
            Console.WriteLine("Sever is listening...");
            listener.Start();
            Socket socket = listener.AcceptSocket();

            //2. Receive
            byte[] data = new byte[1024];
            
            socket.Receive(data);

            {
            //     //lets take a new CSP with a new 2048 bit rsa key pair
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

            //input a message
            // Console.Write("input a message: ");
            // var plainTextData = Console.ReadLine();

            // //for encryption, always handle bytes...
            // var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(plainTextData);

            // //apply pkcs#1.5 padding and encrypt our data 
            // var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

            // //we might want a string representation of our cypher text... base64 will do
            // var cypherText = Convert.ToBase64String(bytesCypherText); // encrypt done
            // Console.WriteLine("encrypted message: " + cypherText); 

            //first, get our bytes back from the base64 string ...
            //var bytesCypherText = Convert.FromBase64String(bytes);

            //we want to decrypt, therefore we need a csp and load our private key
            // csp = new RSACryptoServiceProvider();
            // csp.ImportParameters(privKey);

            // int len_data = bytes[0];
            // byte[] data_new = new byte[len_data];
            // data_new = shiftArray(bytes, len_data);

            // int len_data = bytesCypherText[0];
            // byte[] data_new = new byte[len_data];
            // data_new = shiftArray(bytesCypherText, len_data);

            //decrypt and strip pkcs#1.5 padding
            //var bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

            //get our original plainText back...
            //var plainTextData = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);
            //Console.WriteLine("decrypted message: " + plainTextData);

            }

            
            
            
            //Console.WriteLine("Encypted: " + string.Join(", ", data));
            int len_data = data[0];
            byte[] data_new = new byte[len_data];
            data_new = shiftArray(data, len_data);
            //Console.Write("len_data: ");
            //Console.WriteLine(len_data);
            //Console.WriteLine("Encypted-1: " + string.Join(", ", data));
            
            //Console.WriteLine("data receive: {0}", data_new);
            byte[] str = DecryptByte(data_new);
            string data_new_str = System.Text.Encoding.UTF8.GetString(str);
           Console.WriteLine("Client name: \"" + data_new_str + "\"");

            //3. Send
            //byte[] encrypted = EncryptTextToMemory("Hello, " + str, key, IV);
            //socket.Send(encrypted);
            //4. Close
            Console.WriteLine("Server is closing...");
            socket.Close();
            listener.Stop();
        }
    }
}