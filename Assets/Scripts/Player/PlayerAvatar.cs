using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using VRTK;

public class PlayerAvatar : Photon.MonoBehaviour {

	[SerializeField] int m_healthPoint = 100;
	[SerializeField] GameObject ResultUI;

	[SerializeField] GameObject leftHand;
	[SerializeField] GameObject rightHand;

	VRTK_HeadsetFade headsetFade;

	PhotonView m_photonView;

	void Start() {
		m_photonView = GetComponent<PhotonView> ();
		if(!m_photonView.isMine) {
			return;
		}
		// 初期設定
		SetPlayerName ("Player-Id: " + PhotonNetwork.player.ID);
		SetPlayerDeathState (false);
		SetHealthPoint (m_healthPoint);

		headsetFade = GameObject.Find ("PlayArea").GetComponent<VRTK_HeadsetFade> ();

		BgmManager.Instance.Play ("City");

		// countdown
		if(ModeManager.Instance.GetMode() == "PvE") {
			GameObject.Find("Countdown").GetComponent<Countdown>().Activate ();
		}
	}

	void Update() {
		if(!m_photonView.isMine){
			return;
		}
		// NOTE: 開発用
		if (Input.GetKeyDown (KeyCode.Space)) {
			int index = (int)Random.Range (1f,4f);
			string projectileName = null;
			switch (index) {
				case 1:
					projectileName = "Magic/Thunder/Projectile";
					break;
				case 2:
					projectileName = "Magic/Fire/Projectile";
					break;
				case 3:
					projectileName = "Magic/Dark/Projectile";
					break;
			}
			var projectile = PhotonNetwork.Instantiate (projectileName, transform.position + transform.forward * 4f, transform.rotation, 0);
			projectile.GetComponent<Rigidbody> ().AddForce (transform.forward * 10f, ForceMode.VelocityChange);
		}
	}

	void SetPlayerName(string name) {
		this.gameObject.name = name;
		transform.Find ("NameUI").gameObject.GetComponent<TextMesh> ().text = name;
	}

	void SetHealthPoint(int num) {
		m_healthPoint = num > 0 ? num : 0;
		transform.Find ("HealthPointUI").gameObject.GetComponent<TextMesh> ().text = m_healthPoint.ToString();

		leftHand.GetComponent<PlayerAvatarLeftController> ().SetHP (m_healthPoint);
	}

	// ストリーム同期
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			//Debug.Log ("writing...");
			// 自分の情報を送る
			string myName = this.gameObject.name;
			int myHealthPoint = m_healthPoint;
			stream.SendNext (myName);
			stream.SendNext (myHealthPoint);
		} else {
			//Debug.Log ("receiving...");
			// 他人の情報を受け取る
			string otherName = (string)stream.ReceiveNext();
			int otherHealthPoint = (int)stream.ReceiveNext();
			SetPlayerName (otherName);
			SetHealthPoint (otherHealthPoint);
		}
	}

	void OnTriggerEnter(Collider i_other) {
		if (!m_photonView.isMine) {
			return;
		}
		StartCoroutine ("ShowDamageUI", i_other);
	}

	IEnumerator ShowDamageUI(Collider i_other) {
		string tagName = TagUtility.getParentTagName (i_other.gameObject);
		if(tagName == "Magic" || tagName == "Bullet"){
			headsetFade.Fade (Color.red, 0.1f);
			yield return null;
			headsetFade.Unfade (1f);
		}
	}

	public void TakeDamage(GameObject i_projectile) {

		Debug.Log (string.Format("{0}に攻撃が当たった", this.gameObject.name));

		if(!m_photonView.isMine || i_projectile == null) {
			return;
		}

		string projectileTag = i_projectile.tag;

		SetHealthPoint (m_healthPoint - DamageManager.Instance.GetDamagePoint(projectileTag));

		Debug.Log (string.Format("プレイヤー{0}の残りHP : {1}", PhotonNetwork.player.ID, m_healthPoint));

		if(m_healthPoint <= 0){
			if (m_photonView.isMine) {
				SetPlayerDeathState (true);
				string text = "You Lose...";
				DrawResult(text, new Color(0f,0f,1f));
				// stop
				StartCoroutine("Pause");
			}
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

	public void SetPlayerDeathState( bool isDeath ) {
		var properties  = new ExitGames.Client.Photon.Hashtable();
		properties.Add ("player-id", PhotonNetwork.player.ID);
		properties.Add ("isDeath", isDeath);
		PhotonNetwork.player.SetCustomProperties( properties );
	}

	public void OnPhotonPlayerPropertiesChanged( object[] i_playerAndUpdatedProps ){
		if(!m_photonView.isMine) {
			return;
		}
		// 全員分回す
		var aliveList = new ArrayList();
		var deathList = new ArrayList();
		foreach(var p in PhotonNetwork.playerList) {
			Debug.Log ("player-id : " + p.CustomProperties["player-id"].ToString() + " - " + "isDeath : " + p.CustomProperties["isDeath"].ToString());
			if ((bool)p.CustomProperties ["isDeath"]) {
				deathList.Add (p);
			} else {
				aliveList.Add (p);
			}
		}
		if(aliveList.Count == 1 && deathList.Count > 0) {
			var winner = (PhotonPlayer)aliveList[0];
			var winnerId = winner.CustomProperties["player-id"];
			if((int)PhotonNetwork.player.ID == (int)winnerId) {
				string text = "Player ID: " + winnerId.ToString() + ", Win!!";
				DrawResult(text, new Color(1f,0f,0f));
			}
		}
	}

	void DrawResult(string text, Color color) {
		var tm = ResultUI.gameObject.GetComponent<TextMesh> ();
		tm.text = text;
		tm.color = color;
	}

	IEnumerator Pause() {
		headsetFade.Fade (new Color(0f,0f,0f,0.8f), 0.5f);
		yield return new WaitForSeconds (1f);
		Time.timeScale = 0f;
	}

}