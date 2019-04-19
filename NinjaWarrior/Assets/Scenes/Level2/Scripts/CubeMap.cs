using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;


public class CubeMap : MonoBehaviour
{
    public List<NavMeshSurface> m_surfaces;
    public Cube m_c;
    public int m_steps = 5;
    public int m_elements=5;
    public float m_gape = 2;
    Cube[] m_platforms;
    public GameObject test;
    void Start()
    {
        for (int i = 0; i < m_steps; i++)
        {
           for(int j=0; j< m_elements; j++)
           {
                Cube cgenerated = Instantiate(m_c);
                cgenerated.transform.parent = this.transform; //inside this game object
                cgenerated.transform.position = m_c.transform.position+new Vector3(i*m_gape,0,j*m_gape);
           }
        }
        m_c.enabled = false;
        GameObject t =Instantiate(test);
        t.transform.Translate(new Vector3(-7, 0, 0));
        //update nav mesh
        for (int i = 0; i < m_surfaces.Capacity; i++)
        {
            m_surfaces[i].BuildNavMesh();
         
            Debug.Log("update navmesh");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

