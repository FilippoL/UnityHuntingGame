using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateComponent : MonoBehaviour {

	public Animator animator;

	// Update is called once per frame
	void Update () {
		GetComponent<Canvas> ().enabled = animator.GetBool ("Scope");
	}
}
