using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourCycle : MonoBehaviour {
    Color color;
    //Color[] colors;
    Renderer rend;
    int decrease;
    int increase;
	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        //colors = new Color[7];
        color = Color.red;
        decrease = 0;
        increase = 1;
	}
	
	// Update is called once per frame
	void Update () {
        color[decrease] -= 0.01f;
        color[increase] += 0.01f;
        if (color[increase]>=1) {
            color[increase] = 1;
            color[decrease] = 0;
            decrease++;
            increase++;
            if (decrease > 2) { decrease = 0; }
            if (increase > 2) { increase = 0; }
            if (decrease == increase) { increase++; }
        }
        rend.material.SetColor("_EmissionColor", color);
	}
}
