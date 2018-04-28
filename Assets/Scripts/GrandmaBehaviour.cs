using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmaBehaviour : MonoBehaviour {
	//Empty target object with rot trans scale values
	public Transform Target;

	private Material _material;
	private Vector3 desired_direction;

	private Vector3 first_position;

	//Variables for movements
	public float translation_speed = 6.0F;
	public float rotation_speed = 1.0F;

	public enum states {COUNTING = 0, WATCHING, GAMEOVER};

	[System.NonSerialized]
	public states current_state;

	// Use this for initialization
	void Start () {
		_material = GetComponent<Renderer>().material;
		StartCoroutine (StateSwapper ());
	}

	// Update is called once per frame
	void Update () {

		switch (current_state) {

		case states.WATCHING:
			{
				_material.color = Color.Lerp(_material.color, Color.white, Mathf.PingPong(Time.deltaTime, 1));
				Watch ();
				break;
			}
		case states.COUNTING:
			{
				_material.color = Color.Lerp(_material.color, Color.yellow, Mathf.PingPong(Time.deltaTime, 1));
				Count ();
				break;
			}
		case states.GAMEOVER:
			{
				_material.color = Color.white;
				StopCoroutine (StateSwapper ());
				break;
			}
		default:
			break;
		}

	}


	void Watch()	{

		/*************************Same as seek but gradually decrease speed based on range**********/
		desired_direction = Target.position - transform.position;

		float distance = desired_direction.magnitude;
		float deceleration = distance / 10;

		float new_speed = translation_speed * deceleration;

		desired_direction.Normalize();
		Vector3 desired_velocity = desired_direction * (new_speed * Time.deltaTime);

		transform.right = desired_velocity.normalized;

		if (Target.position != first_position) {
			ChangeState (states.GAMEOVER);
		}
	}	

	void Count()	{


	
	}

	void ChangeState(states s)
	{
		current_state = s;
	}

	IEnumerator StateSwapper()
	{
		yield return null;

		while (true) {

			_material.color = Color.green;
			ChangeState (states.COUNTING);
			yield return new WaitForSeconds (Random.Range(3, 6));
				
			_material.color = Color.red;
			ChangeState (states.WATCHING);
			first_position = Target.position;
			yield return new WaitForSeconds (Random.Range(3, 6));
				
			if (current_state == states.GAMEOVER) {
				break;
			}
				
			}
	}


}
