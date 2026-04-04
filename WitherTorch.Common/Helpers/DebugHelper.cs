using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

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
        public static void ThrowUnless(bool condition)
        {
            if (!condition)
                throw new DebugException($"{nameof(ThrowUnless)} called!");
        }

        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void ThrowUnless(bool condition, string message)
        {
            if (!condition)
                throw new DebugException(message);
        }

        [DebuggerHidden]
        [Conditional("DEBUG")]
        [DoesNotReturn]
        public static void Throw() => throw new DebugException($"{nameof(DebugHelper)}.{nameof(Throw)} called!");

        [DebuggerHidden]
        [Conditional("DEBUG")]
        [DoesNotReturn]
        public static void Throw(string message) => throw new DebugException(message);

        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Assert([DoesNotReturnIf(false)] bool condition)
        {
            if (condition)
                Debug.Fail(string.Empty, string.Empty);
        }

        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Assert([DoesNotReturnIf(false)] bool condition, string? message)
        {
            if (condition)
                Debug.Fail(message, string.Empty);
        }

        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Assert([DoesNotReturnIf(false)] bool condition, string? message, string detailMessage)
        {
            if (condition)
                Debug.Fail(message, detailMessage);
        }

        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Fail(string? message) => Debug.Fail(message);

        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Fail(string? message, string? detailMessage) => Debug.Fail(message, detailMessage);

        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void BreakIf(bool condition)
        {
            if (Debugger.IsAttached && condition)
                Debugger.Break();
        }

        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void BreakUnless(bool condition)
        {
            if (Debugger.IsAttached && !condition)
                Debugger.Break();
        }

        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Break()
        {
            if (Debugger.IsAttached)
                Debugger.Break();
        }
    }
}
