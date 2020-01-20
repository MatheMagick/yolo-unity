using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
  public int fps = 1;
  float elapsed;
  Camera cam;

  void Start()
  {
    cam = GetComponent<Camera>();
    cam.enabled = false;
  }

  void Update()
  {
    elapsed += Time.deltaTime;
    if (elapsed > 1 / fps)
    {
      elapsed = 0;
      cam.Render();

    }
  }
}