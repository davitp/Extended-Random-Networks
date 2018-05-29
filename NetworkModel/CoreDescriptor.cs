namespace NetworkModel
{
    public struct CoreDescriptor
    {

        /// <summary>
        /// Creates new core descriptor
        /// </summary>
        public CoreDescriptor(int k, int vertexCount, int edgeCount)
        {
            this.K = k;
            this.VertexCount = vertexCount;
            this.EdgeCount = edgeCount;
        }

        /// <summary>
        /// The K
        /// </summary>
        public readonly int K;

        /// <summary>
        /// The vertex count
        /// </summary>
        public readonly int VertexCount;

        /// <summary>
        /// The edge count
        /// </summary>
        public readonly int EdgeCount;
    }
}