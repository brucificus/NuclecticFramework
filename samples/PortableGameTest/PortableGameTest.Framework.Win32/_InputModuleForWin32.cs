using Autofac;
using Nuclectic.Input;
using Nuclectic.Input.Devices;

namespace PortableGameTest.Framework
{
    public class _InputModuleForWin32
        : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DirectInputConverter>().As<DirectInputConverter>().InstancePerDependency();
            builder.RegisterType<DirectInputManager>().As<IDirectInputManager>().SingleInstance();
	        
			builder.RegisterType<WindowMessageInputManager>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
