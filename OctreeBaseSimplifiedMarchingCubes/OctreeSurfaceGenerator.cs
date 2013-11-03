using System;
using System.Collections.Generic;
using System.Text;
using BaseClasses;
namespace OSMC
{
    public class NodeParms
    {
        public byte Config;
        public int D;
        public override string ToString()
        {
            if (OctreeTable.ConfigToNormalTypeId[Config] < OctreeTable.NormalTypeIdToNormal.Length)
                return "\"" + OctreeTable.ConfigToNormalTypeId[Config] + "," + D + "\"";
            else
                return "not simple";
        }
    }
    public class OctreeSurfaceGenerator
    {
        private BitMap3d bmp;
        private Queue<OctreeNode<NodeParms>> nodequeue;
        private RegionOctree<NodeParms> tree;
        private MeshBuilder_IntegerVertex mb;
        private Int16Triple[] tempArray;

        public OctreeSurfaceGenerator(BitMap3d image)
        {
            this.bmp = image;
            nodequeue = new Queue<OctreeNode<NodeParms>>();
            tempArray = new Int16Triple[8]; 
            tree = new RegionOctree<NodeParms>(bmp.width, bmp.height, bmp.depth);
            mb = new MeshBuilder_IntegerVertex(bmp.width, bmp.height, bmp.depth);
        }

        public Mesh GenerateSurface()
        {
            ConstructTree();
            Shrink();
            ExtractTriangles();
            return mb.GetMesh();
        }

        private void ConstructTree()
        {
            int width = bmp.width;
            int height = bmp.height;
            int depth = bmp.depth;
            Int16Triple[] tempArray = new Int16Triple[8];
            for (int k = 0; k < depth - 1; k++)
            {
                for (int j = 0; j < height - 1; j++)
                {
                    for (int i = 0; i < width - 1; i++)
                    {
                        byte value = 0;
                        for (int pi = 0; pi < 8; pi++)
                        {
                            tempArray[pi].X = i + TableConsts.PointIndexToPointDelta[pi].X;
                            tempArray[pi].Y = j + TableConsts.PointIndexToPointDelta[pi].Y;
                            tempArray[pi].Z = k + TableConsts.PointIndexToPointDelta[pi].Z;
                            if (InRange(tempArray[pi].X, tempArray[pi].Y, tempArray[pi].Z) &&
                                IsWhite(tempArray[pi].X, tempArray[pi].Y, tempArray[pi].Z))
                            {
                                value |= TableConsts.PointIndexToFlag[pi];
                            }
                        }
                        if (value != 0 && value != 255)
                        {
                            OctreeNode<NodeParms> leafnode = tree.CreateToLeafNode(i, j, k);
                            leafnode.Parms = new NodeParms();
                            leafnode.Parms.Config = value;
                            leafnode.Parms.D = CaculateD(i, j, k, value);
                            leafnode.Visited = true;
                            if (!leafnode.Parent.Visited)
                            {
                                leafnode.Parent.Visited = true;
                                nodequeue.Enqueue(leafnode.Parent);
                            }
                        }
                    }
                }
            }
        }

        private void Shrink()
        {
            while (nodequeue.Count != 0)
            {
                OctreeNode<NodeParms> node = nodequeue.Dequeue();
                int D = int.MinValue;
                if (CanMergeNode(node, ref D))
                {
                    node.Parms = new NodeParms();
                    node.Parms.Config = CaculateConfig(node.Children);
                    node.Parms.D = D;
                    node.ClearChildren();
                    if (!node.Parent.Visited)
                    {
                        nodequeue.Enqueue(node.Parent);
                        node.Parent.Visited = true;
                    }
                }
            }
        }

