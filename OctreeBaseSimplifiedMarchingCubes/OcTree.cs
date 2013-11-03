using System;
using System.Collections.Generic;
using System.Text;

namespace OSMC
{
    public struct BoxRange
    {
        public int XMin;//所代表范围的X轴下界
        public int YMin;//所代表范围的Y轴下界
        public int ZMin;//所代表范围的Z轴下界
        public int XMax;//所代表范围的X轴下界
        public int YMax;//所代表范围的Y轴下界
        public int ZMax;//所代表范围的Z轴下界
        public bool GetIsSinglePoint()
        {
            return XMin == XMax && YMin == YMax && ZMin == ZMax;
        }
    }
    public struct LocationInfo
    {
        public BoxRange Range;
        public int IndexInParent;//自己在父节点孩子数组中的索引
        public int LayerIndex;//自己所在的层索引
        public override string ToString()
        {
            return string.Format("[{0},{1}][{2},{3}][{4},{5}]", Range.XMin, Range.XMax, Range.YMin, Range.YMax, Range.ZMin, Range.ZMax);
        }
    }
    public class OctreeNode<T>
    {
        #region Properties
        public OctreeNode<T>[] Children;//孩子指针,数组大小为8
        public OctreeNode<T> Parent;//父节点指针
        public T Parms;//携带的参数
        public LocationInfo Location;
        public bool Visited = false;
        #endregion
        #region Methods
        public void ClearChildren()
        {
            for (int i = 0; i < 8; i++)
            {
                if (Children[i] != null)
                {
                    Children[i].Parent = null;
                    Children[i] = null;
                }
            }
        }
        public bool IsLeaf()
        {
            for (int i = 0; i < 8; i++)
            {
                if (Children[i] != null)
                    return false;
            }
            return true;
        }//返回是否是叶子节点
        public int GetLayerIndex()
        {
            return Location.LayerIndex;
        }
        public int GetIndexInParent()
        {
            return Location.IndexInParent;
        }
        public BoxRange GetRange()
        {
            return Location.Range;
        }
        public override string ToString()
        {
            if (IsLeaf())
            {
                return Location.ToString() + " " + Parms.ToString()+"Leaf";
            }
            else
            {
                return Location.ToString() + " " + Parms.ToString();
            }
        }
        #endregion
        
    }//BON八叉树节点
    public class RegionOctree<T>
    {
        #region private
        private int Width;//树所关联空间范围的X上界
        private int Height;//树所关联空间范围的Y上界
        private int Depth;//树所关联空间范围的Z上界
        private int Scale;//2的幂包围盒边长
        private int LayerNum;//层次数
        private OctreeNode<T>[] NodeLayers;//指代一条由根通往叶子的路径
        private OctreeNode<T> Root;//树根节点
        private int NodeCount;//所有节点总数
        private int LeafCount;//叶子节点
        #endregion

