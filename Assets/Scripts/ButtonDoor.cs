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
    bool ison;
	protected virtual void Start () {
        ison = false;
        buttons = 0;
        //height = 0;
        baseposition = transform.position + Vector3.zero;
        OpenPosition = baseposition + OpenHeight * transform.up;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	protected virtual void FixedUpdate () {
        if (buttons>=ButtonsNeeded) {
            rb.MovePosition(Vector3.MoveTowards(transform.position, OpenPosition, Time.fixedDeltaTime));
            //height += Time.deltaTime;
            /*if (Vector3.Dot( transform.position+transform.up*Time.deltaTime - OpenPosition,transform.up ) <0 )
            {
                rb.MovePosition(OpenPosition);
            }
            else {
                rb.MovePosition(rb.position + transform.up * Time.deltaTime);
            }*/
        }
        else if (buttons<ButtonsNeeded) {
            rb.MovePosition(Vector3.MoveTowards(transform.position, baseposition, 3*Time.fixedDeltaTime));
            //height -= 3*Time.deltaTime;

            /*if (Vector3.Dot(transform.position - transform.up * Time.deltaTime - baseposition, -transform.up) < 0) {
                rb.MovePosition(baseposition);
            }
            else {
                rb.MovePosition(rb.position - 3 * Vector3.up * Time.deltaTime);
            }*/
        }
	}

    public void AddButton(int addone=1) {
        buttons += addone;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.tag=="Crate" && other.gameObject.GetComponent<Rigidbody>() != null) {
            other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(transform.position.x, 0, transform.position.z) * Time.deltaTime, ForceMode.Acceleration);
        }*/
        buttons++;
        /*if (other.gameObject.tag == "Crate")
        {
            other.transform.parent = transform;
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        buttons--;
        /*if (other.gameObject.tag=="Crate") {
            other.transform.parent = null;
        }*/
    }
}
