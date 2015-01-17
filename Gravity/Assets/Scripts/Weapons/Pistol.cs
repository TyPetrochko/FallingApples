using UnityEngine;
using System.Collections;



public class Pistol : Photon.MonoBehaviour {
	public LineRenderer laser;
	public Transform firepoint;
	public float AimAssistConstant = .35f; // How much AutoAssist helps, where 1 is not at all, and 0 freezes rotation completely
	public int Range = 100;
	public float Kickback = 2.5f;
	public float power = 30f;
	public float damage = 5f;
	public float zoomFOV = 30;
	public float zoomSpeed = 5;
	private float maxPOV;
	private Camera cam;
	private ShiftMouseLook [] looks;
	private RigidBodyFPS rigidFPS;


	// Use this for initialization
	void Start () {
		cam = GetComponentInParent<Camera>();
		maxPOV = cam.fieldOfView;
		looks = GetComponentsInParent<ShiftMouseLook>();
		rigidFPS = GetComponentInParent<RigidBodyFPS>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1") && photonView.isMine){
			PhotonNetwork.Instantiate("PistolShot", firepoint.position, firepoint.rotation, 0);
		}
		RaycastHit hit;
		if (Physics.Raycast(firepoint.position, firepoint.forward, out hit, Range)){
			laser.SetPosition(1, new Vector3(0, 0, hit.distance));
			GameObject obj = hit.collider.gameObject;
			if(Input.GetButtonDown("Fire1")  && photonView.isMine){
				// "Kick" back as if firing a large gun

				GetComponentInParent<Rigidbody>().velocity += (transform.position-hit.point).normalized*Kickback;

				if (obj.tag == "Player"){

					// Code for successful hit here!
					Vector3 blastDirection = hit.point-firepoint.position;
					obj.GetPhotonView().RPC("Nudge", PhotonTargets.AllViaServer, new object[] {blastDirection, power});
					obj.GetPhotonView().RPC("Damage", PhotonTargets.AllViaServer, new object[] {damage});
					Debug.Log("HIT!");
				}
				PhotonNetwork.Instantiate("Explosion", hit.point, Quaternion.identity, 0);
			}
			else if(Input.GetButton("Fire2")){
				AimAssist();
			}else{
				RemoveAimAssist();
			}
		}
	}

	void AimAssist(){
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomFOV, Time.deltaTime*zoomSpeed);
		foreach (ShiftMouseLook m in looks){
			m.AimAssistConstant = AimAssistConstant;
		}
		rigidFPS.speedMultiplierConstant = AimAssistConstant;
	}
	void RemoveAimAssist(){
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, maxPOV, Time.deltaTime*zoomSpeed);
		foreach (ShiftMouseLook m in looks){
			m.AimAssistConstant = 1;
		}
		rigidFPS.speedMultiplierConstant = 1;
	}
}
