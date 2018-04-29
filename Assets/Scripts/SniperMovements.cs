using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SniperMovements : MonoBehaviour
{
	public Camera cam;
	public GameObject bulletObj;
	public float bulletSpeed;
	public Animator animator;
	[System.NonSerialized] public bool isScope;
	public AudioClip shooting;
	public AudioClip reloading;

	public AudioSource audio_source;
	//Need the initial camera FOV so we can zoom
	float initialFOV;
	//To change the zoom
	int currentZoom = 1;

	//To change sensitivity when zoomed
	SmoothMouseLook mouseLook;
	public float standardSensitivity;
	float zoomSensitivity = 0.1f;

	void Start() 
	{
		//Lock and hide the mouse cursor 
		Cursor.lockState = CursorLockMode.Locked;

		initialFOV = cam.fieldOfView;

		mouseLook = GetComponent<SmoothMouseLook>();

	}

	void Update() 
	{   
		//animation stuff
		if ((Input.GetMouseButtonUp(1) ||Input.GetKeyDown (KeyCode.LeftShift))) {
			isScope = !isScope;
			animator.SetBool ("Scope", isScope);
		} 

		if (!isScope) {
			standardSensitivity = mouseLook.Sensibility;
			currentZoom = 1;
		} 


		ZoomSight();
		StartCoroutine(FireBullet());			

	}

	void ZoomSight()
	{
		if (currentZoom == 1) 
		{
			//Change sensitivity
			mouseLook.Sensibility = standardSensitivity;
		}
		else 
		{
			//Change sensitivity
			mouseLook.Sensibility = zoomSensitivity;
		}

		//Zoom with mouse wheel
		if (Input.GetAxis("Mouse ScrollWheel") > 0 && isScope)
		{
			currentZoom += 1;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0 && isScope)
		{
			currentZoom -= 1;
		}

		//Keep it between 1 and 11, then add 1 when zoom because zoom is between 3 and 12 times
		currentZoom = Mathf.Clamp(currentZoom, 1, 15);

		//No zoom
		if (currentZoom == 1)
		{
			cam.fieldOfView = initialFOV / (float)currentZoom;
		}
		//Zoom the sight
		else
		{
			cam.fieldOfView = initialFOV / ((float)currentZoom + 1f);
		}
	}

	IEnumerator FireBullet()
	{
		if (isScope) {
			if ((Input.GetMouseButtonDown (0) || Input.GetKeyDown ("space")) && !audio_source.isPlaying && Time.timeScale != 0) {
				//Create a new bullet
				GameObject newBullet = Instantiate (bulletObj, cam.transform.position, cam.transform.rotation);

				audio_source.PlayOneShot (shooting);

				//Give it speed
				newBullet.GetComponent<BulletMover> ().speed = bulletSpeed * cam.transform.forward;

				yield return new WaitForSeconds (1);
		
				audio_source.PlayOneShot (reloading);

			}
		}
	}


	private void OnTriggerEnter(Collider coll)
	{
		if (coll.CompareTag ("Leader") && coll.gameObject.GetComponent<FiniteStateMachine>().attacking) {

			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 1);

			Debug.Log ("GAMEOVER");
		}
	}


}

	