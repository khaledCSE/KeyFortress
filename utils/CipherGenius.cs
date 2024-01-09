using System.Security.Cryptography;
using System.Text;

namespace KeyFortress.utils;

public class CipherGenius
{
  private byte[] key;

  public CipherGenius(string EncryptionKey)
  {
    // Ensure a valid encryption key is provided
    if (string.IsNullOrWhiteSpace(EncryptionKey))
    {
      throw new ArgumentException("Encryption key must not be empty or whitespace.", nameof(EncryptionKey));
    }

    // Derive a 256-bit key from the input string using SHA256
    key = SHA256.HashData(Encoding.UTF8.GetBytes(EncryptionKey));
  }

  public string Encrypt(string plainText)
  {
    using Aes aesAlg = Aes.Create();
    aesAlg.Key = key;
    aesAlg.IV = new byte[aesAlg.BlockSize / 8];

    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

    using MemoryStream msEncrypt = new MemoryStream();
    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
    {
      using StreamWriter swEncrypt = new StreamWriter(csEncrypt);
      swEncrypt.Write(plainText);
    }
    return Convert.ToBase64String(msEncrypt.ToArray());
  }

  public string Decrypt(string cipherText)
  {
    using Aes aesAlg = Aes.Create();
    aesAlg.Key = key;
    aesAlg.IV = new byte[aesAlg.BlockSize / 8];

    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

    using MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
    using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
    using StreamReader srDecrypt = new StreamReader(csDecrypt);
    return srDecrypt.ReadToEnd();
  }
}
