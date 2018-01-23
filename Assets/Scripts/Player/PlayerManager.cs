using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayerManager : Photon.PunBehaviour {

	public GameObject spawnPoint;
	public GameObject PracticeSpawnPoint;
	private GameObject[] spawns;

	[Tooltip("Reference to the player avatar prefab")]
	public GameObject playerAvatar;

	public delegate void OnCharacterInstantiated(GameObject character);

	public static event OnCharacterInstantiated CharacterInstantiated;

	VRTK_HeadsetFade headsetFade;

	bool isFaded = false;

	void Awake() {
		if (playerAvatar == null) {
			Debug.LogError("MyNetworkManager is missing a reference to the player avatar prefab!");
		}
		spawns = GameObject.FindGameObjectsWithTag("Respawn");

		headsetFade = GameObject.Find ("PlayArea").GetComponent<VRTK_HeadsetFade> ();

		SwitchSpownPoint ();
	}

	void Update() {
		if(isFaded){
			return;
		}
		headsetFade.Fade (Color.black, 0f);
	}

	void SwitchSpownPoint() {
		string mode = ModeManager.Instance.GetMode ();
		switch(mode) {
			case "Practice":
				spawnPoint = PracticeSpawnPoint;
				break;
		}
	}

	public override void OnJoinedRoom() {
		isFaded = true;
		if (PhotonNetwork.isMasterClient) {
			// マスターが自身を生成
			NewPlayer(Random.Range(0,spawns.Length));

			// 全体カウントダウンのスタート時間をセット
			var properties = new ExitGames.Client.Photon.Hashtable();
			properties.Add ("StartTime", PhotonNetwork.time);
			PhotonNetwork.room.SetCustomProperties (properties);
		}

		Debug.Log (string.Format("StartTime{0}", PhotonNetwork.room.CustomProperties["StartTime"]));
	}

	// 誰かがルームに入室
	public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
		if (PhotonNetwork.isMasterClient) {
			// マスターだけがプレイヤーを生成する権限をもつ
			var idx = PhotonNetwork.otherPlayers.Length;
			photonView.RPC ("NewPlayer", newPlayer, idx);
		}
	}

	[PunRPC]
	void NewPlayer(int idx) {
		var trans = spawns[idx].transform;
		if (spawnPoint) {
			trans = spawnPoint.transform;
		}
		var player = PhotonNetwork.Instantiate(playerAvatar.name, trans.position, trans.rotation, 0);
		player.name = "Player " + PhotonNetwork.player.ID;
		// override my avatar's tag
		player.tag = "MainPlayer";
		player.transform.Find ("Head").transform.Find ("Witch").tag = "MainPlayerAvatar";
		Debug.Log (string.Format ("モード：{0}, {1}を{2}に生成", ModeManager.Instance.GetMode (), player.name, trans.gameObject.name));
	}
}
