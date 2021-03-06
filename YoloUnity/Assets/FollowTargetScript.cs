﻿using UnityEngine;

public class FollowTargetScript : MonoBehaviour
{
    public Transform _followedObject = null;
    public int _secondsBeforeFollowingStarts = 1;
    public int _speed = 10;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.fixedTime < _secondsBeforeFollowingStarts) return;

        transform.position =  Vector3.MoveTowards(transform.position, _followedObject.transform.position, _speed * Time.deltaTime);

        this.transform.LookAt(_followedObject);
        this.transform.Rotate(0,90,0);
    }
}
