using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MovementManager = UnityStandardAssets.Characters.ThirdPerson.MovementManager;

public class FiniteStateMachine : MonoBehaviour {

	IEnumerator _currState;
	IEnumerator _nextState;
	private Vector3 _hitpoint;
	private bool flock;
	public List <Transform> neighbor = new List <Transform>();
	public Transform leader;
	private bool leader_locked = false;

	public Vector3 hitPoint{		
		get { return _hitpoint; }
		set { _hitpoint = value; }
	}


	// Use this for initialization
	void Start () {
		_currState = Moving ();	
		StartCoroutine(StateMachine()); 
	}


	IEnumerator Moving(){
		GetComponent<MovementManager> ().AddSteering(GetComponent<AvoidInput>());
		GetComponent<MovementManager> ().AddSteering(GetComponent<WanderInput>());

		while (_nextState == null) {
			if (Input.GetKeyDown(KeyCode.Tab)) {
				_nextState = Idling ();
				GetComponent<MovementManager> ().ClearSteerings ();
			}
			if (!leader_locked && flock  && leader != null) {
				GetComponent<MovementManager> ().AddSteering(GetComponent<FollowInput>());
				leader_locked = true;
			} 
			if (!flock){
				neighbor.Clear ();
			}

			yield return null;
		}

	}

	IEnumerator Alert(){
		GetComponent<MovementManager> ().AddSteering(GetComponent<AvoidInput>());
		GetComponent<MovementManager> ().AddSteering(GetComponent<EvadeInput>());

		while (_nextState == null) {
			if (GetComponent<EvadeInput>().Steering == Vector3.zero) {
				_hitpoint = Vector3.zero;
				GetComponent<MovementManager> ().ClearSteerings ();
				_nextState = Moving ();
			}
			yield return null;
		}
	}

	IEnumerator NeedsWater(){
		while (_nextState == null) {

			yield return null;
		}

	}

	IEnumerator Idling(){
		GetComponent<MovementManager> ().AddSteering(GetComponent<AvoidInput>());

		while (_nextState == null) {
			if (Input.GetKeyDown(KeyCode.Tab)) {
				_nextState = Moving ();
				GetComponent<MovementManager> ().ClearSteerings ();
			}

			yield return null;
		}
	}

	IEnumerator StateMachine(){
		while (_currState != null) {
			yield return StartCoroutine (_currState);
			_currState = _nextState;
			_nextState = null;
		}
	}

	private void OnTriggerEnter(Collider coll){
		if (coll.tag != null) {
			if (coll.CompareTag("Bullet") || coll.CompareTag("Player") && _currState != Alert() ) {
				GetComponent<MovementManager> ().ClearSteerings ();
				flock = false;
				if (_hitpoint == Vector3.zero) {
					_hitpoint = coll.transform.position;
					Debug.Log (_hitpoint);
				}
				_nextState = Alert ();
				
			}

			if (coll.CompareTag("Leader") &&  !coll.Equals(GetComponent<GameObject>())) {
				flock = true;
				GetComponent<MovementManager> ().RemoveSteering (GetComponent<WanderInput>());
				leader = coll.transform;
			}
			
			if (flock) {
				if (coll.CompareTag ("Enemy")) {
					neighbor.Add (coll.transform);	
					GetComponent<MovementManager> ().AddSteering(GetComponent<EvadeInput>());

				}
			}
		}
	
	}

	private void OnTriggerExit(Collider coll){
		if (coll.tag != null) {

			if (coll.CompareTag("Leader") &&  !coll.Equals(GetComponent<GameObject>())) {
				leader = null;
				flock = false;
				leader_locked = false;
				neighbor.Clear();
				GetComponent<MovementManager> ().RemoveSteering (GetComponent<FollowInput>());
				GetComponent<MovementManager> ().AddSteering (GetComponent<WanderInput>());

			}
			if (coll.CompareTag ("Enemy")) {
				neighbor.Add (coll.transform);	
				GetComponent<MovementManager> ().RemoveSteering(GetComponent<EvadeInput>());

			}

		}
	
	}

}
