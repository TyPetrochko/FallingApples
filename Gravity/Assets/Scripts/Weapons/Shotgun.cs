using UnityEngine;
using System.Collections;



public class Shotgun : Photon.MonoBehaviour {


	public Transform firepoint;
	public float Kickback = 2.5f;
	public float power = 30f;
	public float damage = 5f;

	public float range = 100f;
	public float coneRadius = 10f;	
	public int numberOfRayCasts;

	// Use this for initialization
	void Start () {
		if (numberOfRayCasts == null){
			numberOfRayCasts = (coneRadius*Mathf.PI);
		}
	}
	
	// Update is called once per frame
	void Update () {
		float radiusDeltaPerRevolution = coneRadius/numberOfRayCasts;

		int angle = 0;
		int currentRadius = 0;

		// Draw Raycasts in a spiral shape
		for (int f = 0; f<numberOfRayCasts; f++){
			Debug.DrawRay();// FINISH THIS RIGHT HERE
		}

		if(Input.GetButtonDown("Fire1")  && photonView.isMine){
			// "Kick" back as if firing a large gun
			
			GetComponentInParent<Rigidbody>().velocity += -transform.InverseTransformVector(Vector3.forward)*Kickback;


		}




		
	}

	void OnTriggerStay(Collider c){
		GameObject obj = c.gameObject;
		if(Input.GetButtonDown("Fire1")  && photonView.isMine){

			Destroy(this);
			
			if (obj.tag == "Player"){
				
				// Code for successful hit here!
				Vector3 blastDirection = transform.InverseTransformVector(Vector3.forward);
				obj.GetPhotonView().RPC("Nudge", PhotonTargets.AllViaServer, new object[] {blastDirection, power});
				obj.GetPhotonView().RPC("Damage", PhotonTargets.AllViaServer, new object[] {damage});
				Debug.Log("HIT!");
			}
			PhotonNetwork.Instantiate("Explosion", c.transform.position, Quaternion.identity, 0);
		}
	}

}
