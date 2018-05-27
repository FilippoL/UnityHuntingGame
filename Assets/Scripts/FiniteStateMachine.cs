using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MovementManager = UnityStandardAssets.Characters.ThirdPerson.MovementManager;

public class FiniteStateMachine : MonoBehaviour {

    /// <summary>
    /// Variables to hold states of FSM, used to swap a Coroutine (state) with another.
    /// </summary>
	IEnumerator _currState;
	IEnumerator _nextState;

    /// <summary>
    /// A List of transform of all the closest enemies
    /// </summary>
	[SerializeField]private List <Transform> _neighbors = new List <Transform>();

    /// <summary>
    /// The transform of the leader to be followed when flocking
    /// </summary>
	[SerializeField]private Transform _leader;

    /// <summary>
    /// Maximum number of a single flock
    /// </summary>
	private int max__flock = 5;

    /// <summary>
    /// A bool to check if the leader has already been found
    /// </summary>
	private bool _leader_locked = false;

    /// <summary>
    /// The point to evade when a bullet has been shot near the agent
    /// </summary>
	private Vector3 _hitpoint;

    /// <summary>
    /// Variable to tell if a leader is attacking
    /// </summary>
	private bool _attacking;

    /// <summary>
    /// The message to alert the user an enemy leader is chasing him 
    /// </summary>
	public Text AlertText;

    /// <summary>
    /// Getters
    /// </summary>
	public Vector3 hitPoint{		
		get { return _hitpoint; }
	}

	public bool attacking{		
		get { return _attacking; }
	}
		
	public List <Transform> neighbors{		
		get { return _neighbors; }
	}

	public Transform leader{		
		get { return _leader; }
	}

	void Start () {
        ///First coroutine will always be the move state
		_currState = Moving ();	
		StartCoroutine(StateMachine()); 
	}

    /// <summary>
    /// The function for showing the alert message on screen
    /// </summary>
	IEnumerator ShowMessage (string message, float delay) {
		AlertText.text = message;
		AlertText.enabled = true;
		yield return new WaitForSeconds(delay);
		AlertText.enabled = false;
	}

    /// <summary>
    /// Moving state will have Avoid Input and Wander Input
    /// </summary>
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

    /// <summary>
    /// Alert State will be entered when a bullet has been shot nearby
    /// </summary>
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

    /// <summary>
    /// Flock state will have avoid input, follow to follow the leader and separation to keep distance from other members of the flock
    /// </summary>
	IEnumerator Flock(){
		GetComponent<MovementManager> ().ClearSteerings ();
		GetComponent<MovementManager> ().AddSteering(GetComponent<AvoidInput>());
		GetComponent<MovementManager> ().AddSteering(GetComponent<FollowInput>());
		GetComponent<MovementManager> ().AddSteering(GetComponent<SeparationInput>());
		while (_nextState == null) {
			
			if (_neighbors.Count > max__flock || leader == null) {
				_leader = null;
				_leader_locked = false;
				_neighbors.Clear();
				_nextState = Idling (); //change state
			}
				
			yield return null;
		}

	}

    /// <summary>
    /// Attack will only be entered from leaders and will seek at full speed the player
    /// </summary>
	IEnumerator Attack(){

		StartCoroutine(ShowMessage ("...KILL THE LEADERS BEFORE THEY REACH YOU...", 3));

		GetComponent<MovementManager> ().ClearSteerings ();
		GetComponent<MovementManager> ().AddSteering(GetComponent<AvoidInput>());
		GetComponent<MovementManager> ().AddSteering(GetComponent<SeekInput>());
		_attacking = true;
		gameObject.GetComponentInChildren<Renderer> ().material.color = Color.red;

		while (_nextState == null) {


			yield return null;
		}

	}

    /// <summary>
    /// Idling is actually just a standby for 1.5 seconds, then the agent will go back to moving
    /// </summary>
	IEnumerator Idling(){
		GetComponent<MovementManager> ().ClearSteerings ();
		GetComponent<MovementManager> ().AddSteering(GetComponent<AvoidInput>());
		while (_nextState == null) {

			yield return new WaitForSeconds (1.5f);
			_nextState = Moving ();
			yield return null;
		}
	}

    /// <summary>
    /// The actual StateMachine will handle the state swap and loop
    /// </summary>
	IEnumerator StateMachine(){
		while (_currState != null) {
			yield return StartCoroutine (_currState);
			_currState = _nextState;
			_nextState = null;
		}
	}

    /// <summary>
    /// All the collision is checked in here and states are swapped accordingly
    /// </summary>
	private void OnTriggerEnter(Collider coll){
		if (coll.tag != null) { 
			if (!_leader_locked && coll.CompareTag("Leader") && gameObject.tag != "Leader" //If a leader has already been picked and the agent nearby is a leader then start flocking and following it
				&& !coll.GetComponent<FiniteStateMachine>().attacking) { 

				_leader = coll.transform; //get the leader transform once so doesnt get confused with other leader's
				_leader_locked = true;
				_nextState = Flock (); //change state
			} 
		    
			if (coll.CompareTag("Enemy") && _leader_locked && !_neighbors.Contains(coll.transform)) { 
				_neighbors.Add (coll.transform);
			}				


			if (coll.CompareTag("Bullet") && _currState != Alert() ) { //if a bullet has been detected
				if (gameObject.CompareTag ("Leader") ) {//if the agent is a leader will chase the player
					if (_currState != Attack ()) {
						_nextState = Attack (); //change state
					} 

				} else { //if is an enemy will stop flocking and Evade (Alert state) the point where the bullet has been shot
					_leader = null;
					_leader_locked = false;
					_neighbors.Clear();

					if (_hitpoint == Vector3.zero) { //set the closest point of the shot
						_hitpoint = coll.transform.position;
					}
					_nextState = Alert ();
				}
			}
		}
	}

    /// <summary>
    /// If an enemy is near the agent but is not flocking, then dont add it to the neighbors count
    /// Otherwise do add it.
    /// </summary>
	private void OnTriggerStay(Collider coll){
	
		if (gameObject.CompareTag("Enemy")) {
			if (coll.CompareTag("Enemy")) {
				if (coll.GetComponent<FiniteStateMachine>().leader == null) {
					if (_neighbors.Contains(coll.transform)) {
						_neighbors.Remove (coll.transform);
					}
				}
			}
		}
	}

    /// <summary>
    /// If the leader has left, top flocking and start Moving around
    /// </summary>
	private void OnTriggerExit(Collider coll){
		if (coll.tag != null) {
			if (coll.transform == _leader) {
				_leader = null;
				_leader_locked = false;
				_neighbors.Clear ();
				_nextState = Moving ();

			} else if (coll.CompareTag ("Enemy")) {
				if (_neighbors.Contains(coll.transform)) {
					_neighbors.Remove (coll.transform);
					
				}
			}	
		}
	}

}
