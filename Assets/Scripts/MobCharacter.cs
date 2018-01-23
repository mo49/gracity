using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobCharacter : MonoBehaviour {

	[SerializeField] bool isInvincible = false;
	[SerializeField] bool lookAtPlayer = true;

	Animator m_animator;
	GameObject player;

	void Start() {
		m_animator = GetComponent<Animator> ();
	}

	void OnJoinedRoom() {
		player = GameObject.FindGameObjectWithTag ("camerarig");
		StartCoroutine ("UpdateRot");
	}

	IEnumerator UpdateRot() {
		while(true) {
			transform.LookAt (player.transform);
			yield return null;
		}
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
