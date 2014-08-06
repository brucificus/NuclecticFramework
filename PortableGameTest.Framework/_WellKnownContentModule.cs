using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PortableGameTest.Framework
{
	public class _WellKnownContentModule
		: Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			RegisterWellKnownContent<Effect>(builder, "ScreenMaskEffect");
			RegisterWellKnownContent<Effect>(builder, "DefaultTextEffect");
			RegisterWellKnownContent<Effect>(builder, "SolidColorEffect");
			RegisterWellKnownContent<SpriteFont>(builder, "LucidaSpriteFont");
			RegisterWellKnownContent<SpriteFont>(builder, "Skins/Suave/DefaultFont");
			RegisterWellKnownContent<Texture2D>(builder, "Skins/Suave/SuaveSheet");
			RegisterWellKnownContent<SpriteFont>(builder, "Skins/Suave/TitleFont");
		}

		private void RegisterWellKnownContent<TContent>(ContainerBuilder builder, string name)
		{
			builder.Register(ctx => ctx.Resolve<ContentManager>().Load<TContent>(name))
				.Named<TContent>(name)
				.InstancePerLifetimeScope();
		}
	}
}
