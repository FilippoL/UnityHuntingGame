using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowInput : Inputs {

	public override Vector3 Steering {
		get{
			if (GetComponent<FiniteStateMachine>().leader.position != Vector3.zero) {

				return Steerings.Follow(_origin, GetComponent<FiniteStateMachine>().leader);

			}

			else{
				return Vector3.zero;
			}
		}
	}
}
