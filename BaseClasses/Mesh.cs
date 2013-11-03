using System;
using System.Collections.Generic;
using System.Text;

namespace BaseClasses
{
    public struct Point3d
    {
        public float X;
        public float Y;
        public float Z;
        public Point3d(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public struct Triangle
    {
        public int P0Index;
        public int P1Index;
        public int P2Index;
        public Triangle(int p0index, int p1index, int p2index)
        {
            P0Index = p0index;
            P1Index = p1index;
            P2Index = p2index;
        }
    }

    public class Mesh
    {
        public static bool record = true;
        public List<Point3d> Vertices = null;
        public List<Triangle> Faces = null;
        private int vcount = 0;
        private int fcount = 0;

        public Mesh()
        {
            Vertices = new List<Point3d>();
            Faces = new List<Triangle>();
        }
        public int AddVertex(Point3d toAdd)
        {
            vcount++;
            if(record)
                Vertices.Add(toAdd);
            return vcount-1;
        }
        public int GetVertexCount()
        {
            return vcount;
        }
        public int AddFace(Triangle tri)
        {
            fcount++;
            if (record)
                Faces.Add(tri);
            return fcount-1;
        }
        public int GetFaceCount()
        {
            return fcount;
        }
        public void Clear()
        {
            Vertices.Clear();
            Faces.Clear();
        }
    }
}
