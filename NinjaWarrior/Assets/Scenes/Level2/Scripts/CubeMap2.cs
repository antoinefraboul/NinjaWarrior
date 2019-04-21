using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CubeMap2 : MonoBehaviour
{
    public Camera m_cam;
    public NavMeshAgent m_agent;
    public NavMeshSurface m_surfaces;
    public GameObject m_shape;
    public GameObject[,] m_list;
    public int m_steps = 5;
    public List<Material> m_color;
    public float m_gape = 2;
    [Range(0.0f, 100.0f)]  public float m_rate_side = 20;
    List<GameObject> m_path;
    int m_current_path=0;


    //TODO  : move cam with arrow or set focus on character
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

        for (int x = 0; x < m_path.Count; x++)
            Debug.Log(x + ": " + m_path[x].GetComponent<Renderer>().material.name);
    }

 
    void Update()
    {
        NavMeshHit target;
        foreach (GameObject o in m_list)
        {
            if (m_current_path < m_path.Count && !m_agent.Raycast(o.transform.position, out target))
            {
                //On a cube of the map
                if (m_path.Contains(o))
                {
                    //Cube is part of the path
                    if (o.Equals(m_path[m_current_path]))
                    {
                        //Cube it the next cube to reach
                        Debug.Log("reach: " + o.GetComponent<Renderer>().material);
                        m_current_path++;
                        if (m_current_path == m_path.Count)
                        {
                            Debug.Log("win");
                            showPath();
                        }
                    }
                    else if(m_current_path>0 && !o.Equals(m_path[m_current_path-1]))
                    {
                        //not the next plateform reach
                        o.SetActive(false); 
                        Debug.Log("lose");
                        showPath();
                    }
                }
                else
                {
                    o.SetActive(false);
                    Debug.Log("lose2");
                    showPath();
                }             
            }
        }
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


    void showPath()
    {
        foreach(GameObject  temp in m_list)
        {
            temp.SetActive(m_path.Contains(temp));
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
            float side = Random.Range(0.0f, 100.0f);
            if (side < m_rate_side)
            {
                next_index = index;
                //bounds
                if (index > 0 && index < m_color.Count - 1)
                {
                    index += side < m_rate_side / 2 ? 1 : -1;
                }
                else if (index + 1 < m_color.Count - 1)
                {
                    index += 1;
                }
                else
                {
                    index -= 1;
                }
                i--;
                if (!m_path.Contains(m_list[i,index]))//if plateform is already in the path ignore
                {
                    path.Add(m_list[i, index]);
                }
                else
                {
                    index = next_index;
                }
            }
            else
            {
                //check if the index is in the front neighbourhood of the previous one
                if (next_index - index >= -1 && next_index - index <= 1)
                {
                    index = next_index;
                    path.Add(m_list[i, index]);
                }
                else
                {
                    i--;
                }
            }
        }
    }
}
