using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Nuclex.Input;
using Nuclex.Input.Devices;

namespace PortableGameTest.Framework.Input
{
    public class _InputModuleForWin32
        : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DirectInputConverter>().As<DirectInputConverter>().InstancePerDependency();
            builder.RegisterType<DirectInputManager>().As<IDirectInputManager>().SingleInstance();
	        
			builder.RegisterType<WindowMessageInputManager>().As<IWindowMessageInputManager>().SingleInstance();
        }
    }
}
