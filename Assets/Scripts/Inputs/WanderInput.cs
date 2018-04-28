using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderInput : Inputs {

	public override Vector3 Steering {
		get{
			
			return Steerings.Wander(_origin);
		}
	}
}
