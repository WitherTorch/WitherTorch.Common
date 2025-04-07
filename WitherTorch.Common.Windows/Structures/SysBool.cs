using System.Runtime.InteropServices;

namespace WitherTorch.Common.Windows.Structures
{
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public readonly struct SysBool
    {
        private readonly int _value;

        public readonly bool Value => _value != 0;

        public SysBool(bool value)
        {
            _value = value ? 1 : 0;
        }

        public static implicit operator bool(SysBool boolean)
        {
            return boolean.Value;
        }

        public static implicit operator SysBool(bool boolean)
        {
            return new SysBool(boolean);
        }

        public override readonly string ToString()
        {
            return _value == 0x1 ? bool.TrueString : bool.FalseString;
        }
    }
}
