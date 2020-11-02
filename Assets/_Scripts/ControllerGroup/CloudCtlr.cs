using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCtlr : MonoBehaviour {
    GameObject cloud;
	// Use this for initialization
	void Start () {
        cloud = GameObject.Find("cloud");
	}
	
	// Update is called once per frame
	void Update () {
        cloud.transform.Translate(Vector3.left*0.04f*Time.deltaTime,Space.World);
        if (transform.position.x <= -9)
            transform.Translate(Vector3.right * 19f, Space.World);
    }
}
