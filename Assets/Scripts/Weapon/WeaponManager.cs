using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Photon.MonoBehaviour {

	[SerializeField] int m_thunderNum;
	[SerializeField] int m_fireNum;
	[SerializeField] int m_darkNum;

	[SerializeField] GameObject m_MagicWands;

	float xMin = -120f;
	float xMax = 60f;
	float zMin = -120f;
	float zMax = 200f;

	float yHeight = 0f;

	void OnJoinedRoom () {
		// FIX: Instantiateしたものだけ掴めない
		return;
		if(!PhotonNetwork.isMasterClient) {
			return;
		}
		for(int i = 0; i < m_thunderNum; i++){
			var trans = new Vector3 (Random.Range(xMin,xMax), yHeight, Random.Range(zMin,zMax));
			var rot = Quaternion.identity;
			var wand = PhotonNetwork.Instantiate ("Magic/Thunder/Weapon", trans, rot, 0);
			wand.transform.parent = m_MagicWands.transform;
		}
		for(int i = 0; i < m_fireNum; i++){
			var trans = new Vector3 (Random.Range(xMin,xMax), yHeight, Random.Range(zMin,zMax));
			var rot = Quaternion.identity;
			var wand = PhotonNetwork.Instantiate ("Magic/Fire/Weapon", trans, rot, 0);
			wand.transform.parent = m_MagicWands.transform;
		}
		for(int i = 0; i < m_darkNum; i++){
			var trans = new Vector3 (Random.Range(xMin,xMax), yHeight, Random.Range(zMin,zMax));
			var rot = Quaternion.identity;
			var wand = PhotonNetwork.Instantiate ("Magic/Dark/Weapon", trans, rot, 0);
			wand.transform.parent = m_MagicWands.transform;
		}
	}

}
