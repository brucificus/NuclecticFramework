using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Game.Content;
using Nuclex.Graphics;

namespace PortableGameTest.Framework
{
    public class _FrameworkModule
        : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx => ctx.Resolve<IGraphicsDeviceService>().GraphicsDevice)
                .AsSelf()
                .ExternallyOwned();
	        builder.RegisterType<SpriteBatch>().AsSelf();
	        builder.RegisterType<ContentManager>()
		        .AsSelf()
				.WithParameter(new TypedParameter(typeof(string), "Content"))
		        .WithParameter((pi, ctx) => pi.ParameterType == typeof (IServiceProvider), (pi, ctx) => GraphicsDeviceServiceHelper.MakePrivateServiceProvider(ctx.Resolve<IGraphicsDeviceService>()))
		        .InstancePerLifetimeScope();
        }
    }
}
