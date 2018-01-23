using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode : MonoBehaviour {

	[SerializeField] string debugMode = null;

	[SerializeField] GameObject m_Practice;
	[SerializeField] GameObject m_vPlayer;
	[SerializeField] GameObject m_vEnermy;

	void Awake () {
		if(debugMode != "") {
			ModeManager.Instance.SetMode (debugMode);
		}

		string currentMode = ModeManager.Instance.GetMode ();
		Activate (currentMode);

		Debug.Log ("game mode is... " + currentMode);

	}

	void Activate(string mode) {
		switch (mode) {
		case "Practice":
			m_Practice.SetActive (true);
			break;
		case "PvP":
			m_vPlayer.SetActive (true);
			break;
		case "PvE":
			m_vEnermy.SetActive (true);
			break;
		}
	}

}
