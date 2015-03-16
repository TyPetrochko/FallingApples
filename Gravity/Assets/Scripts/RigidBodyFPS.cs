using UnityEngine;
using System.Collections;

public class RigidBodyFPS : Photon.MonoBehaviour {
	public float speed = 10.0f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;

	public bool canJump = true;
	public float jumpHeight = .5f;
	public float jumpTimeOffset = .2f; // If you hit jump, the time window in which you have to hit the ground

	public bool grounded = false;
	public float slopeConstant = .1f; // A lower value means the character can walk up slopes
	public float airControlHandicap = .1f;
	public float speedMultiplierConstant = 1f;
	public float doubleTapSpeed = .3f;
	public float constantVelocityInfluence = .4f;// A value closer to one decreases the effect of a nudge


	private float jumpTimer;
	private bool doubleTap = false;


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
		jumpTimer = 0f;
		if(photonView.isMine){
			StartCoroutine(handleJumping());
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate(){
		// Only tamper with velocity when we're trying to move
		if(photonView.isMine && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)){

			// Calculate how fast we should be moving
			targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			
			// Convert it to world coordinates
			targetVelocity = transform.TransformDirection(targetVelocity);
			targetVelocity *= speed*speedMultiplierConstant;
			
			
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
			}else{
				rigidbody.AddRelativeForce(directionToPush*airControlHandicap, ForceMode.VelocityChange);
			}
		}else{
			Vector3 localVelocity = transform.InverseTransformDirection(rigidbody.velocity);
			// We don't want to make changes to Y direction
			localVelocity.y = 0;

			Vector3 directionToPush = -localVelocity*.1f;
			if(!grounded){
				directionToPush *= airControlHandicap;
			}
			rigidbody.AddRelativeForce(directionToPush, ForceMode.VelocityChange);


		}
		// Jump
		if (photonView.isMine && Input.GetButton("Jump") && canJump)
		{
			jumpTimer = jumpTimeOffset;
		}


		// We apply gravity manually for more tuning control
		rigidbody.AddRelativeForce(new Vector3 (0, -gravity * rigidbody.mass, 0));
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

	IEnumerator handleJumping () {
		while(true){
			if(jumpTimer>0){

				if (grounded)
				{
					grounded=false;
					jumpTimer = 0f;
					// Calculate direction to move in local coordinates, then convert to world
					localDirection = transform.InverseTransformDirection(rigidbody.velocity) + new Vector3(0, CalculateJumpVerticalSpeed(), 0);
					rigidbody.velocity = transform.TransformDirection(localDirection);
				}else {
					jumpTimer -= .05f;
				}
			}

			yield return new WaitForSeconds(.05f);
		}
	}

}
