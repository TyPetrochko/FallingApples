using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Shotgun : Photon.MonoBehaviour {


	public Transform firepoint;
	public float Kickback = 2.5f;
	public float power = 30f;
	public float damage = 5f;

	public bool debug = true;


	public float range = 100f;
	public float coneRadius = 10f;	
	public int numberOfRaycastRings = 10;
	public int raycastDensityPerRing = 4;


	private Ray[] rays;

	// Use this for initialization
	void Start () {
		if (firepoint==null){
			firepoint = transform;
		}

		List<Ray> l = new List<Ray>();

		l.Add(new Ray(firepoint.position, transform.TransformDirection(new Vector3(0, 0, range))));
		
		for (int c = 1; c<=numberOfRaycastRings; c++){
			float ringRadius = (numberOfRaycastRings/c)*coneRadius;
			int numberOfRaycastsInthisRing = ((int) (2*Mathf.PI))*raycastDensityPerRing;
			float angleToIncrementBy = (2*Mathf.PI)/numberOfRaycastsInthisRing;
			float currentAngle = 0;
			for (int a = 0; a<numberOfRaycastsInthisRing; a++){
				l.Add(new Ray(firepoint.position, new Vector3(Mathf.Cos(currentAngle)*ringRadius, Mathf.Sin(currentAngle)*ringRadius ,range)));
				currentAngle += angleToIncrementBy;
			}
		}

		rays = l.ToArray();
		
	}
	
	// Update is called once per frame
	void Update () {

		if (debug){
			// Draw a Straight-ahead raycast
			Debug.DrawRay(firepoint.position, transform.TransformDirection(new Vector3(0, 0, range)));

			for (int c = 1; c<=numberOfRaycastRings; c++){
				float ringRadius = (numberOfRaycastRings/c)*coneRadius;
				int numberOfRaycastsInthisRing = ((int) (2*Mathf.PI))*raycastDensityPerRing;
				float angleToIncrementBy = (2*Mathf.PI)/numberOfRaycastsInthisRing;
				float currentAngle = 0;
				for (int a = 0; a<numberOfRaycastsInthisRing; a++){
					Debug.DrawRay(firepoint.position, transform.TransformDirection(new Vector3(Mathf.Cos(currentAngle)*ringRadius, Mathf.Sin(currentAngle)*ringRadius ,range)), Color.blue);
					currentAngle += angleToIncrementBy;
				}
			}
		}

		if(Input.GetButtonDown("Fire1")){
			// "Kick" back as if firing a large gun
			
			GetComponentInParent<Rigidbody>().velocity += -firepoint.forward.normalized*Kickback;

			Fire();

		}

		for (int c = 0; c<rays.Length; c++){
			RaycastHit hit;
			if (Physics.Raycast(firepoint.position, firepoint.InverseTransformDirection(rays[c].direction), out hit)){
				if (hit.collider.tag == "Player"){
					// Change reticule, then return
					break;
				}
			}
		}

		//UNDO RETICULE CHANGE (should skip if returned by loop)

	} // END UPDATE

	private void Fire () {

		List<GameObject> p = new List<GameObject>();

		for (int c = 0; c<rays.Length; c++){
			RaycastHit hit;
			if (Physics.Raycast(firepoint.position, transform.TransformDirection(rays[c].direction), out hit)){
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

				d.GetPhotonView().RPC("Nudge", PhotonTargets.AllViaServer, new object[] {d.transform.position-firepoint.position, power});
				d.GetPhotonView().RPC("Damage", PhotonTargets.AllViaServer, new object[] {damage});
			}
		}

	}

}
