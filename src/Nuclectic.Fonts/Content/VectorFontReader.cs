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
using System.Collections.Immutable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Nuclectic.Fonts.Content {

  /// <summary>XNA framework content reader for VectorFonts</summary>
  public class VectorFontReader : ContentTypeReader<VectorFont> {

    /// <summary>Load a vector font from a stored XNA asset</summary>
    /// <param name="input">Reader from which the asset can be read</param>
    /// <param name="existingInstance">Optional existing instance we are reloading</param>
    /// <returns>The loaded VectorFont instance</returns>
    protected override VectorFont Read(ContentReader input, VectorFont existingInstance) {
      float lineHeight = input.ReadSingle();

      // Which index corresponds to which character
      var characterMap = input.ReadObject<Dictionary<char, int>>();

      // Vectors of each character
      var characters = input.ReadObject<List<VectorFontCharacter>>();

      // Special distance adjustments between some characters
      var kerningTable =
        new Dictionary<KerningPair, Vector2>();

      int kerningEntryCount = input.ReadInt32();
      for(int index = 0; index < kerningEntryCount; ++index) {
        char left = input.ReadChar();
        char right = input.ReadChar();
        Vector2 kerning = input.ReadVector2();

        kerningTable.Add(new KerningPair(left, right), kerning);
      }

      #if false
      IGraphicsDeviceService graphicsDeviceService =
        (IGraphicsDeviceService)input.ContentManager.ServiceProvider.GetService(
          typeof(IGraphicsDeviceService)
        );
      #endif

      return new VectorFont(lineHeight, characters.ToImmutableList(), characterMap.ToImmutableDictionary(), kerningTable.ToImmutableDictionary());
    }

  }

} // namespace Nuclex.Fonts.Content
