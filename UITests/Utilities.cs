using System;
using System.Text;

namespace UITests
{
    public static class Utilities
    {
        public static string BaseUrl { get; set; }

        public static string GenerateNumbers(int count)
        {
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < count; i++)
            {
                sb.Append($"{rnd.Next(1, 10)}");
            }

            return sb.ToString();
        }
    }
}
