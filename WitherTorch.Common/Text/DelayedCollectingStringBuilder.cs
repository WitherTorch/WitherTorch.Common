using System.Runtime.CompilerServices;
using System.Text;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;

namespace WitherTorch.Common.Text
{
    internal sealed class DelayedCollectingStringBuilder : DelayedCollectingObject
    {
        private StringBuilder? _builder;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StringBuilder GetObject() => NullSafetyHelper.ThrowIfNull(_builder);

        protected override void GenerateObject()
        {
            _builder = new StringBuilder();
        }

        protected override void DestroyObject()
        {
            _builder = null;
        }
    }
}
