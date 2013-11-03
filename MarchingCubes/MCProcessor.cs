using System;
using System.Collections.Generic;
using System.Text;
using BaseClasses;

namespace MarchingCubes
{
    struct OriginalTriangle
    {
        public Int16Triple CellCoord;
        public int E0;
        public int E1;
        public int E2;
        public OriginalTriangle(int x, int y, int z,int ei0,int ei1,int ei2)
        {
            CellCoord.X = x;
            CellCoord.Y = y;
            CellCoord.Z = z;
            E0 = ei0;
            E1 = ei1;
            E2 = ei2;
        }
    }
    class MCTriangleNetHashTable
    {
        public int CurrentLayerIndex;

        int stx;
        int sty;
        int width;
        int height;
        List<int[,,]> mapList;
        public MCTriangleNetHashTable(int minx, int miny, int width, int height)
        {
            this.stx = minx - 1;
            this.sty = miny - 1;
            this.width = width + 2;
            this.height = height + 2;
            mapList = new List<int[,,]>(2);
            mapList.Add(new int[this.width, this.height,3]);
            mapList.Add(new int[this.width, this.height,3]);
            SetDefaultValue(0);
            SetDefaultValue(1);
        }
        public void SetDefaultValue(int index0_1)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    mapList[index0_1][i, j, 0] = -1;
                    mapList[index0_1][i, j, 1] = -1;
                    mapList[index0_1][i, j, 2] = -1;
                }
            }
        }
        public void IncreaseIndex()
        {
            CurrentLayerIndex++;
            SetDefaultValue(0);
            int[,,] temp = mapList[0];
            mapList[0] = mapList[1];
            mapList[1] = temp;
        }
        public void SetHashValue(int x, int y, int z,int d, int value)
        {
            int index0_1 = z - CurrentLayerIndex;
            mapList[index0_1][x - stx, y - sty,d] = value;
        }
        public int GetHashValue(int x, int y, int z,int d)
        {
            int index0_1 = z - CurrentLayerIndex;
            return mapList[index0_1][x - stx, y - sty,d];
        }
    }
    public class MCProcessor
    {
        public static byte VULF = 1 << 0;
        public static byte VULB = 1 << 1;
        public static byte VLLB = 1 << 2;
        public static byte VLLF = 1 << 3;
        public static byte VURF = 1 << 4;
        public static byte VURB = 1 << 5;
        public static byte VLRB = 1 << 6;
        public static byte VLRF = 1 << 7;
        //以上为体素为实点的位标记
        public static Int16Triple[] PointIndexToPointDelta = new Int16Triple[8]
        {
            new Int16Triple(0, 1, 1 ),
            new Int16Triple(0, 1, 0 ),
            new Int16Triple(0, 0, 0 ),
            new Int16Triple(0, 0, 1 ),
            new Int16Triple(1, 1, 1 ),
            new Int16Triple(1, 1, 0 ),
            new Int16Triple(1, 0, 0 ),
            new Int16Triple(1, 0, 1 )
        };//体元内每个体素相对基准体素坐标的偏移
        public static byte[] PointIndexToFlag = new byte[8]
        {
            VULF,
            VULB,
            VLLB,
            VLLF,
            VURF,
            VURB,
            VLRB,
            VLRF
        };//每个体素对应的位标记
        public static int[,] EdgeIndexToEdgeVertexIndex = new int[12, 2]
        {
	        {0,1}, {1,2}, 
            {2,3},{3,0},
	        {4,5},{5,6}, 
            {6,7}, {7,4},
	        {0,4}, {1,5}, 
            {2,6}, {3,7}
        };//每个边对应的两顶点体素的索引
        public static Int16Quad[] CubeEdgeMapTable = new Int16Quad[12]
        {
            new Int16Quad(0,1,0,1),
            new Int16Quad(0,0,0,0),
            new Int16Quad(0,0,0,1),
            new Int16Quad(0,0,1,0),

            new Int16Quad(1,1,0,1),
            new Int16Quad(1,0,0,0),
            new Int16Quad(1,0,0,1),
            new Int16Quad(1,0,1,0),

            new Int16Quad(0,1,1,2),
            new Int16Quad(0,1,0,2),
            new Int16Quad(0,0,0,2),
            new Int16Quad(0,0,1,2),
        };


        BitMap3d bmp;
        int d;
        int h;
        int w;
        int wh;
        public MCProcessor(BitMap3d bitmap)
        {
            this.bmp = bitmap;
        }
        public Mesh GenerateSurface()
        {
            d = bmp.depth;
            h = bmp.height;
            w = bmp.width;
            wh = w * h;
            Int16Triple[] temp = new Int16Triple[8];
            Mesh m = new Mesh();
            OriginalTriangle[] tempTriangles = new OriginalTriangle[6];
            MCTriangleNetHashTable hash = new MCTriangleNetHashTable(0, 0, w, h);

            for (int k = 0; k <= d - 1; k++)
            {
                for (int j = 0; j <= h - 1; j++)
                {
                    for (int i = 0; i <= w - 1; i++)
                    {
                        byte value = GetConfig(temp, bmp, i, j, k);
                        if (value == 0 || value == 255)
                            continue;
                        int tcount = ExtractTriangles(temp, value, i, j, k, tempTriangles);
                        for (int tindex = 0; tindex < tcount; tindex++)
                        {
                            MergeTriangleIntoMesh(m, hash, tempTriangles[tindex]);
                        }
                    }
                }
                hash.IncreaseIndex();
            }
            return m;
        }

        private byte GetConfig(Int16Triple[] temp, BitMap3d flagsMap, int indexInWidth, int indexInHeight, int indexInDepth)
        {
            byte value = 0;
            for (int pi = 0; pi < 8; pi++)
            {
                temp[pi].X = indexInWidth + PointIndexToPointDelta[pi].X;
                temp[pi].Y = indexInHeight + PointIndexToPointDelta[pi].Y;
                temp[pi].Z = indexInDepth + PointIndexToPointDelta[pi].Z;
                if (temp[pi].X < w && temp[pi].X >= 0
                    && temp[pi].Y < h && temp[pi].Y >= 0
                    && temp[pi].Z < d && temp[pi].Z >= 0
                    && bmp.data[temp[pi].X + w * (temp[pi].Y) + wh * (temp[pi].Z)] == BitMap3d.WHITE)
                {
                    value |= PointIndexToFlag[pi];
                }
            }
            return value;
        }

        private int ExtractTriangles(Int16Triple[] temp, byte value, int indexInWidth, int indexInHeight, int indexInDepth, OriginalTriangle[] result)
        {
            int tcount = 0;
            if (MCTable.TriTable[value, 0] != -1)
            {
                int index = 0;
                while (MCTable.TriTable[value, index] != -1)
                {
                    int e0index = MCTable.TriTable[value, index];
                    int e1index = MCTable.TriTable[value, index + 1];
                    int e2index = MCTable.TriTable[value, index + 2];
                    result[tcount] = new OriginalTriangle(indexInWidth,  indexInHeight, indexInDepth,e0index,e1index,e2index);
                    tcount++;
                    index += 3;
                }
            }
            return tcount;
        }

        private void MergeTriangleIntoMesh(Mesh mesh, MCTriangleNetHashTable hashMap, OriginalTriangle ot)
        {
            int e0i= CubeEdgeMapTable[ot.E0].D;
            int p0x = ot.CellCoord.X + CubeEdgeMapTable[ot.E0].A;
            int p0y = ot.CellCoord.Y + CubeEdgeMapTable[ot.E0].B;
            int p0z = ot.CellCoord.Z + CubeEdgeMapTable[ot.E0].C;


            int e1i = CubeEdgeMapTable[ot.E1].D;
            int p1x = ot.CellCoord.X + CubeEdgeMapTable[ot.E1].A;
            int p1y = ot.CellCoord.Y + CubeEdgeMapTable[ot.E1].B;
            int p1z = ot.CellCoord.Z + CubeEdgeMapTable[ot.E1].C;


            int e2i = CubeEdgeMapTable[ot.E2].D;
            int p2x = ot.CellCoord.X + CubeEdgeMapTable[ot.E2].A;
            int p2y = ot.CellCoord.Y + CubeEdgeMapTable[ot.E2].B;
            int p2z = ot.CellCoord.Z + CubeEdgeMapTable[ot.E2].C;


            int p0i;
            int p1i;
            int p2i;
            int index = 0;
            index = hashMap.GetHashValue(p0x, p0y, p0z,e0i);
            if (index == -1)
            {
                Point3d interp = GetIntersetedPoint(ot.CellCoord.X, ot.CellCoord.Y, ot.CellCoord.Z, ot.E0);
                p0i = mesh.AddVertex(interp);
                hashMap.SetHashValue(p0x, p0y, p0z,e0i,p0i);
            }
            else
            {
                p0i = index;
            }

            index = hashMap.GetHashValue(p1x, p1y, p1z,e1i);
            if (index == -1)
            {
                Point3d interp = GetIntersetedPoint(ot.CellCoord.X, ot.CellCoord.Y, ot.CellCoord.Z, ot.E1);
                p1i = mesh.AddVertex(interp);
                hashMap.SetHashValue(p1x, p1y, p1z,e1i ,p1i);
            }
            else
            {
                p1i = index;
            }

            index = hashMap.GetHashValue(p2x, p2y, p2z,e2i);
            if (index == -1)
            {
                Point3d interp = GetIntersetedPoint(ot.CellCoord.X, ot.CellCoord.Y, ot.CellCoord.Z, ot.E2);
                p2i = mesh.AddVertex(interp);
                hashMap.SetHashValue(p2x, p2y, p2z,e2i ,p2i);
            }
            else
            {
                p2i = index;
            }

            Triangle t = new Triangle(p0i, p1i, p2i);
            mesh.AddFace(t);
        }

        private Point3d GetIntersetedPoint(int cx,int cy,int cz, int ei)
        {
            int p0i = EdgeIndexToEdgeVertexIndex[ei, 0];
            int p1i = EdgeIndexToEdgeVertexIndex[ei, 1];

            int p0X = cx+PointIndexToPointDelta[p0i].X;
            int p0Y = cy + PointIndexToPointDelta[p0i].Y;
            int p0Z = cz + PointIndexToPointDelta[p0i].Z;

            int p1X = cx + PointIndexToPointDelta[p1i].X;
            int p1Y = cy + PointIndexToPointDelta[p1i].Y;
            int p1Z = cz + PointIndexToPointDelta[p1i].Z;
           
            return new Point3d((p0X+p1X)/2.0f,(p0Y+p1Y)/2.0f,(p0Z+p1Z)/2.0f);
        }
    }
}
