using UnityEngine;
using System.Collections;
using NetBase;

public class Bullet : Photon.MonoBehaviour {
	public AudioClip hitBulletSound;

	[SerializeField] int m_speed = 20;

	void Start() {
		GetComponent<Rigidbody>().velocity = transform.forward * m_speed;
	}
		
	void OnTriggerEnter(Collider other) {
		var hit = other.gameObject;

		if (hit.GetComponent<Gun>() != null) {
			return;
		}

		if(other.tag == "Player") {
			hit.GetComponent<PlayerAvatar> ().TakeDamage (this.gameObject);
			if (PhotonNetwork.isMasterClient) {
				NetworkAudio.SendPlayClipAtPoint(hitBulletSound, transform.position, 1.0f);
			}
		}

	}

}
