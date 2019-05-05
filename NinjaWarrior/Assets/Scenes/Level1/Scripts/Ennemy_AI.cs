using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Ennemy_AI : MonoBehaviour
{
    public GameController gameController;

    public NavMeshAgent agent;

    public ThirdPersonCharacter character;

    public GameObject target;

    public GameManager gameManager;

    bool init= false;
    bool regularSpeed;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<GameManager>().Resume();
        FindObjectOfType<OptionsManager>().PlaySound("bgm_action_5");
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

    void GameOver()
    {
        //m_over = true;
        FindObjectOfType<GameManager>().GameOver();
    }

    public void initAI()
    {
        init = true;
    }

    public void resetAI()
    {
        init = false;
    }

    void OnCollisionEnter(Collision col)
    {
        string tag = col.gameObject.tag;
        if (tag == "speed_down" && regularSpeed)
        {
            agent.speed = agent.speed/3;
            regularSpeed = false;
        }

        if (tag == "win")
        {
            gameManager.Invoke("Win", 0);
        }

        if (tag == "IA")
        {
            gameManager.GameOver();
        }
    }

}
