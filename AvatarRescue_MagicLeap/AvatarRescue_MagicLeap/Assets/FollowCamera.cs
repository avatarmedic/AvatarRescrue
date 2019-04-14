using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform CameraTrans;
    public float speed = 1f;
    public float angle = 0;
    void Update()
    {
        transform.position = CameraTrans.position;
        angle = speed * Time.deltaTime;
        transform.RotateAround(Vector3.up, angle);
    }
}
