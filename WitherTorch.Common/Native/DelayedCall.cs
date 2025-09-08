using System;

namespace WitherTorch.Common.Native
{
    internal sealed class DelayedCall : DelayedCollectingObject
    {
        private readonly Action _action;

        public DelayedCall(Action action) => _action = action;

        protected override void GenerateObject() { }

        protected override void DestroyObject() => _action.Invoke();
    }
}
