using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollider : MonoBehaviour {

    public delegate void OnChildTriggerEnter(ChildCollider callingChildCollider, Collider otherCollider);
    public OnChildTriggerEnter onChildTriggerEnter;
    public delegate void OnChildTriggerExit(ChildCollider callingChildCollider, Collider otherCollider);
    public OnChildTriggerExit onChildTriggerExit;
    public delegate void OnChildCollisionEnter(ChildCollider callingChildCollider, Collision otherCollider);
    public OnChildCollisionEnter onChildCollisionEnter;
    public delegate void OnChildCollisionExit(ChildCollider callingChildCollider, Collision otherCollider);
    public OnChildCollisionExit onChildCollisionExit;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        onChildTriggerEnter(this, other);
        return;
    }

    private void OnTriggerExit(Collider other)
    {
        onChildTriggerExit(this, other);
        return;
    }

    private void OnCollisionEnter(Collision collision)
    {
        onChildCollisionEnter(this, collision);
        return;
    }

    private void OnCollisionExit(Collision collision)
    {
        onChildCollisionExit(this, collision);
        return;
    }
}