        private void ExtractTriangles()
        {
            nodequeue.Enqueue(tree.GetRoot());
            while (nodequeue.Count != 0)
            {
                OctreeNode<NodeParms> node = nodequeue.Dequeue();
                if (node.IsLeaf())
                {
                    if (IsSingleCell(node))
                    {
                        GenerateFaceLeaf(node);
                    }
                    else
                    {
                        GenerateFace(node);
                    }
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (node.Children[i] != null)
                            nodequeue.Enqueue(node.Children[i]);
                    }
                }
            }//采用层次遍历寻找需要抽取三角片的节点
        }

        private bool IsSingleCell(OctreeNode<NodeParms> node)
        {
            BoxRange loc = node.GetRange();
            return loc.GetIsSinglePoint();
        }

        private bool InRange(int x, int y, int z)
        {
            return x >= 0 && x < bmp.width && y >= 0 && y < bmp.height && z >= 0 && z < bmp.depth;
        }

        private bool IsWhite(int x, int y, int z)
        {
            return bmp.GetPixel(x, y, z) == BitMap3d.WHITE;
        }

        private void GenerateFace(OctreeNode<NodeParms> node)
        {
            //需要找到该节点的端点位置
            byte cubeConfig = node.Parms.Config;
            int index = 0;
            while (MCTable.TriTable[cubeConfig, index] != -1)
            {
                int ei1 = MCTable.TriTable[cubeConfig, index];
                int ei2 = MCTable.TriTable[cubeConfig, index + 1];
                int ei3 = MCTable.TriTable[cubeConfig, index + 2];

                int normalTypeId = OctreeTable.ConfigToNormalTypeId[node.Parms.Config];

                Int16Triple p1 = GetIntersetedPointAtEdge(node, ei1, OctreeTable.NormalTypeIdToNormal[normalTypeId], node.Parms.D);
                Int16Triple p2 = GetIntersetedPointAtEdge(node, ei2, OctreeTable.NormalTypeIdToNormal[normalTypeId], node.Parms.D);
                Int16Triple p3 = GetIntersetedPointAtEdge(node, ei3, OctreeTable.NormalTypeIdToNormal[normalTypeId], node.Parms.D);

                mb.AddTriangle(p1, p2, p3);
                index += 3;
            }
        }

        private void GenerateFaceLeaf(OctreeNode<NodeParms> node)
        {
            for (int k = 0; k < 8; k++)
            {
                tempArray[k].X = node.Location.Range.XMin + TableConsts.PointIndexToPointDelta[k].X;
                tempArray[k].Y = node.Location.Range.YMin + TableConsts.PointIndexToPointDelta[k].Y;
                tempArray[k].Z = node.Location.Range.ZMin + TableConsts.PointIndexToPointDelta[k].Z;
            }
            byte value = node.Parms.Config;
            int index = 0;
            while (SMCTable.TableFat[value, index] != -1)
            {
                Int16Triple t0 = tempArray[SMCTable.TableFat[value, index]];
                Int16Triple t1 = tempArray[SMCTable.TableFat[value, index + 1]];
                Int16Triple t2 = tempArray[SMCTable.TableFat[value, index + 2]];
                mb.AddTriangle(t0, t1, t2);
                index += 3;
            }
        }

        private bool CanMergeNode(OctreeNode<NodeParms> node, ref int D)
        {
            byte normalType = OctreeTable.NormalNotSimple; ;
            for (int i = 0; i < 8; i++)
            {
                if (node.Children[i] != null)
                {
                    if (node.Children[i].Parms == null)
                    {
                        return false;//说明其下存在不能合并的节点 合并失败
                    }
                    else
                    {
                        byte normalTypeId = OctreeTable.ConfigToNormalTypeId[node.Children[i].Parms.Config];
                        if (normalTypeId != OctreeTable.NormalNotSimple)
                        {
                            normalType = normalTypeId;
                            D = node.Children[i].Parms.D;//记录其中的共面配置信息
                        }
                        else
                        {
                            return false;//遇到了非共面配置 合并失败
                        }
                    }
                }
            }
            for (int i = 0; i < 8; i++)
            {
                if (node.Children[i] != null)
                {
                    byte normalTypeId = OctreeTable.ConfigToNormalTypeId[node.Children[i].Parms.Config];
                    if (normalTypeId != normalType || node.Children[i].Parms.D != D)
                    {
                        return false;//体元配置均为共面类型但不是同一种的话也不中
                    }
                }
            }
            return true;
        }

