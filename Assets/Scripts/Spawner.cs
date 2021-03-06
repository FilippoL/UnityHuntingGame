﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MovementManager = UnityStandardAssets.Characters.ThirdPerson.MovementManager;

public class Spawner : MonoBehaviour {

	public GameObject spawn_what;
	public float radius;
	public bool fixed_number;
	public bool leader_based;
	public int leader_frequency;

	public Text AlertText;

	private List <GameObject> objects_spawned = new List<GameObject>();

	[Range(0,500)] public int max_objects;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < max_objects; i++) {
			Vector3 pos = new Vector3 (transform.position.x + RandomPointOnUnitCircle (radius).x, transform.position.y, transform.position.z + RandomPointOnUnitCircle (radius).y);
			Vector3 dir = transform.position - pos;
			
			try {
				objects_spawned.Add(Instantiate (spawn_what, pos,  Quaternion.LookRotation(dir * (i + 1))));
				if (leader_based) {
					if ((i % leader_frequency) == 0) {
						objects_spawned[i].tag = "Leader";
						objects_spawned[i].GetComponent<MovementManager>().canjump = true;	
						objects_spawned[i].GetComponent<FiniteStateMachine>().AlertText = AlertText;
					} 	
				}
				if ((i % 2) == 0 ) {
					objects_spawned[i].GetComponent<MovementManager>().canjump = true;	
				} 
				
			} catch (MissingReferenceException ex) {
				Debug.Log( ex.Message);
			}

		}
	}

	public static Vector2 RandomPointOnUnitCircle(float radius)
	{
		float angle = Random.Range (0f, Mathf.PI * 2);
		float x = Mathf.Sin (angle) * radius;
		float y = Mathf.Cos (angle) * radius;

		return new Vector2 (x, y);

	}

	// Update is called once per frame
	void Update () {

		/*if (fixed_number) {
			if (GameObject.FindGameObjectsWithTag ("Enemy").Length < max_objects) {
				Instantiate (spawn_what, new Vector3 (transform.position.x + RandomPointOnUnitCircle (radius).x, transform.position.y, transform.position.z + RandomPointOnUnitCircle (radius).y) , Quaternion.LookRotation(new Vector3(0.0f,0.0f,0.0f)));
			}
		}*/
	}
}
