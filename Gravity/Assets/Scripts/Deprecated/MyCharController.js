

// ************ DEPRECATED ****************//

// Old Vars
var moveSpeed: float = 6; // move speed
var turnSpeed: float = 90; // turning speed (degrees/second)
var lerpSpeed: float = 10; // smoothing speed
var deltaGround: float = 0.1; // character is grounded up to this distance
var jumpSpeed: float = 10; // vertical jump initial speed
var jumpRange: float = 10; // range to detect target wall

private var surfaceNormal: Vector3; // current surface normal
private var myNormal: Vector3; // character normal
private var distGround: float; // distance from character position to ground
private var jumping = false; // flag &quot;I'm jumping to wall&quot;
private var vertSpeed: float = 0; // vertical jump current speed 

// My Vars
var speed : float = 10;
var strafeSpeed : float = 5;
var gravity: float = 10;
var jumpStrength: float = 10;
var isGrounded: boolean;

private var upDirection: Vector3;
private var velocity: Vector3;
private var lastPressedJump : float = -100;
private var canJump = true;






function Start(){
	rigidbody.freezeRotation = true;
	rigidbody.useGravity = false;
	velocity = Vector3.zero;// LOCAL
	isGrounded = false;
	distGround = collider.height/2;
	Debug.Log(distGround);
}

function FixedUpdate(){
}

function Update(){

	transform.Translate(velocity*Time.deltaTime);

	if (Input.GetButtonDown("Jump")){ // jump pressed:
		lastPressedJump = Time.time;
	}
	
	if(Time.time-lastPressedJump <= 1 && isGrounded){
		lastPressedJump = -100;
		isGrounded = false;
		velocity = Vector3.up * jumpStrength;
		return;
	}
	
	//check if grounded
	var ray: Ray;
	var hit: RaycastHit;
	Debug.DrawRay(transform.position, -transform.up*100, Color.blue);
	ray = Ray(transform.position, -transform.up); // cast ray downwards
	if (Physics.Raycast(ray, hit)){ 
		if(hit.distance <= distGround + deltaGround && velocity.y <=0){
			isGrounded = true;
			velocity = Vector3.zero;
			
			
		}else{
			isGrounded=false;
		}
	}
	else {
		isGrounded = false;
	}
	
	
	if(Input.GetKeyDown(KeyCode.LeftShift)){
		transform.Rotate(Vector3.left * 90, Space.World);
	}
	
	
	
	
	//now apply movement
	transform.Translate(Vector3(Input.GetAxis("Horizontal")*Time.deltaTime*strafeSpeed, 0, Input.GetAxis("Vertical")*Time.deltaTime*speed));
	
	if(!isGrounded){
		velocity += -Vector3.up*gravity*Time.deltaTime;
	}
	
	
	
}

function OnCollisionEnter(collision: Collision){
	Debug.Log("HIII");
}




//@script RequireComponent (RigidBody)