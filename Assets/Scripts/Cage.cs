using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : ButtonDoor {
    public GameObject prisoner;
    Friend prisonerscript;
    bool released;
	// Use this for initialization
	protected override void Start () {
        released = false;
        prisonerscript = prisoner.GetComponent<Friend>();
        base.Start();
	}
	
	// Update is called once per frame
	protected override void FixedUpdate () {
        base.FixedUpdate();
        if (buttons >= ButtonsNeeded && !released) {
            released = true;
            if (prisonerscript != null)
            {
                prisonerscript.Free();
            }
        }
	}
}
