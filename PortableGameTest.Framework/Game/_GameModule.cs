using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Nuclex.Game.Content;
using Nuclex.Game.Packing;
using Nuclex.Game.States;
using PortableGameTest.Framework.Game.States;

namespace PortableGameTest.Framework.Game
{
    public class _GameModule
        : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SharedContentManager>().As<ISharedContentService>().SingleInstance();

            builder.RegisterType<ArevaloRectanglePacker>().As<RectanglePacker>().InstancePerDependency();
            builder.RegisterType<CygonRectanglePacker>().As<RectanglePacker>().InstancePerDependency();
            builder.RegisterType<SimpleRectanglePacker>().As<RectanglePacker>().InstancePerDependency();

	        builder.RegisterType<ManualGameStateManager>().AsSelf().As<IManualGameStateService>().InstancePerLifetimeScope();
	        builder.RegisterType<AutoGameStateManager>().AsImplementedInterfaces().AsSelf().InstancePerLifetimeScope();
        }
    }
}
