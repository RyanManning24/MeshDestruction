using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTriangle : MonoBehaviour
{
    List<Vector3> vertices = new List<Vector3>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    int subMeshIndex;

    public List<Vector3> Vertices { get { return vertices; } set { vertices = value; } }
    public List<Vector3> Normals { get { return normals; } set { normals = value; } }
    public List<Vector2> UVs { get { return uvs; } set { uvs = value; } }
    public int SubMeshIndex {  get { return subMeshIndex; } set { SubMeshIndex = value; } }

    public MeshTriangle(Vector3[] _vertices, Vector3[] _normals, Vector2[] _uvs, int _submeshIndex)
    {
        Clear();

        Vertices.AddRange(_vertices);
        Normals.AddRange(_normals);
        UVs.AddRange(_uvs);

        subMeshIndex = _submeshIndex;
    }

    public void Clear()
    {
        vertices.Clear();
        normals.Clear();
        uvs.Clear();

        subMeshIndex = 0;
    }



}
