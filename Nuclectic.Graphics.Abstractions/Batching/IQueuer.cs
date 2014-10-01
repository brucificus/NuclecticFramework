using Microsoft.Xna.Framework.Graphics;

namespace Nuclectic.Graphics.Abstractions.Batching
{
    public interface IQueuer<VertexType> where VertexType : struct, IVertexType
    {
        /// <summary>Informs the queuer that a new drawing cycle is about to start</summary>
        void Begin();

        /// <summary>Informs the queuer that the current drawing cycle has ended</summary>
        void End();

        /// <summary>Queues a series of indexed primitives</summary>
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
        void Queue(
            VertexType[] vertices, int startVertex, int vertexCount,
            short[] indices, int startIndex, int indexCount,
            PrimitiveType type, DrawContext context
            );

        /// <summary>Queues a series of primitives</summary>
        /// <param name="vertices">Primitive vertices</param>
        /// <param name="startVertex">Index of vertex to begin drawing with</param>
        /// <param name="vertexCount">Number of vertices to draw</param>
        /// <param name="type">Type of primitives to draw</param>
        /// <param name="context">Desired graphics device settings for the primitives</param>
        void Queue(
            VertexType[] vertices, int startVertex, int vertexCount,
            PrimitiveType type, DrawContext context
            );
    }
}