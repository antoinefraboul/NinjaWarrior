using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navmeshmanager : MonoBehaviour
{
    public NavMeshSurface m_surface;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_surface.BuildNavMesh();
    }
}
