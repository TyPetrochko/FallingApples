#pragma strict

function Start () {

}

function Update () {
	
	
	if (Input.GetKeyDown(KeyCode.LeftShift)){
		var upDirection = transform.TransformDirection(Vector3.up);
		
		var forwardness = Vector3.Dot(transform.forward, Vector3.forward);
		var rightness = Vector3.Dot(transform.forward, Vector3.right);
		var upness = Vector3.Dot(transform.forward, Vector3.up);
		
		Debug.Log(forwardness+" "+rightness+" "+upness);
		
		if(forwardness < -.5){
		Debug.Log("FLIP");
			transform.Rotate(Vector3.Cross(upDirection, Vector3.forward)*90, Space.World);
		}
		if(forwardness > .5){
			transform.Rotate(Vector3.Cross(upDirection, -Vector3.forward)*90, Space.World);
		}
		if(rightness > -.5){
			transform.Rotate(Vector3.Cross(upDirection, -Vector3.right)*90, Space.World);
		}
		if(rightness < .5){
			transform.Rotate(Vector3.Cross(upDirection, Vector3.right)*90, Space.World);
		}
		if(upness > -.5){
			transform.Rotate(Vector3.Cross(upDirection, -Vector3.up)*90, Space.World);
		}
		if(upness < .5){
			transform.Rotate(Vector3.Cross(upDirection, Vector3.up)*90, Space.World);
		}
	
	}
}