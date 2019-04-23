using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public Camera cam;

    public Ennemy_AI[] tab_ai;

    public Canvas myCanvas;

    private Vector3 pos_cam;
    private Quaternion rot_cam;
    private bool reflexion_mode;
    // Start is called before the first frame update
    void Start()
    {
        pos_cam = cam.transform.position;
        rot_cam = cam.transform.rotation;

        cam.transform.position = new Vector3(0, 10, 0);
        cam.transform.rotation = Quaternion.Euler(new Vector3(90,0,0));

        reflexion_mode = true;
    }

    // Update is called once per frame
    void Update()
    {
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

    public void ReloadLevel()
    {
        SceneManager.LoadScene("Maze");
    }
}
