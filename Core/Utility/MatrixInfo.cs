using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Utility
{
    /// <summary>
    /// Type of Research or Generation parameter, which represents input matrix file.
    /// Path - input matrix file full path.
    /// Size - size of network, which represents the matrix file.
    /// Matrix file can be {0, 1} matrix or list of pairs (n, m), which shows existance of link between n and m.
    /// If Size is 0, it is ignored and the size of matrix retrieved from file information.
    /// </summary>
    public struct MatrixPath
    {
        public string Path { get; set; }
        public int Size { get; set; }

        public override string ToString()
        {
            return Path;
        }
    };

    /// <summary>
    /// Representation of adjacency matrix and branches of the network read from file.
    /// <note>Branches property is null, if the network is not hierarchical.</note>
    /// </summary>
    public struct MatrixInfoToRead
    {
        public String fileName;
        public ArrayList Matrix;
        public ArrayList Branches;
    }

    /// <summary>
    /// Representation of adjacency matrix and branches of the network to be written to file.
    /// <note>Branches property is null, if the network is not hierarchical.</note>
    /// </summary>
    public struct MatrixInfoToWrite
    {
        public bool[,] Matrix;
        public UInt32[][] Branches;
    }

    /// <summary>
    /// 
    /// </summary>
    public struct NeighbourshipInfoToWrite
    {
        public List<KeyValuePair<int, int>> Neighbourship;
        public UInt32[][] Branches;
    }
}
