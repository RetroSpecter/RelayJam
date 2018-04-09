using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		Unit target = other.gameObject.GetComponent<Unit> ();
		if (target != null) {
			target.changeStats (-1, -1, -1, -1);
			Destroy (gameObject);
		}
	}
}
