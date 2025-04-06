#if NET472_OR_GREATER
namespace WitherTorch.Common.Helpers
{
    unsafe partial class ParseHelper
    {
        private enum ConvertionErrors
        {
            None = 0,
            Format = 1,
            Overflow = 2,
        }

        private static ConvertionErrors ParseToInt32Core(char* input, int length, out int result)
        {
            result = 0;
            if (length <= 0)
                return ConvertionErrors.Format;
            bool isNegative;
            char* end = input + length;
            char c = *input;
            switch (c)
            {
                case '+':
                    isNegative = false;
                    input++;
                    break;
                case '-':
                    isNegative = true;
                    input++;
                    break;
                default:
                    isNegative = false;
                    break;
            }
            for (c = *input; input < end; input++)
            {
                if (result > int.MaxValue / 10)
                    return ConvertionErrors.Overflow;
                result *= 10;
                if (c < '0' || c > '9')
                    return ConvertionErrors.Format;
                result += c - '0';
            }
            if (isNegative)
            {
                if (result == int.MinValue)
                    return ConvertionErrors.None;
                if (result < 0)
                    return ConvertionErrors.Overflow;
                result = -result;
            }
            else
            {
                if (result < 0)
                    return ConvertionErrors.Overflow;
            }
            return ConvertionErrors.None;
        }
    }
}
#endif