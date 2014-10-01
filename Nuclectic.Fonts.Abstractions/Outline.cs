namespace Nuclectic.Fonts
{
    /// <summary>Stores the starting index and the vertex count of a character outline</summary>
    public struct Outline {

        /// <summary>Initializes a new character outline</summary>
        /// <param name="startVertexIndex">Index of the vertex with which the outline starts</param>
        /// <param name="vertexCount">Number of vertices in this outline</param>
        public Outline(int startVertexIndex, int vertexCount) {
            this.StartVertexIndex = startVertexIndex;
            this.VertexCount = vertexCount;
        }

        /// <summary>Index of the vertex with which the outline begins</summary>
        public int StartVertexIndex;
        /// <summary>Total number of vertices the outline consists of</summary>
        public int VertexCount;

    }
}