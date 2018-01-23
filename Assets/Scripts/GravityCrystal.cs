using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetBase;
using VRTK;

public class GravityCrystal : Photon.MonoBehaviour {

	[SerializeField] AudioClip hitSound;
	[SerializeField] GameObject m_gcEffect;
	[SerializeField] bool isActive;

	float m_vGravityRange = 3f;

	VRTK_HeadsetFade headsetFade;

	void Start() {
		headsetFade = GameObject.Find ("PlayArea").GetComponent<VRTK_HeadsetFade> ();
	}

	[PunRPC]
	public void GetHit(string parentTag) {
		if(!isActive) {
			return;
		}
		if(parentTag == "Magic") {
			ChangeGravity ( new Vector3(
				Random.Range(- m_vGravityRange, m_vGravityRange),
				- Random.Range(3f, 17f),
				Random.Range(- m_vGravityRange, m_vGravityRange)
			));
			ChangeRotation ( Quaternion.Euler(
				Random.Range(-5f, 5f),
				Random.Range(-5f, 5f),
				Random.Range(-5f, 5f)
			));
			// explosion
			var gcePos = transform.position;
			gcePos.y = 5f;
			PhotonNetwork.Instantiate("GravityCrystalExplosion", gcePos, Quaternion.identity, 0);

			AudioSource.PlayClipAtPoint(hitSound, GameObject.FindGameObjectWithTag("camerarig").transform.position, 1.0f);
			// UI
			StartCoroutine("ShowGravityChangeUI");

			Deactivate ();
			Invoke ("Activate", 10f);
		}
	}

	void ChangeGravity(Vector3 i_gravity) {
		var properties  = new ExitGames.Client.Photon.Hashtable();
		properties.Add( "roomGravity", i_gravity );
		PhotonNetwork.room.SetCustomProperties( properties );
	}

	void ChangeRotation(Quaternion i_rot) {
		var properties  = new ExitGames.Client.Photon.Hashtable();
		properties.Add( "roomRotation", i_rot );
		PhotonNetwork.room.SetCustomProperties( properties );
	}

	[PunRPC]
	void NetHit() {
		AudioSource.PlayClipAtPoint(hitSound, GameObject.FindGameObjectWithTag("camerarig").transform.position, 1.0f);
		// UI
		StartCoroutine("ShowGravityChangeUI");

		Deactivate ();
		Invoke ("Activate", 10f);
	}

	IEnumerator ShowGravityChangeUI() {
		SetGravityHeadlineUI ("Gravity Change");
		headsetFade.Fade (new Color(1f,0f,0f,0.1f), 0.1f);

		yield return new WaitForSeconds(2f);

		headsetFade.Unfade (0.5f);
		SetGravityHeadlineUI ();
	}

	void SetGravityHeadlineUI(string text = null) {
		TextMesh gravityUI = null;
		var parentTransform = GameObject.FindGameObjectWithTag ("MainPlayer").transform;
		foreach(Transform childTransform in parentTransform.transform) {
			foreach(Transform grandChildTransform in childTransform.transform) {
				if(grandChildTransform.name == "GravityHeadlineUI") {
					gravityUI = grandChildTransform.gameObject.GetComponent<TextMesh> ();
					gravityUI.text = text;
				}
			}
		}
	}
		
	void Deactivate() {
		isActive = false;
		m_gcEffect.SetActive (false);
	}

	void Activate() {
		isActive = true;
		m_gcEffect.SetActive (true);
	}

}
