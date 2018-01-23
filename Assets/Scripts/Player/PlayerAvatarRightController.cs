using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarRightController : MonoBehaviour {

	[SerializeField] GameObject m_gravityUI;

	public void SetGravityUI(Vector3 gravity) {
		string text = "Gravity\n";
			text += "x: " + GetRoundFloat(gravity.x).ToString() + "\n";
			text += "y: " + Mathf.Abs(GetRoundFloat(gravity.y)).ToString() + "\n";
			text += "z: " + GetRoundFloat(gravity.z).ToString();
		m_gravityUI.GetComponent<TextMesh> ().text = text;
	}

	float GetRoundFloat(float n) {
		return Mathf.Round (n * 100) / 100;
	}

}
