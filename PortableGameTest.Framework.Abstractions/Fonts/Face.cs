namespace Nuclex.Fonts
{
    /// <summary>Stores three vertex indices forming a triangle</summary>
    public struct Face {

        /// <summary>Initializes a new character face triangle</summary>
        /// <param name="firstVertexIndex">Index of the triangle's first vertex</param>
        /// <param name="secondVertexIndex">Index of the triangle's second vertex</param>
        /// <param name="thirdVertexIndex">Index of the triangle's third vertex</param>
        public Face(int firstVertexIndex, int secondVertexIndex, int thirdVertexIndex) {
            this.FirstVertexIndex = firstVertexIndex;
            this.SecondVertexIndex = secondVertexIndex;
            this.ThirdVertexIndex = thirdVertexIndex;
        }

        /// <summary>Index of the first vertex of the triangle</summary>
        public int FirstVertexIndex;
        /// <summary>Index of the second vertex of the triangle</summary>
        public int SecondVertexIndex;
        /// <summary>Index of the third vertex of the triangle</summary>
        public int ThirdVertexIndex;
    }
}