using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MovementManager = UnityStandardAssets.Characters.ThirdPerson.MovementManager;

public class FiniteStateMachine : MonoBehaviour {

	IEnumerator _currState;
	IEnumerator _nextState;

	private List <Transform> _neighbors = new List <Transform>();
	private Transform _leader;
	private int max__flock = 5;
	private bool _leader_locked = false;
	private Vector3 _hitpoint;
	private bool _flock;
	private bool _attacking;
	private float _time_since_here;


	public Vector3 hitPoint{		
		get { return _hitpoint; }
	}

	public bool attacking{		
		get { return _attacking; }
	}

	public bool flocking{		
		get { return _flock; }
	}
		
	public List <Transform> neighbors{		
		get { return _neighbors; }
	}

	public Transform leader{		
		get { return _leader; }
	}

	void Start () {
		_currState = Moving ();	
		StartCoroutine(StateMachine()); 
	}


	IEnumerator Moving(){
		GetComponent<MovementManager> ().ClearSteerings ();
		GetComponent<MovementManager> ().AddSteering(GetComponent<AvoidInput>());
		GetComponent<MovementManager> ().AddSteering(GetComponent<WanderInput>());
		while (_nextState == null) {
			if (Input.GetKeyDown(KeyCode.Tab)) {
				_nextState = Idling ();
			}


			yield return null;
		}

	}

	IEnumerator Alert(){
		GetComponent<MovementManager> ().ClearSteerings ();
		GetComponent<MovementManager> ().AddSteering(GetComponent<AvoidInput>());
		GetComponent<MovementManager> ().AddSteering(GetComponent<EvadeInput>());
		while (_nextState == null) {
			if (GetComponent<EvadeInput>().Steering == Vector3.zero) {
				_nextState = Moving ();
			}

			yield return null;
		}
	}

	IEnumerator Flock(){
		GetComponent<MovementManager> ().ClearSteerings ();
		GetComponent<MovementManager> ().AddSteering(GetComponent<AvoidInput>());
		GetComponent<MovementManager> ().AddSteering(GetComponent<FollowInput>());
		GetComponent<MovementManager> ().AddSteering(GetComponent<SeparationInput>());
		while (_nextState == null) {
			if (GetComponent<FollowInput>().Steering == Vector3.zero) {
				_nextState = Moving ();
			}
			yield return null;
		}

	}

	IEnumerator Attack(){
		GetComponent<MovementManager> ().ClearSteerings ();
		GetComponent<MovementManager> ().AddSteering(GetComponent<AvoidInput>());
		GetComponent<MovementManager> ().AddSteering(GetComponent<SeekInput>());
		_attacking = true;
		gameObject.GetComponentInChildren<Renderer> ().material.color = Color.red;

		while (_nextState == null) {


			yield return null;
		}

	}

	IEnumerator NeedsWater(){
		while (_nextState == null) {

			yield return null;
		}

	}

	IEnumerator Idling(){
		GetComponent<MovementManager> ().ClearSteerings ();
		GetComponent<MovementManager> ().AddSteering(GetComponent<AvoidInput>());
		while (_nextState == null) {


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
		if (coll.tag != null && coll.transform != transform) {
			if (!_leader_locked && coll.CompareTag("Leader") && gameObject.tag != "Leader" && 
				_neighbors.Count < max__flock && !coll.GetComponent<FiniteStateMachine>().attacking) { 

				_leader = coll.transform; //get the leader transform once so doesnt get confused with other leader's
				_flock = true; 
				_leader_locked = true;
				_nextState = Flock (); //change state
			} 

			if (coll.CompareTag("Enemy") && coll.GetComponent<FiniteStateMachine>().flocking && _flock) {
				_neighbors.Add (coll.transform);	
			}				

			if (coll.CompareTag("Bullet") && _currState != Alert() ) { //if is too close to the bullet or the player, evade it
				if (gameObject.CompareTag ("Leader") ) {
					if (_currState != Attack ()) {
						_nextState = Attack (); //change state
					} 

				} else {
					_leader = null;
					_flock = false;
					_leader_locked = false;
					_neighbors.Clear();

					if (_hitpoint == Vector3.zero) {
						_hitpoint = coll.transform.position;
					}
					_nextState = Alert ();
				}
			}
		}
	}

	private void OnTriggerExit(Collider coll){
		if (coll.tag != null) {
			if (coll.CompareTag ("Leader") && coll.transform == leader) {
				_leader = null;
				_flock = false;
				_leader_locked = false;
				_neighbors.Clear ();
				_nextState = Moving ();

			} else if (coll.CompareTag ("Enemy") && coll.GetComponent<FiniteStateMachine> ().flocking && _flock) {
				neighbors.Remove (coll.transform);
			}	
		}
	}

}
