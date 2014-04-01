using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        }
    }
}
