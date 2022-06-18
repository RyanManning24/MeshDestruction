using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    public static bool currentlyCutting;
    public static Mesh originalMesh;

    public static void Cut(GameObject originalGameobject, Vector3 contactPoint, Vector3 direction, 
        Material cutMaterial = null, bool fill = true, bool addRigidBody = false)
    {
        if(currentlyCutting)
        {
            return;
        }

        currentlyCutting = true;

        Plane plane = new Plane(originalGameobject.transform.InverseTransformDirection(-direction),
            originalGameobject.transform.InverseTransformDirection(contactPoint));
        originalMesh = originalGameobject.GetComponent<MeshFilter>().mesh;
        List<Vector3> addedVertices = new List<Vector3>();

        GeneratedMesh leftMesh = new GeneratedMesh();
        GeneratedMesh rightMesh = new GeneratedMesh();

        int[] submeshIndices;
        int triangleIndexA, triangleIndexB, triangleIndexC;

        for(int i = 0; i < originalMesh.subMeshCount; i++)
        {
            submeshIndices = originalMesh.GetTriangles(i);
            
            for(int j = 0; j < submeshIndices.Length; j+=3)
            {
                triangleIndexA = submeshIndices[j];
                triangleIndexB = submeshIndices[j + 1];
                triangleIndexC = submeshIndices[j + 2];

                //MeshTriangle currentTriangle = GetTriangle(triangleIndexA, triangleIndexB,triangleIndexC);

                bool triangleALeftSide = plane.GetSide(originalMesh.vertices[triangleIndexA]);
                bool triangleBLeftSide = plane.GetSide(originalMesh.vertices[triangleIndexB]);
                bool triangleCLeftSide = plane.GetSide(originalMesh.vertices[triangleIndexC]);

                if(triangleALeftSide && triangleBLeftSide && triangleCLeftSide)
                {
                    //leftMesh.AddTriangle(currentTriangle);
                }
                else if(!triangleALeftSide && !triangleBLeftSide && !triangleCLeftSide)
                {
                    //rightMesh.AddTriangle(currentTriangle);
                }
                else
                {
                    //CutTriangle(plane, currentTriangle, triangleALeftSide,triangleBLeftSide,
                       // triangleCLeftSide,leftMesh,rightMesh,addedVertices);
                }
            
            }
        }

    }

    private static void CutTriangle(Plane _plane, MeshTriangle _triangle, bool _trinagleALeftSide, bool _triangleBLeftSide,
        bool _triangleCLeftSide, GeneratedMesh _leftSide, GeneratedMesh _rightSide, List<Vector3> addedVertices)
    {
        List<bool> leftSide = new List<bool>();
        leftSide.Add(_trinagleALeftSide);
        leftSide.Add(_triangleBLeftSide);
        leftSide.Add(_triangleCLeftSide);

        MeshTriangle leftMeshTriangle = new MeshTriangle(new Vector3[2], new Vector3[2], new Vector2[2], _triangle.SubMeshIndex);
        MeshTriangle rightMeshTriangle = new MeshTriangle(new Vector3[2], new Vector3[2], new Vector2[2], _triangle.SubMeshIndex);

        bool left = false;
        bool right = false;

        for (int i = 0; i < 3; i++)
        {
            if(leftSide[i])
            {
                if(!left)
                {
                    left = true;

                    leftMeshTriangle.Vertices[0] = _triangle.Vertices[i];
                    leftMeshTriangle.Vertices[1] = leftMeshTriangle.Vertices[0];

                    leftMeshTriangle.UVs[0] = _triangle.UVs[i];
                    leftMeshTriangle.UVs[1] = leftMeshTriangle.UVs[0];

                    leftMeshTriangle.Normals[0] = _triangle.Normals[i];
                    leftMeshTriangle.Normals[1] = leftMeshTriangle.Normals[0];
                }
                else
                {
                    leftMeshTriangle.Vertices[1] = _triangle.Vertices[i];
                    leftMeshTriangle.Normals[1] = _triangle.Normals[i];
                    leftMeshTriangle.UVs[1] = _triangle.UVs[i];
                }
            }
            else
            {
                if(!right)
                {
                    right = true;

                    rightMeshTriangle.Vertices[0] = _triangle.Vertices[i];
                    rightMeshTriangle.Vertices[1] = rightMeshTriangle.Vertices[0];

                    rightMeshTriangle.UVs[0] = _triangle.UVs[i];
                    rightMeshTriangle.UVs[1] = rightMeshTriangle.UVs[0];

                    rightMeshTriangle.Normals[0] = _triangle.Normals[i];
                    rightMeshTriangle.Normals[1] = rightMeshTriangle.Normals[0];
                }
                else
                {
                    rightMeshTriangle.Vertices[1] = _triangle.Vertices[i];
                    rightMeshTriangle.Normals[1] = _triangle.Normals[i];
                    rightMeshTriangle.UVs[1] = _triangle.UVs[i];
                }
            }
        }

        float normalizeDistance;
        float distance;
        _plane.Raycast(new Ray(leftMeshTriangle.Vertices[0], (rightMeshTriangle.Vertices[0] - leftMeshTriangle.Vertices[0]).normalized), out distance);

        normalizeDistance = distance / (rightMeshTriangle.Vertices[0] - leftMeshTriangle.Vertices[0]).magnitude;
        Vector3 vertLeft = Vector3.Lerp(leftMeshTriangle.Vertices[0], rightMeshTriangle.Vertices[0], normalizeDistance);
        addedVertices.Add(vertLeft);

        Vector3 normalLeft = Vector3.Lerp(leftMeshTriangle.Normals[0], rightMeshTriangle.Normals[0], normalizeDistance);
        Vector2 uvLeft = Vector2.Lerp(leftMeshTriangle.UVs[0], rightMeshTriangle.UVs[0], normalizeDistance);

        _plane.Raycast(new Ray(leftMeshTriangle.Vertices[1], (rightMeshTriangle.Vertices[1] - leftMeshTriangle.Vertices[1]).normalized), out distance);

        normalizeDistance = distance / (rightMeshTriangle.Vertices[1] - leftMeshTriangle.Vertices[1]).magnitude;
        Vector3 vertRight = Vector3.Lerp(leftMeshTriangle.Vertices[1], rightMeshTriangle.Vertices[1], normalizeDistance);
        addedVertices.Add(vertRight);

        Vector3 normalRight = Vector3.Lerp(leftMeshTriangle.Normals[1], rightMeshTriangle.Normals[1], normalizeDistance);
        Vector2 uvRight = Vector2.Lerp(leftMeshTriangle.UVs[1], rightMeshTriangle.UVs[1], normalizeDistance);


        MeshTriangle currentTriangle;
        Vector3[] updatedVertices = new Vector3[] { leftMeshTriangle.Vertices[0], vertLeft, vertRight };
        Vector3[] updatedNormals = new Vector3[] { leftMeshTriangle.Normals[0], normalLeft, normalRight };
        Vector2[] updatedUVs = new Vector2[] { leftMeshTriangle.UVs[0], uvLeft, uvRight };

        currentTriangle = new MeshTriangle(updatedVertices, updatedNormals, updatedUVs, _triangle.SubMeshIndex);

        if(updatedVertices[0] != updatedVertices[1] && updatedVertices[0] != updatedVertices[2])
        {
            if(Vector3.Dot(Vector3.Cross(updatedVertices[1] - updatedVertices[0], updatedVertices[2] - updatedVertices[0]), updatedNormals[0]) < 0)
            {
                //FlipTriangle(currentTriangle);
            }
            _leftSide.AddTriangle(currentTriangle);
        }

        updatedVertices = new Vector3[] { leftMeshTriangle.Vertices[0], leftMeshTriangle.Vertices[1], vertRight };
        updatedNormals = new Vector3[] { leftMeshTriangle.Normals[0], leftMeshTriangle.Normals[1], normalRight };
        updatedUVs = new Vector2[] { leftMeshTriangle.UVs[0], leftMeshTriangle.UVs[1], uvRight };

        if(updatedVertices[0] != updatedVertices[1] && updatedVertices[0] != updatedVertices[2])
        {
            if(Vector3.Dot(Vector3.Cross(updatedVertices[1] - updatedVertices[0], updatedVertices[2] - updatedVertices[0] ), updatedNormals[0]) < 0)
            {
                //FlipTriangle(currentTriangle);
            }
            _leftSide.AddTriangle(currentTriangle);
        }

    }

    public static void FillCut(List<Vector3> addedVertices, Plane _plane, GeneratedMesh _leftMesh, GeneratedMesh _rightMesh)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> polygone = new List<Vector3>();

        for(int i = 0; i < addedVertices.Count; i++)
        {
            if(!vertices.Contains(addedVertices[i]))
            {
                polygone.Clear();
                polygone.Add(addedVertices[i]);
                polygone.Add(addedVertices[i + 1]);

                vertices.Add(addedVertices[i]);
                vertices.Add(addedVertices[i + 1]);

                EvaluatePairs(addedVertices, vertices, polygone);
                //Fill(polygone, _plane, _leftMesh, _rightMesh);
            }
        }


    }

    public static void EvaluatePairs(List<Vector3> _addedVertices, List<Vector3> _vertices, List<Vector3> _polygone)
    {
        bool isDone = false;
        while(!isDone)
        {
            isDone = true;
            for(int i = 0; i < _addedVertices.Count; i+=2)
            {
                if (_addedVertices[i] == _polygone[_polygone.Count - 1] && !_vertices.Contains(_addedVertices[i + 1]))
                {
                    isDone = false;
                    _polygone.Add(_addedVertices[i + 1]);
                    _vertices.Add(_addedVertices[i + 1]);
                }
                else if (_addedVertices[i + 1] == _polygone[_polygone.Count - 1] && !_vertices.Contains(_addedVertices[i]))
                {
                    isDone = false;
                    _polygone.Add(_addedVertices[i]);
                    _polygone.Add(_addedVertices[i]);
                }
            }
        }
    }

    public static void Fill(List<Vector3> _vertices, Plane _plane, GeneratedMesh _leftMesh, GeneratedMesh _rightMesh)
    {
        Vector3 centerPosition = Vector3.zero;
        for(int i = 0; i < _vertices.Count; i++)
        {
            centerPosition += _vertices[i];
        }
        centerPosition = centerPosition / _vertices.Count;

        Vector3 up = new Vector3()
        {
            x = _plane.normal.x,
            y = _plane.normal.y,
            z = _plane.normal.z
        };

        Vector3 left = Vector3.Cross(_plane.normal, _plane.normal);

        Vector3 displacement = Vector3.zero;
        Vector2 uv1 = Vector2.zero;
        Vector2 uv2 = Vector2.zero;

        for(int i = 0; i < _vertices.Count; i++)
        {
            displacement = _vertices[i] - centerPosition;
            uv1 = new Vector2()
            {
                x = .5f + Vector3.Dot(displacement, left),
                y = .5f + Vector3.Dot(displacement, up)
            };

            Vector3[] vertices = new Vector3[] { _vertices[i], _vertices[(i + 1) % _vertices.Count], centerPosition };
            Vector3[] normals = new Vector3[] { -_plane.normal, -_plane.normal, -_plane.normal };
            Vector2[] uvs = new Vector2[] { uv1, uv2, new Vector2(0.5f, 0.5f) };

            MeshTriangle currentTriangle = new MeshTriangle(vertices, normals, uvs, originalMesh.subMeshCount + 1);

            if(Vector3.Dot(Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]) , normals[0]) < 0)
            {
                //FlipTriangle(currentTriangle);
            }
            _leftMesh.AddTriangle(currentTriangle);

            normals = new Vector3[] { _plane.normal, _plane.normal, _plane.normal };
            currentTriangle = new MeshTriangle(vertices, normals, uvs, originalMesh.subMeshCount + 1);

            if(Vector3.Dot(Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]), normals[0]) < 0)
            {
                //FlipTriangle(currentTriangle);
            }
            _rightMesh.AddTriangle(currentTriangle);
        }

    }



}
