using System;

namespace WitherTorch.Common.Windows.ObjectModels
{
    [Flags]
    public enum FileOpenDialogOptions : uint
    {
        OverwritePrompt = 0x2,
        StrictFileTypes = 0x4,
        NoChangeDirectory = 0x8,
        PickFolders = 0x20,
        ForceFileSystem = 0x40,
        AllNonStorageItems = 0x80,
        NoValidate = 0x100,
        AllowMultiSelect = 0x200,
        PathMustExists = 0x800,
        FileMustExists = 0x1000,
        CreatePrompt = 0x2000,
        ShareAware = 0x4000,
        NoReadonlyReturn = 0x8000,
        NoTestFileCreate = 0x10000,
        HideMruPlaces = 0x20000,
        HidePinnedPlaces = 0x40000,
        NoDereferenceLinks = 0x100000,
        OkButtonNeedsInteraction = 0x200000,
        DontAddToRecent = 0x2000000,
        ForceShowHidden = 0x10000000,
        DefaultNoMiniMode = 0x20000000,
        ForcePreviewPanel = 0x40000000,
        SupportStreamableItems = 0x80000000
    }

    public enum FolderDirectionOfAddPlace
    {
        Bottom= 0,
        Top = 1
    }
}
