using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmSwitcher : MonoBehaviour {

	[SerializeField] string m_fromArea;
	[SerializeField] string m_toArea;
	[SerializeField] string m_targetObjName = null;
	[SerializeField] string m_targetObjTag = null;
	[SerializeField] bool checkIsMine;

	bool isDisabled = false;

	void Start() {
		isDisabled = ModeManager.Instance.GetMode () == "PvE" && gameObject.transform.parent.tag == "Player";
	}

	void OnTriggerEnter(Collider i_other) {
		if(isDisabled) {
			return;
		}
		// TODO: Playerタグが取得できない謎
		if (checkIsMine) {
			if (IsOther (i_other)) {
				PlayAudio (i_other, m_toArea);
			}
		} else {
			PlayAudio(i_other, m_toArea);
		}
	}

	void OnTriggerExit(Collider i_other) {
		if(isDisabled) {
			return;
		}
		if (checkIsMine) {
			if (IsOther (i_other)) {
				PlayAudio (i_other, m_fromArea);
			}
		} else {
			PlayAudio(i_other, m_fromArea);
		}
	}

	void PlayAudio(Collider i_other, string i_area) {
		if(i_other.gameObject.name == m_targetObjName) {
			BgmManager.Instance.Play (i_area);
		}
		if(i_other.tag == m_targetObjTag) {
			BgmManager.Instance.Play (i_area);
		}
	}

	bool IsOther(Collider i_other) {
		var parentPhotonView = transform.parent.GetComponent<PhotonView>();
		return ((bool)parentPhotonView && !parentPhotonView.isMine);
	}

}
