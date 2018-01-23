using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour {

	private static DamageManager m_Instance;
	private static string modeName;

	public static DamageManager Instance {
		get {
			if (m_Instance == null) {
				GameObject obj = new GameObject ("DamageManager");
				m_Instance = obj.AddComponent<DamageManager> ();
			}
			return m_Instance;
		}
	}

	public int GetDamagePoint(string tagName) {
		int damagePoint = 0;
		switch (tagName) {
			case "Bullet/Gun":
				damagePoint = 1;
				break;
			case "Magic/Thunder":
				damagePoint = 5;
				break;
			case "Magic/Fire":
				damagePoint = 8;
				break;
			case "Magic/Dark":
				damagePoint = 10;
				break;
		}
		return damagePoint;
	}
}
