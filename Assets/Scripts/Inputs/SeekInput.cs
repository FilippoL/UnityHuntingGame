using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekInput : Inputs {

	public override Vector3 Steering {
		get{
			if (!_target) {
				if (GameObject.FindGameObjectWithTag ("Player").GetComponent<CapsuleCollider>() != null) {

					return Steerings.Seek (_origin, GameObject.FindGameObjectWithTag ("Player").GetComponent<CapsuleCollider> ().transform);

				}

				return Vector3.zero;
			}

			else{
				return Steerings.Seek(_origin, _target);
			}
		}
	}
}
