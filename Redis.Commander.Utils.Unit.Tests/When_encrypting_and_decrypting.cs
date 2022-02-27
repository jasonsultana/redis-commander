using NUnit.Framework;
using Redis.Commander.Utils;
using System;
using System.Linq;

namespace Redis.Commander.Utils.Unit.Tests;

public class When_encrypting_and_decrypting
{
    [Test]
    public void Decrypted_value_should_match_encrypted_value()
    {
        var plainText = "Hello, World!";
        var cypherText = Crypto.Encrypt(plainText);
        var decryptedText = Crypto.Decrypt(cypherText);

        Console.WriteLine($"Got encrypted text: {cypherText}");

        Assert.AreEqual(plainText, decryptedText);
    }

    [Test]
    public void Key_should_be_deterministic()
    {
        var key = Crypto.GetKey();
        foreach (var e in Enumerable.Range(1, 100))
        {
            var newKey = Crypto.GetKey();
            if (newKey != key)
            {
                throw new Exception($"Generated key mismatch at attempt ${e}");
            }
        }
    }
}
