using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

	float initialSize;
	Vector2 initialPosition;
	public float zoomSpeed;

	public float zoomOutSize;
	public Vector2 zoomOutPosition;
	bool zoom = false;
	bool zooming = false;

	GameObject cameraObject;

	void Start() {
		cameraObject = Camera.main.gameObject;
		initialSize = Camera.main.orthographicSize;
		initialPosition = cameraObject.transform.position;
	}

	public void ZoomSetting() {
		if (zooming)
			return;
		zooming = true;
		zoom = !zoom;
		StartCoroutine (ZoomProcess (zoom));
	}

	IEnumerator ZoomProcess(bool zoom) {
		float start = zoom ? initialSize : zoomOutSize;
		float end = !zoom ? initialSize : zoomOutSize;

		Vector2 startPosition = cameraObject.transform.position;
		Vector2 endPosition = !zoom ? initialPosition : zoomOutPosition;

		float t = 0;
		while (t < 1) {
			yield return null;
			t = Mathf.Min(1, t + (Time.deltaTime * zoomSpeed));
			Vector2 nextCameraPoint = Vector2.Lerp (startPosition, endPosition, t);
			cameraObject.transform.position = new Vector3 (nextCameraPoint.x, nextCameraPoint.y, cameraObject.transform.position.z);
			Camera.main.orthographicSize = Mathf.Lerp (start, end, t);
		}
		yield return null;
		zooming = false;
	}
}
