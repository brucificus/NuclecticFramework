using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Nuclex.UserInterface;

namespace PortableGameTest.Framework.UserInterface
{
    public class _UserInterfaceModule
        : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GuiManager>().As<IGuiService>().InstancePerDependency();
            builder.RegisterType<Screen>().As<IScreen>().InstancePerDependency();
        }
    }
}
