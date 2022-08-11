using System;
using System.Runtime.InteropServices;

using System.Windows.Forms;

namespace CyborgBuilder
{
    public class KeyHandler
    {
        private readonly int key;
        private readonly IntPtr hWnd;
        private readonly int id;
        public KeyHandler(Keys key, Form form)
        {
            this.key = (int)key;
            hWnd = form.Handle;
            id = GetHashCode();
        }
        public override int GetHashCode()
        {
            return key ^ hWnd.ToInt32();
        }
        public bool Register()
        {
            return RegisterHotKey(hWnd, id, 0, key);
        }
        public bool Unregister()
        {
            return UnregisterHotKey(hWnd, id);
        }
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
    public static class Constants
    {
        public const int WM_HOTKEY_MSG_ID = 0x0312;
        public const int F9_LPARAM = 7864320;
        public const int F10_LPARAM = 7929856;
        public const int F11_LPARAM = 7995392;
    }
}
