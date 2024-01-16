using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace HabboGallery.Installer.Interop;

internal static unsafe partial class ShellLink
{
    public static void CreateAndSave(string description, string targetPath, string shortcutPath)
    {
        Guid shellLinkclassId = new("00021401-0000-0000-C000-000000000046");
        Guid shellLinkinterfaceId = new(IShellLinkW.IID);
        CoCreateInstance(
            &shellLinkclassId,
            null,
            /* CLSCTX_INPROC_SERVER */ 1,
            &shellLinkinterfaceId,
            out nint shellLinkObj).ThrowIfFailed();

        var comWrappers = new StrategyBasedComWrappers();
        var shellLink = (IShellLinkW)comWrappers.GetOrCreateObjectForComInstance(shellLinkObj, CreateObjectFlags.None);

        fixed (char* descriptionPtr = description)
        fixed (char* targetPathPtr = targetPath)
        fixed (char* shortcutPathPtr = shortcutPath)
        {
            shellLink.SetDescription(descriptionPtr);
            shellLink.SetPath(targetPathPtr);

            Guid persistFileInterfaceId = new(IPersistFile.IID);
            Marshal.QueryInterface(shellLinkObj, ref persistFileInterfaceId, out nint persistFileObj).ThrowIfFailed();

            var file = (IPersistFile)comWrappers.GetOrCreateObjectForComInstance(persistFileObj, CreateObjectFlags.None);
            file.Save(shortcutPathPtr, false);

            Marshal.Release(shellLinkObj);
            Marshal.Release(persistFileObj);
        }
    }

    private static void ThrowIfFailed(this int hresult)
    {
        if (hresult < 0)
        {
            Marshal.ThrowExceptionForHR(hresult);
        }
    }

    [LibraryImport("ole32")]
    public static partial int CoCreateInstance(Guid* rclsid, void* pUnkOuter, uint dwClsContext, Guid* riid, out nint ppv);

    [GeneratedComInterface]
    [Guid(IID)]
    public partial interface IShellLinkW
    {
        public const string IID = "000214F9-0000-0000-C000-000000000046";

        void GetPath(char* pszFile, int cch, void* pfd, uint fFlags);
        void GetIDList(void** ppidl);
        void SetIDList(void** pidl);
        void GetDescription(char* pszName, int cch);
        void SetDescription(char* pszName);
        void GetWorkingDirectory(char* pszDir, int cch);
        void SetWorkingDirectory(char* pszDir);
        void GetArguments(char* pszArgs, int cch);
        void SetArguments(char* pszArgs);
        void GetHotkey(ushort* pwHotkey);
        void SetHotkey(ushort wHotkey);
        void GetShowCmd(int* piShowCmd);
        void SetShowCmd(int iShowCmd);
        void GetIconLocation(char* pszIconPath, int cch, int* piIcon);
        void SetIconLocation(char* pszIconPath, int iIcon);
        void SetRelativePath(char* pszPathRel, uint dwReserved);
        void Resolve(void* hwnd, uint fFlags);
        void SetPath(char* pszFile);
    }

    [GeneratedComInterface]
    [Guid(IID)]
    public partial interface IPersistFile : IPersist
    {
        public const string IID = "0000010B-0000-0000-C000-000000000046";

        void IsDirty();
        void Load(char* pszFileName, uint dwMode);
        void Save(char* pszFileName, [MarshalAs(UnmanagedType.U4)] bool fRemember);
        void SaveCompleted(char* pszFileName);
        void GetCurFile(char** ppszFileName);
    }

    [GeneratedComInterface]
    [Guid(IID)]
    public partial interface IPersist
    {
        public const string IID = "0000010C-0000-0000-C000-000000000046";

        void GetClassID(Guid* pClassID);
    }
}