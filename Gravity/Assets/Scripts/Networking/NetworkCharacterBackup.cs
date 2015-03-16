using UnityEngine;
using System.Collections;

public class NetworkCharacterBackup : Photon.MonoBehaviour {
	
	private Vector3 correctPos;
	private Quaternion correctRot;
	public Vector3 veloc;
	private Quaternion cameraRot;
	
	private Transform myTransform;
	public Transform cameraTransform;
	
	private float timeSinceLastUpdate;
	private Vector3 lastPos;
	
	public int RotationLerpSpeed = 15;
	public int SendRate = 20;
	public int SendRateOnSerialize = 10;
	
	// Use this for initialization
	void Start () {
		cameraRot = Quaternion.identity;
		timeSinceLastUpdate= 0.0f;
		lastPos = Vector3.zero;
		myTransform = transform;
		veloc = Vector3.zero;
		
		correctPos = Vector3.zero;
		correctRot = Quaternion.identity;
		
		PhotonNetwork.sendRate = SendRate;
		PhotonNetwork.sendRateOnSerialize = SendRateOnSerialize;
	}
	
	// Update is called once per frame
	void Update () {
		if (!photonView.isMine){
			if(lastPos != correctPos){
				// We updated correctPos, so update position/rotation
				timeSinceLastUpdate = 0.0f;
				myTransform.position = correctPos;
				lastPos = correctPos;
			}else{
				//Make a best guess as to where we are
				timeSinceLastUpdate += Time.deltaTime;
				myTransform.position = correctPos + veloc*timeSinceLastUpdate;
			}
			
			cameraTransform.localRotation = Quaternion.Lerp(cameraTransform.localRotation, cameraRot, Time.deltaTime*RotationLerpSpeed);
			myTransform.rotation = Quaternion.Lerp(myTransform.rotation, correctRot, Time.deltaTime*RotationLerpSpeed);
			
		}
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting && photonView.isMine){
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(rigidbody.velocity);
			stream.SendNext(cameraTransform.localRotation);
		}else 
		{
			this.correctPos = (Vector3) stream.ReceiveNext();
			this.correctRot = (Quaternion) stream.ReceiveNext();
			this.veloc = (Vector3) stream.ReceiveNext();
			cameraRot= (Quaternion) stream.ReceiveNext();
		}
	}
}
