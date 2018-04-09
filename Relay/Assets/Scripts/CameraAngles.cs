using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAngles : MonoBehaviour {

	float initialSize;
	Vector2 initialPosition;

	public float zoomOutSize;
	public Vector2 zoomOutPosition;
	bool zoom = false;

	void Start() {
	}

	public void ZoomSetting() {
		zoom = !zoom;

	}

	IEnumerator ZoomSetting(bool zoom) {
	}
}
