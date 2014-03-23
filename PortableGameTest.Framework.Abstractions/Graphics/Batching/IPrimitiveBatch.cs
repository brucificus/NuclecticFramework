using Microsoft.Xna.Framework.Graphics;
using Nuclex.Graphics.Batching;

namespace PortableGameTest.Framework.Graphics.Batching
{
    public interface IPrimitiveBatch<VertexType> where VertexType : struct, IVertexType
    {
        /// <summary>Begins the drawing process</summary>
        /// <param name="queueingStrategy">
        ///   By what criteria to queue primitives and when to draw them
        /// </param>
        void Begin(QueueingStrategy queueingStrategy);

        /// <summary>Ends the drawing process</summary>
        void End();

        /// <summary>Draws a series of triangles</summary>
        /// <param name="vertices">Triangle vertices</param>
        /// <param name="context">Desired graphics device settings for the primitives</param>
        void Draw(VertexType[] vertices, DrawContext context);

        /// <summary>Draws a series of primitives</summary>
        /// <param name="vertices">Primitive vertices</param>
        /// <param name="type">Type of primitives to draw</param>
        /// <param name="context">Desired graphics device settings for the primitives</param>
        void Draw(VertexType[] vertices, PrimitiveType type, DrawContext context);

        /// <summary>Draws a series of primitives</summary>
        /// <param name="vertices">Primitive vertices</param>
        /// <param name="startVertex">Index of vertex to begin drawing with</param>
        /// <param name="vertexCount">Number of vertices to draw</param>
        /// <param name="type">Type of primitives to draw</param>
        /// <param name="context">Desired graphics device settings for the primitives</param>
        void Draw(
            VertexType[] vertices, int startVertex, int vertexCount,
            PrimitiveType type, DrawContext context
            );

        /// <summary>Draws a series of indexed triangles</summary>
        /// <param name="vertices">Triangle vertices</param>
        /// <param name="indices">Indices of the vertices to draw</param>
        /// <param name="context">Desired graphics device settings for the primitives</param>
        void Draw(VertexType[] vertices, short[] indices, DrawContext context);

        /// <summary>Draws a series of indexed primitives</summary>
        /// <param name="vertices">Primitive vertices</param>
        /// <param name="indices">Indices of the vertices to draw</param>
        /// <param name="type">Type of primitives to draw</param>
        /// <param name="context">Desired graphics device settings for the primitives</param>
        void Draw(
            VertexType[] vertices, short[] indices, PrimitiveType type, DrawContext context
            );

        /// <summary>Draws a series of indexed primitives</summary>
        /// <param name="vertices">Primitive vertices</param>
        /// <param name="startVertex">
        ///   Index in the vertex array of the first vertex. This vertex will become
        ///   the new index 0 for the index buffer.
        /// </param>
        /// <param name="vertexCount">Number of vertices to draw</param>
        /// <param name="indices">Indices of the vertices to draw</param>
        /// <param name="startIndex">Index of the vertex index to begin drawing with</param>
        /// <param name="indexCount">Number of vertex indices to draw</param>
        /// <param name="type">Type of primitives to draw</param>
        /// <param name="context">Desired graphics device settings for the primitives</param>
        void Draw(
            VertexType[] vertices, int startVertex, int vertexCount,
            short[] indices, int startIndex, int indexCount,
            PrimitiveType type, DrawContext context
            );
    }
}