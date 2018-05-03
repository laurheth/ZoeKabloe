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
	protected override void Update () {
        base.Update();
        if (buttons >= ButtonsNeeded && !released) {
            released = true;
            if (prisonerscript != null)
            {
                prisonerscript.Free();
            }
        }
	}
}
