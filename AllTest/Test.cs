using System;
using System.Collections.Generic;
using System.Text;
using BaseClasses;
using MarchingCubes;
using SimplifiedMarchingCubes;
using System.Diagnostics;
using OSMC;

namespace AllTest
{
    public static class TestFunctions
    {
        public static bool output = true;
        static Stopwatch watch = new Stopwatch();
        public static void Test()
        {
            Statistics.Do3();
            //Mesh.record = true;
            //BitMap3d bmp = Sample2.CreateSampleForTableX4();
            //OutputData(bmp, "TableX4");
        }

        static Int16Triple RunMC(BitMap3d bmp,string type)
        {
           watch.Reset();
           watch.Start();
           MCProcessor mc = new MCProcessor(bmp);
           Mesh mesh= mc.GenerateSurface();
           watch.Stop();
           Console.WriteLine("MC Run time : " + watch.ElapsedMilliseconds + "ms,"+"v: "+mesh.GetVertexCount()+" f:"+mesh.GetFaceCount());
            if(output)
                 PlyManager.Output(mesh, type+"_"+mesh.GetVertexCount()+"_"+mesh.GetFaceCount()+".ply");
            return new Int16Triple(mesh.GetVertexCount(),mesh.GetFaceCount(),0);
        }
        static Int16Triple RunSMC(BitMap3d bmp, string type)
        {
            watch.Reset();
            watch.Start();
            SMCProcessor mc = new SMCProcessor(bmp);
            Mesh mesh = mc.GenerateSurface();
            watch.Stop();
            Console.WriteLine("SMC Run time : " + watch.ElapsedMilliseconds + "ms," + "v: " + mesh.GetVertexCount() + " f:" + mesh.GetFaceCount());
            if (output)
                PlyManager.Output(mesh, type + "_" + mesh.GetVertexCount() + "_" + mesh.GetFaceCount() + ".ply");
            return new Int16Triple(mesh.GetVertexCount(), mesh.GetFaceCount(), 0);
        }
        static Int16Triple RunOSMC(BitMap3d bmp, string type)
        {
            watch.Reset();
            watch.Start();
            OctreeSurfaceGenerator mc = new OctreeSurfaceGenerator(bmp);
            Mesh mesh = mc.GenerateSurface();
            watch.Stop();
            Console.WriteLine("OSMC Run time : " + watch.ElapsedMilliseconds + "ms," + "v: " + mesh.GetVertexCount() + " f:" + mesh.GetFaceCount());
            if (output)
                PlyManager.Output(mesh, type + "_" + mesh.GetVertexCount() + "_" + mesh.GetFaceCount() + ".ply");
            return new Int16Triple(mesh.GetVertexCount(), mesh.GetFaceCount(), 0);
        }

        static void OutputData(BitMap3d bmp,string type)
        {
            RunMC(bmp, type + "_mc");
            RunSMC(bmp, type + "_smc");
            RunOSMC(bmp, type + "_osmc");
        }
    }
}
