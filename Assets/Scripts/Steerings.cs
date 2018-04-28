using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steerings {
	static public List <Vector3> RandomPositions = new List <Vector3>();

	public static Vector3 Flee(Transform Origin, Transform Target)	{

		//Empty target object with rot trans scale values
		Vector3 desired_direction = new Vector3();
		Vector3 moveVector;
		float translation_speed = 60.0F;

		desired_direction = Origin.position - Target.position;

		float distance = desired_direction.magnitude;
		float deceleration = distance / 15;

		float new_speed = translation_speed / (deceleration*2);

		desired_direction.Normalize();

		moveVector = desired_direction * (new_speed  );

		return moveVector;
	}

	public static Vector3 Arrive(Transform Origin, Transform Target)	{
		//Empty target object with rot trans scale values
		Vector3 desired_direction = new Vector3();
		Vector3 moveVector;
		float translation_speed = 10.0F;

		desired_direction = Target.position - Origin.position;

		float distance = desired_direction.magnitude;

		float deceleration = distance / 5;

		float new_speed = translation_speed * deceleration;

		Origin.rotation = Quaternion.Slerp (Origin.rotation, Quaternion.LookRotation (desired_direction), 5  );

		desired_direction.Normalize();

		moveVector = desired_direction * (new_speed  );

		return moveVector.normalized;
	}	
		

	public static Vector3 Idle(Transform Origin, Transform Target)	{

		return Vector3.zero;
	}	

	public static Vector3 Pursuit(Transform Origin, Transform Target)	{
	
		Vector3 desired_direction = new Vector3();
		Vector3 moveVector;
		int max_velocity = 1;
		int iteration_ahead = 5;
		int safe_distance = 4;
		Vector3 target_velocity = Vector3.zero;


		Vector3 future_pos = Target.position + (target_velocity * iteration_ahead);

		desired_direction = future_pos - Origin.position;

		float distance = desired_direction.magnitude;

		float deceleration;
		if (distance > 1f) {
			if (distance > safe_distance) {
				moveVector = desired_direction * Mathf.Min((distance), max_velocity)  ;
				return moveVector;
			} else {
				deceleration = distance / 10;
				moveVector = desired_direction * deceleration;
				return moveVector;
			} 
		}	else {
			return Vector3.zero;
		}

	}

	public static Vector3 Evade(Transform Origin, Vector3 Target)	{

		//Empty target object with rot trans scale values
		Vector3 desired_direction = new Vector3();
		Vector3 moveVector;

		int safeDistance = 10;
		desired_direction =  Origin.position - Target;

		if (desired_direction.magnitude < safeDistance) {

			moveVector = desired_direction.normalized;

			Debug.DrawRay (Origin.position, moveVector, Color.magenta);
			return moveVector.normalized;
		} else {
		
			return Vector3.zero;
		}

	}

	public static Vector3 Separate(Transform Origin, Vector3 Target)	{

		//Empty target object with rot trans scale values
		Vector3 desired_direction = new Vector3();
		Vector3 moveVector;

		desired_direction =  Origin.position - Target;

		moveVector = desired_direction.normalized;

		Debug.DrawRay (Origin.position, moveVector, Color.grey);
			
		return moveVector.normalized;
	}
		

	public static Vector3 Follow(Transform Origin, Transform Target)	{
		//Empty target object with rot trans scale values
		Vector3 desired_direction = new Vector3();
		Vector3 moveVector;

		Vector3 target_velocity = Target.gameObject.GetComponent<Rigidbody> ().velocity;

		desired_direction = (Target.TransformPoint(new Vector3(0.0f,0.0f,-0.75f))) - Origin.position;

		desired_direction.Normalize();

		moveVector = desired_direction * target_velocity.magnitude;

		return moveVector.normalized;
	}	

	public static Vector3 Wander (Transform Origin)
	{
		float maxJitter = 0.25f;
		Vector3 desired_direction = Origin.forward;
		desired_direction = new Vector3 (desired_direction.x + Random.Range(-maxJitter, maxJitter), 0, desired_direction.z + Random.Range(-maxJitter, maxJitter));
		return desired_direction.normalized;
	}


	public static Vector3 Avoid (Transform Origin)
	{
		Rigidbody rb = Origin.gameObject.GetComponent<Rigidbody> ();

		int max_velocity = 1;

		Vector3 ahead = Origin.position + Origin.forward + (Mathf.Min(rb.velocity.magnitude, 10) * rb.velocity.normalized);

		List <Vector3> most_threatening = new List <Vector3>();
		Collider[] hit_Colliders = Physics.OverlapSphere(ahead, 2f);

		int closest_distance_index = 0;
		Vector3 direction_to_obstacle = new Vector3();

		foreach (var item in hit_Colliders) {
			if (item.CompareTag("Obstacle")) {
				most_threatening.Add (item.ClosestPoint (Origin.position));	
			}else if (item.CompareTag("Enemy") && Origin.gameObject.CompareTag("Leader") ) {
				most_threatening.Add (item.ClosestPoint (Origin.position));	
			}
		}

		for (int i = 0; i < most_threatening.Count; i++) {
			if(Vector3.Dot((most_threatening[i] - Origin.position).normalized, Origin.forward) > 0.5f){
				if (Vector3.Distance(Origin.position, most_threatening[i]) >
					Vector3.Distance(Origin.position, most_threatening[closest_distance_index])) {
					closest_distance_index = i;
					Debug.DrawLine (Origin.position, most_threatening[i], Color.green);
				} 
			}
		}

		if (most_threatening.Count != 0) {
			Debug.DrawLine (Origin.position, most_threatening[closest_distance_index], Color.red);
			Debug.DrawLine (Origin.position, ahead, Color.cyan);

			direction_to_obstacle = most_threatening[closest_distance_index] - Origin.position;
			
			direction_to_obstacle.y = 0.0f;
			
			Vector3 avoid_direction = new Vector3(-direction_to_obstacle.z, direction_to_obstacle.y, direction_to_obstacle.x).normalized;

			float sign = Mathf.Sign (Vector3.Dot (Origin.right, direction_to_obstacle));

			float forceMult = Mathf.Min(direction_to_obstacle.magnitude * 10, max_velocity ); 

			Debug.DrawRay (Origin.position, avoid_direction * forceMult * sign, Color.blue);

			return (avoid_direction * forceMult * sign).normalized;

		} else {
			return Vector3.zero;
		}
	
	}


}
