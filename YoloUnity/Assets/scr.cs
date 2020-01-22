using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class scr : MonoBehaviour
{
  private GameObject _otherSphere;

  // Start is called before the first frame update
    void Start()
    {
      _otherSphere = FindObjectsOfType<GameObject>().First(x => x.tag == "kur");
    }

    // Update is called once per frame
    void Update()
    {
      var distance = _otherSphere.transform.position - transform.position;

      transform.Translate(distance * 0.005f);

      //var targetRotation = Quaternion.LookRotation (distance);
      //var str = Mathf.Min (1 * Time.deltaTime, 1);
      //transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, str);
    }
}
