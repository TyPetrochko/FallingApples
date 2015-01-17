using UnityEngine;
using System.Collections;


	/*
	 * Describes health and remote procedure calls 
	 * for player class
	 */


public class PlayerCore : Photon.MonoBehaviour {


	public float maxHealthMass = 20;
	public float healthMass = 20;

	public int maxDistance = 120;

	private Transform t;
	// Use this for initialization
	void Start () {
		t = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (healthMass <= 0){
			Respawn();
		}
		if (t.position.magnitude > maxDistance){
			healthMass = 0;
		}

	}

	void Respawn(){
		RandomMatchmaker.SpawnPlayer();
		PhotonNetwork.Destroy (photonView);
	}

	[RPC]
	void Nudge(Vector3 direction, float power){
		if(photonView.isMine){
			rigidbody.velocity += direction.normalized*power;
		}else{
			//GetComponent<NetworkCharacter>().veloc += direction*power;
		}
	}
	[RPC]
	void Damage (float damage) {
		if(photonView.isMine){
			healthMass -= damage;
		}
	}
}
