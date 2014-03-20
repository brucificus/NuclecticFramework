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
using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nuclex.Fonts.Effects
{

    /// <summary>An effect for rendering 8 bit alpha-only fonts</summary>
    internal static class SolidColorEffect
    {

//        #region Effect file source code
//#if DEBUG
//#if !XBOX360

//        const string EffectSource =
//          "// --------------------------------------------------------------------------------- //\n" +
//          "// SolidColor Effect\n" +
//          "// --------------------------------------------------------------------------------- //\n" +
//          "//\n" +
//          "// Fills any polygons drawn with it in a solid color\n" +
//          "//\n" +
//          "\n" +
//          "// Concatenated view and projection matrix\n" +
//          "float4x4 WorldViewProjection : VIEWPROJECTION;\n" +
//          "float4 TextColor : TEXTCOLOR;\n" +
//          "\n" +
//          "// --------------------------------------------------------------------------------- //\n" +
//          "// Vertex Shader\n" +
//          "// --------------------------------------------------------------------------------- //\n" +
//          "\n" +
//          "// Vertex shader output values. These are sent to the pixel shader.\n" +
//          "struct VS_OUTPUT {\n" +
//          "  float4 Position : POSITION0;\n" +
//          "  float4 Color : COLOR0;\n" +
//          "};\n" +
//          "\n" +
//          "// The vertex shader function\n" +
//          "VS_OUTPUT VertexShader(float4 Position : POSITION0) {\n" +
//          "  VS_OUTPUT Output;\n" +
//          "\n" +
//          "  Output.Position = mul(Position, WorldViewProjection);\n" +
//          "  Output.Color = TextColor;\n" +
//          "\n" +
//          "  return Output;\n" +
//          "}\n" +
//          "\n" +
//          "// --------------------------------------------------------------------------------- //\n" +
//          "// Pixel Shader\n" +
//          "// --------------------------------------------------------------------------------- //\n" +
//          "\n" +
//          "// The pixel shader function\n" +
//          "float4 PixelShader(float4 Color : COLOR0) : COLOR0 {\n" +
//          "  return Color;\n" +
//          "};\n" +
//          "\n" +
//          "// --------------------------------------------------------------------------------- //\n" +
//          "// Techniques\n" +
//          "// --------------------------------------------------------------------------------- //\n" +
//          "technique SolidColorFill {\n" +
//          "  pass P0 {\n" +
//          "    VertexShader = compile vs_1_1 VertexShader();\n" +
//          "    PixelShader  = compile ps_1_1 PixelShader();\n" +
//          "\n" +
//          "    CullMode = None;\n" +
//          "    ZWriteEnable = false;\n" +
//          "\n" +
//          "    AlphaBlendEnable = true;\n" +
//          "    SrcBlend = SrcAlpha;\n" +
//          "    DestBlend = InvSrcAlpha;\n" +
//          "  }\n" +
//          "}\n";

//        /// <summary>Compiles the effect for the provided graphics device</summary>
//        /// <returns>The compiled effect</returns>
//        internal static void Compile()
//        {

//            // Compile the effect code for x86 platforms
//            {
//                CompiledEffect compiledEffect = Effect.CompileEffectFromSource(
//                  EffectSource, null, null, CompilerOptions.None, TargetPlatform.Windows
//                );
//                if (!compiledEffect.Success)
//                    throw new Exception("Error compiling effect: " + compiledEffect.ErrorsAndWarnings);

//                using (FileStream effectFile = new FileStream("solidcolor.x86", FileMode.Create))
//                {
//                    byte[] code = compiledEffect.GetEffectCode();
//                    effectFile.Write(code, 0, code.Length);
//                }
//            }

//            // Compile the effect code for XBox 360 platforms
//            {
//                CompiledEffect compiledEffect = Effect.CompileEffectFromSource(
//                  EffectSource, null, null, CompilerOptions.None, TargetPlatform.Xbox360
//                );
//                if (!compiledEffect.Success)
//                    throw new Exception("Error compiling effect: " + compiledEffect.ErrorsAndWarnings);

//                using (FileStream effectFile = new FileStream("solidcolor.360", FileMode.Create))
//                {
//                    byte[] code = compiledEffect.GetEffectCode();
//                    effectFile.Write(code, 0, code.Length);
//                }
//            }

//        }

//#endif // !XBOX360
//#endif // DEBUG
//        #endregion

        #region EffectCode

#if XBOX360

    static readonly byte[] EffectCode = new byte[] {
      0xFE, 0xFF, 0x09, 0x01, 0x00, 0x00, 0x01, 0xC4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03,
      0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x60, 0x00, 0x00, 0x00, 0x78, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14, 0x57, 0x6F, 0x72, 0x6C,
      0x64, 0x56, 0x69, 0x65, 0x77, 0x50, 0x72, 0x6F, 0x6A, 0x65, 0x63, 0x74, 0x69, 0x6F, 0x6E, 0x00,
      0x00, 0x00, 0x00, 0x0F, 0x56, 0x49, 0x45, 0x57, 0x50, 0x52, 0x4F, 0x4A, 0x45, 0x43, 0x54, 0x49,
      0x4F, 0x4E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0xB8,
      0x00, 0x00, 0x00, 0xC8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x01,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x0A, 0x54, 0x65, 0x78, 0x74, 0x43, 0x6F, 0x6C, 0x6F, 0x72, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x0A, 0x54, 0x45, 0x58, 0x54, 0x43, 0x4F, 0x4C, 0x4F, 0x52, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0F,
      0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
      0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
      0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
      0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
      0x00, 0x00, 0x00, 0x03, 0x50, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x53, 0x6F, 0x6C, 0x69,
      0x64, 0x43, 0x6F, 0x6C, 0x6F, 0x72, 0x46, 0x69, 0x6C, 0x6C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02,
      0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x04,
      0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x8C,
      0x00, 0x00, 0x00, 0xA8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xB0,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01, 0xA8, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x5C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xDC,
      0x00, 0x00, 0x00, 0xD8, 0x00, 0x00, 0x00, 0x5D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF4,
      0x00, 0x00, 0x00, 0xF0, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x0C,
      0x00, 0x00, 0x01, 0x08, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x2C,
      0x00, 0x00, 0x01, 0x28, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x4C,
      0x00, 0x00, 0x01, 0x48, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x6C,
      0x00, 0x00, 0x01, 0x68, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x8C,
      0x00, 0x00, 0x01, 0x88, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0xA0, 0x10, 0x2A, 0x11, 0x00, 0x00, 0x00, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x24,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x58,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x30, 0x00, 0x00, 0x00, 0x1C,
      0x00, 0x00, 0x00, 0x23, 0xFF, 0xFF, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1C, 0x70, 0x73, 0x5F, 0x33, 0x5F, 0x30, 0x00, 0x32,
      0x2E, 0x30, 0x2E, 0x37, 0x36, 0x38, 0x30, 0x2E, 0x30, 0x00, 0xAB, 0xAB, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x24, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x10, 0x21, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0xF0, 0xA0,
      0x00, 0x00, 0x00, 0x00, 0x10, 0x01, 0xC4, 0x00, 0x22, 0x00, 0x00, 0x00, 0xC8, 0x0F, 0x80, 0x00,
      0x00, 0x00, 0x00, 0x00, 0xE2, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xBC, 0x10, 0x2A, 0x11, 0x01,
      0x00, 0x00, 0x01, 0x44, 0x00, 0x00, 0x00, 0x78, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0xE8, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0xDB, 0xFF, 0xFE, 0x03, 0x00,
      0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xD4,
      0x00, 0x00, 0x00, 0x44, 0x00, 0x02, 0x00, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x50,
      0x00, 0x00, 0x00, 0x60, 0x00, 0x00, 0x00, 0x70, 0x00, 0x02, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x84, 0x00, 0x00, 0x00, 0x94, 0x54, 0x65, 0x78, 0x74, 0x43, 0x6F, 0x6C, 0x6F,
      0x72, 0x00, 0xAB, 0xAB, 0x00, 0x01, 0x00, 0x03, 0x00, 0x01, 0x00, 0x04, 0x00, 0x01, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x57, 0x6F, 0x72, 0x6C, 0x64, 0x56, 0x69, 0x65, 0x77, 0x50, 0x72, 0x6F,
      0x6A, 0x65, 0x63, 0x74, 0x69, 0x6F, 0x6E, 0x00, 0x00, 0x03, 0x00, 0x03, 0x00, 0x04, 0x00, 0x04,
      0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x76, 0x73, 0x5F, 0x33, 0x5F, 0x30, 0x00, 0x32,
      0x2E, 0x30, 0x2E, 0x37, 0x36, 0x38, 0x30, 0x2E, 0x30, 0x00, 0xAB, 0xAB, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x78, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x10, 0x21, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
      0x00, 0x00, 0x02, 0x90, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0xF0, 0xA0, 0x00, 0x00, 0x10, 0x08,
      0x10, 0x01, 0x10, 0x03, 0x00, 0x00, 0x12, 0x00, 0xC2, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x04,
      0x00, 0x00, 0x12, 0x00, 0xC4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x08, 0x00, 0x00, 0x22, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x05, 0xF8, 0x00, 0x00, 0x00, 0x00, 0x06, 0x88, 0x00, 0x00, 0x00, 0x00,
      0xC8, 0x01, 0x80, 0x3E, 0x00, 0xA7, 0xA7, 0x00, 0xAF, 0x00, 0x00, 0x00, 0xC8, 0x02, 0x80, 0x3E,
      0x00, 0xA7, 0xA7, 0x00, 0xAF, 0x00, 0x01, 0x00, 0xC8, 0x04, 0x80, 0x3E, 0x00, 0xA7, 0xA7, 0x00,
      0xAF, 0x00, 0x02, 0x00, 0xC8, 0x08, 0x80, 0x3E, 0x00, 0xA7, 0xA7, 0x00, 0xAF, 0x00, 0x03, 0x00,
      0xC8, 0x0F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x22, 0x04, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    };
    
#endif // XBOX360

#if !XBOX360

        // Compiled effect code for x86 platforms
        static readonly byte[] EffectCode = new byte[] {
      0x01, 0x09, 0xFF, 0xFE, 0xC4, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00,
      0x02, 0x00, 0x00, 0x00, 0x60, 0x00, 0x00, 0x00, 0x78, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x04, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x57, 0x6F, 0x72, 0x6C,
      0x64, 0x56, 0x69, 0x65, 0x77, 0x50, 0x72, 0x6F, 0x6A, 0x65, 0x63, 0x74, 0x69, 0x6F, 0x6E, 0x00,
      0x0F, 0x00, 0x00, 0x00, 0x56, 0x49, 0x45, 0x57, 0x50, 0x52, 0x4F, 0x4A, 0x45, 0x43, 0x54, 0x49,
      0x4F, 0x4E, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0xB8, 0x00, 0x00, 0x00,
      0xC8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x0A, 0x00, 0x00, 0x00, 0x54, 0x65, 0x78, 0x74, 0x43, 0x6F, 0x6C, 0x6F, 0x72, 0x00, 0x00, 0x00,
      0x0A, 0x00, 0x00, 0x00, 0x54, 0x45, 0x58, 0x54, 0x43, 0x4F, 0x4C, 0x4F, 0x52, 0x00, 0x00, 0x00,
      0x01, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00,
      0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
      0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
      0x05, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
      0x06, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
      0x03, 0x00, 0x00, 0x00, 0x50, 0x30, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x53, 0x6F, 0x6C, 0x69,
      0x64, 0x43, 0x6F, 0x6C, 0x6F, 0x72, 0x46, 0x69, 0x6C, 0x6C, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
      0x01, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
      0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x8C, 0x00, 0x00, 0x00,
      0xA8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xB0, 0x01, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0xA8, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x07, 0x00, 0x00, 0x00, 0x92, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xDC, 0x00, 0x00, 0x00,
      0xD8, 0x00, 0x00, 0x00, 0x93, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF4, 0x00, 0x00, 0x00,
      0xF0, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x01, 0x00, 0x00,
      0x08, 0x01, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2C, 0x01, 0x00, 0x00,
      0x28, 0x01, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4C, 0x01, 0x00, 0x00,
      0x48, 0x01, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x01, 0x00, 0x00,
      0x68, 0x01, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x8C, 0x01, 0x00, 0x00,
      0x88, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x74, 0x00, 0x00, 0x00, 0x01, 0x01, 0xFF, 0xFF, 0xFE, 0xFF, 0x17, 0x00, 0x43, 0x54, 0x41, 0x42,
      0x1C, 0x00, 0x00, 0x00, 0x23, 0x00, 0x00, 0x00, 0x01, 0x01, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x70, 0x73, 0x5F, 0x31,
      0x5F, 0x31, 0x00, 0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66, 0x74, 0x20, 0x28, 0x52, 0x29,
      0x20, 0x44, 0x33, 0x44, 0x58, 0x39, 0x20, 0x53, 0x68, 0x61, 0x64, 0x65, 0x72, 0x20, 0x43, 0x6F,
      0x6D, 0x70, 0x69, 0x6C, 0x65, 0x72, 0x20, 0x39, 0x2E, 0x31, 0x35, 0x2E, 0x37, 0x37, 0x39, 0x2E,
      0x30, 0x30, 0x30, 0x30, 0x00, 0xAB, 0xAB, 0xAB, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x80,
      0x00, 0x00, 0xE4, 0x90, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x01, 0x00, 0x00,
      0x01, 0x01, 0xFE, 0xFF, 0xFE, 0xFF, 0x45, 0x00, 0x43, 0x54, 0x41, 0x42, 0x1C, 0x00, 0x00, 0x00,
      0xDB, 0x00, 0x00, 0x00, 0x01, 0x01, 0xFE, 0xFF, 0x02, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0xD4, 0x00, 0x00, 0x00, 0x44, 0x00, 0x00, 0x00, 0x02, 0x00, 0x04, 0x00,
      0x01, 0x00, 0x00, 0x00, 0x50, 0x00, 0x00, 0x00, 0x60, 0x00, 0x00, 0x00, 0x70, 0x00, 0x00, 0x00,
      0x02, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x84, 0x00, 0x00, 0x00, 0x94, 0x00, 0x00, 0x00,
      0x54, 0x65, 0x78, 0x74, 0x43, 0x6F, 0x6C, 0x6F, 0x72, 0x00, 0xAB, 0xAB, 0x01, 0x00, 0x03, 0x00,
      0x01, 0x00, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x57, 0x6F, 0x72, 0x6C,
      0x64, 0x56, 0x69, 0x65, 0x77, 0x50, 0x72, 0x6F, 0x6A, 0x65, 0x63, 0x74, 0x69, 0x6F, 0x6E, 0x00,
      0x03, 0x00, 0x03, 0x00, 0x04, 0x00, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x76, 0x73, 0x5F, 0x31, 0x5F, 0x31, 0x00, 0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66, 0x74,
      0x20, 0x28, 0x52, 0x29, 0x20, 0x44, 0x33, 0x44, 0x58, 0x39, 0x20, 0x53, 0x68, 0x61, 0x64, 0x65,
      0x72, 0x20, 0x43, 0x6F, 0x6D, 0x70, 0x69, 0x6C, 0x65, 0x72, 0x20, 0x39, 0x2E, 0x31, 0x35, 0x2E,
      0x37, 0x37, 0x39, 0x2E, 0x30, 0x30, 0x30, 0x30, 0x00, 0xAB, 0xAB, 0xAB, 0xFE, 0xFF, 0x01, 0x00,
      0x50, 0x52, 0x45, 0x53, 0x1F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x0F, 0x90,
      0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xC0, 0x00, 0x00, 0xE4, 0x90, 0x00, 0x00, 0xE4, 0xA0,
      0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0xC0, 0x00, 0x00, 0xE4, 0x90, 0x01, 0x00, 0xE4, 0xA0,
      0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0xC0, 0x00, 0x00, 0xE4, 0x90, 0x02, 0x00, 0xE4, 0xA0,
      0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0xC0, 0x00, 0x00, 0xE4, 0x90, 0x03, 0x00, 0xE4, 0xA0,
      0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0xD0, 0x04, 0x00, 0xE4, 0xA0, 0xFF, 0xFF, 0x00, 0x00
    };

#endif // !XBOX360

        #endregion // EffectCode

        /// <summary>Creates the effect for the specified graphics device</summary>
        /// <param name="graphicsDevice"></param>
        /// <returns></returns>
        public static Effect Create(GraphicsDevice graphicsDevice)
        {
            return new Effect(graphicsDevice, EffectCode);
        }

    }


} // namespace Nuclex.Fonts
