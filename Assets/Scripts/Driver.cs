using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour {

    public Robot robot;
    //GameObject cophat;

    public void GetHit(int damage) {
        robot.GetHit(damage);
    }

    public IEnumerator Die()
    {
        GameObject cophat = transform.Find("Armature/Head/cophat").gameObject;
        cophat.GetComponent<Rigidbody>().isKinematic = false;
        cophat.transform.SetParent(null);
        GetComponent<Rigidbody>().isKinematic = false;
        transform.SetParent(null);
        //GetComponent<ParticleSystem>().Play();
        Vector3 size = transform.localScale;
       
        for (int i = 0; i < 150; i++)
        {
            if (i < 100)
            {
                size[1] /= 1.05f;
            }
            else
            {
                size /= 1.1f;
            }
            transform.localScale = size;
            yield return null;
        }
        Destroy(gameObject);
    }
}
