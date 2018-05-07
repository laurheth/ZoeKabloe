using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    public GameObject PushMe;
    public GameObject Door;
    //public GameObject manhole;
    public bool IsSticky;
    ButtonDoor doorscript;
    Manhole manholescript;
    Material ButtonLight;
    bool ison;
    //bool permaon;
	// Use this for initialization
	void Start () {
        ButtonLight = PushMe.GetComponent<Renderer>().material;
        doorscript = Door.GetComponent<ButtonDoor>();
        if (doorscript==null) {
            manholescript = Door.GetComponent<Manhole>();
        }
	}

    public bool IsOn() {
        return ison;
    }

	private void OnTriggerStay(Collider other)
	{
        //if (other.tag == "Door") { return; }
        //Debug.Log(other.name);
        if (!ison) {
            if (doorscript != null)
            {
                doorscript.AddButton();
            }
            else if (manholescript != null)
            {
                manholescript.AddButton();
            }
        }
        ison = true;
        ButtonLight.EnableKeyword("_EMISSION");
	}

	private void OnTriggerExit(Collider other)
	{
        if (IsSticky) { return; }
        if (ison)
        {
            if (doorscript != null)
            {
                doorscript.AddButton(-1);
            }
        }
        ison = false;
        ButtonLight.DisableKeyword("_EMISSION");
	}
}
