using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Nuclex.Geometry;

namespace PortableGameTest.Framework.Geometry
{
    public class _GeometryModule
        : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DefaultRandom>().As<IRandom>();
        }
    }
}
