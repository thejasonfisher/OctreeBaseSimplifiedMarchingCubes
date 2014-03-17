#ifndef IMAGEVOLUME_H
#define IMAGEVOLUME_H
#include <stdio.h>
#include <string.h>
#define IMAGE_INDEXTYPE long long
class ImageVolume_8bit
{
public:
	unsigned char* Data;
	int Width;
	int Height;
	int Depth;
	ImageVolume_8bit(int width,int height, int depth);
	~ImageVolume_8bit();
	IMAGE_INDEXTYPE GetLength();
	void ReadVolume(const char* fileName);
	void SaveRaw(const char* fileName);
};
ImageVolume_8bit::ImageVolume_8bit(int width,int height, int depth)
{
	this->Width=width;
	this->Height=height;
	this->Depth=depth;
	Data=new unsigned char[GetLength()];
	memset(Data,0,GetLength());
}
ImageVolume_8bit::~ImageVolume_8bit()
{
	delete[] Data;
}
IMAGE_INDEXTYPE ImageVolume_8bit::GetLength()
{
	return (IMAGE_INDEXTYPE)Width*Height*Depth;
}
void ImageVolume_8bit::ReadVolume(const char* fileName)
{
	FILE* file = fopen(fileName,"rb");
	if( file == NULL )
	{
		printf("open the file failed\n");
	}
	fread(Data,sizeof(unsigned char),GetLength(),file );
	fclose(file);
	return;
}
void ImageVolume_8bit::SaveRaw(const char* fileName)
{
	FILE *const nfile = fopen(fileName,"wb");
	fwrite(Data,sizeof(unsigned char),GetLength(),nfile);
	fclose(nfile);
	return;
}
#endif