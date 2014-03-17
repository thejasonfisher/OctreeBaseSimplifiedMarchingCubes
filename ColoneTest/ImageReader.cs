using System;
using System.Collections.Generic;
using System.Text;
using BaseClasses;

namespace ColoneTest
{
    static class ImageReader
    {
        public static BitMap3d ReadSegImage(string path)
        {
            byte[] data = System.IO.File.ReadAllBytes(path);
            byte[] size=new byte[4];
            size[0]=data[0];size[1]=data[1];size[2]=data[2];size[3]=data[3];
            int width = BitConverter.ToInt32(size, 0);
            size[0] = data[4]; size[1] = data[5]; size[2] = data[6]; size[3] = data[7];
            int height = BitConverter.ToInt32(size, 0);
            size[0] = data[8]; size[1] = data[9]; size[2] = data[10]; size[3] = data[11];
            int depth = BitConverter.ToInt32(size, 0);
            byte[] newdata = new byte[data.Length - 12];
            for (int i = 0; i < data.Length-12; i++)
            {
                newdata[i] = data[i + 12];
            }
            BitMap3d bmp = new BitMap3d(newdata,width,height,depth);
            return bmp;
        }

        public static void ProcessImage(BitMap3d bmp)
        {
            byte[] data = bmp.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 200)
                {
                    data[i] = 0;
                }
            }
        }

        internal static BitMap3d GetLayers(BitMap3d bmp, int p1, int p2)
        {
            BitMap3d bmp2 = new BitMap3d(bmp.width,bmp.height,p2-p1+1,0);
            for (int i = 0; i < bmp.width; i++)
            {
                for (int j = 0; j < bmp.height; j++)
                {
                    for (int k = p1; k <= p2; k++)
                    {
                        bmp2.SetPixel(i, j, k - p1, bmp.GetPixel(i, j, k));
                    }
                }
            }
                return bmp2;

        }
    }
}
