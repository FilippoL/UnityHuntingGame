using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class SmoothMouseLook : MonoBehaviour {

	private float sensitivityX;
	private float sensitivityY;

	private float zooming_sensitivityX;
	private float zooming_sensitivityY;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationX = 0F;
	float rotationY = 0F;

	public bool zooming;

	private List<float> rotArrayX = new List<float>();
	float rotAverageX = 0F;	

	private List<float> rotArrayY = new List<float>();
	float rotAverageY = 0F;

	public float frameCounter = 20;

	Quaternion originalRotation;


	public float Sensibility
	{
		set{sensitivityX = value;
			sensitivityY = value;}

		get{return sensitivityX;}
	}

	public float ZoomingSensibility
	{
		set{zooming_sensitivityX = value;
			zooming_sensitivityY = value;}

		get{return zooming_sensitivityX;}
	}


	void Start ()
	{		
		sensitivityX = sensitivityY = 1;
		zooming_sensitivityX = zooming_sensitivityY = 0.1f;
		Rigidbody rb = GetComponent<Rigidbody>();	
		if (rb)
			rb.freezeRotation = true;
		originalRotation = transform.localRotation;
	}

	void Update ()
	{
		rotAverageY = 0f;
		rotAverageX = 0f;

		if (zooming) {
			rotationY += Input.GetAxis("Mouse Y") * zooming_sensitivityX;
			rotationX += Input.GetAxis("Mouse X") * zooming_sensitivityY;
		} else {
			rotationY += Input.GetAxis("Mouse Y") * sensitivityX;
			rotationX += Input.GetAxis("Mouse X") * sensitivityY;
		}

		rotArrayY.Add(rotationY);
		rotArrayX.Add(rotationX);

		if (rotArrayY.Count >= frameCounter) {
			rotArrayY.RemoveAt(0);
		}
		if (rotArrayX.Count >= frameCounter) {
			rotArrayX.RemoveAt(0);
		}

		for(int j = 0; j < rotArrayY.Count; j++) {
			rotAverageY += rotArrayY[j];
		}
		for(int i = 0; i < rotArrayX.Count; i++) {
			rotAverageX += rotArrayX[i];
		}

		rotAverageY /= rotArrayY.Count;
		rotAverageX /= rotArrayX.Count;

		rotAverageY = ClampAngle (rotAverageY, minimumY, maximumY);
		rotAverageX = ClampAngle (rotAverageX, minimumX, maximumX);

		Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
		Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);

		transform.localRotation = originalRotation * xQuaternion * yQuaternion;

	}

	public static float ClampAngle (float angle, float min, float max)
	{
		angle = angle % 360;
		if ((angle >= -360F) && (angle <= 360F)) {
			if (angle < -360F) {
				angle += 360F;
			}
			if (angle > 360F) {
				angle -= 360F;
			}			
		}
		return Mathf.Clamp (angle, min, max);
	}


}