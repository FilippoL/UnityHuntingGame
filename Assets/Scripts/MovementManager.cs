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

		public bool canjump{		
			get { return _canjump; }
			set { _canjump = value; }
		}

		private bool _canjump;

		public bool crouched{		
			get { return _crouched; }
			set { _crouched = value; }
		}

		private bool _crouched;


		public bool jumping{		
			get { return _jumping; }
			set { _jumping = value; }
		}
		private bool _jumping;

		// Use this for initialization
		void Start () {
			m_Character = GetComponent<ThirdPersonCharacter>();
		}

		// Update is called once per frame
		void Update () {
			
			if (_steerings.Count > 0) {
				Vector3 FinalSteer = Vector3.zero;

				foreach (var steering in _steerings) {
					if (GetComponent<EvadeInput> ().Steering != Vector3.zero && _canjump) {
						_jumping = true;
					} else {
						_jumping = false;
					}
					FinalSteer += steering.Steering;
				}

				Debug.DrawRay (m_Character.transform.position, FinalSteer);
				m_Character.Move (FinalSteer.normalized, false, _jumping);				
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