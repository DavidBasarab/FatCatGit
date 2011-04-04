using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandLineUnitTester
{
    public static class Extensions
    {
        private static Random _random;
        public static Random Random
        {
            get { return _random ?? (_random = new Random()); }
            set { _random = value; }
        }

        public static string GetRandomCharcterString(int length)
        {
            var retValue = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                retValue.Append(GetRandomCharacter().ToString());
            }

            return retValue.ToString();
        }

        public static char GetRandomCharacter()
        {
            while (true)
            {
                var nextRan = Random.Next(47, 122);

                switch (nextRan)
                {
                    case 47:
                    case 58:
                    case 59:
                    case 60:
                    case 61:
                    case 62:
                    case 63:
                    case 64:
                    case 91:
                    case 92:
                    case 93:
                    case 94:
                    case 95:
                    case 96:
                        continue;
                    default:
                        return (char)(nextRan);
                }
            }
        }
    }
}
