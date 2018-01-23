using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Boundary : MonoBehaviour {

	[SerializeField] string PlayerObjName;
	[SerializeField] bool isTriggerEnter;

	void OnTriggerEnter(Collider i_other) {
		if(!isTriggerEnter) {
			return;
		}
		ResetPos (i_other);
	}

	void OnTriggerExit(Collider i_other) {
		if(isTriggerEnter) {
			return;
		}
		ResetPos (i_other);
	}

	void ResetPos(Collider i_other) {
		if(i_other.gameObject.name == PlayerObjName) {
			Debug.Log ("位置をリセット");
			Vector3 startPos = new Vector3 (0f, 2f, 0f);
			var sdkManager = VRTK_SDKManager.instance;
			if (sdkManager != null && sdkManager.loadedSetup.actualBoundaries != null) {
				sdkManager.loadedSetup.actualBoundaries.transform.position = startPos;
				sdkManager.loadedSetup.actualBoundaries.transform.rotation = Quaternion.identity;
			} else {
				GameObject.FindGameObjectWithTag("camerarig").transform.position = startPos;
				GameObject.FindGameObjectWithTag("camerarig").transform.rotation = Quaternion.identity;
			}
		}
	}

}
