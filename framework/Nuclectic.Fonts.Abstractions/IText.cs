using Microsoft.Xna.Framework.Graphics;

namespace Nuclectic.Fonts
{
    public interface IText
    {
        /// <summary>Vertices containing the text's outline or face coordinates</summary>
        VertexPositionNormalTexture[] Vertices { get; }

        /// <summary>
        ///   Indices describing which vertices to connect by lines or triangles
        /// </summary>
        short[] Indices { get; }

        /// <summary>Type of primitives to draw</summary>
        PrimitiveType PrimitiveType { get; }

        /// <summary>Total width of the string in world units</summary>
        float Width { get; }

        /// <summary>Total height of the string in world units</summary>
        float Height { get; }
    }
}