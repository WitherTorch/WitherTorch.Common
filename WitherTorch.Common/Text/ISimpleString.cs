using System.Collections.Generic;

namespace WitherTorch.Common.Text
{
    public interface ISimpleString : IEnumerable<char>
    {
        int Length { get; }
        char this[int index] { get; }

        bool Contains(char value);
        bool Contains(char value, int startIndex, int count);
        bool Contains(string value);
        bool Contains(string value, int startIndex, int count);
        bool Contains(StringBase value);
        bool Contains(StringBase value, int startIndex, int count);
        bool EndsWith(char value);
        bool EndsWith(string value);
        bool EndsWith(StringBase value);
        int IndexOf(char value);
        int IndexOf(char value, int startIndex, int count);
        int IndexOf(string value);
        int IndexOf(string value, int startIndex, int count);
        int IndexOf(StringBase value);
        int IndexOf(StringBase value, int startIndex, int count);
        bool PartiallyEquals(string other, int startIndex);
        bool PartiallyEquals(string other, int startIndex, int count);
        bool PartiallyEquals(StringBase other, int startIndex);
        bool PartiallyEquals(StringBase other, int startIndex, int count);
        StringSlice Slice(int startIndex);
        StringSlice Slice(int startIndex, int count);
        StringSlice Slice(nuint startIndex);
        StringSlice Slice(nuint startIndex, nuint count);
        bool StartsWith(char value);
        bool StartsWith(string value);
        bool StartsWith(StringBase value);
        char[] ToCharArray();
        string ToString();
    }
}