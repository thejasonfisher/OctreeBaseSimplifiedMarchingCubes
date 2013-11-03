using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BaseClasses
{
    public struct Int16Triple
    {
        public int X;
        public int Y;
        public int Z;
        public Int16Triple(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
    public struct Int16Quad
    {
        public int A;
        public int B;
        public int C;
        public int D;
        public Int16Quad(int a, int b, int c, int d)
        {
            this.A = a;
            this.B = b;
            this.C = c;
            this.D = d;
        }
    }
}
