using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SettingsScript : MonoBehaviour {

	public AudioMixer audioMixer;

	public Dropdown resolutionDropdown;


	Resolution[] resolutions;

	// Use this for initialization
	void Start () {
		resolutions = Screen.resolutions;

		resolutionDropdown.ClearOptions ();

		List<string> options = new List<string> ();

		int currentResolutionIndex = 0;
		bool found_curr_res = false;

		foreach (Resolution res in resolutions) {
			string option = res.width + "x" + res.height;

			if (!options.Contains(option)) {
				options.Add (option);
			}

			if (res.width == Screen.currentResolution.width &&
				res.height == Screen.currentResolution.height) {
				found_curr_res = true;
			}

			if (!found_curr_res) {
				currentResolutionIndex++;
			}
		}

		resolutionDropdown.AddOptions (options);
		resolutionDropdown.value = currentResolutionIndex;

	}

	public void SetResolution(int res)
	{
		Resolution resolution = resolutions [res];
		Screen.SetResolution (resolution.width, resolution.height, Screen.fullScreen);
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
