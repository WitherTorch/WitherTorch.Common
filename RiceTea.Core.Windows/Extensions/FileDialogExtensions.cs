using System;
using System.Runtime.CompilerServices;

using RiceTea.Core.Helpers;
using RiceTea.Core.Native;
using RiceTea.Core.Threading;
using RiceTea.Core.Windows.ObjectModels;

namespace RiceTea.Core.Windows.Extensions;

public static class FileDialogExtensions
{
    private static readonly LazyTiny<ShellItem> _desktopItemLazy = new LazyTiny<ShellItem>(
        static () => ShellItem.Create(Environment.SpecialFolder.Desktop), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetDefaultFolderAsDesktop(this FileDialog _this)
        => _this.SetDefaultFolder(_desktopItemLazy.Value);

    public static unsafe void SetFileType(this FileDialog _this, string name, string pattern)
    {
        FileDialogFilterSpecification filterSpec = new FileDialogFilterSpecification()
        {
            Name = NativeMethods.AllocCStyleUtf16String(name),
            Specfication = NativeMethods.AllocCStyleUtf16String(pattern)
        };
        try
        {
            _this.SetFileTypes(1u, &filterSpec);
        }
        finally
        {
            NativeMethods.FreeMemory(filterSpec.Name);
            NativeMethods.FreeMemory(filterSpec.Specfication);
        }
    }

    public static unsafe void SetFileTypes(this FileDialog _this, params (string name, string pattern)[] fileTypes)
    {
        if (fileTypes is null)
            return;

        int length = fileTypes.Length;
        if (length <= 0)
            return;

        uint count = unchecked((uint)length);

        NativeMemoryPool pool = NativeMemoryPool.Shared;
        TypedNativeMemoryBlock<FileDialogFilterSpecification> buffer = pool.Rent<FileDialogFilterSpecification>(count);
        FileDialogFilterSpecification* ptr = buffer.NativePointer;
        UnsafeHelper.InitBlock(ptr, 0, count * UnsafeHelper.SizeOf<FileDialogFilterSpecification>());
        ref (string name, string pattern) fileTypeRef = ref UnsafeHelper.GetArrayDataReference(fileTypes);
        try
        {
            for (uint i = 0; i < count; i++)
            {
                (string name, string pattern) = UnsafeHelper.AddTypedOffset(ref fileTypeRef, i);
                ptr[i] = new FileDialogFilterSpecification()
                {
                    Name = NativeMethods.AllocCStyleUtf16String(name),
                    Specfication = NativeMethods.AllocCStyleUtf16String(pattern)
                };
            }
            _this.SetFileTypes(count, ptr);
        }
        finally
        {
            for (uint i = 0; i < count; i++)
            {
                FileDialogFilterSpecification specification = ptr[i];
                NativeMethods.FreeMemory(specification.Name);
                NativeMethods.FreeMemory(specification.Specfication);
            }
            pool.Return(buffer);
        }
    }
}
