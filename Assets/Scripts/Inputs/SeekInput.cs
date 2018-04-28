using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekInput : Inputs {

	public override Vector3 Steering {
		get{
			if (!_target) {
				/*if (LookForTarget() != null) {

					return Steerings.Arrive (_origin, LookForTarget ());
				} else {
				}*/
				return Vector3.zero;
			}

			else{
				return Steerings.Arrive(_origin, _target);
			}
		}
	}
}
