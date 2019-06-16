using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class CameraManager : MonoBehaviour
{
    public Transform m_target;
    public float m_smoothspeed = 0.125f;
    public Vector3 m_offset;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredPosition = m_target.position + m_offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, m_smoothspeed);
        transform.position = smoothPosition;

        transform.LookAt(m_target);
    }
}
