#ifndef MCPROCESSOR_H
#define  MCPROCESSOR_H
#include "Mesh.h"
#include "MCTable.h"
#include "ImageVolume.h"
#define  MESH_INDEXTYPE int
class MCTriangleNetHashTable
{
public:
	int CurrentLayerIndex;
private:
	int stx;
	int sty;
	int width;
	int height;
	std::vector<MESH_INDEXTYPE***> mapList;
	void Init3dArray(MESH_INDEXTYPE***& pointer,int width,int height,int depth,MESH_INDEXTYPE value)
	{
		pointer=new MESH_INDEXTYPE**[width];
		for(int i=0;i<width;i++)
		{
			pointer[i]=new MESH_INDEXTYPE*[height];
			for(int j=0;j<height;j++)
			{
				pointer[i][j]=new MESH_INDEXTYPE[3];
				for(int k=0;k<depth;k++)
				{
					pointer[i][j][k]=value;
				}
			}
		}
	}
	void Release3dArray(MESH_INDEXTYPE***& pointer,int width,int height,int depth)
	{
		for(int i=0;i<width;i++)
		{
			for(int j=0;j<height;j++)
				delete[] pointer[i][j];
			delete[] pointer[i];
		}
		delete[] pointer;
	}
public:
	MCTriangleNetHashTable(int minx,int miny,int width, int height)
	{
		this->stx = minx - 1;
		this->sty = miny - 1;
		this->width=width+2;
		this->height=height+2;
		mapList.reserve(2);
		MESH_INDEXTYPE*** map1=NULL;
		MESH_INDEXTYPE*** map2=NULL;
		Init3dArray(map1,this->width,this->height,3,-1);
		Init3dArray(map2,this->width,this->height,3,-1);
		mapList.push_back(map1);
		mapList.push_back(map2);
		SetDefaultValue(0);
		SetDefaultValue(1);
	}
	~MCTriangleNetHashTable()
	{
		if(mapList[0]!=NULL)
			Release3dArray(mapList[0],width,height,3);
		if(mapList[1]!=NULL)
			Release3dArray(mapList[1],width,height,3);
		mapList[0]=NULL;
		mapList[1]=NULL;
	}
	void SetDefaultValue(int index0_1)
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				(mapList[index0_1])[i][j][0] = -1;
				(mapList[index0_1])[i][j][1] = -1;
				(mapList[index0_1])[i][j][2] = -1;
			}
		}
	}
	void IncreaseIndex()
	{
		CurrentLayerIndex++;
		SetDefaultValue(0);
		MESH_INDEXTYPE*** temp=mapList[0];
		mapList[0] = mapList[1];
		mapList[1] = temp;
	}
	inline void SetHashValue(int x,int y,int z,int d,MESH_INDEXTYPE value)
	{
		int index0_1 = z - CurrentLayerIndex;
		(mapList[index0_1])[x-stx][y-sty][d] = value;
	}
	inline MESH_INDEXTYPE GetHashValue(int x, int y, int z,int d)
	{
		int index0_1 = z - CurrentLayerIndex;
		return (mapList[index0_1])[x-stx][y-sty][d];
	}
};
struct OriginalTriangle
{
public:
	Int16Triple CellCoord;
	int E0;
	int E1;
	int E2;
	OriginalTriangle(int x, int y, int z, int ei0, int ei1, int ei2):CellCoord(x,y,z),E0(ei0),E1(ei1),E2(ei2)
	{
	}
	OriginalTriangle():CellCoord(-1,-1,-1),E0(-1),E1(-1),E2(-1)
	{
	}
};
class MCProcessor
{
private:
	ImageVolume_8bit& bmp;
	int d;
	int h;
	int w;
	int wh;
public:
	MCProcessor(ImageVolume_8bit& bitmap):bmp(bitmap)
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
		MCTriangleNetHashTable hash(0, 0, w, h);
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
	Point3d GetIntersetedPoint(int cx, int cy, int cz, int ei)
	{
		int p0i = MCTable::EdgeIndexToEdgeVertexIndex[ei][0];
		int p1i = MCTable::EdgeIndexToEdgeVertexIndex[ei][1];

		int p0X = cx + MCTable::PointIndexToPointDelta[p0i].X;
		int p0Y = cy + MCTable::PointIndexToPointDelta[p0i].Y;
		int p0Z = cz + MCTable::PointIndexToPointDelta[p0i].Z;

		int p1X = cx + MCTable::PointIndexToPointDelta[p1i].X;
		int p1Y = cy + MCTable::PointIndexToPointDelta[p1i].Y;
		int p1Z = cz + MCTable::PointIndexToPointDelta[p1i].Z;

		return Point3d((p0X + p1X) / 2.0f, (p0Y + p1Y) / 2.0f, (p0Z + p1Z) / 2.0f);
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
			temp[pi].X = indexInWidth + MCTable::PointIndexToPointDelta[pi].X;
			temp[pi].Y = indexInHeight +MCTable::PointIndexToPointDelta[pi].Y;
			temp[pi].Z = indexInDepth + MCTable::PointIndexToPointDelta[pi].Z;
			if (InImage(temp[pi].X ,temp[pi].Y ,temp[pi].Z)&&IsInSurface(temp[pi].X ,temp[pi].Y ,temp[pi].Z))
			{
				value |= MCTable::PointIndexToPointDelta[pi].V;
			}
		}
		return value;
	}
	int ExtractTriangles(Int16Triple* temp, unsigned char value, int indexInWidth, int indexInHeight, int indexInDepth, OriginalTriangle* result)
	{
		int tcount = 0;
		if (MCTable::Table[value][0] != -1)
		{
			int index = 0;
			while (MCTable::Table[value][index] != -1)
			{
				int e0index = MCTable::Table[value][index];
				int e1index = MCTable::Table[value][index + 1];
				int e2index = MCTable::Table[value][index + 2];
				result[tcount] = OriginalTriangle(indexInWidth, indexInHeight, indexInDepth, e0index, e1index, e2index);
				tcount++;
				index += 3;
			}
		}
		return tcount;
	}
	void MergeTriangleIntoMesh(Mesh& mesh, MCTriangleNetHashTable& hashMap, OriginalTriangle& ot)
	{
		int e0i = CubeEdgeMapTable[ot.E0].D;
		int p0x = ot.CellCoord.X + CubeEdgeMapTable[ot.E0].A;
		int p0y = ot.CellCoord.Y + CubeEdgeMapTable[ot.E0].B;
		int p0z = ot.CellCoord.Z + CubeEdgeMapTable[ot.E0].C;


		int e1i = CubeEdgeMapTable[ot.E1].D;
		int p1x = ot.CellCoord.X + CubeEdgeMapTable[ot.E1].A;
		int p1y = ot.CellCoord.Y + CubeEdgeMapTable[ot.E1].B;
		int p1z = ot.CellCoord.Z + CubeEdgeMapTable[ot.E1].C;


		int e2i = CubeEdgeMapTable[ot.E2].D;
		int p2x = ot.CellCoord.X + CubeEdgeMapTable[ot.E2].A;
		int p2y = ot.CellCoord.Y + CubeEdgeMapTable[ot.E2].B;
		int p2z = ot.CellCoord.Z + CubeEdgeMapTable[ot.E2].C;


		int p0i;
		int p1i;
		int p2i;
		int index = 0;
		index = hashMap.GetHashValue(p0x, p0y, p0z, e0i);
		if (index == -1)
		{
			Point3d interp = GetIntersetedPoint(ot.CellCoord.X, ot.CellCoord.Y, ot.CellCoord.Z, ot.E0);
			p0i = mesh.AddVertex(interp);
			hashMap.SetHashValue(p0x, p0y, p0z, e0i, p0i);
		}
		else
		{
			p0i = index;
		}

		index = hashMap.GetHashValue(p1x, p1y, p1z, e1i);
		if (index == -1)
		{
			Point3d interp = GetIntersetedPoint(ot.CellCoord.X, ot.CellCoord.Y, ot.CellCoord.Z, ot.E1);
			p1i = mesh.AddVertex(interp);
			hashMap.SetHashValue(p1x, p1y, p1z, e1i, p1i);
		}
		else
		{
			p1i = index;
		}

		index = hashMap.GetHashValue(p2x, p2y, p2z, e2i);
		if (index == -1)
		{
			Point3d interp = GetIntersetedPoint(ot.CellCoord.X, ot.CellCoord.Y, ot.CellCoord.Z, ot.E2);
			p2i = mesh.AddVertex(interp);
			hashMap.SetHashValue(p2x, p2y, p2z, e2i, p2i);
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