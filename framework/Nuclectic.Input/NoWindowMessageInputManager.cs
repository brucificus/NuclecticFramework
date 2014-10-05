using System;
using Nuclectic.Input.Devices;

namespace Nuclectic.Input
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
