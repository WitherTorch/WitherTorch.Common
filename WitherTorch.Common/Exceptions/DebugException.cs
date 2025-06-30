using System;

namespace WitherTorch.Common.Exceptions
{
    public sealed class DebugException : Exception
    {
        public DebugException(string? message) : base(message) { }
    }
}
