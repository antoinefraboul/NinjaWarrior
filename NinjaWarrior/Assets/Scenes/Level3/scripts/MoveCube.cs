using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveCube : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshSurface m_surface;
    public Vector3 m_target = Vector3.one;
    public float m_speed = 1f;
    public bool m_loop=true;
    private Vector3 m_to;
  

    private Vector3 m_start;
    void Start()
    {
        m_start = this.transform.position;
        m_target += m_start;
        m_to = m_target;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_loop)
        {
            if (this.transform.position.Equals(m_start)) m_to = m_target; //forward
            if (this.transform.position.Equals(m_target)) m_to = m_start;  //backward
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, m_to, m_speed * Time.deltaTime);


        //m_surface.UpdateNavMesh(thi);





        /*
        if (!this.transform.position.Equals(m_to))
        {
            this.transform.Translate(m_speed * Time.deltaTime, 0, 0);

        }else if (m_loop)
        {

        }
        */
        

    }
}
