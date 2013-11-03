using System;
using System.Collections.Generic;
using System.Text;
using BaseClasses;

namespace AllTest
{
    public static class Sample2
    {
        static void ClearBoundary(BitMap3d image)
        {
            byte[] data = image.data;
            for (int k = 0; k < image.depth; k++)
            {
                for (int j = 0; j < image.height; j++)
                {
                    for (int i = 0; i < image.width; i++)
                    {
                        int index = k * image.width * image.height + j * image.width + i;
                        if (i == 0 || i == image.width - 1 || j == 0 || j == image.height - 1 || k == 0 || k == image.depth - 1)
                        {
                            data[index] = BitMap3d.BLACK;
                        }
                    }
                }
            }
        }
        //static string dirPath = @"D://VTKproj//TreeRaw//";
        public static BitMap3d CreateSampleTedX1()
        {
            BitMap3d image = new BitMap3d(100, 100, 100, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\Ted_100.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > 128)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            return image;
        }
        public static BitMap3d CreateSampleTedX2()
        {
            BitMap3d image = new BitMap3d(200,200,200, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\Ted_200.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > 128)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            return image;
        }
        public static BitMap3d CreateSampleTedX3()
        {
            BitMap3d image = new BitMap3d(300,300,300,BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\Ted_300.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > 128)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            return image;
        }
        public static BitMap3d CreateSampleTedX4()
        {
            BitMap3d image = new BitMap3d(400, 400, 400, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\Ted_400.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > 128)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            return image;
        }


