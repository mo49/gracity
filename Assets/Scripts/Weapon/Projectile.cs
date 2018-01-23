using UnityEngine;
using System.Collections;
using NetBase;

public class Projectile : Photon.MonoBehaviour {
	
	[SerializeField] AudioClip hitSound;
	[SerializeField] int m_speed = 20;

	PhotonView photonView;

	void Start() {
		photonView = GetComponent<PhotonView> ();
		GetComponent<Rigidbody>().velocity = transform.forward * m_speed;
	}

	void OnTriggerEnter(Collider other) {
		var hit = other.gameObject;

		if (hit.GetComponent<Weapon>() != null) {
			return;
		}

		if(other.tag == "Player") {
			if(ModeManager.Instance.GetMode() == "PvE") {
				return;
			}
			hit.GetComponent<PlayerAvatar> ().TakeDamage (this.gameObject);
			if (PhotonNetwork.isMasterClient) {
				ShowDamageEffect ();
				NetworkAudio.SendPlayClipAtPoint(hitSound, transform.position, 1.0f);

				GetComponent<PhotonView> ().TransferOwnership (PhotonNetwork.player.ID);
				PhotonNetwork.Destroy (gameObject);
			}
		}

		if (TagUtility.getParentTagName(other.gameObject) == "Monster") {
			if (PhotonNetwork.isMasterClient) {
				ShowDamageEffect ();
				NetworkAudio.SendPlayClipAtPoint(hitSound, transform.position, 1.0f);

				if (other.GetComponent<PhotonView> () != null) {
					other.GetComponent<PhotonView>().RPC ("GetHit", PhotonTargets.All, gameObject.tag);
				}

				GetComponent<PhotonView> ().TransferOwnership (PhotonNetwork.player.ID);
				PhotonNetwork.Destroy (gameObject);
			}
		}

		if (other.tag == "GravityCrystal") {
			if (PhotonNetwork.isMasterClient) {
				string parentTag = TagUtility.getParentTagName (gameObject);
				other.GetComponent<PhotonView>().RPC ("GetHit", PhotonTargets.All, parentTag);

				GetComponent<PhotonView> ().TransferOwnership (PhotonNetwork.player.ID);
				PhotonNetwork.Destroy (gameObject);
			}
		}

	}

	void ShowDamageEffect() {
		PhotonNetwork.Instantiate (
			gameObject.tag + "/DamageEffect",
			transform.position,
			Quaternion.identity,
			0
		);
	}

}
