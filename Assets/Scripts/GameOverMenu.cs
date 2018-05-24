
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour {

	public Text Score_Text;

	private int score;
	private int highscore;

	string highScoreKey = "HighScore";
	string currentScore = "Score";


	void Start () {
		Cursor.lockState = CursorLockMode.None;
		highscore = PlayerPrefs.GetInt (highScoreKey, 0);
		score = PlayerPrefs.GetInt (currentScore, 0);
	}

	void Update () {

		if (highscore < score) {
			Score_Text.text = "NEW HIGH SCORE: " + score;
		} else {
			Score_Text.text = "YOUR SCORE: " + score + "\nHIGHEST SCORE: " + highscore;
		}
			
	}
		
	public void GoToMainMenu(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 2);
	}

	public void QuitGame(){

		if (highscore < score) {
			PlayerPrefs.SetInt (highScoreKey, score);
			PlayerPrefs.Save ();
		}

		PlayerPrefs.SetInt (currentScore, 0);
		PlayerPrefs.Save ();

		Application.Quit ();
	}
}
