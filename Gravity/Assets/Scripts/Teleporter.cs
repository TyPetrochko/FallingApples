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
			c.gameObject.transform.rotation = destination.rotation;
			c.gameObject.transform.position = destination.position;
		}
	}

}
