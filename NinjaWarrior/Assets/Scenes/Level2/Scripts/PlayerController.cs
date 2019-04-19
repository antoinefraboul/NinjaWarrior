﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoBehaviour
{
    public Camera m_cam;
    public NavMeshAgent m_agent;
    public ThirdPersonCharacter m_character;
    // Start is called before the first frame update
    void Start()
    {
        m_agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit target;

            if(Physics.Raycast(ray,out target))
            {
                //Move agent to target
                m_agent.SetDestination(target.point);
            }
        }

        if (m_agent.remainingDistance > m_agent.stoppingDistance)
        {
            m_character.Move(m_agent.desiredVelocity, false, false);

        }
        else
        {
            m_character.Move(Vector3.zero,false,false);
        }
    }
}