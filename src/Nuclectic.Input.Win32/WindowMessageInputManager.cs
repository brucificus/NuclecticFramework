using System;
using Nuclectic.Input.Devices;

namespace Nuclectic.Input
{
	public class WindowMessageInputManager
		: IWindowMessageInputManager, IDisposable
	{
		private IntPtr _WindowHandle;
		private WindowMessageFilter _WindowMessageFilter;

		public WindowMessageInputManager()
		{
			_WindowHandle = UnsafeNativeMethods.GetActiveWindow();
			_WindowMessageFilter = new WindowMessageFilter(_WindowHandle);
		}

		public bool IsWindowMessageInputAvailable { get { return true; } }
		public IKeyboard GetKeyboard() { return new WindowMessageKeyboard(_WindowMessageFilter); }

		public IMouse GetMouse() { return new WindowMessageMouse(_WindowMessageFilter); }

		public void Dispose()
		{
			if (_WindowMessageFilter != null)
			{
				_WindowMessageFilter.Dispose();
				_WindowMessageFilter = null;
			}
		}
	}
}