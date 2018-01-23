using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour {

	[SerializeField] float m_speed = 1f;
	[SerializeField] float m_radius = 1.5f;

	float time;
	Vector3 pos;

	void Start() {
		pos = new Vector3(0f,6f,0f);	
	}

	void Update () {
		pos.x = Mathf.Sin (Time.time * m_speed) * m_radius;
		pos.z = Mathf.Cos (Time.time * m_speed) * m_radius;
		transform.localPosition = pos;
	}
}
