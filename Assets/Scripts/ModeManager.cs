using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour {

	private static ModeManager m_Instance;
	private static string modeName;

	public static ModeManager Instance {
		get {
			if (m_Instance == null) {
				GameObject obj = new GameObject ("ModeManager");
				m_Instance = obj.AddComponent<ModeManager> ();
			}
			return m_Instance;
		}
	}

	public void SetMode(string mode) {
		modeName = mode;
	}

	public string GetMode() {
		return modeName;
	}

}
