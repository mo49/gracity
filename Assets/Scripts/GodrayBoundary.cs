using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodrayBoundary : MonoBehaviour {

	[SerializeField] bool isTriggerEnter;

	void OnTriggerEnter(Collider i_other) {
		if(!isTriggerEnter){
			return;
		}
		Destroy (i_other.gameObject);
	}
	void OnTriggerExit(Collider i_other) {
		if(isTriggerEnter){
			return;
		}
		Destroy (i_other.gameObject);
	}

}
