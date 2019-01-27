using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnHandler : MonoBehaviour 
{
    public float force;
    public float lift;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.forward * 10f);
    }

    void OnCollisionEnter(Collision c)
    { 
        if (c.gameObject.tag == "Player")
        {
            Vector3 dir = c.contacts[0].point - transform.position;

            dir = -dir.normalized;
            GetComponent<Rigidbody>().AddForce(dir * force);
            GetComponent<Rigidbody>().AddForce(Vector3.up * lift);
        }
    }
}
