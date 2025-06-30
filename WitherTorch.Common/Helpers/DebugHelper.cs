using System.Diagnostics;

using WitherTorch.Common.Exceptions;

namespace WitherTorch.Common.Helpers
{
    public static class DebugHelper
    {
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void ThrowIf(bool condition)
        {
            if (condition)
                throw new DebugException($"{nameof(ThrowIf)} called!");
        }

        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void ThrowIf(bool condition, string message)
        {
            if (condition)
                throw new DebugException(message);
        }

        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Throw()
        {
            throw new DebugException($"{nameof(Throw)} called!");
        }

        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Throw(string message)
        {
            throw new DebugException(message);
        }

        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Break()
        {
            if (!Debugger.IsAttached)
                return;
            Debugger.Break();
        }
    }
}
