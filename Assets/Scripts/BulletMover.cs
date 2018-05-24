using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour {

	[System.NonSerialized]public Vector3 speed;
	private Camera cam;

	private bool goingToHit;
//	private Vector3 OriginalPos;

	private GameObject score;

	// Use this for initialization
	void Start () {
		GetComponent<Transform> ().Rotate (new Vector3(90.0f,0.0f,0.0f));
		GetComponent<Rigidbody>().AddForce(speed);
		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
	}

	void Update(){

	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag ("Enemy")||collision.gameObject.CompareTag ("Leader")) {
			Destroy (collision.gameObject);
			score = GameObject.FindGameObjectWithTag("ScoreText");
			score.GetComponent<ScoreHandler>().IncreaseScore ();
			Debug.Log ("HIT");
		}
		if (!collision.collider.isTrigger) {
			
			Destroy (this.gameObject);
		}
	}

}
