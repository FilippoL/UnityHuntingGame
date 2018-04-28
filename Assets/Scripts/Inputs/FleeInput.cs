using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeInput : Inputs {

	public override Vector3 Steering {
		get{
			if (!_target) {
				/*if (LookForTarget() != null) {

					return Steerings.Flee (_origin, LookForTarget ());
				} else {
					return Vector3.zero;
				}*/
				return Vector3.zero;
			}

			else{
				return Steerings.Flee(_origin, _target);
			}
		}
	}
}
