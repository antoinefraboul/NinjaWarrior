using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    Mesh m_mesh;
    Vector3[] m_vertices;
    int[] m_triangles;

    public Vector3 m_offset = Vector3.zero, m_size = new Vector3(1,1,1);

    void Start()
    {
        CreateShape(m_offset,m_size);
        UpdateMesh();
    }
    
    void CreateShape(Vector3 offset, Vector3 size)
    {
        m_vertices = new Vector3[]
        {
            new Vector3 (0, 0, 0),
            new Vector3 (1, 0, 0),
            new Vector3 (1, 1, 0),
            new Vector3 (0, 1, 0),
            new Vector3 (0, 1, 1),
            new Vector3 (1, 1, 1),
            new Vector3 (1, 0, 1),
            new Vector3 (0, 0, 1),
        };
        /*

        for (int i = 0; i < m_vertices.Length; i++)
        {
            m_vertices[i].Scale(size);
            m_vertices[i] += offset;
        }*/

        m_triangles = new int[]
        {
            0, 2, 1, //face front
		    0, 3, 2,
            2, 3, 4, //face top
		    2, 4, 5,
            1, 2, 5, //face right
		    1, 5, 6,
            0, 7, 4, //face left
		    0, 4, 3,
            5, 4, 7, //face back
		    5, 7, 6,
            0, 6, 7, //face bottom
		    0, 1, 6
        };
    }

    void UpdateMesh()
    {
        m_mesh = GetComponent<MeshFilter>().mesh;

        m_mesh.Clear();
        m_mesh.vertices = m_vertices;
        m_mesh.triangles = m_triangles;
        m_mesh.Optimize();
        m_mesh.RecalculateNormals();
    }
}
