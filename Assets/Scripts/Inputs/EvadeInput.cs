using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeInput : Inputs {
	public override Vector3 Steering {
		get{

			Vector3 final_evade = Vector3.zero;
			if (GetComponent<FiniteStateMachine>().neighbor.Count > 0) {
				foreach (var agent in GetComponent<FiniteStateMachine>().neighbor) {
					final_evade += Steerings.Separate (_origin, agent.position);
				}

				final_evade.x /= GetComponent<FiniteStateMachine> ().neighbor.Count;
				final_evade.z /= GetComponent<FiniteStateMachine> ().neighbor.Count;

			}

			if (GetComponent<FiniteStateMachine>().hitPoint != Vector3.zero) {
		

				final_evade += Steerings.Evade (_origin, GetComponent<FiniteStateMachine> ().hitPoint);

			}


			return final_evade;
		}
	}
}
