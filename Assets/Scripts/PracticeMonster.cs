using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeMonster : MonoBehaviour {

	Animator animator;

	void Awake() {
		animator = GetComponent<Animator> ();
	}

	void OnTriggerEnter(Collider other) {
		if(TagUtility.getParentTagName(other.gameObject) == "Magic") {
			animator.SetTrigger ("Get Hit");
		}
	}
}
