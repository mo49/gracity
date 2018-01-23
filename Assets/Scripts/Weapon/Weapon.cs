using UnityEngine;
using System.Collections;
using VRTK;
using NetBase;
using NetVRTK;

public class Weapon : Photon.MonoBehaviour {
	[SerializeField] Transform projectileSpawn;
	[SerializeField] AudioClip fireSound;
	[SerializeField] string projectileName;
	[SerializeField] float m_volume = 0.5f;
	[SerializeField] float m_recastTime = 1.0f;

	bool isCharged = true;

	private bool fired;

	void Awake() {
		GetComponent<VRTK_InteractableObject>().InteractableObjectUsed += new InteractableObjectEventHandler(DoFire);
	}

	void Update() {
		if (fired) {
			CmdFire();
			fired = false;
		}
	}

	void CmdFire() {
		var bullet = PhotonNetwork.Instantiate(
			projectileName,
			projectileSpawn.position,
			projectileSpawn.rotation,
			0);

		photonView.RPC("NetFire", PhotonTargets.All);
	}

	[PunRPC]
	void NetFire() {
		AudioSource.PlayClipAtPoint(fireSound, transform.position, m_volume);
	}

	void DoFire(object sender, InteractableObjectEventArgs e) {
		if(!isCharged) {
			return;
		}
		fired = true;
		isCharged = false;
		Invoke ("Charge", m_recastTime);
	}

	void Charge() {
		isCharged = true;
	}

}
