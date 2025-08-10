namespace WitherTorch.Common.Structures
{
    public readonly record struct IndexedValue<T>(int Index, T Value) { }

    public readonly record struct NativeIndexedValue<T>(nuint Index, T Value) { }
}
