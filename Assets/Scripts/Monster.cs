using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Photon.MonoBehaviour {

	[SerializeField] int maxHP = 300;
	public int hp;
	public bool isDeath = false;
	private MonsterStatusUI monsterStatusUI;
	private MonsterManager monsterManager;

	[SerializeField] float walkSpeed = 1f;

	private PhotonView photonView;
	private UnityEngine.AI.NavMeshAgent navAgent;
	private Animator animator;
	private Vector3 lastPos;

	bool hasFlyAttacked = false;
	bool isInvincible = false;

	void Start () {
		hp = maxHP;
		monsterStatusUI = GetComponentInChildren<MonsterStatusUI> ();
		monsterManager = GameObject.Find ("MonsterManager").GetComponent<MonsterManager>();

		photonView = GetComponent<PhotonView> ();
		navAgent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		animator = GetComponent<Animator> ();
		lastPos = transform.position;

		SetSpeed (walkSpeed);
		animator.SetFloat ("Forward", 1f);

		if(!PhotonNetwork.isMasterClient) {
			return;
		}

		StartCoroutine ("SetGoal");

		//monsterManager.SetAliveMonsterUI (GetAliveNum());
	}

	void Update () {
		if(!PhotonNetwork.isMasterClient) {
			return;
		}
		// walk
		float dist = Vector3.Distance (lastPos, transform.position) * (1 / Time.deltaTime / navAgent.speed);
		lastPos = transform.position;
		animator.SetFloat ("Forward", dist);
	}

	IEnumerator SetGoal() {
		while (true) {
			navAgent.destination = new Vector3 (
				Random.Range(-100f, 100f),
				0f,
				Random.Range(-150f, 150f)
			);
			yield return new WaitForSeconds (60);
		}
	}

	void SetSpeed(float speed){
		navAgent.speed = speed;
	}

	void Stop() {
		SetSpeed (0f);
	}

	[PunRPC]
	public void GetHit(string tagName) {
		if(isInvincible) {
			return;
		}

		Stop ();
		// 一定時間後に歩き出す
		StopCoroutine("RestartWalk");

		hp -= DamageManager.Instance.GetDamagePoint (tagName);
		monsterStatusUI.UpdateHP ();

		float percent = (float)GetCurrentHP () / (float)GetMaxHP ();

		if (percent <= 0) {
			isInvincible = true;
			isDeath = true;
			Invoke ("Die", 1f);
			if(GetAliveNum() <= 0) {
				// win
				ShowResult("win");
				GameObject.Find ("Countdown").GetComponent<Countdown> ().isCompleted = true;
			}
		} else if (percent <= 0.3f && !hasFlyAttacked) {
			hasFlyAttacked = true;
			isInvincible = true;
			animator.SetTrigger ("FlyAttack");
			StartCoroutine("RestartWalk", 8f);
		} else {
			animator.SetTrigger ("GetHit");
			StartCoroutine("RestartWalk", 3f);
		}

		Debug.Log(string.Format("タグ{0}, モンスターにダメージ{1}, 残りHP{2}", tagName, DamageManager.Instance.GetDamagePoint (tagName), hp ));
	}

	public int GetMaxHP() {
		return maxHP;
	}

	public int GetCurrentHP() {
		return hp;
	}

	public int GetAliveNum() {
		int aliveCount = 0;
		GameObject[] dragons = GameObject.FindGameObjectsWithTag ("Monster/Dragon");
		foreach(var dragon in dragons) {
			if(!dragon.GetComponent<Monster>().isDeath) {
				aliveCount++;
			}
		}
		return aliveCount;
	}

	IEnumerator RestartWalk(float d) {
		yield return new WaitForSeconds (d);
		SetSpeed (walkSpeed);
		animator.SetFloat ("Forward", 1f);
		isInvincible = false;
	}

	void Die() {
		animator.SetTrigger ("Die");
		gameObject.tag = "Untagged";
		monsterManager.SetAliveMonsterUI (GetAliveNum());
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
