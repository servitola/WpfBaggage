using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;

namespace WpfBaggage.HotKey
{

	/* USING
	 * HotKey hotkey = new HotKey((HwndSource)HwndSource.FromVisual(App.Current.MainWindow));
hotkey.Modifiers = HotKey.ModifierKeys.Alt | HotKey.ModifierKeys.Control;
	 //возможно hotkey.Key = System.Windows.Input.Key.V;
hotkey.Keys = System.Windows.Forms.Keys.V;
hotkey.HotKeyPressed += hotkey_HotKeyPressed;
hotkey.Enabled = true; */

	public class HotKey
	{

		#region Private Fields

		private const int WM_HOTKEY = 786;
		private bool _isEnabled;
		private bool _disposed;
		private int id;
		private HwndSourceHook hook;
		private HwndSource hwndSource;

		#endregion Private Fields

		#region DllImport

		[DllImport("user32", CharSet = CharSet.Ansi,SetLastError = true, ExactSpelling = true)]
		private static extern int RegisterHotKey(IntPtr hwnd, int id, int modifiers, int key);

		[DllImport("user32", CharSet = CharSet.Ansi,SetLastError = true, ExactSpelling = true)]
		private static extern int UnregisterHotKey(IntPtr hwnd, int id);

		#endregion DllImport

		public enum ModifierKeys
		{
			Alt = 1,
			Control = 2,
			Shift = 4,
			Win = 8
		}

		public HotKey(HwndSource hwndSource)
		{
			hook = WndProc;
			this.hwndSource = hwndSource;
			hwndSource.AddHook(hook);

			var rand = new Random((int)DateTime.Now.Ticks);
			id = rand.Next();
		}

		public Key Key { get; set; }

		public ModifierKeys Modifiers { get; set; }

		public event EventHandler<HotKeyEventArgs> HotKeyPressed;

		public bool IsEnabled
		{
			get { return _isEnabled; }
			set
			{
				if (value == _isEnabled) return;

				if ((int)hwndSource.Handle != 0)
				{
					if (value)
						RegisterHotKey(hwndSource.Handle, id, (int)Modifiers, KeyInterop.VirtualKeyFromKey(Key));
					else
						UnregisterHotKey(hwndSource.Handle, id);

					var error = Marshal.GetLastWin32Error();
					if (error != 0)
						throw new System.ComponentModel.Win32Exception(error);
				}

				_isEnabled = value;
			}
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == WM_HOTKEY)
			{
				if ((int)wParam == id)
					if (HotKeyPressed != null)
						HotKeyPressed(this, new HotKeyEventArgs(this));
			}

			return new IntPtr(0);
		}

		#region Disposing

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
				hwndSource.RemoveHook(hook);
			
			IsEnabled = false;

			_disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~HotKey()
		{
			Dispose(false);
		}

		#endregion Disposing
	}
}