using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidInput : Inputs {

	public override Vector3 Steering {
		get{
			return Steerings.Avoid (_origin);
		}
	}
}
