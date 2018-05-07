using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotor : Damager {

    Drone parentdrone;

	private void Start()
	{
        active = true;
        parentdrone = GetComponentInParent<Drone>();
	}

	private void OnCollisionEnter(Collision collision)
	{
        if (active && collision.rigidbody!=null)// && collision.rigidbody.mass>12)
        {
            if (collision.gameObject.tag != "DroneOkay")
            {
                this.OnCollisionStay(collision);
                parentdrone.HitRotor(this,collision);
                if (collision.gameObject.tag == "Monster") {
                    if (collision.gameObject.GetComponent<Slime>() != null) {
                        collision.gameObject.GetComponent<Slime>().GetHit(10);
                    }
                }
            }
        }
	}
}
