using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour {

	[System.NonSerialized]public Vector3 speed;
	private Camera cam;

	private bool goingToHit;
//	private Vector3 OriginalPos;


	// Use this for initialization
	void Start () {
		GetComponent<Transform> ().Rotate (new Vector3(90.0f,0.0f,0.0f));
		GetComponent<Rigidbody>().AddForce(speed);

	}

	void Update(){

		int layerMask = 1 << 8;

		layerMask = ~layerMask;


		RaycastHit hit;

		if (Physics.Raycast(transform.position, transform.TransformDirection(-transform.forward), out hit, Mathf.Infinity, layerMask)) {
			Debug.DrawRay (transform.position, transform.TransformDirection(-transform.forward));
			if (hit.collider.CompareTag("Enemy")) {
				goingToHit = true;
				cam = GameObject.FindGameObjectWithTag ("Player").GetComponent<Camera>();
				//OriginalPos = cam.transform.position;
			}
		}


		if (goingToHit) {
				
			cam.transform.SetPositionAndRotation (transform.position, Quaternion.LookRotation (cam.transform.TransformPoint (new Vector3 (-1.0f, 0.0f, 0.0f))));
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag ("Enemy")) {
			Destroy (collision.gameObject);
			Debug.Log ("HIT");
		}
		if (!collision.collider.isTrigger) {
			
			Destroy (this.gameObject);
		}
	}

}
