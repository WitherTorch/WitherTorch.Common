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
            private readonly SysBool32 _autoReset;

            private SysBool32 _state;

            public readonly bool IsAutoReset => _autoReset;

            public readonly SysBool32 State => UnsafeHelper.As<int, SysBool32>(
                    InterlockedHelper.Read(in UnsafeHelper.As<SysBool32, int>(ref UnsafeHelper.AsRefIn(in _state))));

            public static IntPtr GetWaitingHandleFromEvent(RawWaitingEvent* source)
                => (IntPtr)(&source->_state);

            public static RawWaitingEvent* GetEventFromWaitingHandle(IntPtr waitingHandle)
                => (RawWaitingEvent*)(((SysBool32*)waitingHandle) - 1);

            public RawWaitingEvent(bool initialState, bool autoReset)
            {
                _autoReset = autoReset;
                _state = initialState;
            }

            public bool Set() => !UnsafeHelper.As<int, SysBool32>(
                InterlockedHelper.Exchange(ref UnsafeHelper.As<SysBool32, int>(ref _state), UnsafeHelper.As<SysBool32, int>(SysBool32.True)));

            public bool Reset() => UnsafeHelper.As<int, SysBool32>(
                InterlockedHelper.Exchange(ref UnsafeHelper.As<SysBool32, int>(ref _state), UnsafeHelper.As<SysBool32, int>(SysBool32.False)));
        }
    }
}
