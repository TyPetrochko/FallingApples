using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
public class ShiftMouseLook : MonoBehaviour {
	
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -85F;
	public float maximumY = 85F;

	public float AimAssistConstant = 1f;
	
	float rotationY = 0F;
	
	void Update ()
	{

		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
			
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY* AimAssistConstant;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX* AimAssistConstant, 0);
		}
		else
		{
			// The amount we're trying to rotate
			var toRotate =  Input.GetAxis("Mouse Y") * sensitivityY* AimAssistConstant;
			// Make sure attempted look angle is not out of bounds
			if(!((transform.localEulerAngles.x + -toRotate) > (-minimumY) && (transform.localEulerAngles.x + -toRotate) < (360 - maximumY))){
				transform.Rotate(new Vector3(-toRotate, 0, 0), Space.Self);
			}else if (transform.localEulerAngles.x - toRotate < 180){
				// Looking too far down
				transform.localEulerAngles = new Vector3(-minimumY, 0, 0) ;
			}else{
				// Looking too far up
				transform.localEulerAngles = new Vector3(360-maximumY, 0, 0);
			}

		}
	}
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
}