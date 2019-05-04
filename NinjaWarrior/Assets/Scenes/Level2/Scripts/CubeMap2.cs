using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CubeMap2 : MonoBehaviour
{
    public Camera m_cam;
    public NavMeshAgent m_agent;
    public NavMeshSurface m_surfaces;
    public GameObject m_shape;
    public GameObject[,] m_list;
    public int m_random_steps_max = 4;
    public int m_steps = 4;
    public List<Material> m_color;
    public float m_gape = 2;
    public GameObject m_UI;
    public List<GameObject> m_color_icons;
    public TextMeshProUGUI m_steps_text;
    [Range(0.0f, 100.0f)]  public float m_rate_side = 20;
    List<GameObject> m_path;
    int m_next=0;
    bool m_over = false;

    //TODO  : move cam with arrow or set focus on character
    void Start()
    {
        //Randomly improve number of steps
        m_steps+= Random.Range(0, m_random_steps_max);
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
        updateColorIcons();

        //Show all the cube map from the cam
        m_cam.orthographicSize += m_path.Count*1.4f;
    }

 
    void Update()
    {
        if (Time.timeScale > 0)
        {
            //UI
            m_UI.SetActive(true);
            //cam
            if (Input.GetMouseButton(1))
            {
                m_cam.transform.position -= m_cam.transform.right * Input.GetAxis("Mouse X");
                m_cam.transform.position -= m_cam.transform.up * Input.GetAxis("Mouse Y");
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && m_cam.orthographicSize-1 > 0) // forward
            {
                m_cam.orthographicSize--;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                m_cam.orthographicSize ++;
            }

            //Cube Map
            NavMeshHit target;
            foreach (GameObject o in m_list)
            {
                if (m_next < m_path.Count && !m_agent.Raycast(o.transform.position, out target))
                {
                    //On a cube of the map
                    if (m_path.Contains(o))
                    {
                        //Cube is part of the path
                        if (o.Equals(m_path[m_next]))
                        {
                            //Cube it the next cube to reach
                            //Debug.Log("reach: " + o.GetComponent<Renderer>().material);
                            m_next++;
                            updateColorIcons();
                            updatePlateform();

                            if (m_next == m_path.Count)
                            {
                                showPath();
                                FindObjectOfType<GameManager>().Invoke("Win", 1.5f);
                            }
                        }
                        else if (m_next > 0 && !o.Equals(m_path[m_next - 1]))
                        {
                            //not the next plateform reach
                            o.SetActive(false);
                            showPath();
                            Invoke("GameOver", 0.5f);
                        }
                    }
                    else
                    {
                        o.SetActive(false);
                        showPath();
                        Invoke("GameOver", 0.5f);

                    }
                }
            }
        }
        else
        {
            if(!m_over)
            m_UI.SetActive(false);
        }
    }


    void GameOver()
    {
        m_over = true;
        FindObjectOfType<GameManager>().GameOver();
    }

    void updatePlateform()
    {
        if (m_next > 0)
        {
            GameObject currentpos = m_path[m_next - 1];

            for (int i = 0; i < m_steps; i++)
            {
                for(int j=0; j < m_color.Count; j++)
                {
                    if (m_list[i, j].Equals(currentpos))
                    {
                        //disable behind
                        for (int x = 0; x < i; x++)
                        {
                            for (int y = 0; y < m_color.Count; y++)
                                m_list[x, y].SetActive(false);
                        }
                    }
                }
            }
        }
    }


    void updateColorIcons()
    {
        Queue<GameObject> q=new Queue<GameObject>();
        foreach (GameObject o in m_path)
            q.Enqueue(o);
        for (int i = 0; i < m_next; i++)
            q.Dequeue(); //enqueu already reach

        if (q.Count > 0)
        {
            foreach (GameObject o in m_color_icons)
            {
                if (q.Count > 0)
                {
                    o.GetComponent<Image>().color = q.Dequeue().GetComponent<Renderer>().material.color;
                }
                else
                {
                    o.SetActive(false);
                }
            }
            m_steps_text.SetText((m_path.Count - m_next).ToString());
        }
        else
        {
            m_UI.SetActive(false);
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
