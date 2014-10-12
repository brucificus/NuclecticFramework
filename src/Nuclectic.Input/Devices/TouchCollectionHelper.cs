#region CPL License

/*
Nuclex Framework
Copyright (C) 2002-2011 Nuclex Development Labs

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

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Nuclectic.Input.Devices
{
	/// <summary>An XNA TouchCollection that can be modified</summary>
	internal static class TouchCollectionHelper
	{
		/// <summary>
		///     Represents a method that can completely replace the private data of a given <see cref="TouchCollection" />
		/// </summary>
		/// <param name="touchCollection">The <see cref="TouchCollection" /> to modify</param>
		/// <param name="privateCollectionNewValue">
		///     The new set of <see cref="TouchLocation" /> to assign into
		///     <paramref name="touchCollection" />
		/// </param>
		public delegate void SetPrivateCollectionDelegate(ref TouchCollection touchCollection, TouchLocation[] privateCollectionNewValue);

		/// <summary>Initializes the static fields of the class</summary>
		static TouchCollectionHelper() { SetPrivateCollection = createSetPrivateCollectionDelegate(); }

		/// <summary>Removes all touch locations from the collection</summary>
		/// <param name="touchCollection">Touch collection that will be cleared</param>
		public static void Clear(ref TouchCollection touchCollection) { SetPrivateCollection(ref touchCollection, new TouchLocation[] {}); }

		/// <summary>
		///     Creates a delegate that can completely replace the private data of a given <see cref="TouchCollection" />
		/// </summary>
		/// <returns>A delegate that can completely replace the private data of a given <see cref="TouchCollection" /></returns>
		private static SetPrivateCollectionDelegate createSetPrivateCollectionDelegate()
		{
			FieldInfo privateCollectionField = typeof (TouchCollection).GetTypeInfo().GetDeclaredField("_collection");
			Type byrefTouchCollection = typeof (TouchCollection).MakeByRefType();

			ParameterExpression instance = Expression.Parameter(byrefTouchCollection, "instance");
			ParameterExpression value = Expression.Parameter(typeof (TouchLocation[]), "value");

			Expression<SetPrivateCollectionDelegate> expression =
				Expression.Lambda<SetPrivateCollectionDelegate>(
															    Expression.Assign(
																				  Expression.Field(instance, privateCollectionField),
																				  value
																	),
																instance,
																value
					);

			return expression.Compile();
		}

		/// <summary>Adds a touch location to a TouchCollection</summary>
		public static void AddTouchLocation(
			ref TouchCollection touchCollection,
			int id,
			TouchLocationState state,
			float x,
			float y,
			TouchLocationState prevState,
			float prevX,
			float prevY
			)
		{
			var touchLocation = new TouchLocation(id, state, new Vector2(x, y), prevState, new Vector2(prevX, prevY));
			var newPrivateCollection = touchCollection.Concat(new TouchLocation[] {touchLocation}).ToArray();
			SetPrivateCollection(ref touchCollection, newPrivateCollection);
		}

		private static readonly SetPrivateCollectionDelegate SetPrivateCollection;
	}
} // namespace Nuclex.Input.Devices