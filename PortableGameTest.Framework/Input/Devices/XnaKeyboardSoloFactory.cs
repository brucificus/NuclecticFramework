using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuclectic.Input.Abstractions.Devices;
using Nuclex.Input.Devices;

namespace PortableGameTest.Framework.Input.Devices
{
	public class XnaKeyboardSoloFactory : IKeyboardSoloFactory
	{
		public IKeyboard GetKeyboard()
		{
			return new XnaKeyboardSolo();
		}
	}
}
