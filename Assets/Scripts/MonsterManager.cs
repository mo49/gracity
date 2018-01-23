using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MonsterManager : MonoBehaviour {

	[SerializeField] string[] monsters;
	[SerializeField] GameObject MonsterParent;

	GameObject[] spawns;

	void OnJoinedRoom () {
		if(!PhotonNetwork.isMasterClient){
			StartCoroutine ("InitMonsterUI");
			return;
		}
		StartCoroutine ("InitMonsterUI");
		NewMonsters ();

	}
		
	void NewMonsters() {
		spawns = GameObject.FindGameObjectsWithTag("MonsterSpawn");
		foreach(var monster in monsters.Select((v, i) => new{v, i})) {
			var m = PhotonNetwork.Instantiate (monster.v, spawns[monster.i].transform.position, Quaternion.identity, 0);
			m.transform.parent = MonsterParent.transform;
		}
	}

	IEnumerator InitMonsterUI() {
		yield return new WaitForSeconds (0.5f);
		SetAliveMonsterUI (GameObject.FindGameObjectsWithTag("Monster/Dragon").Length);
	}

	public void SetAliveMonsterUI(int num) {
		TextMesh monsterUI = null;
		Transform parentTransform = GameObject.FindGameObjectWithTag ("MainPlayer").transform;
		foreach(Transform childTransform in parentTransform) {
			foreach(Transform grandChildTransform in childTransform) {
				if(grandChildTransform.name == "MonsterUI") {
					monsterUI = grandChildTransform.GetComponent<TextMesh> ();
				}
			}
		}
		monsterUI.text = "Monster: " + num;
	}

}
