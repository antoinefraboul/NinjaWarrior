using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public Camera cam;

    public Ennemy_AI[] tab_ai;

    public Canvas myCanvas;

    private Vector3 pos_cam;
    private Quaternion rot_cam;
    private bool reflexion_mode;
    private bool m_over;
    public NavMeshSurface m_surfaces;
    //public GameObject m_UI;
    // Start is called before the first frame update
    void Start()
    {
        m_surfaces.BuildNavMesh();

        pos_cam = cam.transform.position;
        rot_cam = cam.transform.rotation;

        cam.transform.position = new Vector3(0, 10, 0);
        cam.transform.rotation = Quaternion.Euler(new Vector3(90,0,0));

        reflexion_mode = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            //UI
            //m_UI.SetActive(true);
            //cam
            if (Input.GetMouseButton(1))
            {
                cam.transform.position -= cam.transform.right * Input.GetAxis("Mouse X");
                cam.transform.position -= cam.transform.up * Input.GetAxis("Mouse Y");
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && cam.orthographicSize - 1 > 0) // forward
            {
                cam.orthographicSize--;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                cam.orthographicSize++;
            }
        }

        if (reflexion_mode)
        {

        }
        
        if (Input.GetMouseButton(1))
        {
            cam.transform.position = pos_cam;
            cam.transform.rotation = rot_cam;
            run();
        }
        
    }

    void run()
    {
        foreach (Ennemy_AI ai in tab_ai)
        {
            ai.initAI();
        }
    }

    public void reset()
    {
        foreach (Ennemy_AI ai in tab_ai)
        {
            ai.resetAI();
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene("Maze");
    }
}
