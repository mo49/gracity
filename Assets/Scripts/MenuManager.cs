using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class MenuManager : MonoBehaviour {

	public void GoStartMenu() {
		SteamVR_LoadLevel.Begin ("StartMenu");
	}

}
