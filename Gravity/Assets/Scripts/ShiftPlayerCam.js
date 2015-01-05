#pragma strict

private var Player : GameObject;
private var CanShift : boolean;

// These are used to queue up the next shift to perform
// The "null" values are Vector3.zero and 0, as they are initialized
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
	// Are we allowed to shift right now?
	if(CanShift){
		CanShift = false; // Disable shifting while in mid-shift
		
		// Store where we're looking at so we can keep looking in the same spot, along with necessary rotation values
		var pointToLookAt : RaycastHit;
		var correctRotation;
		var localDirectionToLook;
		var whereToTurnTo : Vector3;
		var rotation : float = 0;
		
		// Gradually increment rotation
		while (rotation<90){
			var inc = RotateSpeed*Time.deltaTime;
			correctRotation = Physics.Raycast(Ray(transform.position, transform.forward), pointToLookAt);
			Player.transform.RotateAround(transform.position, axis, posOrNegDirection*inc);
			rotation +=inc;
			// Only correct rotation if the raycast hit actually worked -else we'll get a bug
			if(correctRotation){
				localDirectionToLook = transform.InverseTransformPoint(pointToLookAt.point);
				transform.LookAt(transform.TransformPoint(localDirectionToLook), transform.up);
				transform.localEulerAngles = Vector3(transform.localEulerAngles.x, 0, 0);
				
				whereToTurnTo = Player.transform.InverseTransformPoint(pointToLookAt.point);
				whereToTurnTo.y=0;
				Player.transform.LookAt(Player.transform.TransformPoint(whereToTurnTo), Player.transform.up);
			}
			// Wait a frame
			yield;
		}
		// Calculate how much farther (positive or negative) we have to rotate to get to a perfect 90 degrees
		var correction = 90 - rotation;
		
		// Lock rotation to 90 degrees
		Player.transform.RotateAround(transform.position, axis, posOrNegDirection*correction);		
		CanShift=true; // Now ready to shift again
		if(NextDirectionToShift != 0 && (axis == NextAxisToShift || axis == -NextAxisToShift)){
			var a = NextAxisToShift;
			var b = NextDirectionToShift;
			// Only bother setting the direction to zero, since we don't check the axis for "null" state
			NextDirectionToShift = 0;
			shiftPlayer(a, b);
		}else{
			NextDirectionToShift = 0;
		}
	}else{
		// Queue up the next axis and direction to shift by
		NextAxisToShift = axis;
		NextDirectionToShift = posOrNegDirection;
	}
		
}