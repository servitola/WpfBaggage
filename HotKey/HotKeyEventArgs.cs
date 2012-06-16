using System;

namespace WpfBaggage.HotKey
{
	public class HotKeyEventArgs : EventArgs
	{
		public HotKey Hotkey { get; private set; }

		public HotKeyEventArgs(HotKey hotkey)
		{
			Hotkey = hotkey;
		}
	}
}