using System;
using System.Runtime.CompilerServices;
using System.Security;

using InlineMethod;

using LocalsInit;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;

namespace WitherTorch.Common.Windows.ObjectModels
{
    [SuppressUnmanagedCodeSecurity]
    public unsafe sealed class FileDialog : ModalWindow
    {
        //CLSID_FileOpenDialog = {dc1c5a9c-e88a-4dde-a5a1-60f82a20aef7}
        public static readonly Guid CLSID_FileOpenDialog = new Guid(0xdc1c5a9c, 0xe88a, 0x4dde, 0xa5, 0xa1, 0x60, 0xf8, 0x2a, 0x20, 0xae, 0xf7);
        //IID_FileOpenDialog = {42f85136-db7e-439c-85f1-e4075d135fc8}
        public static readonly Guid IID_FileOpenDialog = new Guid(0x42f85136, 0xdb7e, 0x439c, 0x85, 0xf1, 0xe4, 0x07, 0x5d, 0x13, 0x5f, 0xc8);

        private new enum MethodTable
        {
            _Start = ModalWindow.MethodTable._End,
            SetFileTypes = _Start,
            SetFileTypeIndex,
            GetFileTypeIndex,
            Advise,
            Unadvise,
            SetOptions,
            GetOptions,
            SetDefaultFolder,
            SetFolder,
            GetFolder,
            GetCurrentSelection,
            SetFileName,
            GetFileName,
            SetTitle,
            SetOkButtonLabel,
            SetFileNameLabel,
            GetResult,
            AddPlace,
            SetDefaultExtension,
            Close,
            SetClientGuid,
            ClearClientData,
            SetFilter,
            _End
        }

        public static FileDialog CreateFileOpenDialog()
        {
            FileDialog? result = CoCreateInstance<FileDialog>(CLSID_FileOpenDialog, IID_FileOpenDialog);
            return result is null ? throw new InvalidOperationException("result is null!") : result;
        }

        public FileDialog() : base() { }

        public FileDialog(void* nativePointer, ReferenceType pointerType) : base(nativePointer, pointerType) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetFileTypes(params ComDialogFilterSpecification[] rgFilterSpec)
        {
            fixed (ComDialogFilterSpecification* ptr = rgFilterSpec)
                SetFileTypes(unchecked((uint)rgFilterSpec.Length), ptr);
        }

        public void SetFileTypes(uint cFileTypes, ComDialogFilterSpecification* rgFilterSpec)
        {
            void* nativePointer = NativePointer;
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.SetFileTypes);
            int hr = ((delegate*<void*, uint, ComDialogFilterSpecification*, int>)functionPointer)(nativePointer, cFileTypes, rgFilterSpec);
            ThrowHelper.ThrowExceptionForHR(hr);
        }

        public void SetFileTypeIndex(uint iFileType)
        {
            void* nativePointer = NativePointer;
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.SetFileTypeIndex);
            int hr = ((delegate*<void*, uint, int>)functionPointer)(nativePointer, iFileType);
            ThrowHelper.ThrowExceptionForHR(hr);
        }

        [LocalsInit(false)]
        public uint GetFileTypeIndex()
        {
            uint iFileType;
            void* nativePointer = NativePointer;
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.GetFileTypeIndex);
            int hr = ((delegate*<void*, uint*, int>)functionPointer)(nativePointer, &iFileType);
            ThrowHelper.ThrowExceptionForHR(hr);
            return iFileType;
        }

        [LocalsInit(false)]
        public FileOpenDialogOptions GetOptions()
        {
            FileOpenDialogOptions result;
            void* nativePointer = NativePointer;
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.GetOptions);
            int hr = ((delegate*<void*, FileOpenDialogOptions*, int>)functionPointer)(nativePointer, &result);
            ThrowHelper.ThrowExceptionForHR(hr);
            return result;
        }

        public unsafe void SetOptions(FileOpenDialogOptions fos)
        {
            void* nativePointer = NativePointer;
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.SetOptions);
            int hr = ((delegate*<void*, FileOpenDialogOptions, int>)functionPointer)(nativePointer, fos);
            ThrowHelper.ThrowExceptionForHR(hr);
        }

        public void SetDefaultFolder(ShellItem item)
        {
            void* nativePointer = NativePointer;
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.SetDefaultFolder);
            int hr = ((delegate*<void*, void*, int>)functionPointer)(nativePointer, item == null ? null : item.NativePointer);
            ThrowHelper.ThrowExceptionForHR(hr);
        }

        public void SetFolder(ShellItem item)
        {
            void* nativePointer = NativePointer;
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.SetFolder);
            int hr = ((delegate*<void*, void*, int>)functionPointer)(nativePointer, item == null ? null : item.NativePointer);
            ThrowHelper.ThrowExceptionForHR(hr);
        }

        [LocalsInit(false)]
        public ShellItem GetFolder()
        {
            void* nativePointer = NativePointer;
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.GetFolder);
            int hr = ((delegate*<void*, void**, int>)functionPointer)(nativePointer, &nativePointer);
            ThrowHelper.ThrowExceptionForHR(hr, nativePointer);
            return new ShellItem(nativePointer, ReferenceType.Owned);
        }

        public void SetTitle(string title)
        {
            fixed (char* ptr = title)
                SetTitleCore(ptr);
        }

        [Inline(InlineBehavior.Remove)]
        private void SetTitleCore(char* title)
        {
            void* nativePointer = NativePointer;
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.SetTitle);
            int hr = ((delegate*<void*, char*, int>)functionPointer)(nativePointer, title);
            ThrowHelper.ThrowExceptionForHR(hr);
        }

        [LocalsInit(false)]
        public ShellItem GetResult()
        {
            void* nativePointer = NativePointer;
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.GetResult);
            int hr = ((delegate*<void*, void**, int>)functionPointer)(nativePointer, &nativePointer);
            ThrowHelper.ThrowExceptionForHR(hr, nativePointer);
            return new ShellItem(nativePointer, ReferenceType.Owned);
        }

        public void AddPlace(ShellItem psi, FolderDirectionOfAddPlace fdap)
        {
            void* nativePointer = NativePointer;
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.AddPlace);
            int hr = ((delegate*<void*, void*, FolderDirectionOfAddPlace, int>)functionPointer)(nativePointer, psi.NativePointer, fdap);
            ThrowHelper.ThrowExceptionForHR(hr, nativePointer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetClientGuid(in Guid guid)
            => SetClientGuid(UnsafeHelper.AsPointerIn(in guid));

        public void SetClientGuid(Guid* guid)
        {
            void* nativePointer = NativePointer;
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.SetClientGuid);
            int hr = ((delegate*<void*, Guid*, int>)functionPointer)(nativePointer, guid);
            ThrowHelper.ThrowExceptionForHR(hr, nativePointer);
        }

        public void ClearClientData()
        {
            void* nativePointer = NativePointer;
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.ClearClientData);
            int hr = ((delegate*<void*, int>)functionPointer)(nativePointer);
            ThrowHelper.ThrowExceptionForHR(hr, nativePointer);
        }
    }
}
