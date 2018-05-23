using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour {

    public Text Name;
    public Text mobility;
    public Text health;
    public Text attack;
    public Text attackRange;
    public Text defense;

    public Color friendly = Color.blue;
    public Color enemy = Color.red;

    public void showStats(Unit u) {
        this.gameObject.SetActive(true);
        Name.text = u.unitName;
        mobility.text = "speed: " + u.mobility;
        health.text = "HP: "  + u.health;
        attack.text = "attack: " + u.attack;
        attackRange.text = "range: " + u.attackRange;
        defense.text = "defense: " + u.defense;

        GetComponent<Image>().color = (u is EnemyUnit) ? enemy : friendly;
    }

    public void hide() {
        this.gameObject.SetActive(false);
    }
}
