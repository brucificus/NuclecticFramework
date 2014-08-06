using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using PortableGameTest.Framework.Input.Devices;

namespace PortableGameTest.Framework.Input
{
	public class _InputModuleForWinRT : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<XnaKeyboardSoloFactory>().AsImplementedInterfaces().SingleInstance();
		}
	}
}
