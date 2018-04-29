using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowInput : Inputs {

	public override Vector3 Steering {
		get{
			if (GetComponent<FiniteStateMachine>().leader != null) {
				return Steerings.Follow(_origin, GetComponent<FiniteStateMachine>().leader);
			}

			else{
				return Vector3.zero;
			}
		}
	}
}
