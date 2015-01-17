using UnityEngine;
using System.Collections;

public class RigidBodyFPS : MonoBehaviour {
	public float speed = 10.0f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public bool canJump = true;
	public float jumpHeight = .5f;
	public bool grounded = false;
	public float slopeConstant = .1f; // A lower value means the character can walk up slopes
	public float airControlHandicap = .1f;
	public float speedMultiplierConstant = 1f;
	public float doubleTapSpeed = .3f;
	public float sprintSpeed = 2f;
	public float constantVelocityInfluence = .4f;// A value closer to one decreases the effect of a nudge

	private float sprintMultiplier = 1f;
	private bool doubleTap = false;
	private bool sprinting = false;
	private PlayerCore pc;

	private Vector3 targetVelocity;
	private Vector3 velocity;
	private Vector3 velocityChange;
	private Vector3 localDirection;


	// Use this for initialization
	void Start () {
		rigidbody.freezeRotation = true;
		rigidbody.useGravity = false;
		pc = GetComponent<PlayerCore>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical")>0){
			if (doubleTap){
				doubleTap = false;
				sprinting = true;
				sprintMultiplier = sprintSpeed;
				return;
			}else{
				doubleTap = true;
				StartCoroutine(DisableDoubleTap());
			}
		}else if (Input.GetAxisRaw("Vertical")<1){
			sprintMultiplier = 1f;
			sprinting = false;
		}
	}

	void FixedUpdate(){
		// Calculate how fast we should be moving
		targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		
		// Convert it to world coordinates
		targetVelocity = transform.TransformDirection(targetVelocity);
		targetVelocity *= speed*speedMultiplierConstant*sprintMultiplier;
		
		
		// Use a force that attempts to reach our target velocity
		velocity = rigidbody.velocity;
		velocityChange = (targetVelocity - velocity);
		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);


		// When character is VERY damaged, they can't change their speed as quickly
		float damageBias = (pc.healthMass/pc.maxHealthMass)*constantVelocityInfluence;
		// Convert back to local coordinates
		Vector3 directionToPush  = transform.InverseTransformDirection(velocityChange)*damageBias;
		directionToPush.y = 0;

		if (grounded)
		{

			rigidbody.AddRelativeForce(directionToPush, ForceMode.VelocityChange);
			
			// Jump
			if (canJump && Input.GetButton("Jump"))
			{
				grounded=false;
				// Calculate direction to move in local coordinates, then convert to world
				localDirection = transform.InverseTransformDirection(rigidbody.velocity) + new Vector3(0, CalculateJumpVerticalSpeed(), 0);
				rigidbody.velocity = transform.TransformDirection(localDirection);
			}
		} else {
			// We apply gravity manually for more tuning control
			rigidbody.AddRelativeForce(new Vector3 (0, -gravity * rigidbody.mass, 0));	
			
			rigidbody.AddRelativeForce(directionToPush*airControlHandicap, ForceMode.VelocityChange);

			
		}
		grounded=false;
	}

	private float CalculateJumpVerticalSpeed ()
	{
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * jumpHeight * gravity);
	}
	public void OnCollisionStay (Collision collision)
	{
		// Only ground the character if the collision normal is facing up, 
		// meaning we hit a ground rather than another game object
		if(transform.InverseTransformDirection(collision.contacts[0].normal).y>.1){
			grounded = true;	
		} else{
			//rigidbody.AddRelativeForce(new Vector3 (0, -gravity * rigidbody.mass, 0));	
		}
	}

	IEnumerator DisableDoubleTap(){
		yield return new WaitForSeconds(doubleTapSpeed);
		if (!sprinting){
			doubleTap=false;
		}
	}
}
