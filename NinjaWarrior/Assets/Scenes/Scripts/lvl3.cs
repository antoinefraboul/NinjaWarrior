using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class lvl3 : MonoBehaviour
{
    public GameObject sphere;
    public ThirdPersonCharacter m_character;
    public GameObject m_target;
    public NavMeshAgent m_agent;
    // Start is called before the first frame update
    void Start()
    {
        sphere.GetComponent<Rigidbody>().useGravity = false;
        //Sounds
        FindObjectOfType<OptionsManager>().PlaySound("marshmello");
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    void Update()
    {
        if (m_character.transform.position.x > 37)
        {
            //super sale mais la flemme
            sphere.GetComponent<Rigidbody>().useGravity = true;
        }

        if (m_character.transform.position.x == m_target.transform.position.x && m_character.transform.position.z == m_target.transform.position.z)
        {
            FindObjectOfType<GameManager>().NextLevel();
        }
    }

    IEnumerator Delay()
    {
        float tmp =m_agent.speed;
        m_agent.speed = 0;
        yield return new WaitForSeconds(10);
        m_agent.speed = tmp;
    }
}
