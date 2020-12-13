using System.IO;
using System.Text;
using NUnit.Framework;

namespace RC4Lib.Test
{
    public class RC4SteamTests
    {

        [Test]
        public void EncryptStreamTest()
        {
            // arrange
            var key = "Key";
            var srcFile = new FileStream("Data/test1.txt", FileMode.Open);
            var cipherText = new byte[srcFile.Length];

            // action
            var rc4 = new RC4Stream(Encoding.ASCII.GetBytes(key), srcFile);
            rc4.Read(cipherText, 0, cipherText.Length);

            // assert
            Assert.AreEqual(new byte[] { 0xBB, 0xF3, 0x16, 0xE8, 0xD9, 0x40, 0xAF, 0x0A, 0xD3 }, cipherText);

            srcFile.Close();
        }

        [Test]
        public void EncryptToStreamTest()
        {
            // arrange
            var key = "Key";
            var srcFile = new FileStream("Data/test1.txt", FileMode.Open);
            var destStream = new MemoryStream();

            // action
            var rc4 = new RC4Stream(Encoding.ASCII.GetBytes(key), srcFile);
            rc4.CopyTo(destStream, 1);

            // assert
            var cipherText = new byte[destStream.Length];
            destStream.Seek(0, SeekOrigin.Begin);
            destStream.Read(cipherText, 0, cipherText.Length);
            Assert.AreEqual(new byte[] { 0xBB, 0xF3, 0x16, 0xE8, 0xD9, 0x40, 0xAF, 0x0A, 0xD3 }, cipherText);

            srcFile.Close();
        }

        [Test]
        public void DecryptToStreamTest()
        {
            // arrange
            var key = "Key";
            var srcStream = new MemoryStream();
            var destStream = new MemoryStream();
            srcStream.Write(new byte[] { 0xBB, 0xF3, 0x16, 0xE8, 0xD9, 0x40, 0xAF, 0x0A, 0xD3 });
            srcStream.Seek(0, SeekOrigin.Begin);

            // action
            var rc4 = new RC4Stream(Encoding.ASCII.GetBytes(key), srcStream);
            rc4.CopyTo(destStream, 1);

            // assert
            destStream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(destStream);
            var plainText = reader.ReadToEnd();
            Assert.AreEqual("Plaintext", plainText);
        }
    }
}