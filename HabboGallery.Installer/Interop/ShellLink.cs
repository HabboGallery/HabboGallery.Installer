using System.Runtime.InteropServices;

using TerraFX.Interop.Windows;

using static TerraFX.Interop.Windows.Windows;

namespace HabboGallery.Installer.Interop;

internal static unsafe class ShellLink
{
    public static void CreateAndSave(string description, string targetPath, string shortcutPath)
    {
        using ComPtr<IShellLinkW> shellLink = default;

        ThrowIfFailed(CoCreateInstance(
            __uuidof<TerraFX.Interop.Windows.ShellLink>(),
            null,
            (uint)CLSCTX.CLSCTX_INPROC_SERVER,
            __uuidof<IShellLinkW>(),
            (void**)shellLink.GetAddressOf()));

        fixed (char* descriptionPtr = description)
        fixed (char* targetPathPtr = targetPath)
        {
            ThrowIfFailed(shellLink.Get()->SetDescription(descriptionPtr));
            ThrowIfFailed(shellLink.Get()->SetPath(targetPathPtr));

            fixed (char* shortcutPathPtr = shortcutPath)
            {
                ComPtr<IPersistFile> file = default;
                ThrowIfFailed(shellLink.As(&file));

                ThrowIfFailed(file.Get()->Save(shortcutPathPtr, false));

                file.Dispose();
            }
        }
    }

    private static void ThrowIfFailed(HRESULT hr)
    {
        if (FAILED(hr))
        {
            Marshal.ThrowExceptionForHR(hr);
        }
    }
}