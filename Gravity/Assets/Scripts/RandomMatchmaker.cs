using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RandomMatchmaker : MonoBehaviour {
	public GameObject Crosshairs;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;
		PhotonNetwork.ConnectUsingSettings("0.1");
	}
	

	// Update is called once per frame
	void Update () {
	}

	void OnJoinedLobby()
	{
		PhotonNetwork.JoinRandomRoom();
		Debug.Log("Joined room");

	}

	void OnPhotonRandomJoinFailed()
	{
		PhotonNetwork.CreateRoom(null);

	}

	void OnJoinedRoom() 
	{
		Instantiate(Crosshairs, Vector3.zero, Quaternion.identity);
		SpawnPlayer();
	}
	public static void SpawnPlayer(){

		GameObject player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
		
		foreach(MonoBehaviour m in player.GetComponentsInChildren<MonoBehaviour>()){
			if(m.enabled == false){
				m.enabled=true;
			}
			
		}
		player.GetComponent<Rigidbody>().isKinematic = false;
		Camera cam = player.GetComponentInChildren<Camera>();
		cam.enabled = true;
		AudioListener audio = player.GetComponentInChildren<AudioListener>();
		audio.enabled = true;
	}
}
