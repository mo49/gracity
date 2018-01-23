using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SetupPlayerAvatar : Photon.MonoBehaviour {

	VRTK_HeadsetFade headsetFade;

	void Awake() {
		if (!photonView.isMine) {
			return;
		}
		headsetFade = GameObject.Find ("PlayArea").GetComponent<VRTK_HeadsetFade> ();
		headsetFade.Fade (Color.black, 0f);
	}
		
	void Start () {
        if (!photonView.isMine) {
            return;
        }
		var sdkManager = VRTK_SDKManager.instance;
		// VRTK_SDKManagerm, Simulatorにはないっぽい？
		if (sdkManager != null && sdkManager.loadedSetup.actualBoundaries != null) {
			// Move the camera rig to where the player was spawned
			Debug.Log ("CemeraRigの位置を移動");
			sdkManager.loadedSetup.actualBoundaries.transform.position = transform.position;
			sdkManager.loadedSetup.actualBoundaries.transform.rotation = transform.rotation;
		} else {
			// simulator
			GameObject.FindGameObjectWithTag("camerarig").transform.position = transform.position;
			GameObject.FindGameObjectWithTag("camerarig").transform.rotation = transform.rotation;
		}

		headsetFade.Unfade (2f);
    }
}
