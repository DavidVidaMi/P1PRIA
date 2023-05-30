using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    void Update()
    {
        transform.Translate(transform.right * Time.deltaTime * (Mathf.PingPong(Time.time * 2 / 2.5f, 1) * 8 - 4));
    }
}
