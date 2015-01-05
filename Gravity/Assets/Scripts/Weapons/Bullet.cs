using UnityEngine;
using System.Collections;

public class Bullet : Photon.MonoBehaviour {
	public float BulletSpeed = 50;
	// The player who fired the shot
	public GameObject firer;
	// Use this for initialization

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(0, 0, Time.deltaTime*BulletSpeed));
	}

	void OnTriggerEnter(Collider col){
		// Make sure it's not a self-collision
		if (col.gameObject.tag == "Player"){
			bool isBulletMine = photonView.isMine;
			bool isPlayerMine = col.gameObject.GetComponent<PhotonView>().isMine;
			if(!isBulletMine && isPlayerMine){
				// The bullet isn't mine, but the player is
				PlayerCore h = col.gameObject.GetComponent<PlayerCore>();
				h.health -= 5;
			} else if (isBulletMine && !isPlayerMine){
				// The bullet is mine, but the player isn't

				PhotonNetwork.Instantiate("Explosion", transform.position- transform.TransformVector(new Vector3(0, 0, BulletSpeed)*Time.deltaTime)*3, transform.rotation, 0);

				PhotonNetwork.Destroy(gameObject);
			}

		}else{
			if(photonView.isMine){
				PhotonNetwork.Instantiate("Explosion", transform.position- transform.TransformVector(new Vector3(0, 0, BulletSpeed)*Time.deltaTime)*3, transform.rotation, 0);
				PhotonNetwork.Destroy(gameObject);
			}

		}

	}
}
