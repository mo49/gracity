using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class LinkStart : MonoBehaviour {

	[SerializeField] GameObject m_godRayPrefab;
	[SerializeField] GameObject m_godRay;

	float rayStartPos = 200f;
	float tunnelRadius = 12f;

	float godraySpeed = 80f;
	float godraySpeedRate = 0.5f; // レイの速度を徐々に速く

	float minWaitTime = 0f;
	float maxWaitTime = 0.2f;
	float waitTimeRate = 0.002f; // 生成スパンを徐々に短く

	VRTK_HeadsetFade headsetFade;

	void Start() {
		// reset
		PhotonNetwork.Disconnect();
		PhotonNetwork.LeaveLobby();
		PhotonNetwork.LeaveRoom();

		headsetFade = GameObject.Find ("PlayArea").GetComponent<VRTK_HeadsetFade> ();
		headsetFade.Unfade (0.5f);

		StartCoroutine ("Fire");
	}

	IEnumerator Fire() {
		for(int i = 0; i < 1000; i++) {

			// position
			Vector3 pos = Random.insideUnitCircle * tunnelRadius;
			pos.z = rayStartPos;

			var godRay = Instantiate (m_godRayPrefab, pos, Quaternion.Euler(-90f,0f,0f));
			godRay.transform.parent = m_godRay.transform;

			// color
			Color color = new Color(Random.value, Random.value, Random.value);
			godRay.GetComponent<Renderer> ().material.color = color;

			// addforce
			godRay.GetComponent<Rigidbody>().AddForce(
				godRay.transform.up * godraySpeed * Random.Range(1f,1.5f),
				ForceMode.VelocityChange
			);

			if (minWaitTime < maxWaitTime) {
				godraySpeed += godraySpeedRate;
				maxWaitTime -= waitTimeRate;
			} else {
				// 生成スパンが最短になったらシーンをロード
				Invoke ("GoNextScene", 5f);
			}

			yield return new WaitForSeconds(Random.Range(minWaitTime,maxWaitTime));
		}
	}

	void GoNextScene() {
		// コルーチンを止める場合
		// StopCoroutine ("Fire");

		// fade
		headsetFade.Fade (Color.black, 0.5f);
		SteamVR_LoadLevel.Begin("Gracity");
	}
}
