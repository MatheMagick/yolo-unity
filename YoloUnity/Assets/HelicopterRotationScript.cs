using UnityEngine;

public class HelicopterRotationScript : MonoBehaviour
{
    public float _moveSpeed = 0.5f;
    public float _rotationSpeed = 500;

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKey(KeyCode.A))
      {
        transform.Translate(-_moveSpeed,0,0);
      } else if (Input.GetKey(KeyCode.D))
      {
        transform.Translate(_moveSpeed,0,0);
      } else if (Input.GetKey(KeyCode.W))
      {
        transform.Translate(0,_moveSpeed,0);
      } else if (Input.GetKey(KeyCode.S))
      {
        transform.Translate(0,-_moveSpeed,0);
      }

      if (Input.GetKeyDown(KeyCode.L)) {
        transform.RotateAround(transform.position, Vector3.up, -_rotationSpeed * Time.deltaTime);
      }
      if (Input.GetKeyDown(KeyCode.J)) {
        transform.RotateAround(transform.position, Vector3.up, _rotationSpeed * Time.deltaTime);
      }
      if (Input.GetKeyDown(KeyCode.I)) {
        transform.RotateAround(transform.position, Vector3.right, -_rotationSpeed * Time.deltaTime);
      }
      if (Input.GetKeyDown(KeyCode.K)) {
        transform.RotateAround(transform.position, Vector3.right, _rotationSpeed * Time.deltaTime);
      }
    }
}