        public static BitMap3d CreateSampleEngineX1()
        {
            BitMap3d image;
            image = new BitMap3d(256, 256, 128, BitMap3d.BLACK);
            image.ReadRaw("D://VTKproj//TreeRaw//engine_256_256_128.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 64 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }
        public static BitMap3d CreateSampleEngineX2()
        {
            BitMap3d image;
            image = new BitMap3d(512, 512, 256, BitMap3d.BLACK);
            image.ReadRaw("D://VTKproj//TreeRaw//engine_512_512_256.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 64 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }
        public static BitMap3d CreateSampleEngineX3()
        {
            BitMap3d image;
            image = new BitMap3d(768, 768, 384, BitMap3d.BLACK);
            image.ReadRaw("D://VTKproj//TreeRaw//engine_768_768_384.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 64 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }
        public static BitMap3d CreateSampleEngineX4()
        {
            BitMap3d image;
            image = new BitMap3d(1024, 1024, 512, BitMap3d.BLACK);
            image.ReadRaw("D://VTKproj//TreeRaw//engine_1024_1024_512.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 64 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }

        public static BitMap3d CreateSampleForFanX1()
        {
            BitMap3d image = new BitMap3d(100, 100, 100, BitMap3d.BLACK);
            image.ReadRaw("D://VTKproj//TreeRaw//marschnerlobb_100_100_100.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 89 && data[i] <= 255)
                    data[i] = BitMap3d.BLACK;
                else
                    data[i] = BitMap3d.WHITE;
            }
            ClearBoundary(image);
            return image;
        }
        public static BitMap3d CreateSampleForFanX15()
        {
            BitMap3d image = new BitMap3d(150, 150, 150, BitMap3d.BLACK);
            image.ReadRaw("D://VTKproj//TreeRaw//marschnerlobb_150_150_150.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 89 && data[i] <= 255)
                    data[i] = BitMap3d.BLACK;
                else
                    data[i] = BitMap3d.WHITE;
            }
            ClearBoundary(image);
            return image;
        }
        public static BitMap3d CreateSampleForFanX2()
        {
            BitMap3d image = new BitMap3d(200, 200, 200, BitMap3d.BLACK);
            image.ReadRaw("D://VTKproj//TreeRaw//marschnerlobb_200_200_200.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 89 && data[i] <= 255)
                    data[i] = BitMap3d.BLACK;
                else
                    data[i] = BitMap3d.WHITE;
            }
            ClearBoundary(image);
            return image;
        }
        public static BitMap3d CreateSampleForFanX3()
        {
            BitMap3d image = new BitMap3d(300, 300, 300, BitMap3d.BLACK);
            image.ReadRaw("D://VTKproj//TreeRaw//marschnerlobb_300_300_300.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 89 && data[i] <= 255)
                    data[i] = BitMap3d.BLACK;
                else
                    data[i] = BitMap3d.WHITE;
            }
            ClearBoundary(image);
            return image;
        }
        public static BitMap3d CreateSampleForFanX4()
        {
            BitMap3d image = new BitMap3d(400, 400, 400, BitMap3d.BLACK);
            image.ReadRaw("D://VTKproj//TreeRaw//marschnerlobb_400_400_400.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 89 && data[i] <= 255)
                    data[i] = BitMap3d.BLACK;
                else
                    data[i] = BitMap3d.WHITE;
            }
            ClearBoundary(image);
            return image;
        }

        public static BitMap3d CreateSampleForLobsterX1()
        {
            BitMap3d image = new BitMap3d(301, 324, 56, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\lobster_301_324_56.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 37 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }
        public static BitMap3d CreateSampleForLobsterX2()
        {
            BitMap3d image = new BitMap3d(602, 648, 112, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\lobster_602_648_112.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 37 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }

        public static BitMap3d CreateSampleForTableX1()
        {
            BitMap3d image = new BitMap3d(197, 70, 101, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\table_197_70_101.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 37 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }
        public static BitMap3d CreateSampleForTableX2()
        {
            BitMap3d image = new BitMap3d(394, 140, 203, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\table_394_140_203.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 37 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }
        public static BitMap3d CreateSampleForTableX3()
        {
            BitMap3d image = new BitMap3d(591, 210, 304, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\table_591_210_304.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 37 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }
        public static BitMap3d CreateSampleForTableX4()
        {
            BitMap3d image = new BitMap3d(788, 280, 406, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\table_788_280_406.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 37 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }

        public static BitMap3d CreateSampleForTeapotX1()
        {
            BitMap3d image = new BitMap3d(256, 256, 178, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\BostonTeapot_256_256_178.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 24 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }
        public static BitMap3d CreateSampleForTeapotX2()
        {
            BitMap3d image = new BitMap3d(512, 512, 356, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\BostonTeapot_512_512_356.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 24 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }

        public static BitMap3d CreateSampleForBonsaiX1()
        {
            BitMap3d image = new BitMap3d(256, 256, 256, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\bonsai_256_256_256.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 51 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }
        public static BitMap3d CreateSampleForBonsaiX2()
        {
            BitMap3d image = new BitMap3d(512, 512, 512, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\bonsai_512_512_512.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 51 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }

        public static BitMap3d CreateSampleForSkullX1()
        {
            BitMap3d image = new BitMap3d(256, 256, 256, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\skull_256_256_256.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 43 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }
        public static BitMap3d CreateSampleForSkullX2()
        {
            BitMap3d image = new BitMap3d(512, 512, 512, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\skull_512_512_512.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 43 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }

        public static BitMap3d CreateSampleForMRIX1()
        {
            BitMap3d image = new BitMap3d(256, 256, 124, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\mri_ventricles_256_256_124.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 84 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }
        public static BitMap3d CreateSampleForMRIX2()
        {
            BitMap3d image = new BitMap3d(512, 512, 248, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\mri_ventricles_512_512_248.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 84 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }

        public static BitMap3d CreateSampleForBackPackX1()
        {
            BitMap3d image = new BitMap3d(512, 512, 373, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\backpack_512_512_373.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 46 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }

        public static BitMap3d CreateSampleForPhantomX1()
        {
            BitMap3d image = new BitMap3d(512, 512, 442, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\phantom_512_512_442.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 42 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }

        public static BitMap3d CreateSampleForProneX1()
        {
            BitMap3d image = new BitMap3d(512, 512, 463, BitMap3d.BLACK);
            image.ReadRaw(@"D:\VTKproj\TreeRaw\prone8_512_512_463.raw");
            byte[] data = image.data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 88 && data[i] <= 255)
                    data[i] = BitMap3d.WHITE;
                else
                    data[i] = BitMap3d.BLACK;
            }
            ClearBoundary(image);
            return image;
        }

    }
}
