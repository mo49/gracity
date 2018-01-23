//
//  VR-Studies
//  Created by miuccie miurror on 11/04/2016.
//  Copyright 2016 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class EyeController : MonoBehaviour {
	

	//------------------------------------------------------------------------------------------------------------------------------//
	Image indicator;
	[SerializeField] VRTK_ControllerEvents rightController;

	RaycastHit hitInfo;
	GameObject hitObject;
	bool       hasClicked = false;

	//------------------------------------------------------------------------------------------------------------------------------//
	void Start () {
		indicator = transform.Find ("Indicator").GetComponent<Image>();
		//rightController = GameObject.Find ("RightController").GetComponent<VRTK_ControllerEvents> ();
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	void FixedUpdate () {

		// 物理オブジェクトのヒットテスト
		bool hasHit = Physics.Raycast (transform.position, transform.forward, out hitInfo, 10);
		if ( hasHit ) {

			//ターゲットが変更された場合
			if ( hitObject != hitInfo.collider.gameObject ) {

				// 以前のターゲットを無効に
				if ( hitObject ) {
					AnimateIndicator (false);
					DispatchHitEvent (false);
				}

				//ヒットイベント発行
				hitObject = hitInfo.collider.gameObject;
				hasClicked = false;
				DispatchHitEvent (true);

			} else {

				//インジケーターアニメーション開始
				if( hasClicked == false ){
					if (rightController.triggerClicked) {
						AnimateIndicator (true);
					} else {
						AnimateIndicator (false);
					}
				}

				//インジケーターが100％になったらクリックイベント発行
				if ( indicator.fillAmount >= 1 ) {
					hasClicked = true;
					indicator.fillAmount = 0;
					DispatchClickEvent();
				}
			}

		} else {

			//インジケーターアニメーション停止
			AnimateIndicator(false);
			DispatchHitEvent(false);
			hitObject = null;
			hasClicked = false;
		}
	} 

	//------------------------------------------------------------------------------------------------------------------------------//
	void AnimateIndicator(bool isOn) {
		if (isOn) {
			indicator.fillAmount += 0.8f * Time.deltaTime;
		} else {
			indicator.fillAmount = 0;
		}
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public interface IEyeControllerTarget {
		void OnEyeContollerHit(bool isOn);
		void OnEyeContollerClick();
		void OnTriggerClick(bool isClick);
	}

	// 視線が当たる
	void DispatchHitEvent (bool isOn) {
		if (hitObject) {
			var target = hitObject.GetComponent<IEyeControllerTarget> ();
			if (target != null) {
				target.OnEyeContollerHit( isOn );
			}
		}
	}

	// トリガーを押す
	void DispatchTriggerClickEvent(bool isClick) {
		if (hitObject) {
			var target = hitObject.GetComponent<IEyeControllerTarget> ();
			if (target != null) {
				target.OnTriggerClick( isClick );
			}
		}
	}

	// インジケータがたまったらクリック
	void DispatchClickEvent () {
		if (hitObject) {
			var target = hitObject.GetComponent<IEyeControllerTarget> ();
			if (target != null) {
				target.OnEyeContollerClick();
			}
		}
	}

}
