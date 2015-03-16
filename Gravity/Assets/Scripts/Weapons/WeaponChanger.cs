using UnityEngine;
using System.Collections;

public class WeaponChanger : Photon.MonoBehaviour {

	public GameObject [] guns;
	public Transform gunLocation;
	private GameObject currentGun;

	// Use this for initialization
	void Start () {
		if (guns.Length != 0){
			photonView.RPC ("SetGun", PhotonTargets.All, (int) 0);
		}
	}
	
	// Update is called once per frame
	void Update () {

		// Offset is used since pressing 1 corresponds
		// to guns[0]
		for(int a = 1; a<(guns.Length+1);a++) {
			if(Input.GetKeyDown(""+a)){
				photonView.RPC("SetGun", PhotonTargets.All, (int) a-1);
			}
		}
	}

	[RPC] void SetGun (int gunNum){
		GameObject gun = guns[gunNum];
		if (currentGun !=null){
			Destroy(currentGun);
		}
		GameObject s = Instantiate(gun, gunLocation.position, gunLocation.rotation) as GameObject;
		s.transform.parent = gunLocation;
		currentGun = s;
		if(photonView.isMine){
			foreach(MonoBehaviour m in s.GetComponents<MonoBehaviour>()){
				if(m.enabled == false){
					m.enabled=true;
				}
				
			}
			AssaultGun ag = s.GetComponentInChildren<AssaultGun>();
			Camera c = GetComponentInChildren<Camera>();
			if (c !=null && ag!=null){
				ag.aimpoint = c.transform;
			}
		}
		
	}
}
