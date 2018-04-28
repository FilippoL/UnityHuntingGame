using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBack : MonoBehaviour {

	private void OnTriggerEnter(Collider coll){
		if (coll.CompareTag("Enemy")) {
			coll.transform.Rotate (new Vector3(0.0f,180.0f,0.0f));
		}

	}
}
