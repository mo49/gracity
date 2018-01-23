using System;
using UnityEngine;

public class MyNetworkManager : Photon.PunBehaviour {
	public string gameVersion = "1.0";

	[Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
	public byte MaxPlayersPerRoom = 4;
	public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

	void Awake() {
		PhotonNetwork.autoJoinLobby = false;    // we join randomly. always. no need to join a lobby to get the list of rooms.
		PhotonNetwork.automaticallySyncScene = true;
		PhotonNetwork.logLevel = Loglevel;
		PhotonNetwork.ConnectUsingSettings(gameVersion);
	}

	public override void OnConnectedToMaster() {
		Debug.Log("Connected to master");

		Debug.Log("Joining random room...");
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinedLobby() {
		Debug.Log("Joined lobby");

		Debug.Log("Joining random room...");
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
		Debug.Log("Can't join random room!");

		Debug.Log("Creating room...");
		RoomOptions roomOptions = new RoomOptions () {
			MaxPlayers = MaxPlayersPerRoom,
			IsOpen = true,
			IsVisible = true,
		};
		PhotonNetwork.CreateRoom (null, roomOptions, null);
	}

	public override void OnCreatedRoom() {
		Debug.Log("Created room");
	}

	public override void OnJoinedRoom() {
		Debug.Log("Joined room");
		Room room = PhotonNetwork.room;
		PhotonPlayer player = PhotonNetwork.player;
		Debug.Log ("PhotonManager: ルーム入室成功　部屋名:" + room.Name + ", プレイヤーID:" + player.ID);
		Debug.Log ("PhotonManager: 部屋情報:" + room + ", ルームマスター？:" + player.IsMasterClient);
	}

	public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
		Debug.Log("Player connected");
	}

	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
		Debug.Log("Player disconnected");
	}

	public override void OnFailedToConnectToPhoton(DisconnectCause cause) {
		Debug.Log("Couldn't connect to Photon network");
	}

	public override void OnConnectionFail(DisconnectCause cause) {
		Debug.Log("Connection failed to the Photon network");
	}

	public override void OnDisconnectedFromPhoton() {
		Debug.Log("We got disconnected form the Photon network");
	}
}
