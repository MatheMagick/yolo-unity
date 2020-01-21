using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterRotationScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private const float _speed = 0.5f;

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

      if (Input.GetKeyDown(KeyCode.L)) {
        transform.RotateAround(transform.position, Vector3.up, -500 * Time.deltaTime);
      }
      if (Input.GetKeyDown(KeyCode.J)) {
        transform.RotateAround(transform.position, Vector3.up, -500 * Time.deltaTime);
      }
      if (Input.GetKeyDown(KeyCode.I)) {
        transform.RotateAround(transform.position, Vector3.right, -500 * Time.deltaTime);
      }
      if (Input.GetKeyDown(KeyCode.K)) {
        transform.RotateAround(transform.position, Vector3.right, 500 * Time.deltaTime);
      }
    }
}
