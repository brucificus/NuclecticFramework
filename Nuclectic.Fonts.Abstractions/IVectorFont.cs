using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Nuclectic.Fonts
{
    public interface IVectorFont
    {
        /// <summary>Constructs the outline of the specified string</summary>
        /// <param name="text">String to construct an outline of</param>
        /// <returns>The outline of the specified string</returns>
        IText Outline(string text);

        /// <summary>Constructs a mesh of the strings face plane</summary>
        /// <param name="text">Text to construct a flat polygon mesh of</param>
        /// <returns>The filled string mesh</returns>
        IText Fill(string text);

        /// <summary>Constructs an extruded polygon mesh of the string</summary>
        /// <param name="text">String from which to construct a polygon mesh</param>
        /// <returns>The extruded string mesh</returns>
        IText Extrude(string text);

        /// <summary>Height of a single line of text in this font</summary>
        float LineHeight { get; }

        /// <summary>List of the characters contained in this font</summary>
        IReadOnlyList<IVectorFontCharacter> Characters { get; }

        /// <summary>Maps unicode character to indices into the character list</summary>
        IReadOnlyDictionary<char, int> CharacterMap { get; }

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
        IReadOnlyDictionary<KerningPair, Vector2> KerningTable { get; }
    }
}