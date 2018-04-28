using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {

	[SerializeField, Range(1.0f, 1000.0f)] float wind_force = 500.0f;
	[SerializeField] Vector3 _direction = new Vector3();


	private void OnValidate(){
		_direction.Normalize ();	
	}


	private void OnTriggerStay(Collider other){

		var otherbody = other.gameObject.GetComponent<Rigidbody> ();
		if (otherbody != null) {

			var distance = other.transform.position - transform.position;

			otherbody.AddForce (_direction * (wind_force - distance.magnitude) * Time.deltaTime, ForceMode.Force);

		}

	}
		
}
