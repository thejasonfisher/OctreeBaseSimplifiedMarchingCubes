using System;
using System.Collections.Generic;
using System.Text;

namespace BaseClasses
{
    class HashTable_2dArray<T>
    {
        struct DepthAndValue<T1>
        {
            public int K;
            public T1 Value;
            public DepthAndValue(int k, T1 value)
            {
                K = k;
                Value = value;
            }
        }
        List<DepthAndValue<T>>[,] mapHash;
        int stx;
        int sty;
        int stz;
        int w;
        int h;
        int d;
        public HashTable_2dArray(int stx,int sty,int stz,int width, int height, int depth)
        {
            this.stx = stx-2;
            this.sty = sty-2;
            this.stz = stz-2;
            this.w = width+2;
            this.h = height+2;
            this.d = depth+2;
            mapHash = new List<DepthAndValue<T>>[this.w, this.h];
        }
        public void SetHashValue(int x, int y, int z, T value)
        {
            if (mapHash[x-stx, y-sty] == null)
            {
                mapHash[x - stx, y - sty] = new List<DepthAndValue<T>>();
                mapHash[x - stx, y - sty].Add(new DepthAndValue<T>(z, value));
            }
            else
            {
                mapHash[x - stx, y - sty].Add(new DepthAndValue<T>(z, value));
            }
        }
        static int FindK(List<DepthAndValue<T>> list, int k)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].K == k)
                    return i;
            }
            return -1;
        }
        public bool GetHashValue(int x, int y, int z, ref T value)
        {
            if (mapHash[x - stx, y - sty] != null)
            {
                int index = FindK(mapHash[x - stx, y - sty], z);
                if (index == -1)
                {
                    return false;
                }
                else
                {
                    value = mapHash[x - stx, y - sty][index].Value;
                    return true;
                }
            }
            else
                return false;
        }
    }
    public class MeshBuilder_IntegerVertex
    {
        Mesh mesh;
        HashTable_2dArray<int> hashMap;
        int width;
        int height;
        int depth;
        public MeshBuilder_IntegerVertex(int width, int height, int depth)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
            mesh = new Mesh();
            this.hashMap = new HashTable_2dArray<int>(0,0,0,width, height, depth);
        }
        public void AddTriangle(Int16Triple p0, Int16Triple p1,Int16Triple p2)
        {
            int p0i;
            int p1i;
            int p2i;
            int index = 0;
            bool hasValue;
            hasValue = hashMap.GetHashValue(p0.X, p0.Y, p0.Z, ref index);
            if (!hasValue)
            {
                p0i = mesh.AddVertex(new Point3d(p0.X, p0.Y, p0.Z));
                hashMap.SetHashValue(p0.X, p0.Y, p0.Z, p0i);
            }
            else
            {
                p0i = index;
            }

            hasValue = hashMap.GetHashValue(p1.X, p1.Y, p1.Z, ref index);
            if (!hasValue)
            {
                p1i = mesh.AddVertex(new Point3d(p1.X, p1.Y, p1.Z));
                hashMap.SetHashValue(p1.X, p1.Y, p1.Z, p1i);
            }
            else
            {
                p1i = index;
            }

            hasValue = hashMap.GetHashValue(p2.X, p2.Y, p2.Z, ref index);
            if (!hasValue)
            {
                p2i = mesh.AddVertex(new Point3d(p2.X, p2.Y, p2.Z));
                hashMap.SetHashValue(p2.X, p2.Y, p2.Z, p2i);
            }
            else
            {
                p2i = index;
            }
            if (p0i != p1i && p0i != p2i && p1i != p2i)
            {
                Triangle t = new Triangle(p0i, p1i, p2i);
                mesh.AddFace(t);
            }
        }
        public Mesh GetMesh()
        {
            return mesh;
        }
    }
}
