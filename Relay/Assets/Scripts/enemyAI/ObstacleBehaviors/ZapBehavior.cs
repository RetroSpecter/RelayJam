using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapBehavior : MonoBehaviour {

	bool electricityOn = false;
	SpriteRenderer sr;

	void Start() {
		sr = GetComponent<SpriteRenderer> ();
		InvokeRepeating ("SwitchElectricity", 1.0f, 1.0f);
	}
		
	void SwitchElectricity() {
		electricityOn = !electricityOn;
		if (electricityOn) {
			sr.color = Color.yellow;
		} else {
			sr.color = Color.white;
		}
	}
}
