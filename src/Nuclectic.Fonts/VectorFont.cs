#region CPL License

/*
Nuclex Framework
Copyright (C) 2002-2008 Nuclex Development Labs

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

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Nuclectic.Fonts
{
	/// <summary>Vector-based font for creating three-dimensional text</summary>
	/// <remarks>
	///   <para>
	///     Whereas bitmap based fonts copy pre-rendered image of the characters onto
	///     the screen, vector based fonts store the vertices that make up a fonts 'hull'
	///     and render the text with actual polygons at runtime.
	///   </para>
	///   <para>
	///     For normal usage, after loading a VectorFont instance through the
	///     Content Manager, use one of the three mesh generation methods:
	///     <list type="bullet">
	///       <item>
	///         <term>Outline()</term>
	///         <description>
	///           This method outlines the given string and returns a text mesh that
	///           contains the side faces of the entire string in a single vertex list.
	///         </description>
	///       </item>
	///       <item>
	///         <term>Fill()</term>
	///         <description>
	///           Probably the most-used variant, this will build a flat mesh from the
	///           string you provide. The mesh only contains front-facing polygons and
	///           is ideal for normal text display, with the advantage that text won't
	///           become distorted or blurry when it is zoomed / rotated.
	///         </description>
	///       </item>
	///       <item>
	///         <term>Extrude()</term>
	///         <description>
	///           This method builds a complete sealed 3D mesh from the string you
	///           specify. The length of extrusion is always 1.0 units, centered
	///           about the middle of that range, giving you the ability to scale the
	///           extrusion level at will using the text's transformation matrix.
	///         </description>
	///       </item>
	///     </list>
	///   </para>
	///   <para>
	///     The vector font class also gives you full access to the underlying data of
	///     a font, enabling you to use it for other purposes such as collision detection
	///     or volume-based particle emitters that will make your credits or intro text
	///     look more dynamic. This data is contained in the character instances you can
	///     access through this class. To find the character index for a specific unicode
	///     letter, use the CharacterMap which enlists any letter that the font can provide.
	///   </para>
	/// </remarks>
	public class VectorFont : IVectorFont
	{
		/// <summary>Constructs a new vector font</summary>
		/// <param name="lineHeight">
		///   Height of a single line of text in this font
		/// </param>
		/// <param name="characters">List of Characters contained in the font</param>
		/// <param name="characterMap">
		///   Map used to associate unicode characters with character indices
		/// </param>
		/// <param name="kerningTable">
		///   Kerning data for adjusting the space between specific characters
		/// </param>
		internal VectorFont(
			float lineHeight,
			IReadOnlyList<IVectorFontCharacter> characters, IReadOnlyDictionary<char, int> characterMap,
			IReadOnlyDictionary<KerningPair, Vector2> kerningTable
			)
		{
			this.lineHeight = lineHeight;
			this.characters = characters;
			this.characterMap = characterMap;
			this.kerningTable = kerningTable;
		}

		/// <summary>Constructs the outline of the specified string</summary>
		/// <param name="text">String to construct an outline of</param>
		/// <returns>The outline of the specified string</returns>
		public IText Outline(string text) { return new OutlinedText(this, text); }

		/// <summary>Constructs a mesh of the strings face plane</summary>
		/// <param name="text">Text to construct a flat polygon mesh of</param>
		/// <returns>The filled string mesh</returns>
		public IText Fill(string text) { return new FilledText(this, text); }

		/// <summary>Constructs an extruded polygon mesh of the string</summary>
		/// <param name="text">String from which to construct a polygon mesh</param>
		/// <returns>The extruded string mesh</returns>
		public IText Extrude(string text) { return new ExtrudedText(this, text); }

		/// <summary>Height of a single line of text in this font</summary>
		public float LineHeight { get { return this.lineHeight; } }

		/// <summary>List of the characters contained in this font</summary>
		public IReadOnlyList<IVectorFontCharacter> Characters { get { return this.characters; } }

		/// <summary>Maps unicode character to indices into the character list</summary>
		public IReadOnlyDictionary<char, int> CharacterMap { get { return this.characterMap; } }

		/// <summary>
		///   Kerning table for adjusting the positions of specific character combinations
		/// </summary>
		/// <remarks>
		///   Certain character combination, such as the two consecutive characters 'AV'
		///   have diagonal shapes that would cause the characters to visually appear
		///   is if they were further apart from each other. Kerning adjusts the distances
		///   between such characters to keep the perceived character distance at the
		///   same level for all character combinations.
		/// </remarks>
		public IReadOnlyDictionary<KerningPair, Vector2> KerningTable { get { return this.kerningTable; } }

		/// <summary>Height of a single line of text in this font</summary>
		private float lineHeight;

		/// <summary>Characters contained in the font</summary>
		private IReadOnlyList<IVectorFontCharacter> characters;

		/// <summary>Look-up map for indices by unicode character</summary>
		private IReadOnlyDictionary<char, int> characterMap;

		/// <summary>
		///   Kerning table for adjusting the positions of specific character combinations
		/// </summary>
		private IReadOnlyDictionary<KerningPair, Vector2> kerningTable;
	}
} // namespace Nuclex.Fonts