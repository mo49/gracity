using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayerAvatarHeadController : Photon.MonoBehaviour {

	[SerializeField] TextMesh resultUI;
	[SerializeField] AudioClip victorySound;

	VRTK_HeadsetFade headsetFade;
	AudioSource _audio;

	void Start() {
		headsetFade = GameObject.Find ("PlayArea").GetComponent<VRTK_HeadsetFade> ();
		_audio = GetComponent<AudioSource> ();
	}

	public void ShowResultUI(string result) {
		string resultText = null;
		if(result == null) {
			return;
		}
		headsetFade.Fade (new Color(0f,0f,0f,0.3f), 0.5f);

		// win
		if(result == "win") {
			_audio.PlayOneShot(victorySound);
			resultUI.color = Color.yellow;
			resultText = "Mission Complete!";
		}
		else if(result == "lose") {
			resultUI.color = Color.white;
			resultText = "Time Over...";
			Invoke ("StopCompletely", 1f);
		}

		resultUI.text = resultText;
	}

	void StopCompletely() {
		Time.timeScale = 0;
	}
	
}