        private Int16Triple GetIntersetedPointAtEdge(OctreeNode<NodeParms> node, int edgeIndex, Int16Triple normal, int d)
        {
            int x = 0, y = 0, z = 0;
            BoxRange range = node.GetRange();
            switch (edgeIndex)
            {
                case 0: { x = range.XMin; y = range.YMax + 1; return new Int16Triple(x, y, (d - normal.X * x - normal.Y * y) / normal.Z); }
                case 2: { x = range.XMin; y = range.YMin; return new Int16Triple(x, y, (d - normal.X * x - normal.Y * y) / normal.Z); }
                case 4: { x = range.XMax + 1; y = range.YMax + 1; return new Int16Triple(x, y, (d - normal.X * x - normal.Y * y) / normal.Z); }
                case 6: { x = range.XMax + 1; y = range.YMin; return new Int16Triple(x, y, (d - normal.X * x - normal.Y * y) / normal.Z); }

                case 8: { y = range.YMax + 1; z = range.ZMax + 1; return new Int16Triple((d - normal.Y * y - normal.Z * z) / normal.X, y, z); }
                case 9: { y = range.YMax + 1; z = range.ZMin; return new Int16Triple((d - normal.Y * y - normal.Z * z) / normal.X, y, z); }
                case 10: { y = range.YMin; z = range.ZMin; return new Int16Triple((d - normal.Y * y - normal.Z * z) / normal.X, y, z); }
                case 11: { y = range.YMin; z = range.ZMax + 1; return new Int16Triple((d - normal.Y * y - normal.Z * z) / normal.X, y, z); }

                case 1: { x = range.XMin; z = range.ZMin; return new Int16Triple(x, (d - normal.X * x - normal.Z * z) / normal.Y, z); }
                case 3: { x = range.XMin; z = range.ZMax + 1; return new Int16Triple(x, (d - normal.X * x - normal.Z * z) / normal.Y, z); }
                case 5: { x = range.XMax + 1; z = range.ZMin; return new Int16Triple(x, (d - normal.X * x - normal.Z * z) / normal.Y, z); }
                case 7: { x = range.XMax + 1; z = range.ZMax + 1; return new Int16Triple(x, (d - normal.X * x - normal.Z * z) / normal.Y, z); }

                default: throw new Exception();
            }
        }

        private int CaculateD(int cx, int cy, int cz, byte config)
        {
            byte index = OctreeTable.ConfigToEqType[config];
            if (index >= OctreeTable.EqTypeToEqQuad.Length)
                return int.MinValue;
            Int16Quad eq = OctreeTable.EqTypeToEqQuad[index];
            return eq.D + eq.A * cx + eq.B * cy + eq.C * cz;
        }

        private byte CaculateConfig(OctreeNode<NodeParms>[] children)
        {
            byte firstc = 0;
            int index = -1;
            for (int i = 0; i < 8; i++)
            {
                if (children[i] != null && children[i].Parms != null)
                {
                    firstc = children[i].Parms.Config;
                    index = i;
                    break;
                }
            }
            byte midPointValue = (((firstc) & (TableConsts.PointIndexToFlag[TableConsts.MidVoxelIndex[index]])) == 0) ? TableConsts.BLACKVOXEL : TableConsts.WHITEVOXEL;
            int ret = 0;
            for (int i = 0; i < 8; i++)
            {
                byte config = 0;
                if (children[i] != null && children[i].Parms != null)
                {
                    config = children[i].Parms.Config;
                }
                else
                {
                    config = midPointValue;
                }
                byte flag=TableConsts.PointIndexToFlag[TableConsts.VertexVoxelIndex[i]];
                int r=(config) & (flag);
                ret |=r;
            }
            return (byte)ret;
        }

    }
}
