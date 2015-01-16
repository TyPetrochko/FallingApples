using UnityEngine;
using System.Collections;


	/*
	 * Describes health and remote procedure calls 
	 * for player class
	 */


public class PlayerCore : Photon.MonoBehaviour {


	public float maxHealth = 20;
	public float health = 20;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0){
			Respawn();
		}

	}

	void Respawn(){
		RandomMatchmaker.SpawnPlayer();
		PhotonNetwork.Destroy (photonView);
	}

	[RPC]
	void Nudge(Vector3 direction, float power){
		if(photonView.isMine){
			rigidbody.velocity += direction*power;
		}else{
			//GetComponent<NetworkCharacter>().veloc += direction*power;
		}
	}
	[RPC]
	void Damage (float damage) {
		if(photonView.isMine){
			health -= damage;
		}
	}
}
