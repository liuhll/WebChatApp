using System;

namespace Jeuci.WeChatApp.Common.Tools
{
    public class RandomHelper
    {
        private static Random  rand = new Random();

        private const string Alphabet =
        "0123456789";

        public static string GenerateString(int size)
        {
            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = Alphabet[rand.Next(Alphabet.Length)];
            }
            return new string(chars);
        }

        public static string GenerateNonce()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}