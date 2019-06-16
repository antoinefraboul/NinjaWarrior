using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoBehaviour
{
    public Camera m_cam;
    public NavMeshAgent m_agent;
    public ThirdPersonCharacter m_character;
    public GameObject m_target;
    public bool m_reach_target=true;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        m_agent.updateRotation = false;
        if (m_reach_target)
        {
            m_agent.SetDestination(m_target.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_reach_target)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit target;

                if (Physics.Raycast(ray, out target))
                {
                    //Move agent to target
                    m_agent.SetDestination(target.point);
                }
            }
        }

        if (Time.timeScale > 0)
        {

            if (m_agent.remainingDistance > m_agent.stoppingDistance)
            {
                m_character.Move(m_agent.desiredVelocity, false, false);
            }
            else
            {
                m_character.Move(Vector3.zero, false, false);
            }
        }
        else
        {
            //stop character

            m_character.Move(Vector3.zero, false, false);
        }
    }
}
