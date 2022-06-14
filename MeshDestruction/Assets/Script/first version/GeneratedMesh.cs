using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedMesh : MonoBehaviour
{
    List<Vector3> vertices = new List<Vector3>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    List<List<int>> submeshIndices = new List<List<int>>();

    public List<Vector3> Vertices { get { return vertices; } set { vertices = value; } }
    public List<Vector3> Normals { get { return normals; } set { normals = value; } }
    public List<Vector2> UVs { get { return uvs; } set { uvs = value; } }
    public List<List<int>> SubMeshIndicies { get { return submeshIndices; }  set { submeshIndices = value; } }

    public void AddTriangle(MeshTriangle meshTriangle)
    {
        int currentVerticeCount = vertices.Count;

        vertices.AddRange(meshTriangle.Vertices);
        normals.AddRange(meshTriangle.Normals);
        uvs.AddRange(meshTriangle.UVs);

        if(submeshIndices.Count < meshTriangle.SubMeshIndex + 1)
        {
            for(int i = submeshIndices.Count; i < meshTriangle.SubMeshIndex + 1; i++)
            {
                submeshIndices.Add(new List<int>());
            }
        }

        for (int i = 0; i < 3; i++)
        {
            submeshIndices[meshTriangle.SubMeshIndex].Add(currentVerticeCount + i);
        }
    }
}
