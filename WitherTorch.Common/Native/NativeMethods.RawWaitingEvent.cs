using System;
using System.Runtime.InteropServices;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Structures;

namespace WitherTorch.Common.Native
{
    partial class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential, Size = 8)]
        private unsafe struct RawWaitingEvent
        {
            public const int HandleSize = sizeof(uint);

            private readonly SysBool32 _autoReset;

            private uint _state;

            public readonly bool IsAutoReset => _autoReset;

            public readonly bool State => MathHelper.ToBoolean(InterlockedHelper.Read(in _state));

            public static IntPtr GetWaitingHandleFromEvent(RawWaitingEvent* source)
                => (IntPtr)(&source->_state);

            public static RawWaitingEvent* GetEventFromWaitingHandle(IntPtr waitingHandle)
                => (RawWaitingEvent*)(((SysBool32*)waitingHandle) - 1);

            public RawWaitingEvent(bool initialState, bool autoReset)
            {
                _autoReset = autoReset;
                _state = MathHelper.BooleanToUInt32(initialState);
            }

            public bool Set() => InterlockedHelper.Exchange(ref _state, Booleans.TrueInt) == Booleans.FalseInt;

            public bool Reset() => InterlockedHelper.Exchange(ref _state, Booleans.FalseInt) != Booleans.FalseInt;
        }
    }
}
