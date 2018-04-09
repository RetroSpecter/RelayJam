using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineUnit : EnemyUnit {

	bool machineIsActive = false;
	IEnumerator myMachineEffect;

	//Assigns the coroutine to local variable, and sets machine stats
	public void Awake() {
		myMachineEffect = MachineEffect ();
		mobility = 0;
		health = 999;
		attack = 0;
		attackRange = 0;
		defense = 999;
	}

	//When a machine unit "takes damage", it activates or deactivates as opposed to be destroyed
	public override void takeDamage(Unit other, action actionFinished) {
		StartCoroutine(shake.screenshake(this.gameObject, 2, 1));
		StartCoroutine(checkHealth(actionFinished));
		changeMachineStatus();
	}

	//Starts or Stops the Machine
	public void changeMachineStatus() {
		machineIsActive = !machineIsActive;
		if (machineIsActive) {
			StartCoroutine (myMachineEffect);
		} else {
			StopCoroutine(myMachineEffect);
		} 
	}

	//Overrides the shading logic assigned to the default enemies
	public override void setActive(bool active) {
		//GetComponent<SpriteRenderer>().color = Color.white;
	}

	//Empty Coroutine that can be set when you inherit the MachineUnit class
	public virtual IEnumerator MachineEffect() {
		yield return null;
	}
}
