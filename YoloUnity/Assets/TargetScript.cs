using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private const float _speed = 0.01f;

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKey(KeyCode.A))
      {
        transform.Translate(-_speed,0,0);
      } else if (Input.GetKey(KeyCode.D))
      {
        transform.Translate(_speed,0,0);
      } else if (Input.GetKey(KeyCode.W))
      {
        transform.Translate(0,_speed,0);
      } else if (Input.GetKey(KeyCode.S))
      {
        transform.Translate(0,-_speed,0);
      }
    }
}
