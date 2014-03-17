using System;
using System.Collections.Generic;
using System.Text;
using BaseClasses;
using MarchingCubes;

namespace ColoneTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //BitMap3d bmp = ImageReader.ReadSegImage(@"D:\Uproj\WI_3093_P_iso.seg");
            //BitMap3d nbmp = ImageReader.GetLayers(bmp, 70, 99);
            //nbmp.SaveRaw(@"D:\Uproj\WI_3093_P_iso70.99.raw");
            BitMap3d bmp = new BitMap3d(512, 512, 30, 0);
            bmp.ReadRaw(@"D:\Uproj\WI_3093_P_iso70.99.raw");
            //ImageReader.ProcessImage(bmp);
            ConcreteMCP mc = new ConcreteMCP(bmp);
            mc.SetThreshold(180.5f);
            Mesh m = mc.GenerateSurface();
            PlyManager.Output(m, @"D:\Uproj\WI_3093_P_iso70.99.180.5r.ply");
        }
    }
}
