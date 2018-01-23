using UnityEngine;
using System.Collections;
using VRTK;
using NetBase;
using NetVRTK;

public class Gun : Photon.MonoBehaviour {
	public Transform bulletSpawn;
	public AudioClip fireGunSound;

	private bool fired;

	void Awake() {
		GetComponent<VRTK_InteractableObject>().InteractableObjectUsed += new InteractableObjectEventHandler(DoFireGun);
	}

	void Update() {
		if (fired) {
			CmdFire();
			fired = false;
		}
	}

	void CmdFire() {
		// Create the Bullet from the Bullet Prefab
		// (gets replicated automatically to all clients)
		var bullet = PhotonNetwork.Instantiate(
			"Bullet",
			bulletSpawn.position,
			bulletSpawn.rotation,
			0);

		// Now play sound and animation locally and on all other clients
		photonView.RPC("NetFire", PhotonTargets.All);
	}

	[PunRPC]
	void NetFire() {
		AudioSource.PlayClipAtPoint(fireGunSound, transform.position, 1.0f);
	}

	void DoFireGun(object sender, InteractableObjectEventArgs e) {
		fired = true;
	}

}
