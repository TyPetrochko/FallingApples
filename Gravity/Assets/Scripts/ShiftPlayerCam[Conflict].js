#pragma strict

private var Player : GameObject;
private var CanShift : boolean;

// These are used to queue up the next shift to make
private var NextAxisToShift : Vector3 = Vector3.zero;
private var NextDirectionToShift : float = 0;


public var RotateSpeed : float = 250;




function Start () {
	Player = transform.parent.gameObject;
	CanShift = true;
}

function Update () {
	
	if(Input.GetKeyDown(KeyCode.Escape)){
		Screen.lockCursor = !Screen.lockCursor;
		Screen.fullScreen = !Screen.fullScreen;
	}

	// Is player trying to upshift?
	if ((Input.GetKey(KeyCode.LeftShift) && Input.GetButtonDown("upshift")) || (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetButton("upshift"))){
		var pointToLookAt : RaycastHit;
		var correctRotation = Physics.Raycast(Ray(transform.position, transform.forward), pointToLookAt);
		
		var currentPosition = transform.position;
		var upDirection = Player.transform.TransformDirection(Vector3.up);
		
		var forwardness = Vector3.Dot(Player.transform.forward, Vector3.forward);
		var rightness = Vector3.Dot(Player.transform.forward, Vector3.right);
		var upness = Vector3.Dot(Player.transform.forward, Vector3.up);
		
		var fabs = Mathf.Abs(forwardness);
		var rabs = Mathf.Abs(rightness);
		var uabs = Mathf.Abs(upness);
		
		if(fabs>rabs && fabs>uabs){
			if(forwardness < -.5){
				shiftPlayer(Vector3.Cross(upDirection, Vector3.forward).normalized, 1);
			}
			else{
				shiftPlayer(Vector3.Cross(upDirection, Vector3.forward).normalized, -1);
			}
		}else if (rabs >fabs && rabs > uabs){
			if(rightness > -.5){
				shiftPlayer(Vector3.Cross(upDirection, Vector3.right).normalized, -1);
			}
			else{
				shiftPlayer(Vector3.Cross(upDirection, Vector3.right).normalized, 1);
			}
		}else if (uabs > rabs && uabs > fabs){
			if(upness > -.5){
				shiftPlayer(Vector3.Cross(upDirection, Vector3.up).normalized, -1);		
			}
			else {
				shiftPlayer(Vector3.Cross(upDirection, Vector3.up).normalized, 1);
			}
		}
		
	}
	
	// Is player trying to downshift?
	else if ((Input.GetKey(KeyCode.LeftShift) && Input.GetButtonDown("downshift")) || (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetButton("downshift"))){
		correctRotation = Physics.Raycast(Ray(transform.position, transform.forward), pointToLookAt);
		
		currentPosition = transform.position;
		upDirection = Player.transform.TransformDirection(Vector3.up);
		
		forwardness = Vector3.Dot(Player.transform.forward, Vector3.forward);
		rightness = Vector3.Dot(Player.transform.forward, Vector3.right);
		upness = Vector3.Dot(Player.transform.forward, Vector3.up);
		
		fabs = Mathf.Abs(forwardness);
		rabs = Mathf.Abs(rightness);
		uabs = Mathf.Abs(upness);
		
		if(fabs>rabs && fabs>uabs){
			if(forwardness < -.5){
				shiftPlayer(Vector3.Cross(upDirection, Vector3.forward).normalized, -1);
			}
			else{
				shiftPlayer(Vector3.Cross(upDirection, Vector3.forward).normalized, 1);
			}
		}else if (rabs >fabs && rabs > uabs){
			if(rightness > -.5){
				shiftPlayer(Vector3.Cross(upDirection, Vector3.right).normalized, 1);
			}
			else{
				shiftPlayer(Vector3.Cross(upDirection, Vector3.right).normalized, -1);
			}
		}else if (uabs > rabs && uabs > fabs){
			if(upness > -.5){
				shiftPlayer(Vector3.Cross(upDirection, Vector3.up).normalized, 1);		
			}
			else {
				shiftPlayer(Vector3.Cross(upDirection, Vector3.up).normalized, -1);
			}
		}
	}
	
	// Is player trying to leftshift
	else if ((Input.GetKey(KeyCode.LeftShift) && Input.GetButtonDown("leftshift")) || (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetButton("leftshift"))){
		
		correctRotation = Physics.Raycast(Ray(transform.position, transform.forward), pointToLookAt);
		
		currentPosition = transform.position;
		upDirection = Player.transform.TransformDirection(Vector3.up);
		
		forwardness = Vector3.Dot(Player.transform.forward, Vector3.forward);
		rightness = Vector3.Dot(Player.transform.forward, Vector3.right);
		upness = Vector3.Dot(Player.transform.forward, Vector3.up);
		
		fabs = Mathf.Abs(forwardness);
		rabs = Mathf.Abs(rightness);
		uabs = Mathf.Abs(upness);
		
		
		
		if(fabs>rabs && fabs>uabs){
			if(forwardness < -.5){
				shiftPlayer(Vector3.forward, 1);
			}
			else{
				shiftPlayer(Vector3.forward, -1);
			}
		}else if (rabs >fabs && rabs > uabs){
			if(rightness > -.5){
				shiftPlayer(Vector3.right, -1);
			}
			else{
				shiftPlayer(Vector3.right, 1);
			}
		}else if (uabs > rabs && uabs > fabs){
			if(upness > -.5){
				shiftPlayer(Vector3.up, -1);	
			}
			else {
				shiftPlayer(Vector3.up, 1);	
			}
		}
	}
	
	// Is player trying to rightshift
	else if ((Input.GetKey(KeyCode.LeftShift) && Input.GetButtonDown("rightshift")) || (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetButton("rightshift"))){
		
		correctRotation = Physics.Raycast(Ray(transform.position, transform.forward), pointToLookAt);
		
		currentPosition = transform.position;
		upDirection = Player.transform.TransformDirection(Vector3.up);
		
		forwardness = Vector3.Dot(Player.transform.forward, Vector3.forward);
		rightness = Vector3.Dot(Player.transform.forward, Vector3.right);
		upness = Vector3.Dot(Player.transform.forward, Vector3.up);
		
		fabs = Mathf.Abs(forwardness);
		rabs = Mathf.Abs(rightness);
		uabs = Mathf.Abs(upness);
		
		
		
		if(fabs>rabs && fabs>uabs){
			if(forwardness < -.5){
				shiftPlayer(Vector3.forward, -1);
			}
			else{
				shiftPlayer(Vector3.forward, 1);
			}
		}else if (rabs >fabs && rabs > uabs){
			if(rightness > -.5){
				shiftPlayer(Vector3.right, 1);
			}
			else{
				shiftPlayer(Vector3.right, -1);
			}
		}else if (uabs > rabs && uabs > fabs){
			if(upness > -.5){
				shiftPlayer(Vector3.up, 1);	
			}
			else {
				shiftPlayer(Vector3.up, -1);	
			}
		}
	}
}


