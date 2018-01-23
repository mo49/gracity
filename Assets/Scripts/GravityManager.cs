using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class GravityManager : MonoBehaviour {

	[SerializeField] GameObject m_world;
	[SerializeField] float m_jumpPower = 10f;
	[SerializeField] float m_rotateSpeed = 1f;

	public void OnPhotonCustomRoomPropertiesChanged( ExitGames.Client.Photon.Hashtable changedProperties ){
		object value = null;
		if (changedProperties.TryGetValue ("roomGravity", out value)) {
			ChangeGravity ((Vector3)value);
		}
		if(changedProperties.TryGetValue ("roomRotation", out value)) {
			ChangeRotation ((Quaternion)value);
		}
	}

	void Jump() {
		// 上方向に飛ばす
		var camerarig = GameObject.FindGameObjectWithTag("camerarig");
		if (camerarig) {
			camerarig.GetComponent<Rigidbody> ().AddForce (0f, m_jumpPower, 0f, ForceMode.VelocityChange);
		}
	}

	void ChangeGravity(Vector3 value) {
		Vector3 gravity = value;
		Physics.gravity = gravity;
		Debug.Log ( string.Format("重力を変更しました x:{0}, y:{1}, z:{2}", gravity.x, gravity.y, gravity.z) );
		GameObject.FindGameObjectWithTag ("MainPlayer").transform.Find("Right Hand").GetComponent<PlayerAvatarRightController> ().SetGravityUI (gravity);
		Invoke ("Jump", 0.5f);
	}

	void ChangeRotation(Quaternion value) {
		Quaternion rot = value;
		Debug.Log( string.Format("回転を変更しました x:{0}, y:{1}, z:{2}", rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z) );
	}

	void Update() {
		// シミュレータ用
		if(Input.GetKeyDown(KeyCode.Alpha0)) {
			ChangeGravity (new Vector3(0f,-9.81f,0f));
		}
		/*
		m_world.transform.rotation = Quaternion.Lerp(
			m_world.transform.rotation,
			rot,
			Time.deltaTime * m_rotateSpeed
		);
		*/
	}
}
