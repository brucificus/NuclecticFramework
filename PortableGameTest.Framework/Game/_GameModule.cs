using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Nuclectic.Game.Content;
using Nuclectic.Game.State;
using PortableGameTest.Framework.Game.States;

namespace PortableGameTest.Framework.Game
{
    public class _GameModule
        : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SharedContentManager>().As<ISharedContentService>().SingleInstance();

	        builder.RegisterType<ManualGameStateManager>().AsSelf().As<IManualGameStateService>().SingleInstance();
			builder.RegisterType<AutoGameStateManager>().AsImplementedInterfaces().AsSelf().SingleInstance();
        }
    }
}
