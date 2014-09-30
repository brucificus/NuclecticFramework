using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuclectic.Input.Abstractions;
using Nuclectic.Input.Abstractions.Devices;
using Nuclex.Input;
using Nuclex.Input.Devices;

namespace PortableGameTest.Framework.Input
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
        public IKeyboard GetKeyboard()
        {
            return new WindowMessageKeyboard(_WindowMessageFilter);
        }

        public IMouse GetMouse()
        {
            return new WindowMessageMouse(_WindowMessageFilter);
        }

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
