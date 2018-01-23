using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

	[SerializeField] float m_duration;

	void Start () {
		Destroy (this.gameObject, m_duration);
	}
}
