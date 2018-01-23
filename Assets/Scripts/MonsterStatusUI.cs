using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStatusUI : MonoBehaviour {
	
	[SerializeField] Monster monster;
	Slider hpSlider;
	Image fillImage;


	void Start () {
		hpSlider = transform.Find ("HPBar").GetComponent <Slider>();
		fillImage = transform.Find ("HPBar/Fill Area/Fill").GetComponent<Image> ();

		hpSlider.value = (float)monster.GetMaxHP () / (float)monster.GetMaxHP (); // 1
	}

	void Update () {
		transform.rotation = GameObject.FindGameObjectWithTag ("camerarig").transform.rotation;
	}

	public void UpdateHP() {
		hpSlider.value = (float)monster.GetCurrentHP () / (float)monster.GetMaxHP ();

		if(hpSlider.value <= 0.3f) {
			fillImage.color = Color.red;
		}
	}
}
