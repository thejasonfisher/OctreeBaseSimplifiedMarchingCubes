using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BaseClasses
{
    public class BitMap3d
    {
        public const byte WHITE = 255;
        public const byte BLACK = 0;
        public byte[] data;
        public int width;
        public int height;
        public int depth;
        public BitMap3d(int width, int height, int depth, byte v)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
            data = new byte[width * height * depth];
            for (int i = 0; i < width * height * depth; i++)
                data[i] = v;
        }
        public BitMap3d(byte[] data, int width, int height, int depth)
        {
            this.data = data;
            this.width = width;
            this.height = height;
            this.depth = depth;
        }
        public void SetPixel(int x, int y, int z, byte v)
        {
            data[x + y * width + z * width * height] = v;
        }
        public byte GetPixel(int x, int y, int z)
        {
            return data[x + y * width + z * width * height];
        }
        public void ReadRaw(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            fs.Read(data, 0, width * height * depth);
            fs.Close();
        }
        public void SaveRaw(string path)
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            fs.Write(data, 0, data.Length);
            fs.Close();
        }
    }
}
