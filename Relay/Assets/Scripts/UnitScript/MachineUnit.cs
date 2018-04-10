using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineUnit : EnemyUnit {

	bool machineIsActive = false;
	bool takingDamage;
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

	//When a machine unit takes damage, it activates or deactivates as opposed to be destroyed
	public override void takeDamage(Unit other, action actionFinished) {
		if (takingDamage) //Bad things happen if a machine is constantly being activated
			return;
		takingDamage = true;
		StartCoroutine (ProcessDamage(actionFinished));
	}

	public override void changeStats(int mobility, int attack, int attackRange, int defense) {
		if (takingDamage) //Bad things happen if a machine is constantly being activated
			return;
		takingDamage = true;
		StartCoroutine (ProcessStatChange());
	}

	//This is to resolve a shaking issue
	private IEnumerator ProcessDamage(action actionFinished) {
		yield return shake.screenshake (this.gameObject, 2, 1);
		yield return StartCoroutine(checkHealth(actionFinished));
		changeMachineStatus();
	}

	private IEnumerator ProcessStatChange() {
		yield return shake.screenshake (this.gameObject, 2, 1);
		yield return null;
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
		takingDamage = false;
	}

	//A hard reset to turn the machine off
	public void ClearMachineStatus() {
		StopCoroutine (myMachineEffect);
		myMachineEffect = MachineEffect ();
		machineIsActive = false;
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
