#region CPL License

/*
Nuclex Framework
Copyright (C) 2002-2009 Nuclex Development Labs

This library is free software; you can redistribute it and/or
modify it under the terms of the IBM Common Public License as
published by the IBM Corporation; either version 1.0 of the
License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
IBM Common Public License for more details.

You should have received a copy of the IBM Common Public
License along with this library
*/

#endregion

using Nuclectic.Graphics.Helpers;
using Nuclectic.Support;
#if !WINDOWS_PHONE
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Nuclectic.Graphics.TriD.SpecialEffects.Masks
{
	/// <summary>Screen mask that fills the screen with a solid color</summary>
	public class ColorScreenMask : ScreenMask<PositionVertex>
	{
		/// <summary>Initializes as new solid color screen mask</summary>
		/// <param name="graphicsDevice">Graphics device the skybox cube lives on</param>
		/// <param name="ownedEffect">The (owned) effect that will be used to draw the screen mask</param>
		public ColorScreenMask(GraphicsDevice graphicsDevice, IOwned<Effect> ownedEffect)
			: base(graphicsDevice, ownedEffect, vertices)
		{
		}

		/// <summary>Color the mask fills the screen with</summary>
		public Color Color
		{
			get { return new Color(this.OwnedEffect.Value.Parameters[0].GetValueVector4()); }
			set { this.OwnedEffect.Value.Parameters[0].SetValue(value.ToVector4()); }
		}

		/// <summary>Vertices used to draw the screen mask</summary>
		private static readonly PositionVertex[] vertices =
		{
			new PositionVertex(new Vector2(-1.0f, +1.0f)),
			new PositionVertex(new Vector2(-1.0f, -1.0f)),
			new PositionVertex(new Vector2(+1.0f, +1.0f)),
			new PositionVertex(new Vector2(+1.0f, -1.0f))
		};

		/// <summary>Content manager the solid color fill effect was loaded from</summary>
		private ContentManager contentManager;
	}
} // namespace Nuclex.Graphics.SpecialEffects.Masks

#endif // !WINDOWS_PHONE