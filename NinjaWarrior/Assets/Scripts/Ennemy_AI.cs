using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Ennemy_AI : MonoBehaviour
{

    public NavMeshAgent agent;

    public ThirdPersonCharacter character;

    public GameObject target;

    bool init= false;
    bool regularSpeed;
    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        regularSpeed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (init)
        {
            agent.SetDestination(target.transform.position);

            if (agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity, false, true);
            }
            else
            {
                character.Move(Vector3.zero, false, true);
            }
        }
        
    }

    public void initAI()
    {
        init = true;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "speed_down" && regularSpeed)
        {
            agent.speed = agent.speed/3;
            regularSpeed = false;
        }
    }

}
