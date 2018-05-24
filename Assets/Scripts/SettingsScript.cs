using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SettingsScript : MonoBehaviour {

	public AudioMixer audioMixer;

	// Use this for initialization
	void Start () {

	}
		
		
	public void SetVolume(float volume)
	{
		audioMixer.SetFloat ("VolumeExposed", volume);
	}

	public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel (qualityIndex);
	}

	public void SetFullScreen(bool isFullScreen)
	{
		Screen.fullScreen = isFullScreen;
	}

}
