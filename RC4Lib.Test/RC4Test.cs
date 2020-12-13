using System.IO;
using System.Text;
using NUnit.Framework;
using System.Security.Cryptography;


namespace RC4Lib.Test
{
  public class RC4Tests
  {
    [Test]
    public void EncryptTest()
    {
      // arrange
      var key = "Key";
      var plainText = "Plaintext";
      var rc4 = new RC4(Encoding.ASCII.GetBytes(key));
      var input = Encoding.ASCII.GetBytes(plainText);
      // action
      var cipherText = rc4.TransformFinalBlock(input, 0, input.Length);
      // assert
      Assert.AreEqual(new byte[] { 0xBB, 0xF3, 0x16, 0xE8, 0xD9, 0x40, 0xAF, 0x0A, 0xD3 }, cipherText);
    }

    [Test]
    public void DecryptTest()
    {
      // arrange
      var key = "Wiki";
      var cipherText = new byte[] { 0x10, 0x21, 0xBF, 0x04, 0x20 };
      var rc4 = new RC4(Encoding.ASCII.GetBytes(key));
      // action
      var plainText = rc4.TransformFinalBlock(cipherText, 0, cipherText.Length);
      // assert
      Assert.AreEqual("pedia", plainText);
    }

    [Test]
    public void CryptoStreamTest()
    {
      var rc4 = new RC4(Encoding.ASCII.GetBytes("Key"));
      byte[] cipherText;
      using (MemoryStream msEncrypt = new MemoryStream())
      {
          using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, rc4, CryptoStreamMode.Write))
          {
              using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
              {

                  //Write all data to the stream.
                  swEncrypt.Write("Plaintext");
              }
              cipherText = msEncrypt.ToArray();
          }
      }

      Assert.AreEqual(new byte[] { 0xBB, 0xF3, 0x16, 0xE8, 0xD9, 0x40, 0xAF, 0x0A, 0xD3 }, cipherText);
    }
  }
}