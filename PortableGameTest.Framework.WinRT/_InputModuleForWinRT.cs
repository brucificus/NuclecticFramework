using Autofac;
using Nuclectic.Input.Devices;

namespace PortableGameTest.Framework
{
	public class _InputModuleForWinRT : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<XnaKeyboardSoloFactory>().AsImplementedInterfaces().SingleInstance();
			builder.RegisterType<XnaMouseFactory>().AsImplementedInterfaces().SingleInstance();
		}
	}
}
