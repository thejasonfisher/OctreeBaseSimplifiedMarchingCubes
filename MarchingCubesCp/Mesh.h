#ifndef MESH_H
#define MESH_H
#include <vector>
#include <algorithm>
struct Int16Quad
{
public:
	int A;
	int B;
	int C;
	int D;
	Int16Quad(int a, int b, int c, int d):A(a),B(b),C(c),D(d)
	{
	}
};
struct Int16Double
{
public:
	int X;
	int Y;
	Int16Double(int x,int y):X(x),Y(y){}
	Int16Double():X(0),Y(0){}
};
struct Int16Triple
{
	int X;
	int Y;
	int Z;
	Int16Triple(int x,  int y,  int z):X(x),Y(y),Z(z){}
	Int16Triple():X(0),Y(0),Z(0){}
};
struct Int16TripleWithByte
{
	unsigned short X;
	unsigned short Y;
	unsigned short Z;
	unsigned char V;
	Int16TripleWithByte(unsigned short x, unsigned short y, unsigned short z, unsigned char v):X(x),Y(y),Z(z),V(v){}
	Int16TripleWithByte():X(0),Y(0),Z(0),V(0){}
};
struct Point3d
{
public:
	float X;
	float Y;
	float Z;
	Point3d():X(0),Y(0),Z(0)
	{
	}
	Point3d(float x, float y, float z):X(x),Y(y),Z(z)
	{
	}
};
struct Triangle
{
public :
	int P0Index;
	int P1Index;
	int P2Index;
	Triangle(int p0index, int p1index, int p2index):P0Index(p0index),P1Index(p1index),P2Index(p2index){}
	Triangle():P0Index(-1),P1Index(-1),P2Index(-1){}
};
class Mesh
{
public:
	std::vector<Point3d> Vertices;
	std::vector<Triangle> Faces;
	Mesh()
	{
	}
	~Mesh()
	{
	}
	int AddVertex(Point3d& toAdd)
	{
		int index = Vertices.size();
		Vertices.push_back(toAdd);
		return index;
	}
	int AddFace(Triangle& tri)
	{
		int index = Faces.size();
		Faces.push_back(tri);
		return index;
	}
};
class PlyManager
{
public:
	static void Output(Mesh& mesh,const char* filename);
	static void OutputEx(Mesh& mesh,const char* filename);
	static void ReadFile(Mesh& mesh,const char* fileName);
	static void ReadFileEx(Mesh& mesh,const char* fileName);
private:
	static void AWriteV(FILE* sw, double v1, double v2, double v3,unsigned char r,unsigned char g,unsigned char b);
	static void AWriteF(FILE* sw, int i1, int i2, int i3);
};
void PlyManager::AWriteV(FILE* file, double v1, double v2, double v3,unsigned char r,unsigned char g,unsigned char b)
{
	fprintf(file,"%.2f %.2f %.2f %d %d %d\n",v1,v2,v3,r,g,b);
}
void PlyManager::AWriteF(FILE* file, int i1, int i2, int i3)
{
	fprintf(file,"%d %d %d %d\n",3,i1,i2,i3);
}
void PlyManager::ReadFile(Mesh& mesh,const char* fileName)
{
	int vcount=0;
	int fcount=0;
	FILE * nfile = fopen(fileName,"r");
	fscanf(nfile,"ply\nformat ascii 1.0\ncomment VCGLIB generated\nelement vertex %d\n",&vcount);
	fscanf(nfile,"property float x\nproperty float y\nproperty float z\nproperty uchar red\nproperty uchar green\nproperty uchar blue\nelement face %d\n",&fcount);
	fscanf(nfile,"property list int int vertex_indices\nend_header\n");
	float v1=0,v2=0,v3=0;
	int r=0,g=0,b=0;
	int i1=0,i2=0,i3=0;
	for(int i=0;i<vcount;i++)
	{
		fscanf(nfile,"%f %f %f %d %d %d\n",&v1,&v2,&v3,&r,&g,&b);
		Point3d p3d(v1,v2,v3);
		mesh.AddVertex(p3d);
	}
	for(int j=0;j<fcount;j++)
	{
		fscanf(nfile,"3 %d %d %d\n",&i1,&i2,&i3);
		Triangle t(i1,i2,i3);
		mesh.AddFace(t);
	}
	fclose(nfile);
}
void PlyManager::Output(Mesh& mesh,const char* filename)
{
	FILE * nfile = fopen(filename,"wb");
	fprintf(nfile,"ply\n");
	fprintf(nfile,"format ascii 1.0\n");
	fprintf(nfile,"comment VCGLIB generated\n");
	fprintf(nfile,"element vertex %d\n",mesh.Vertices.size());
	fprintf(nfile,"property float x\n");
	fprintf(nfile,"property float y\n");
	fprintf(nfile,"property float z\n");
	fprintf(nfile,"property uchar red\n");
	fprintf(nfile,"property uchar green\n");
	fprintf(nfile,"property uchar blue\n");
	fprintf(nfile,"element face %d\n",mesh.Faces.size());
	fprintf(nfile,"property list int int vertex_indices\n");
	fprintf(nfile,"end_header\n");
	for(size_t i=0;i<mesh.Vertices.size();i++)
	{
		AWriteV(nfile,mesh.Vertices[i].X,mesh.Vertices[i].Y,mesh.Vertices[i].Z,255,255,255);
	}
	for(size_t i=0;i<mesh.Faces.size();i++)
	{
		AWriteF(nfile,mesh.Faces[i].P0Index,mesh.Faces[i].P1Index,mesh.Faces[i].P2Index);
	}
	fclose(nfile);
}
#endif