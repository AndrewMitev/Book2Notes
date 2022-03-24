using System.Text;

namespace Convertor.Helpers
{
    public static class StringHelper
    {
        public static string TrimLeadingZeroes(string line)
        {
            int trimIndex = 0;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] != '0')
                {
                    trimIndex = i;
                    break;
                }
            }

            return line.Substring(trimIndex);
        }

        public static string AddLeadingZeroes(long line)
        { 
            string lineString = line.ToString();

            if (lineString.Length < 5)
            {
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < 5 - lineString.Length; i++)
                {
                    builder.Append("0");
                }

                builder.Append(lineString);
                return builder.ToString();
            }

            return lineString;
        }
    }
}
