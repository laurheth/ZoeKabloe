using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    AudioListener audioListener;
	// Use this for initialization
	void Start () {
        audioListener = GetComponent<AudioListener>();
        if (PlayerPrefs.GetInt("Mute")>0.5) {
            audioListener.enabled = false;
        }
        else {
            audioListener.enabled = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Mute"))
        {
            audioListener.enabled = !audioListener.enabled;
            if (audioListener.enabled) {
                PlayerPrefs.SetInt("Mute", 0);
            }
            else {
                PlayerPrefs.SetInt("Mute", 1);
            }
        }
	}
}
