using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuclex.Input.Devices;

namespace PortableGameTest.Framework.Input
{
    public interface IWindowMessageInputManager
    {
        bool IsWindowMessageInputAvailable { get; }

        IKeyboard GetKeyboard();
        IMouse GetMouse();
    }
}
