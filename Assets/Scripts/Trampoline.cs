using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour {

	[SerializeField] string targetObjName;

	void OnTriggerEnter(Collider i_other) {

		if(i_other.gameObject.name == targetObjName){
			float power = Random.Range (25, 30);
			var camerarig = GameObject.FindGameObjectWithTag ("camerarig");
			camerarig.transform.GetComponent<Rigidbody> ().AddForce (camerarig.transform.up * power, ForceMode.VelocityChange);
		}

	}
}
