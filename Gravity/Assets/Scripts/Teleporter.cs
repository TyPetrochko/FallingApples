using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {
	public Transform destination;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter(Collider c){
		if(c.gameObject.tag == "Player" && destination !=null){
			Vector3 localVelocity = c.transform.TransformVector(c.rigidbody.velocity);
			c.transform.position = destination.position;
			c.transform.rotation = destination.rotation;
			c.rigidbody.velocity = c.transform.TransformVector(localVelocity);
		}
	}

}
