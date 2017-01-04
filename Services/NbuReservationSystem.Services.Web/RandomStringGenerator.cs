namespace NbuReservationSystem.Services.Web
{
    using System.Security.Cryptography;
    using System.Text;

    public class RandomStringGenerator : IRandomStringGenerator
    {
        private const int RandomStringLength = 15; // 15 because reasons...
        private static readonly char[] Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890~!@#$".ToCharArray();

        public string Generate()
        {
            var data = new byte[1];
            using (var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[RandomStringLength];
                crypto.GetNonZeroBytes(data);
            }

            var result = new StringBuilder(RandomStringLength);
            foreach (var b in data)
            {
                result.Append(Chars[b % Chars.Length]);
            }

            return result.ToString();
        }
    }
}
