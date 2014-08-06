using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuclex.Input.Devices;
using PortableGameTest.Framework.Input.Devices;

namespace PortableGameTest.Framework.Input
{
	public interface IWindowMessageInputManager : IKeyboardSoloFactory, IMouseFactory
    {
        bool IsWindowMessageInputAvailable { get; }
    }
}
