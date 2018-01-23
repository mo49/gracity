using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarLeftController : Photon.MonoBehaviour {

	[SerializeField] GameObject m_myHealthPoint;
	[SerializeField] TextMesh m_countdownUI;
	Countdown m_countdown = null;

	PhotonView m_photonView;

	void Awake() {
		m_photonView = GetComponent<PhotonView> ();

	}

	public void StartCountdown() {
		// countdownがアクティブになったら
		m_countdown = GameObject.Find("Countdown").GetComponent<Countdown>();
	}
	
	public void SetHP(int hp) {
		string text = "Player Id: " + PhotonNetwork.player.ID + "\n";
		text += "HP: " + hp + "\n";
		m_myHealthPoint.GetComponent<TextMesh> ().text = text;
	}

	void Update() {
		if(!m_countdown) {
			return;
		}
		if(m_photonView.isMine && ModeManager.Instance.GetMode() == "PvE") {
			if(m_countdown.isCompleted) {
				return;
			}
			m_countdownUI.text = "Time: " + Mathf.Round( m_countdown.leftTime ).ToString();
		}
	}
}
