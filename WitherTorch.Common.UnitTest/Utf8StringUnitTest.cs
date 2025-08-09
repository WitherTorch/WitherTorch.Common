using System;
using System.Linq;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Text;

using Xunit;
using Xunit.Abstractions;

namespace WitherTorch.Common.UnitTest
{
    public class Utf8StringUnitTest
    {
        private readonly ITestOutputHelper _output;

        public Utf8StringUnitTest(ITestOutputHelper output)
        {
            _output = output;
        }

        private static unsafe string CreateRandomString(int minimumLength, int maximumLength)
        {
            Random random = new Random();
            int stringLength = random.Next(minimumLength, maximumLength + 1);
            string result = StringHelper.AllocateRawString(stringLength);
            fixed (char* ptr = result)
            {
                int i = 0;
                for (int limit = MathHelper.CeilDiv(stringLength, 2); i < limit; i++)
                {
                    ptr[i] = unchecked((char)random.Next('A', 'z'));
                }
                for (; i < stringLength; i++)
                {
                    char c;
                    while (!char.IsLetterOrDigit(c = unchecked((char)random.Next(0x008F, char.MaxValue)))) ;

                    ptr[i] = c;
                }
            }
            return result;
        }

        [Fact]
        public void GetCharAtTest()
        {
            string target = CreateRandomString(10, 200);
            int selectedIndex = new Random().Next(target.Length);
            StringBase testing = StringBase.Create(target, StringCreateOptions.UseUtf8Compression);
            _output.WriteLine($"Target: {target}\nType = {testing.GetType().FullName}");
            Assert.Equal(testing[selectedIndex], target[selectedIndex]);
        }

        [Fact]
        public void IndexOfAsciiPartTest()
        {
            string target = CreateRandomString(10, 200);
            StringBase testing = StringBase.Create(target, StringCreateOptions.UseUtf8Compression);
            _output.WriteLine($"Target: {target}\nType = {testing.GetType().FullName}");

            int length = target.Length;
            int sideLeftLength = MathHelper.CeilDiv(length, 2);
            char c = target[new Random().Next(sideLeftLength)];
            _output.WriteLine($"Target character: {c}");
            Assert.Equal(testing.IndexOf(c), target.IndexOf(c));
        }

        [Fact]
        public void IndexOfBmpPartTest()
        {
            string target = CreateRandomString(10, 200);
            StringBase testing = StringBase.Create(target, StringCreateOptions.UseUtf8Compression);
            _output.WriteLine($"Target: {target}\nType = {testing.GetType().FullName}");

            int length = target.Length;
            int sideLeftLength = MathHelper.CeilDiv(length, 2);
            char c = target[sideLeftLength+ new Random().Next(length - sideLeftLength)];
            _output.WriteLine($"Target character: {c}");
            Assert.Equal(testing.IndexOf(c), target.IndexOf(c));
        }

        [Fact]
        public void ContainsAsciiPartTest()
        {
            string target = CreateRandomString(10, 200);
            StringBase testing = StringBase.Create(target, StringCreateOptions.UseUtf8Compression);
            _output.WriteLine($"Target: {target}\nType = {testing.GetType().FullName}");

            int length = target.Length;
            int sideLeftLength = MathHelper.CeilDiv(length, 2);
            char c = target[new Random().Next(sideLeftLength)];
            _output.WriteLine($"Target character: {c}");
            Assert.Equal(testing.Contains(c), target.Contains(c));
        }

        [Fact]
        public void ContainsBmpPartTest()
        {
            string target = CreateRandomString(10, 200);
            StringBase testing = StringBase.Create(target, StringCreateOptions.UseUtf8Compression);
            _output.WriteLine($"Target: {target}\nType = {testing.GetType().FullName}");

            int length = target.Length;
            int sideLeftLength = MathHelper.CeilDiv(length, 2);
            char c = target[sideLeftLength+ new Random().Next(length - sideLeftLength)];
            _output.WriteLine($"Target character: {c}");
            Assert.Equal(testing.Contains(c), target.Contains(c));
        }

        [Fact]
        public void ToStringTest()
        {
            string target = CreateRandomString(10, 200);
            StringBase testing = StringBase.Create(target, StringCreateOptions.UseUtf8Compression);
            _output.WriteLine($"Target: {target}\nType = {testing.GetType().FullName}");
            Assert.Equal(testing.ToString(), target);
        }

        [Fact]
        public void SubstringTest1()
        {
            string target = CreateRandomString(10, 200);
            StringBase testing = StringBase.Create(target, StringCreateOptions.UseUtf8Compression);
            _output.WriteLine($"Target: {target}\nType = {testing.GetType().FullName}");
            int targetIndex = new Random().Next(target.Length);
            _output.WriteLine($"Target index: {targetIndex}");
            Assert.Equal(testing.Substring(targetIndex).ToString(), target.Substring(targetIndex));
        }

        [Fact]
        public void SubstringTest2()
        {
            string target = CreateRandomString(10, 200);
            StringBase testing = StringBase.Create(target, StringCreateOptions.UseUtf8Compression);
            _output.WriteLine($"Target: {target}\nType = {testing.GetType().FullName}");
            Random random = new Random();
            int targetIndex = random.Next(target.Length);
            int targetLength = random.Next(target.Length - targetIndex);
            _output.WriteLine($"Target index: {targetIndex}, Target length: {targetLength}");
            Assert.Equal(testing.Substring(targetIndex, targetLength).ToString(), target.Substring(targetIndex, targetLength));
        }

        [Fact]
        public void SplitTest1()
        {
            string target = CreateRandomString(10, 200);
            StringBase testing = StringBase.Create(target, StringCreateOptions.UseUtf8Compression);
            _output.WriteLine($"Target: {target}\nType = {testing.GetType().FullName}");
            char c = target[new Random().Next(target.Length)];
            _output.WriteLine($"Target separator: {c}");
            Assert.Equal(testing.Split(c).Select(val => val.ToString()).ToArray(), target.Split(c));
        }

        [Fact]
        public void SplitTest2()
        {
            string target = CreateRandomString(10, 200);
            StringBase testing = StringBase.Create(target, StringCreateOptions.UseUtf8Compression);
            _output.WriteLine($"Target: {target}\nType = {testing.GetType().FullName}");
            Random random = new Random();
            int targetIndex = random.Next(target.Length);
            int targetLength = random.Next(target.Length - targetIndex);
            string separator = target.Substring(targetIndex, targetLength);
            _output.WriteLine($"Target separator: {separator}");
            Assert.Equal(testing.Split(separator).Select(val => val.ToString()).ToArray(), target.Split([separator], StringSplitOptions.None));
        }
    }
}