using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A neater overview of some of the following behaviours can be found in the last section of the README.md 
/// </summary>
public class Steerings {
	static public List <Vector3> RandomPositions = new List <Vector3>();

    /// <summary>
    /// Seek is very simple behaviour, the agent will just chase at full speed
    /// </summary>
    /// <param name="Origin"> The origin Transform</param>
    /// <param name="Target"> The Target Transform</param>
    /// <returns> Return the vector to the target, not range based</returns>
	public static Vector3 Seek(Transform Origin, Transform Target)	{
	
		Vector3 desired_direction = new Vector3();
		Vector3 moveVector;

		desired_direction = Target.position - Origin.position;

		moveVector = desired_direction;

		return moveVector.normalized;
	}

    /// <summary>
    /// Evade is the opposite of seeking, but this time will evade up to a certain distance and then stop (will change state in this case)
    /// </summary>
    /// <param name="Origin"> The origin Transform</param>
    /// <param name="Target"> The Target Transform</param>
    /// <returns>Returns the vector opposite of the direction to the Target</returns>
	public static Vector3 Evade(Transform Origin, Vector3 Target)	{

		//Empty target object with rot trans scale values
		Vector3 desired_direction = new Vector3();
		Vector3 moveVector;

		int safeDistance = 10;
		desired_direction =  Origin.position - Target;

		if (desired_direction.magnitude < safeDistance) {

			moveVector = desired_direction.normalized;

			Debug.DrawRay (Origin.position, moveVector, Color.magenta);
			return moveVector;

		} else {
		
			return Vector3.zero;
		}

	}

    /// <summary>
    /// Separate will make the agents never get too close when flocking, this takes in account the number of memebers of the flock
    /// </summary>
    /// <param name="Origin"> The origin Transform</param>
    /// <param name="Target"> The List of Target Transform, all the targets are needed to calculate the right distance to be kept between each</param>
    /// <returns>returns the Vector pushing away from Targets</returns>
    public static Vector3 Separate(Transform Origin, List<Transform> Targets)	{

		//Empty target object with rot trans scale values
		Vector3 desired_direction = new Vector3();
		Vector3 moveVector;
		float radius = 4f;

		moveVector = desired_direction;

		foreach (var neighbor in Targets) {
			if (neighbor != null) {
				if (Vector3.Distance (Origin.position, neighbor.position) < radius) {

					desired_direction = neighbor.position - Origin.position;

					float scale = desired_direction.magnitude / (float)Mathf.Sqrt (radius);

					Debug.DrawRay (Origin.position, desired_direction / scale, Color.blue);

					moveVector += desired_direction / scale;

				}
			}
		}

		moveVector /= Targets.Count;
		
		moveVector *= -1;
		return moveVector;
	}

    /// <summary>
    /// Follow is like seek but it will aim for a point specified in local coordinates near the target, also the velocity is range based
    /// </summary>
    /// <param name="Origin"> The origin Transform</param>
    /// <param name="Target"> The Target Transform</param>
    /// <returns>Returns the vector leading to a point near the target specified in local coordinates</returns>
    public static Vector3 Follow(Transform Origin, Transform Target)	{
		//Empty target object with rot trans scale values

		Vector3 desired_direction = new Vector3();
		Vector3 moveVector;

		desired_direction = (Target.TransformPoint(new Vector3(0.0f,0.0f,-1.15f))) - Origin.position;

		Debug.DrawLine (Origin.position, Origin.position + desired_direction, Color.red);

		moveVector = (desired_direction);

		return moveVector;
	}	

    /// <summary>
    /// Wander is just picking up a random position every time
    /// </summary>
    /// <param name="Origin">Just needs the origin point</param>
    /// <returns>Returns the direction to a random point on the map</returns>
	public static Vector3 Wander (Transform Origin)
	{
		float maxJitter = 0.10f;
		Vector3 desired_direction = Origin.forward;
		desired_direction = new Vector3 (desired_direction.x + Random.Range(-maxJitter, maxJitter), 0, desired_direction.z + Random.Range(-maxJitter, maxJitter));
		return desired_direction;
	}

    /// <summary>
    /// Avoiding will make the agent avoid obstacles by checking the closest ones getting the a vector which is basically the "flipped"(/ -> \) direction towards it
    /// </summary>
    /// <param name="Origin">Just needs the origin point</param>
    /// <returns>Returns the vector pushing away from the obstacle</returns>
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
			if (item.CompareTag("Obstacle") || item.CompareTag("Enemy") && Origin.gameObject.CompareTag("Leader")) {
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

			return (avoid_direction * forceMult * sign);

		} else {
			return Vector3.zero;
		}
	
	}


}
