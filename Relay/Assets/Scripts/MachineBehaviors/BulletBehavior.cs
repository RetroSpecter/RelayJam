using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {

	Transform source;

	public void SetSource(Transform t) {
		source = t;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.transform == source) {
			return;
		}
		Unit target = other.gameObject.GetComponent<Unit> ();
		if (target != null) {
			target.changeStats (0, -100, 0, -100);
			Destroy (gameObject);
		}
	}
}
