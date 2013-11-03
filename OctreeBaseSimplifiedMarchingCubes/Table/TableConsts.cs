using System;
using System.Collections.Generic;
using System.Text;
using BaseClasses;
namespace OSMC
{
    public static class TableConsts
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
        public static byte BLACKVOXEL = 0;
        public static byte WHITEVOXEL = 255;
        //

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

        public static int[] VertexVoxelIndex = new int[8]
            {
                2,6,1,5,3,7,0,4
            };
        public static int[] MidVoxelIndex = new int[8]
            {
                4,0,7,3,5,1,6,2
            };
       
    }
}
