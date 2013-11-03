using System;
using System.Collections.Generic;
using System.Text;

namespace AllTest
{
    //struct Row
    //{
    //    public string Name;
    //    public int VCount;
    //    public int FCount;
    //}
    //struct ResultRow
    //{
    //    public string Type;
    //    public string 
    //}
    public static class Statistics
    {
        static string datadirpath = @"F:\workspacecsharp\RecentProject\OctreeBaseSimplifiedMarchingCubes\AllTest\bin\Release\Data";
        static void StatisticsFunc(int mcf,int smcf,int osmcf)
        {
            double r1=(mcf-smcf)*100.0/mcf;
            double r2=(smcf-osmcf)*100.0/smcf;
            double r3=(mcf-osmcf)*100.0/mcf;

            string sr1=r1.ToString("F1");
            string sr2=r2.ToString("F1");
            string sr3=r3.ToString("F1");
            Console.WriteLine(string.Format("result: sd:{0}% , ods:{1}% , odm:{2}%",sr1,sr2,sr3));
        }
        static void StatisticsFunc2(int x1, int x2, int x3,int x4)
        {
            double r1 = (double)x2 / x1;
            double r2 = (double)x3 / x1;
            double r3 = (double)x4 / x1;

            string sr1 = r1.ToString("F2");
            string sr2 = r2.ToString("F2");
            string sr3 = r3.ToString("F2");
            Console.WriteLine(string.Format("result: 1.00, {0}, {1}, {2}", sr1, sr2, sr3));
        }
        static void StatisticsFunc3(int mcf, int smcf)
        {
            double r1 = (mcf - smcf) * 100.0 / mcf;
            string sr1 = r1.ToString("F1");
            Console.WriteLine(string.Format("result: sd:{0}% \n",sr1));
        }
        public static void Do()
        {
            string line;
            while(true)
            {
                line=Console.ReadLine();
                string[] s=line.Split(' ');
                int r1=int.Parse(s[0]);
                int r2=int.Parse(s[1]);
                int r3=int.Parse(s[2]);
                StatisticsFunc(r1,r2,r3);

            }
        }
        public static void Do2()
        {
            string line;
            while (true)
            {
                line = Console.ReadLine();
                string[] s = line.Split(' ');
                int r1 = int.Parse(s[0]);
                int r2 = int.Parse(s[1]);
                int r3 = int.Parse(s[2]);     
                int r4 = int.Parse(s[3]);
                StatisticsFunc2(r1, r2, r3,r4);

            }
        }
        public static void Do3()
        {
            string line;
            while (true)
            {
                line = Console.ReadLine();
                string[] s = line.Split(' ');
                int r1 = int.Parse(s[0]);
                int r2 = int.Parse(s[1]);
                StatisticsFunc3(r1, r2);

            }
        }

    }
}
