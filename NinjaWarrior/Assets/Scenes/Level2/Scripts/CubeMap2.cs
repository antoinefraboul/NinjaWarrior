﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CubeMap2 : MonoBehaviour
{
    public NavMeshSurface m_surfaces;
    public GameObject m_shape;
    public GameObject[,] m_list;
    public int m_steps = 5;
    public List<Material> m_color;
    public float m_gape = 2;
    List<GameObject> m_path;


    void Start()
    {
        m_list = new GameObject[m_steps, m_color.Count];
        Queue<Material> shuffleColor = new Queue<Material>();
        
        for (int i = 0; i < m_steps; i++)
        {
            //reload color
            Shuffle(m_color);
            for (int r = 0; r <  m_color.Count; r++)
            {
                shuffleColor.Enqueue(m_color[r]);
            }
                
            for (int j = 0; j < m_color.Count; j++)
            {
                GameObject cgenerated = Instantiate(m_shape); 
                cgenerated.transform.parent = this.transform; //inside this game object
                cgenerated.transform.position = m_shape.transform.position + new Vector3(i * m_gape, 0, j * m_gape);
                cgenerated.GetComponent<Renderer>().material = shuffleColor.Dequeue();

                NavMeshLink north = cgenerated.AddComponent<NavMeshLink>();
                north.startPoint = new Vector3(-m_gape, 1, 0);
                north.endPoint = new Vector3(0, 1, 0);

                NavMeshLink east = cgenerated.AddComponent<NavMeshLink>();
                east.startPoint = new Vector3(0, 1, 0);
                east.endPoint = new Vector3(0, 1, m_gape);

                NavMeshLink ne = cgenerated.AddComponent<NavMeshLink>();
                ne.startPoint = new Vector3(0, 1, 0);
                ne.endPoint = new Vector3(m_gape, 1, m_gape);

                NavMeshLink nw = cgenerated.AddComponent<NavMeshLink>();
                nw.startPoint = new Vector3(0, 1, 0);
                nw.endPoint = new Vector3(m_gape, 1, -m_gape);

                m_list[i,j]=cgenerated;
            }
        }
        Destroy(m_shape);
        m_surfaces.BuildNavMesh();
        m_path = new List<GameObject>();
        makePath(m_path);

     
      
        
    }

 
    void Update()
    {
        
    }

    void Shuffle(List<Material> list)
    {
        for (var i = 0; i < list.Count-1; ++i)
        {
            var r = Random.Range(i, list.Count);
            var tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }

    void makePath(List<GameObject> path)
    {
        var index = Random.Range(0, m_color.Count);
        path.Add(m_list[0, index]); //initialize first index

        for (int i = 1; i < m_steps; i++)
        {
            var next_index=Random.Range(0, m_color.Count);

            //side
           var side = Random.Range(0, 100);

           if (side < 20)
            {
                //bounds
                if (index > 0 && index < m_color.Count-1)
                {
                    index += side < 10 ? 1 : -1;
                }
                else if (index + 1 < m_color.Count-1)
                {
                    index += 1;
                }
                else
                {
                    index -= 1;
                }
                i--;
                path.Add(m_list[i, index]);
            }
            else
            {
                //check if the index is in the front neighbourhood of the previous one
                if (next_index - index >= -1 && next_index - index <= 1)
                {
                    index = next_index;
                    path.Add(m_list[i, index]);
                    // Debug.Log(index + " " + m_list[i, index].GetComponent<Renderer>().material.name);
                }
                else
                {
                    i--;
                }
            }


        }

        for(int x=0;x<path.Count;x++)
            Debug.Log(x+": "+path[x].GetComponent<Renderer>().material.name);
    }
}