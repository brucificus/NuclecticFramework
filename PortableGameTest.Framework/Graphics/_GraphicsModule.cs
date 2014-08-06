using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Nuclex.Graphics.Batching;
using Nuclex.Graphics.Debugging;
using PortableGameTest.Framework.Support;

namespace PortableGameTest.Framework.Graphics
{
    public class _GraphicsModule
        : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BasicEffectDrawContext>().As<DrawContext>().InstancePerDependency();
            builder.RegisterGeneric(typeof (DeferredQueuer<>)).AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterGeneric(typeof (DrawContextQueuer<>)).AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterGeneric(typeof(DynamicBufferBatchDrawer<>)).AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterType<EffectDrawContext>().As<DrawContext>().InstancePerDependency();
            builder.RegisterGeneric(typeof (ImmediateQueuer<>)).AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterGeneric(typeof (PrimitiveBatch<>)).AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterGeneric(typeof(UserPrimitiveBatchDrawer<>)).AsImplementedInterfaces().InstancePerDependency();

	        builder.RegisterType<DebugDrawer>().As<IDebugDrawingService>()
		        .WithParameterExplicitNamingSupport();
        }
    }
}
