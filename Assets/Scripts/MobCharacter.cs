using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobCharacter : MonoBehaviour {

	[SerializeField] bool isInvincible = false;
	[SerializeField] bool lookAtPlayer = true;

	bool isJoined = false;

	Animator m_animator;
	GameObject player;

	void Start() {
		m_animator = GetComponent<Animator> ();
	}

	void Update() {
		if(!isJoined) {
			return;
		}
		if (!lookAtPlayer) {
			return;
		}
		transform.LookAt (player.transform);
	}

	void OnJoinedRoom() {
		player = GameObject.FindGameObjectWithTag ("camerarig");
		isJoined = true;
	}

	void OnTriggerEnter(Collider i_other) {
		string tagName = TagUtility.getParentTagName (i_other.gameObject);
		bool isWeapon = tagName == "Bullet" || tagName == "Magic";
		if(isWeapon) {
			if(isInvincible) {
				// attack animation
			} else {
				m_animator.SetTrigger ("Damage");
			}
			Destroy (i_other.gameObject);

			ShowDamageEffect (i_other.tag);
		}
	}

	void ShowDamageEffect(string projectileTag) {
		PhotonNetwork.Instantiate (
			projectileTag + "/DamageEffect",
			transform.position,
			Quaternion.identity,
			0
		);
	}

}
