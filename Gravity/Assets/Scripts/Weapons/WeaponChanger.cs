using UnityEngine;
using System.Collections;

public class WeaponChanger : MonoBehaviour {

	public GameObject [] guns;
	public Transform gunLocation;

	private GameObject currentGun;

	// Use this for initialization
	void Start () {
		if (guns.Length != 0){
			SetGun(guns[0]);
		}
	}
	
	// Update is called once per frame
	void Update () {

		// Offset is used since pressing 1 corresponds
		// to guns[0]
		for(int a = 1; a<(guns.Length+1);a++) {
			if(Input.GetKeyDown(""+a)){
				SetGun(guns[a-1]);
			}
		}
	}

	void SetGun (GameObject gun){
		if (gunLocation.childCount==1){
			Destroy(gunLocation.GetChild(0).gameObject);
		}
		GameObject s = (GameObject) Instantiate(gun, gunLocation.position, gunLocation.rotation);
		s.transform.parent = gunLocation;

		foreach(MonoBehaviour m in s.GetComponents<MonoBehaviour>()){
			if(m.enabled == false){
				m.enabled=true;
			}
			
		}
		
	}
}
