var speed = 10.0;
var gravity = 10.0;
var maxVelocityChange = 10.0;
var canJump = true;
var jumpHeight = .5;
var grounded = false;
// A lower value means the character can walk up slopes
var slopeConstant = .1;
var airControlHandicap = .1;
 
@script RequireComponent(Rigidbody, CapsuleCollider)
 
function Awake ()
{
	rigidbody.freezeRotation = true;
	rigidbody.useGravity = false;
}
 
function FixedUpdate ()
{
	if (grounded)
	{
		// Calculate how fast we should be moving
		var targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		
		// Convert it to world coordinates
		targetVelocity = transform.TransformDirection(targetVelocity);
		targetVelocity *= speed;
 
		// Apply a force that attempts to reach our target velocity
		var velocity = rigidbody.velocity;
		var velocityChange = (targetVelocity - velocity);
		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);
		
		// Convert back to local coordinates
		rigidbody.AddRelativeForce(transform.InverseTransformDirection(velocityChange), ForceMode.VelocityChange);
 
		// Jump
		if (canJump && Input.GetButton("Jump"))
		{
			grounded=false;
			// Calculate direction to move in local coordinates, then convert to world
			var localDirection = transform.InverseTransformDirection(rigidbody.velocity) + Vector3(0, CalculateJumpVerticalSpeed(), 0);
			rigidbody.velocity = transform.TransformDirection(localDirection);
		}
	} else {
		// We apply gravity manually for more tuning control
		rigidbody.AddRelativeForce(Vector3 (0, -gravity * rigidbody.mass, 0));	
	
		// Calculate how fast we should be moving
		targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		
		// Convert it to world coordinates
		targetVelocity = transform.TransformDirection(targetVelocity);
		targetVelocity *= speed;
 
		// Apply a force that attempts to reach our target velocity
		velocity = rigidbody.velocity;
		velocityChange = (targetVelocity - velocity);
		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);
		
		// Convert back to local coordinates
		rigidbody.AddRelativeForce(Vector3(transform.InverseTransformDirection(velocityChange).x, 0, transform.InverseTransformDirection(velocityChange).z)*airControlHandicap, ForceMode.VelocityChange);
		
		
	}
	grounded=false;
}
 
function OnCollisionStay (collision : Collision)
{
	// Only ground the character if the collision normal is facing up, 
	// meaning we hit a ground rather than another game object
	if(transform.InverseTransformDirection(collision.contacts[0].normal).y>.1){
		grounded = true;	
	} else{
		rigidbody.AddRelativeForce(Vector3 (0, -gravity * rigidbody.mass, 0));	
	}
}
 
function CalculateJumpVerticalSpeed ()
{
	// From the jump height and gravity we deduce the upwards speed 
	// for the character to reach at the apex.
	return Mathf.Sqrt(2 * jumpHeight * gravity);
}