using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyTrigger : MonoBehaviour {

    public GameObject Door;
    public GameObject Hinge;
    Rigidbody doorrb;
    Rigidbody hingerb;
    float totalmass;
    int currentmass;
    public float massthresh;
    public float maxdescent;
    float descent;
	// Use this for initialization
	void Start () {
        totalmass = 0;
        currentmass = 0;
        doorrb = Door.GetComponent<Rigidbody>();
        hingerb = Hinge.GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
        //if (currentmass > totalmass+1) { currentmass -= 1; }
        //if (currentmass > totalmass + 1) { currentmass -= 1; }
        if (totalmass>massthresh) {
            //doorrb.isKinematic = true;
            if (descent<maxdescent) {
                descent += Time.fixedDeltaTime;
                hingerb.MovePosition(hingerb.position + Vector3.down*Time.fixedDeltaTime);
                doorrb.MovePosition(doorrb.position + Vector3.up * Time.fixedDeltaTime);
            }
        }
        else {
            if (descent>0) {
                descent -= 3*Time.fixedDeltaTime;
                hingerb.MovePosition(hingerb.position - Vector3.down * 3*Time.fixedDeltaTime);
                doorrb.MovePosition(doorrb.position - Vector3.up * 3*Time.fixedDeltaTime);
            }
            //doorrb.isKinematic = false;
        }
	}

	private void OnTriggerEnter(Collider coll)
	{
        if (coll.gameObject.GetComponent<Rigidbody>() != null)
        {
            totalmass += coll.gameObject.GetComponent<Rigidbody>().mass;
        }
	}

    private void OnTriggerExit(Collider coll)
	{
        if (coll.gameObject.GetComponent<Rigidbody>() != null)
        {
            totalmass -= coll.gameObject.GetComponent<Rigidbody>().mass;
        }
	}

}
