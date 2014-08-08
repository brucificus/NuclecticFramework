﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuclex.Input.Devices;

namespace PortableGameTest.Framework.Input.Devices
{
	public class XnaMouseFactory
		: IMouseFactory
	{
		public IMouse GetMouse()
		{
			return new XnaMouse();
		}
	}
}