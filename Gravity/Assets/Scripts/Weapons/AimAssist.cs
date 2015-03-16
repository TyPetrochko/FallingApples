using UnityEngine;
using System.Collections;



public class AimAssist : MonoBehaviour {
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
		if(Input.GetButton("Fire2")){
			Assist();
		}else{
			RemoveAimAssist();
		}
	}
	
	void Assist(){
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
