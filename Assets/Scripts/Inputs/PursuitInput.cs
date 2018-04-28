using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitInput : Inputs {

	public override Vector3 Steering {
		get{
			if (!_target) {
				/*if (LookForTarget() != null) {

					return Steerings.Pursuit (_origin, LookForTarget ());
				} else {
				}*/
				return Vector3.zero;
			}

			else{
				return Steerings.Pursuit(_origin, _target);
			}
		}
	}
}
