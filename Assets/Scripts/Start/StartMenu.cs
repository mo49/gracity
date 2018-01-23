using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

// シミュレーター用
public class StartMenu : MonoBehaviour {

	[SerializeField] GameObject m_linkStart;
	[SerializeField] Material m_linkStartMaterial;

	void Update() {
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			ActivateLinkStart ("PvP");
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)){
			ActivateLinkStart ("PvE");
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)){
			ActivateLinkStart ("Practice");
		}
	}

	void ActivateLinkStart(string mode) {
		ModeManager.Instance.SetMode (mode);

		RenderSettings.skybox = m_linkStartMaterial;
		m_linkStart.SetActive (true);
		gameObject.SetActive (false);
	}

}
