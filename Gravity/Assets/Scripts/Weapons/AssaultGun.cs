using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AssaultGun : MonoBehaviour {
	public string MuzzleFlashResource = "";
	public Color ReticleDefault;
	public Color ReticleHigh;
	public Transform firepoint;
	public Transform aimpoint;
	public float Kickback = 2.5f;
	public float power = 30f;
	public float damage = 5f;
	
	public bool DEBUG = true;
	
	public float range = 100f;
	public float coneRadius = 10f;	
	public int numberOfRaycastRings = 10;
	public int raycastDensityPerRing = 4;
	
	
	private Image reticle;
	private Canvas playerUI;
	
	private Ray[] rays;
	
	// Use this for initialization
	void Start () {

		if (firepoint==null){
			firepoint = transform;
		}
		if (aimpoint == null){
			aimpoint = firepoint;
		}
		
		List<Ray> l = new List<Ray>();
		
		l.Add(new Ray(aimpoint.position, new Vector3(0, 0, range)));
		
		for (int c = 1; c<=numberOfRaycastRings; c++){
			float ringRadius = (numberOfRaycastRings/c)*coneRadius;
			int numberOfRaycastsInthisRing = ((int) (2*Mathf.PI))*raycastDensityPerRing;
			float angleToIncrementBy = (2*Mathf.PI)/numberOfRaycastsInthisRing;
			float currentAngle = 0;
			for (int a = 0; a<numberOfRaycastsInthisRing; a++){
				l.Add(new Ray(aimpoint.position, new Vector3(Mathf.Cos(currentAngle)*ringRadius, Mathf.Sin(currentAngle)*ringRadius ,range)));
				currentAngle += angleToIncrementBy;
			}
		}
		
		rays = l.ToArray();
		
		GameObject playerUIObj = GameObject.Find("PlayerUI");
		playerUI = playerUIObj.GetComponent<Canvas>();
		Image [] imgs =  playerUI.GetComponentsInChildren<Image>();
		foreach (Image i in imgs){
			if(i.gameObject.name == "Reticle"){
				reticle = i;
			}
		}
		
		if(reticle !=null){
			reticle.color = ReticleDefault;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetButtonDown("Fire1")){
			// "Kick" back as if firing a large gun
			GetComponentInParent<Rigidbody>().velocity += -aimpoint.forward.normalized*Kickback;
			Fire();
		}
		
		for (int c = 0; c<rays.Length; c++){
			RaycastHit hit;
			if (Physics.Raycast(aimpoint.position, aimpoint.TransformDirection(rays[c].direction), out hit, range)){
				if(DEBUG){
					Debug.DrawRay(aimpoint.position, aimpoint.TransformDirection(rays[c].direction)*range, Color.red);
				}
				if (hit.collider.tag == "Player"){
					if(reticle !=null){
						reticle.color = ReticleHigh;
						return;
					}
					break;
				}
			}
		}
		reticle.color = ReticleDefault;
		
		
	} // END UPDATE
	
	private void Fire () {
		PhotonNetwork.Instantiate(MuzzleFlashResource, firepoint.position, firepoint.rotation, 0);
		// List of all players hit by this shot
		List<GameObject> p = new List<GameObject>();
		
		for (int c = 0; c<rays.Length; c++){
			RaycastHit hit;
			if (Physics.Raycast(aimpoint.position, transform.TransformDirection(rays[c].direction), out hit)){
				GameObject g = hit.collider.gameObject;
				if (g.tag == "Player"){
					if (!p.Contains(g)){
						p.Add(g);
					}
				}
			}
		}
		
		if (p.Count != 0){
			for (int v = 0; v< p.Count; v++){
				GameObject d = p[v];
				
				d.GetPhotonView().RPC("Nudge", PhotonTargets.All, new object[] {d.transform.position-aimpoint.position, power});
				d.GetPhotonView().RPC("Damage", PhotonTargets.All, new object[] {damage});
			}
		}
		
	}
	
}
