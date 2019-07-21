using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabboGalleryInstaller.Helpers
{
    public static class ShortcutBuilder
    {
        public static void CreateShortcut(string targetPath, string iconPath, string appName, string description, Environment.SpecialFolder folder)
        {
            string shortcutLocation = Path.Combine(Environment.GetFolderPath(folder), appName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = description;
            shortcut.IconLocation = iconPath;
            shortcut.TargetPath = targetPath;
            shortcut.Save();
        }
    }
}
