//
//  VR-Studies
//  Created by miuccie miurror on 11/04/2016.
//  Copyright 2016 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EyeControllerTarget : MonoBehaviour, EyeController.IEyeControllerTarget {

	[SerializeField] string m_modeName;
	[SerializeField] GameObject m_linkStart;
	[SerializeField] GameObject m_menu;
	[SerializeField] Material m_linkStartMaterial;
	[SerializeField] AudioSource m_audio;
	Color _color;

	void Awake() {
		Hover(false);
	}

	public void OnEyeContollerHit( bool isOn ) {
		// 視線マーカーがヒットしたら色を変える
		Hover(isOn);
	}

	public void OnEyeContollerClick() {

		// 視線マーカーでクリック
		m_audio.PlayOneShot(m_audio.clip);
		SteamVR_Fade.Start(Color.white, 0.5f);

		// set mode
		ModeManager.Instance.SetMode(m_modeName);

		Invoke ("ActivateLinkStart", 0.5f);
	}

	public void OnTriggerClick(bool isClick) {
		
	}

	void Hover(bool isOn, float color=0.7f) {
		gameObject.GetComponent<Renderer> ().material.color
				= isOn 
				? new Color (1f, 1f, 1f)
				: new Color (color, color, color);
	}

	void ActivateLinkStart() {
		SteamVR_Fade.Start(Color.clear, 0.5f);
		RenderSettings.skybox = m_linkStartMaterial;
		m_linkStart.SetActive (true);
		m_menu.SetActive (false);
	}

}
