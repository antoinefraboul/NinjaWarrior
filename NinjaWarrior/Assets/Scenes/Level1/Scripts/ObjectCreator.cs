using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    public Camera cam;

    public GameObject cube;

    private Vector3 vec;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction, Color.red);

            if (Physics.Raycast(ray, out hit) && (hit.point.y < 1))
            {
                vec.x = Mathf.FloorToInt(hit.point.x)+ 0.5f ;
                vec.y = Mathf.FloorToInt(hit.point.y) + 0.55f;
                vec.z = Mathf.FloorToInt(hit.point.z) + 0.5f;
                Instantiate(cube, vec, Quaternion.identity);
            }
        }
    }
}
