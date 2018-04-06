using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterAI : Unit {

	public GameObject bullet;
	public float firingSpeed = 1;

	void Start() {
		InvokeRepeating ("LaunchBullet", 1.0f, 1.0f);
	}

	void LaunchBullet() {
		transform.Rotate (0, 0, 90);
		GameObject shot = Instantiate (bullet, transform.position, Quaternion.identity);
		shot.GetComponent<Rigidbody2D> ().velocity = transform.up * firingSpeed;

	}

}
