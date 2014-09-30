using Autofac;
using Nuclectic.Input;
using Nuclectic.Input.Devices;

namespace PortableGameTest.Framework
{
    public class _InputModule
        : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InputManager>().As<IInputService>().SingleInstance();
            builder.RegisterType<NoDirectInputManager>().As<IDirectInputManager>().SingleInstance();
            builder.RegisterType<NoWindowMessageInputManager>().As<IWindowMessageInputManager>().SingleInstance();
	        builder.RegisterType<NoKeyboardSoloFactory>().AsImplementedInterfaces().SingleInstance();
	        builder.RegisterType<NoMouseFactory>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
