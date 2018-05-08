using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBat : MonoBehaviour {

    Rigidbody rb;
    public float rotationrate;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        //rb.angularVelocity=new Vector3(0, rotationrate, 0);
	}

	// Update is called once per frame
	private void Update()
	{
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0,0,rotationrate*Time.deltaTime));
	}
}
