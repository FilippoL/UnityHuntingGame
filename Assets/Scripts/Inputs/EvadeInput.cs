using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeInput : Inputs {
	public override Vector3 Steering {
		get{
			
			Vector3 final_evade = Vector3.zero;

			if (GetComponent<FiniteStateMachine>().hitPoint != Vector3.zero) {
				final_evade += Steerings.Evade (_origin, GetComponent<FiniteStateMachine> ().hitPoint);
			}

			return final_evade;
		}
	}
}
