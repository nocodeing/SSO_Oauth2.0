﻿using System;
using System.Security.Cryptography;

namespace CommonTools
{
    public static class StringExtensions
    {
        public static string GetHash(this string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

    }
}
