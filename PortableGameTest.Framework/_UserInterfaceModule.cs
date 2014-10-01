using Autofac;
using Nuclectic.UserInterface;

namespace PortableGameTest.Framework
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
