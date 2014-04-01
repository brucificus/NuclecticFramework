using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuclex.Input.Devices;

namespace PortableGameTest.Framework.Input
{
    public class NoWindowMessageInputManager
        : IWindowMessageInputManager
    {
        public bool IsWindowMessageInputAvailable { get { return false; } }

        public IKeyboard GetKeyboard()
        {
            throw new NotSupportedException();
        }

        public IMouse GetMouse()
        {
            throw new NotSupportedException();
        }
    }
}
