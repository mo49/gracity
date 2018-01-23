using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideArrow : MonoBehaviour {

	[SerializeField] string enemyTag;
	[SerializeField] float minDis = 1000f;
	[SerializeField] float researchInterval = 1f;

	GameObject nearestEnemy = null;

	float elapsedTime = 0f;

	void Start () {
		string mode = ModeManager.Instance.GetMode ();
		switch (mode) {
			case "PvP":
				enemyTag = "OtherPlayerAvatar";
				break;
			case "PvE":
				enemyTag = "Monster/Dragon";
				break;
			case "Practice":
				enemyTag = "Monster/Dragon";
				break;
			default:
				enemyTag = null;
				break;
		}

		nearestEnemy = GetNearestObj ();
	}

	void Update () {
		if(enemyTag == null) {
			return;
		}

		elapsedTime += Time.deltaTime;

		if(elapsedTime > researchInterval) {
			nearestEnemy = GetNearestObj ();
			elapsedTime = 0;
		}

		if(nearestEnemy){
			transform.LookAt (nearestEnemy.transform);
		}
	}

	GameObject GetNearestObj() {
		GameObject targetObj = null;
		GameObject[] enemys = GameObject.FindGameObjectsWithTag(enemyTag);
		float nearDis = minDis;
		foreach(GameObject enemy in enemys) {
			float tmpDis = Vector3.Distance (transform.position, enemy.transform.position);
			if( tmpDis < nearDis ) {
				nearDis = tmpDis;
				targetObj = enemy;
			}
		}
		return targetObj;
	}
		
}
