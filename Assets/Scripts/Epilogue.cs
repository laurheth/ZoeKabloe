using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Epilogue : MonoBehaviour {

    public GameObject Epilogue1;
    public GameObject Epilogue2;
    public GameObject[] Friends;
    public GameObject TheEndText;
    public GameObject Cover;
    public GameObject TheText;
    //public string[] strings;
    public float letterspersecond;
    bool writing;
    //float time;
    //int numletters;
    //public string test;
    Text thetext;
    int numfree;
    string Zoe;
	// Use this for initialization
	void Start () {
        writing = false;
        numfree = 6;
        if (GameManager.instance != null)
        {
            for (int i = 0; i < 6; i++)
            {
                if (!GameManager.instance.IsFreed(i + 1))
                {
                    Friends[i].SetActive(false);
                    numfree--;
                }
            }
            Destroy(GameManager.instance.gameObject); // no longer needed
        }
        Epilogue1.SetActive(true);
        Epilogue2.SetActive(false);
        TheEndText.SetActive(false);
        Cover.SetActive(true);
        thetext = TheText.GetComponent<Text>();
        Zoe = thetext.text;
        thetext.text = "";
        StartCoroutine(DoEpilogue());
	}

    IEnumerator DoEpilogue() {
        StartCoroutine(WriteText(GetString(0)));
        yield return CrossFade(true);
        while (writing) {
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(4);
        yield return CrossFade(false);
        Epilogue1.SetActive(false);
        if (numfree > 0)
        {
            Epilogue2.SetActive(true);
            for (int i = 1; i < 4; i++)
            {
                thetext.text = "";
                StartCoroutine(WriteText(GetString(i)));
                if (writing)
                {
                    yield return CrossFade(true);
                    while (writing)
                    {
                        yield return new WaitForSeconds(0.5f);
                    }
                    yield return new WaitForSeconds(4);
                }
            }
            yield return CrossFade(false);
            Epilogue2.SetActive(false);
        }
        TheEndText.SetActive(true);
        thetext.text = "";
        yield return CrossFade(true);
    }

    IEnumerator CrossFade(bool inorout) {
        float difference = 0.3f;
        float targalpha;
        float sign;// = -1;

        Image thecover = Cover.GetComponent<Image>();
        Color startcolor = thecover.color;//Color.black;
        if (inorout) {
            difference *= -1;
            targalpha = 0;
            sign = 1;
        }
        else {
            sign = -1;
            startcolor[3] = 0;
            targalpha = 1;
        }

        while (sign*startcolor[3]>sign*targalpha) {
            startcolor[3] += difference * Time.deltaTime;
            thecover.color = startcolor;
            yield return null;
        }

    }

    IEnumerator WriteText(string towrite) {
        if (towrite.Length > 0)
        {
            writing = true;
            int letters = 0;
            float seconds = 0;
            thetext.text = "";
            while (letters < towrite.Length)
            {
                seconds += Time.deltaTime;
                letters = Mathf.CeilToInt(letterspersecond * seconds);
                thetext.text = towrite.Substring(0, letters);
                yield return null;
            }
            writing = false;
        }
    }

    string GetString(int msgnum) {
        string toreturn="";

        switch (msgnum)
        {
            default:
            case 0:
                if (numfree > 0)
                {
                    toreturn = Zoe + " and her " + numfree + " friends finally had their delicious brunch"+
                        " of strawberry pie and slime pie! It was a magical day and a magical time" +
                        " and everyone was very, very happy.";
                }
                else {
                    toreturn = Zoe + " finally got to have her brunch, but sadly, none of her friends " +
                        "could make it because they were still stuck in slime-jail. She ate her" +
                        " pie alone.";
                }
                break;
            case 1:
                if (numfree<6 && numfree>0) {
                    toreturn = "After brunch, they all worked together to free their remaining friends!";
                }
                break;
            case 2:
                if (numfree > 0)
                {
                    toreturn = "Having overthrown the bourgoise Slime Lord, the slimes all moved into " +
                        "Condo Castle and claimed it as a safe haven for all slimes in need of a home. " +
                        "Additionally, rent in the Village was abolished, and all lesser landlords and " +
                        "cops were driven away!";
                }
                break;
            case 3:
                if (numfree>0) {
                    toreturn = "They all lived happily ever after...";
                }
                break;
        }

        return toreturn;
    }
}
