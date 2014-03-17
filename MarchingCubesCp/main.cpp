#include <iostream>
#include "MCProcessor.h"
#include "ImageVolume.h"

int main()
{
	ImageVolume_8bit image(256,256,128);
	//image.ReadVolume("D:\\VTKproj\\engine.raw");
	for(int i=0;i<image.GetLength();i++)
	{
		//if(image.Data[i]>64)
			image.Data[i]=255;
		//else
		//image.Data[i]=0;
	}
	MCProcessor sp(image);
	Mesh* m=sp.GenerateSurface();
	PlyManager::Output(*m,"test.ply");
	delete m;
	return 0;
}