using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterAI : Unit {

	public GameObject bullet;
	public float firingSpeed = 1;

	void LaunchBullet() {
		GameObject shot = Instantiate (bullet, transform.position, Quaternion.identity);
		shot.GetComponent<Rigidbody2D> ().velocity = transform.up * firingSpeed;
	}

}