        private int GetMax2Power(int xmax, int ymax, int zmax, ref int log)
        {
            int max = xmax;
            if (ymax > max)
                max = ymax;
            if (zmax > max)
                max = zmax;
            if ((max & (max - 1)) == 0)
            {
                double L = Math.Log(max, 2);
                log = (int)L + 1;
                return max;
            }
            else
            {
                double L = Math.Log(max, 2);
                log = (int)L + 2;
                return (int)Math.Pow(2, log - 1);
            }
        }
        private int GetIndexOn(int x, int y, int z, int bitindex)
        {
            int ret = 0;
            if ((x & (1 << bitindex)) != 0)
            {
                ret |= 1;
            }
            if ((y & (1 << bitindex)) != 0)
            {
                ret |= 2;
            }
            if ((z & (1 << bitindex)) != 0)
            {
                ret |= 4;
            }
            return ret;
        }
        private void InitRangeByParentAndIndex(OctreeNode<T> node, OctreeNode<T> pnode, int index)
        {
            int deltaX = (pnode.Location.Range.XMax - pnode.Location.Range.XMin + 1) >> 1;
            int deltaY = (pnode.Location.Range.YMax - pnode.Location.Range.YMin + 1) >> 1;
            int deltaZ = (pnode.Location.Range.ZMax - pnode.Location.Range.ZMin + 1)  >> 1;
            if ((index & 1) == 0)
            {
                node.Location.Range.XMin = pnode.Location.Range.XMin;
                node.Location.Range.XMax = pnode.Location.Range.XMin + deltaX - 1;
            }
            else
            {
                node.Location.Range.XMin = pnode.Location.Range.XMin + deltaX;
                node.Location.Range.XMax = pnode.Location.Range.XMax;
            }
            if ((index & 2) == 0)
            {
                node.Location.Range.YMin = pnode.Location.Range.YMin;
                node.Location.Range.YMax = pnode.Location.Range.YMin + deltaY - 1;
            }
            else
            {
                node.Location.Range.YMin = pnode.Location.Range.YMin + deltaY;
                node.Location.Range.YMax = pnode.Location.Range.YMax;
            }
            if ((index & 4) == 0)
            {
                node.Location.Range.ZMin = pnode.Location.Range.ZMin;
                node.Location.Range.ZMax = pnode.Location.Range.ZMin + deltaZ - 1;
            }
            else
            {
                node.Location.Range.ZMin = pnode.Location.Range.ZMin + deltaZ;
                node.Location.Range.ZMax = pnode.Location.Range.ZMax;
            }
        }//使用父节点的信息初始化子节点的范围

        public RegionOctree(int width, int height, int depth)//使用范围构造BON树
        {
            this.Width = width;
            this.Height = height;
            this.Depth = depth;
            Scale = GetMax2Power(Width, Height, Depth, ref LayerNum);
            NodeCount = 0;
            Root = new OctreeNode<T>();
            Root.Location.Range.XMin = 0;
            Root.Location.Range.XMax = Scale - 1;
            Root.Location.Range.YMin = 0;
            Root.Location.Range.YMax = Scale - 1;
            Root.Location.Range.ZMin = 0;
            Root.Location.Range.ZMax = Scale - 1;
            Root.Parent = null;
            Root.Location.IndexInParent = -1;
            Root.Location.LayerIndex = LayerNum - 1;
            Root.Children = new OctreeNode<T>[8];
            NodeLayers = new OctreeNode<T>[LayerNum];
            NodeLayers[0] = Root;
        }
        public OctreeNode<T> CreateToLeafNode(int x, int y, int z)
        {
            if (!(x >= 0 && x < Width && y >= 0 && y < Height && z >= 0 && z < Depth))
                return null;
            LeafCount++;
            for (int i = 1; i <= LayerNum - 1; i++)
            {
                int index = GetIndexOn(x, y, z, LayerNum - i - 1);
                if (NodeLayers[i - 1].Children[index] == null)
                {
                    OctreeNode<T> node = new OctreeNode<T>();
                    NodeCount++;
                    node.Parent = NodeLayers[i - 1];
                    node.Location.IndexInParent = index;
                    node.Children = new OctreeNode<T>[8];
                    node.Location.LayerIndex = NodeLayers[i - 1].Location.LayerIndex - 1;
                    InitRangeByParentAndIndex(node, NodeLayers[i - 1], index);
                    NodeLayers[i - 1].Children[index] = node;
                }
                NodeLayers[i] = NodeLayers[i - 1].Children[index];
            }
            return NodeLayers[NodeLayers.Length - 1];
        }//将关联着坐标（x，y，z）处元素一路插入到底层为叶子节点
        public OctreeNode<T> GetRoot()
        {
            return Root;
        }
        public int GetScale()
        {
            return Scale;
        }
        public int GetNodeCount()
        {
            return NodeCount;
        }
        public int GetLeafCount()
        {
            return LeafCount;
        }


    }
}
