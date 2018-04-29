using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class SmoothMouseLook : MonoBehaviour {

	private float sensitivityX;
	private float sensitivityY;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationX = 0F;
	float rotationY = 0F;

	private List<float> rotArrayX = new List<float>();
	float rotAverageX = 0F;	

	private List<float> rotArrayY = new List<float>();
	float rotAverageY = 0F;

	public float frameCounter = 20;

	Quaternion originalRotation;



	void Start ()
	{		
		sensitivityX = sensitivityY = 1;
		Rigidbody rb = GetComponent<Rigidbody>();	
		if (rb)
			rb.freezeRotation = true;
		originalRotation = transform.localRotation;
	}

	void Update ()
	{
		rotAverageY = 0f;
		rotAverageX = 0f;

		rotationY += Input.GetAxis("Mouse Y") * sensitivityX;
		rotationX += Input.GetAxis("Mouse X") * sensitivityY;

		Debug.Log ( sensitivityX);
		Debug.Log ( sensitivityY);

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

	public float Sensibility
	{
		set{sensitivityX = value;
			sensitivityY = value;}

		get{return sensitivityX;}
	}
}