using System;
using System.Collections.Generic;
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
			RegisterWellKnownContent<Effect>(builder, "ScreenMaskEffect", inSharedContentManager: true);
			RegisterWellKnownContent<Effect>(builder, "DefaultTextEffect", inSharedContentManager: true);
			RegisterWellKnownContent<Effect>(builder, "SolidColorEffect", inSharedContentManager: true);
			RegisterWellKnownContent<SpriteFont>(builder, "LucidaSpriteFont", inSharedContentManager: true);
			RegisterWellKnownContent<SpriteFont>(builder, "Skins/Suave/DefaultFont", inSharedContentManager: true);
			RegisterWellKnownContent<Texture2D>(builder, "Skins/Suave/SuaveSheet", inSharedContentManager: true);
			RegisterWellKnownContent<SpriteFont>(builder, "Skins/Suave/TitleFont", inSharedContentManager: true);
		}

		private void RegisterWellKnownContent<TContent>(ContainerBuilder builder, string name, bool inSharedContentManager)
		{
			if (inSharedContentManager)
			{
				builder.Register(ctx => ctx.ResolveNamed<ContentManager>("Shared").Load<TContent>(name))
					.Named<TContent>(name)
					.SingleInstance();	
			}
			else
			{
				builder.Register(ctx => ctx.Resolve<ContentManager>().Load<TContent>(name))
					.Named<TContent>(name)
					.InstancePerLifetimeScope();				
			}
		}
	}
}
