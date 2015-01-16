using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AllGUI : MonoBehaviour {
	public float LerpFraction = .05f;
	public Transform AltPosition;
	
	private Vector3 originalPosition;
	private RectTransform rt;
	private bool isAtOrigin;


	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform>();
		originalPosition = rt.position;
		isAtOrigin = true;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ToggleLocation () {
		if(isAtOrigin){
			isAtOrigin = false;
			Slide (AltPosition);
		}else{
			isAtOrigin = true;
			ReturnToHome();
		}
	}

	public void ToggleImageActive(){
		Selectable s = GetComponent<Selectable>();
		s.interactable = !s.interactable;
	}



	public void Slide(Transform Destination) {
		if(Destination != null){
			StopCoroutine("Move");
			StartCoroutine("Move", Destination.position);
		}
	}

	public void ReturnToHome () {
		StopCoroutine("Move");
		StartCoroutine("Move", originalPosition);
	}

	private IEnumerator Move (Vector3 whereTo){
		float distance = (whereTo-rt.position).magnitude;
		while(distance>.1){
			rt.position = Vector3.Lerp(rt.position, whereTo, LerpFraction);
			distance =(whereTo-rt.position).magnitude;
			yield return new WaitForSeconds(.01f);
		}
		rt.position = whereTo;
	}

}