// PosOrNegDirection must be 1 or -1
function shiftPlayer (axis : Vector3 , posOrNegDirection : int) : IEnumerator{
	if(CanShift){
		CanShift = false;
		var pointToLookAt : RaycastHit;
		var correctRotation;
		var localDirectionToLook;
		var whereToTurnTo : Vector3;
		var rotation : float = 0;
		while (rotation<90){
			var inc = RotateSpeed*Time.deltaTime;
			correctRotation = Physics.Raycast(Ray(transform.position, transform.forward), pointToLookAt);
			Player.transform.RotateAround(transform.position, axis, posOrNegDirection*inc);
			rotation +=inc;
			
			if(correctRotation){
				localDirectionToLook = transform.InverseTransformPoint(pointToLookAt.point);
				transform.LookAt(transform.TransformPoint(localDirectionToLook), transform.up);
				transform.localEulerAngles = Vector3(transform.localEulerAngles.x, 0, 0);
				
				whereToTurnTo = Player.transform.InverseTransformPoint(pointToLookAt.point);
				whereToTurnTo.y=0;
				Player.transform.LookAt(Player.transform.TransformPoint(whereToTurnTo), Player.transform.up);
			}
			yield;
		}
		var correction = 90 - rotation;
		Player.transform.RotateAround(transform.position, axis, posOrNegDirection*correction);		
		CanShift=true;
		if(NextDirectionToShift != 0 && (axis == NextAxisToShift || axis == -NextAxisToShift)){
			var a = NextAxisToShift;
			var b = NextDirectionToShift;
			NextDirectionToShift = 0;
			shiftPlayer(a, b);
		}
	}else{
		// 
		NextAxisToShift = axis;
		NextDirectionToShift = posOrNegDirection;
	}
		
}