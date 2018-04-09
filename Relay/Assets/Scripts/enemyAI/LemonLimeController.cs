using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemonLimeController : MonoBehaviour {

	public float speed = 50f;
	int health = 100;

	void Start() {
		transform.GetComponent<Rigidbody2D> ().velocity = transform.right * speed;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Bullet") {
			health -= 1;
			if (health <= 0) {
				Destroy (gameObject);
			}
		}
	}

}
