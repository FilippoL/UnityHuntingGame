using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class MovementManager : MonoBehaviour {
	
		private ThirdPersonCharacter m_Character;

		public List <Inputs> _steerings = new List <Inputs>();

		// Use this for initialization
		void Start () {
			m_Character = GetComponent<ThirdPersonCharacter>();
		}

		// Update is called once per frame
		void Update () {
			
			if (_steerings.Count > 0) {
				Vector3 FinalSteer = Vector3.zero;

				foreach (var steering in _steerings) {
					FinalSteer += steering.Steering;
				}
				
				Debug.DrawRay (m_Character.transform.position, FinalSteer);
				m_Character.Move(FinalSteer, false, false);

			} else {
				m_Character.Move(Vector3.zero, false, false);
			}

		}

		public void AddSteering(Inputs v){
			_steerings.Add (v);
		}
		public void RemoveSteering(Inputs v){
			if (_steerings.Contains(v)) {
				_steerings.Remove (v);
			}
		}
		public void ClearSteerings(){
			_steerings.Clear();
		}
	}
}