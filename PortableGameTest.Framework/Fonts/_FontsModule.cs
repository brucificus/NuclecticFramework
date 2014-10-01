using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Xna.Framework.Content;
using Nuclectic.Fonts;
using Nuclectic.Fonts.Content;

namespace PortableGameTest.Framework.Fonts
{
    public class _FontsModule
        : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<VectorFontCharacterReader>().As<ContentTypeReader<IVectorFontCharacter>>().SingleInstance();
            builder.RegisterType<VectorFontReader>().As<ContentTypeReader<IVectorFont>>().SingleInstance();

            builder.RegisterType<TextBatch>().As<ITextBatch>().InstancePerDependency();
        }
    }
}
