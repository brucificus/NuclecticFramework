using Autofac;
using Microsoft.Xna.Framework.Graphics;
using Nuclectic.Graphics.TriD.Batching;
using Nuclectic.Graphics.TriD.Debugging;

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

			builder.Register(ctx=> new DebugDrawer(ctx.Resolve<IGraphicsDeviceService>(), ctx.ResolveNamed<Effect>("SolidColorEffect"), ctx.ResolveNamed<SpriteFont>("LucidaSpriteFont")))
				.As<IDebugDrawingService>();
        }
    }
}
