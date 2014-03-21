using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Nuclex.Fonts
{
    public interface IVectorFontCharacter
    {
        /// <summary>By how much to advance the cursor after drawing this character</summary>
        Vector2 Advancement { get; }

        /// <summary>Vertices for this character</summary>
        /// <remarks>
        ///   This contains the vertices required to draw the outline of the character
        ///   as well as supporting vertices required to draw the character's face as
        ///   a series of triangles. If you're only interested in a character's outlines,
        ///   you can ignore any vertices with an index above the EndVertex of
        ///   the lastmost outline contained in the Outlines list.
        /// </remarks>
        IReadOnlyList<Vector2> Vertices { get; }

        /// <summary>
        ///   Specifies which vertices have to be connected to draw the outlines
        ///   of the character.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     A character can have more than one outline. For example, the equals sign ('=')
        ///     has two unconnected shapes that require two outlines to be drawn. In this
        ///     case, you'd find two outlines, the first one specifying the starting and ending
        ///     vertex for the first stroke and the second one specifying the starting and
        ///     ending vertex for the second stroke.
        ///   </para>
        ///   <para>
        ///     The vertex range specified by each outline should be handled as a single
        ///     line strip (draw a line from the first to the second vertex, then from the
        ///     second to the third, and so on). The final vertex needs to be connected
        ///     to the first vertex again to close the outline.
        ///   </para>
        /// </remarks>
        IReadOnlyList<Outline> Outlines { get; }

        /// <summary>
        ///   Specifies between which vertices triangles have to be drawn to draw a
        ///   polygon-filled character.
        /// </summary>
        IReadOnlyList<Face> Faces { get; }
    }
}