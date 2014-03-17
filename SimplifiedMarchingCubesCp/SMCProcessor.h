#ifndef SMCPROCESSOR_H
#define  SMCPROCESSOR_H
#include "Mesh.h"
#include "SMCTable.h"
#include "ImageVolume.h"
#define  MESH_INDEXTYPE int
class SMCTriangleNetHashTable
{
public:
	int CurrentLayerIndex;
private:
	int stx;
	int sty;
	int width;
	int height;
	std::vector<MESH_INDEXTYPE**> mapList;
	void Init2dArray(MESH_INDEXTYPE**& pointer,int width,int height,MESH_INDEXTYPE value)
	{
		pointer=new MESH_INDEXTYPE*[width];
		for(int i=0;i<width;i++)
		{
			pointer[i]=new MESH_INDEXTYPE[height];
			for(int j=0;j<height;j++)
			{
				pointer[i][j]=value;
			}
		}
	}
	void Release2dArray(MESH_INDEXTYPE**& pointer,int width,int height)
	{
		for(int i=0;i<width;i++)
		{
			delete[] pointer[i];
		}
		delete[] pointer;
	}
public:
	SMCTriangleNetHashTable(int minx,int miny,int width, int height)
	{
		this->stx = minx - 1;
		this->sty = miny - 1;
		this->width=width+2;
		this->height=height+2;
		mapList.reserve(2);
		MESH_INDEXTYPE** map1=NULL;
		MESH_INDEXTYPE** map2=NULL;
		Init2dArray(map1,this->width,this->height,-1);
		Init2dArray(map2,this->width,this->height,-1);
		mapList.push_back(map1);
		mapList.push_back(map2);
		SetDefaultValue(0);
		SetDefaultValue(1);
	}
	~SMCTriangleNetHashTable()
	{
		if(mapList[0]!=NULL)
			Release2dArray(mapList[0],width,height);
		if(mapList[1]!=NULL)
			Release2dArray(mapList[1],width,height);
		mapList[0]=NULL;
		mapList[1]=NULL;
	}
	void SetDefaultValue(int index0_1)
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				(mapList[index0_1])[i][j] = -1;
			}
		}
	}
	void IncreaseIndex()
	{
		CurrentLayerIndex++;
		SetDefaultValue(0);
		MESH_INDEXTYPE** temp=mapList[0];
		mapList[0] = mapList[1];
		mapList[1] = temp;
	}
	inline void SetHashValue(int x,int y,int z,MESH_INDEXTYPE value)
	{
		int index0_1 = z - CurrentLayerIndex;
		(mapList[index0_1])[x-stx][y-sty] = value;
	}
	inline MESH_INDEXTYPE GetHashValue(int x, int y, int z)
	{
		int index0_1 = z - CurrentLayerIndex;
		return (mapList[index0_1])[x-stx][y-sty];
	}
};
struct OriginalTriangle
{
	Int16Triple P0;
	Int16Triple P1;
	Int16Triple P2;
	OriginalTriangle(int p0x, int p0y, int p0z, int p1x, int p1y, int p1z, int p2x, int p2y, int p2z)
	{
		P0.X = p0x;
		P0.Y = p0y;
		P0.Z = p0z;
		P1.X = p1x;
		P1.Y = p1y;
		P1.Z = p1z;
		P2.X = p2x;
		P2.Y = p2y;
		P2.Z = p2z;
	}
	OriginalTriangle()
	{
	}
};
class SMCProcessor
{
private:
	ImageVolume_8bit& bmp;
	int d;
	int h;
	int w;
	int wh;
public:
	SMCProcessor(ImageVolume_8bit& bitmap):bmp(bitmap)
	{
		this->w=bitmap.Width;
		this->h=bitmap.Height;
		this->d=bitmap.Depth;
		this->wh=w*h;
	}
    Mesh* GenerateSurface()
	{
		Int16Triple temp[8];
		Mesh* m = new Mesh();
		OriginalTriangle tempTriangles[4];
		SMCTriangleNetHashTable hash(0, 0, w, h);
		hash.CurrentLayerIndex=-1;
		for (int k = -1; k <= d - 1; k++)
		{
			for (int j = -1; j <= h - 1; j++)
			{
				for (int i = -1; i <= w - 1; i++)
				{
					unsigned char value = GetConfig(temp, bmp, i, j, k);
					if (value == 0 || value == 255)
						continue;
					int tcount = ExtractTriangles(temp, value, i, j, k, tempTriangles);
					for (int tindex = 0; tindex < tcount; tindex++)
					{
						MergeTriangleIntoMesh(*m, hash, tempTriangles[tindex]);
					}
				}
			}
			hash.IncreaseIndex();
		}
		return m;
	}
protected:
	bool IsInSurface(int x,int y,int z)
	{
		 return bmp.Data[x + w * y + wh * z] == 255;
	}
private:
	bool InImage(int x,int y,int z)
	{
		return x < w && x >= 0&& y < h && y >= 0&& z < d && z >= 0;
	}
	unsigned char GetConfig(Int16Triple* temp, ImageVolume_8bit& flagsMap, int indexInWidth, int indexInHeight, int indexInDepth)
	{
		unsigned char value = 0;
		for (int pi = 0; pi < 8; pi++)
		{
			temp[pi].X = indexInWidth + SMCTable::PointIndexToPointDelta[pi].X;
			temp[pi].Y = indexInHeight +SMCTable::PointIndexToPointDelta[pi].Y;
			temp[pi].Z = indexInDepth + SMCTable::PointIndexToPointDelta[pi].Z;
			if (InImage(temp[pi].X ,temp[pi].Y ,temp[pi].Z)&&IsInSurface(temp[pi].X ,temp[pi].Y ,temp[pi].Z))
			{
				value |= SMCTable::PointIndexToPointDelta[pi].V;
			}
		}
		return value;
	}
	int ExtractTriangles(Int16Triple* temp, unsigned char value, int indexInWidth, int indexInHeight, int indexInDepth, OriginalTriangle* result)
	{
		int tcount = 0;
		if (SMCTable::TableFat[value][0] != -1)
		{
			int index = 0;
			while (SMCTable::TableFat[value][index] != -1)
			{
				Int16Triple t0 = temp[SMCTable::TableFat[value][index]];
				Int16Triple t1 = temp[SMCTable::TableFat[value][ index + 1]];
				Int16Triple t2 = temp[SMCTable::TableFat[value][index + 2]];
				result[tcount] = OriginalTriangle(t0.X, t0.Y, t0.Z, t1.X, t1.Y, t1.Z, t2.X, t2.Y, t2.Z);
				tcount++;
				index += 3;
			}
		}
		return tcount;
	}
	void MergeTriangleIntoMesh(Mesh& mesh, SMCTriangleNetHashTable& hashMap, OriginalTriangle& ot)
	{
		int p0x = ot.P0.X;
		int p0y = ot.P0.Y;
		int p0z = ot.P0.Z;
		int p1x = ot.P1.X;
		int p1y = ot.P1.Y;
		int p1z = ot.P1.Z;
		int p2x = ot.P2.X;
		int p2y = ot.P2.Y;
		int p2z = ot.P2.Z;
		int p0i;
		int p1i;
		int p2i;
		int index = 0;
		index = hashMap.GetHashValue(p0x, p0y, p0z);
		if (index == -1)
		{
			Point3d p(p0x, p0y, p0z);
			p0i = mesh.AddVertex(p);
			hashMap.SetHashValue(p0x, p0y, p0z, p0i);
		}
		else
		{
			p0i = index;
		}

		index = hashMap.GetHashValue(p1x, p1y, p1z);
		if (index == -1)
		{
			Point3d p(p1x, p1y, p1z);
			p1i = mesh.AddVertex(p);
			hashMap.SetHashValue(p1x, p1y, p1z, p1i);
		}
		else
		{
			p1i = index;
		}

		index = hashMap.GetHashValue(p2x, p2y, p2z);
		if (index == -1)
		{
			Point3d p(p2x, p2y, p2z);
			p2i = mesh.AddVertex(p);
			hashMap.SetHashValue(p2x, p2y, p2z, p2i);
		}
		else
		{
			p2i = index;
		}

		Triangle t(p0i, p1i, p2i);
		mesh.AddFace(t);
	}
};
#endif