using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour {

	[SerializeField] float maxCount;

	double startTime;
	public float leftTime;

	bool isActive = false;

	public bool isCompleted = false;

	public void Activate() {
		startTime = (double)PhotonNetwork.room.CustomProperties ["StartTime"];
		isActive = true;

		var parentTransform = GameObject.FindGameObjectWithTag ("MainPlayer").transform;
		foreach(Transform childTransform in parentTransform.transform) {
			if(childTransform.name == "Left Hand"){
				childTransform.GetComponent<PlayerAvatarLeftController> ().StartCountdown ();
			}
		}
	}

	void Update () {
		if(!isActive) {
			return;
		}
		if(isCompleted) {
			return;
		}
		double elapsedTime = PhotonNetwork.time - startTime;
		leftTime = maxCount - (float)elapsedTime;
		leftTime = leftTime <= 0 ? 0 : leftTime;
		if(leftTime <= 0) {
			// time over
			ShowResult("lose");
			isActive = false;
		}
		//Debug.Log (leftTime);
	}

	void ShowResult(string result) {
		Transform parentTransform = GameObject.FindGameObjectWithTag ("MainPlayer").transform;
		foreach(Transform childTransform in parentTransform) {
			if(childTransform.tag == "Player") {
				childTransform.GetComponent<PlayerAvatarHeadController> ().ShowResultUI (result);
			}
		}
	}

}
