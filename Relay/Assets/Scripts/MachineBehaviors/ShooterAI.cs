using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterAI : MachineUnit {

	public GameObject bullet;
	public float firingSpeed = 1;

	void LaunchBullet() {
		GameObject shot = Instantiate (bullet, transform.position, Quaternion.identity);
		shot.GetComponent<BulletBehavior> ().SetSource (transform);
		shot.GetComponent<Rigidbody2D> ().velocity = transform.up * firingSpeed;
	}

	public override IEnumerator MachineEffect() {
		while (true) {
			LaunchBullet ();
			yield return new WaitForSeconds (1);
		}
	}

}
