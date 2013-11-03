using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BaseClasses
{
    public class PlyManager
    {
        private static void AWriteV(StreamWriter sw, double v1, double v2, double v3, byte r, byte g, byte b)
        {
            int r1 = (int)r;
            int g1 = (int)g;
            int b1 = (int)b;
            sw.Write(string.Format("{0} {1} {2} {3} {4} {5}\n", v1.ToString("0.0"), v2.ToString("0.0"), v3.ToString("0.0"), r1, g1, b1));
        }
        private static void AWriteF(StreamWriter sw, int i1, int i2, int i3)
        {
            sw.Write(string.Format("{0} {1} {2} {3}\n", 3, i1, i2, i3));
        }
        public static void Output(Mesh mesh, string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.Write("ply\n");
            sw.Write("format ascii 1.0\n");
            sw.Write("comment VCGLIB generated\n");
            sw.Write(string.Format("element vertex {0}\n", mesh.Vertices.Count));
            sw.Write("property float x\n");
            sw.Write("property float y\n");
            sw.Write("property float z\n");
            sw.Write("property uchar red\n");
            sw.Write("property uchar green\n");
            sw.Write("property uchar blue\n");
            sw.Write(string.Format("element face {0}\n", mesh.Faces.Count));
            sw.Write("property list int int vertex_indices\n");
            sw.Write("end_header\n");
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                AWriteV(sw, mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z, 255, 255, 255);
            }
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                AWriteF(sw, mesh.Faces[i].P0Index, mesh.Faces[i].P1Index, mesh.Faces[i].P2Index);
            }
            sw.Close();
            fs.Close();
        }
    }
}
