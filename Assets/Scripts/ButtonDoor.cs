using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : MonoBehaviour {

    // Use this for initialization
    public int ButtonsNeeded;
    public float OpenHeight;
    Vector3 OpenPosition;
    protected int buttons;
    //protected float height;
    protected Rigidbody rb;
    Vector3 baseposition;
	protected virtual void Start () {
        buttons = 0;
        //height = 0;
        baseposition = transform.position + Vector3.zero;
        OpenPosition = baseposition + OpenHeight * Vector3.up;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        if (buttons>=ButtonsNeeded && transform.position.y<OpenPosition.y) {
            
            //height += Time.deltaTime;
            if (transform.position.y+Time.deltaTime>OpenPosition.y)
            {
                rb.MovePosition(OpenPosition);
            }
            else {
                rb.MovePosition(rb.position + Vector3.up * Time.deltaTime);
            }
        }
        else if (buttons<ButtonsNeeded && transform.position.y > baseposition.y) {
            //height -= 3*Time.deltaTime;

            if (transform.position.y-Time.deltaTime<baseposition.y) {
                rb.MovePosition(baseposition);
            }
            else {
                rb.MovePosition(rb.position - 3 * Vector3.up * Time.deltaTime);
            }
        }
	}

    public void AddButton(int addone=1) {
        buttons += addone;
    }
}
