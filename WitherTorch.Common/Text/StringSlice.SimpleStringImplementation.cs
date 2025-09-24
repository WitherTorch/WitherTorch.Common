namespace WitherTorch.Common.Text
{
    partial struct StringSlice : ISimpleString
    {
        int ISimpleString.Length => (int)_length;

        public bool Contains(char value) => _original.Contains(value, (int)_startIndex, (int)_length);

        public bool Contains(char value, int startIndex, int count)
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(string value)
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(string value, int startIndex, int count)
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(StringBase value)
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(StringBase value, int startIndex, int count)
        {
            throw new System.NotImplementedException();
        }

        public bool EndsWith(char value)
        {
            throw new System.NotImplementedException();
        }

        public bool EndsWith(string value)
        {
            throw new System.NotImplementedException();
        }

        public bool EndsWith(StringBase value)
        {
            throw new System.NotImplementedException();
        }

        public int IndexOf(char value)
        {
            throw new System.NotImplementedException();
        }

        public int IndexOf(char value, int startIndex, int count)
        {
            throw new System.NotImplementedException();
        }

        public int IndexOf(string value)
        {
            throw new System.NotImplementedException();
        }

        public int IndexOf(string value, int startIndex, int count)
        {
            throw new System.NotImplementedException();
        }

        public int IndexOf(StringBase value)
        {
            throw new System.NotImplementedException();
        }

        public int IndexOf(StringBase value, int startIndex, int count)
        {
            throw new System.NotImplementedException();
        }

        public bool PartiallyEquals(string other, int startIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool PartiallyEquals(string other, int startIndex, int count)
        {
            throw new System.NotImplementedException();
        }

        public bool PartiallyEquals(StringBase other, int startIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool PartiallyEquals(StringBase other, int startIndex, int count)
        {
            throw new System.NotImplementedException();
        }

        public StringBase Remove(int startIndex)
        {
            throw new System.NotImplementedException();
        }

        public StringBase Remove(int startIndex, int count)
        {
            throw new System.NotImplementedException();
        }

        public StringSlice Slice(nuint startIndex)
        {
            throw new System.NotImplementedException();
        }

        public StringSlice Slice(nuint startIndex, nuint count)
        {
            throw new System.NotImplementedException();
        }

        public bool StartsWith(char value)
        {
            throw new System.NotImplementedException();
        }

        public bool StartsWith(string value)
        {
            throw new System.NotImplementedException();
        }

        public bool StartsWith(StringBase value)
        {
            throw new System.NotImplementedException();
        }

        public char[] ToCharArray()
        {
            throw new System.NotImplementedException();
        }
    }
}
