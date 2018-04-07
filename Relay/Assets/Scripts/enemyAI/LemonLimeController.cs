using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemonLimeController : MonoBehaviour {

	public float speed = 50f;

	void Start() {
		transform.GetComponent<Rigidbody2D> ().velocity = transform.right * speed;
	}

}
