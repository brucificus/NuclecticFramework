using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Nuclectic.Input.Abstractions;
using Nuclex.Input;
using PortableGameTest.Framework.Input.Devices;

namespace PortableGameTest.Framework.Input
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
