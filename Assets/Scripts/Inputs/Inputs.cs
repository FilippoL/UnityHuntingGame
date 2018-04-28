using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour {
	protected Transform _target; 
	protected Transform last_rot;
	protected Transform _origin;

	public virtual Vector3 Steering {
		get{ 
			return Vector3.zero;
		}
	}

	void Start () {
		 
		_origin = GetComponent<Transform> ();
	}

	/*	
	public Transform LookForTarget (){
		
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 50);
		foreach (var item in hitColliders) {
			if (item.gameObject.tag == "Bullet") {

					return item.transform;
			} 
		}
	
		return null;

	}*/
		
}
