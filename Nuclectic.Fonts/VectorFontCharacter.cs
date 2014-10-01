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

namespace Nuclectic.Fonts {

  /// <summary>Stores the data of a character in a vector font</summary>
  /// <remarks>
  ///   <para>
  ///     Each character in a vector font has an array of vertices that store the
  ///     outline points for the font and in some cases contains additional
  ///     supporting vertices required to draw filled text with triangles.
  ///   </para>
  ///   <para>
  ///     You can either access this data any make use of it for your own purposes,
  ///     or use one of the vector font's provided methods for constructing an
  ///     outline font, a flat font or an extruded font.
  ///   </para>
  /// </remarks>
  public class VectorFontCharacter : IVectorFontCharacter {
      /// <summary>Initializes new vector font character</summary>
    /// <param name="advancement">
    ///   By what to advance the pen after the character was drawn
    /// </param>
    /// <param name="vertices">Vertices used by this character</param>
    /// <param name="outlines">Vertex indices for drawing the character's outline</param>
    /// <param name="faces">Vertex indices for filling the character</param>
    internal VectorFontCharacter(
      Vector2 advancement, IReadOnlyList<Vector2> vertices, IReadOnlyList<Outline> outlines, IReadOnlyList<Face> faces
    ) {
      this.advancement = advancement;
      this.vertices = vertices;
      this.outlines = outlines;
      this.faces = faces;
    }

    /// <summary>By how much to advance the cursor after drawing this character</summary>
    public Vector2 Advancement {
      get { return this.advancement; }
    }

    /// <summary>Vertices for this character</summary>
    /// <remarks>
    ///   This contains the vertices required to draw the outline of the character
    ///   as well as supporting vertices required to draw the character's face as
    ///   a series of triangles. If you're only interested in a character's outlines,
    ///   you can ignore any vertices with an index above the EndVertex of
    ///   the lastmost outline contained in the Outlines list.
    /// </remarks>
    public IReadOnlyList<Vector2> Vertices
    {
      get { return this.vertices; }
    }

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
    public IReadOnlyList<Outline> Outlines
    {
      get { return this.outlines; }
    }

    /// <summary>
    ///   Specifies between which vertices triangles have to be drawn to draw a
    ///   polygon-filled character.
    /// </summary>
    public IReadOnlyList<Face> Faces
    {
      get { return this.faces; }
    }

    /// <summary>How far to advance the cursor after this character is rendered</summary>
    private Vector2 advancement;
    /// <summary>Vertices used by this character</summary>
    private IReadOnlyList<Vector2> vertices;
    /// <summary>Vertex index ranges to use for drawing the character's outlines</summary>
    private IReadOnlyList<Outline> outlines;
    /// <summary>Vertex indices to use for filling the character with triangles</summary>
    private IReadOnlyList<Face> faces;

  }
} // namespace Nuclex.Fonts
