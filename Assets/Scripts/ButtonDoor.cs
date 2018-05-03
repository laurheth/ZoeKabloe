using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : MonoBehaviour {

    // Use this for initialization
    public int ButtonsNeeded;
    public float OpenHeight;
    protected int buttons;
    protected float height;
    protected Rigidbody rb;
	protected virtual void Start () {
        buttons = 0;
        height = 0;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        if (buttons>=ButtonsNeeded && height<OpenHeight) {
            height += Time.deltaTime;
            rb.MovePosition(rb.position + Vector3.up * Time.deltaTime);
        }
        else if (buttons<ButtonsNeeded && height>0) {
            height -= 3*Time.deltaTime;
            rb.MovePosition(rb.position - 3*Vector3.up * Time.deltaTime);
        }
	}

    public void AddButton(int addone=1) {
        buttons += addone;
    }
}
