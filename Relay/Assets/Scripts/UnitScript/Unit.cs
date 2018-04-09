using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a Unit represets a character Unit for the game. Has statistics
// and methods to move and attack otehr units
public class Unit : MonoBehaviour {

    public delegate void unitAction(Unit u);
    public unitAction move;
    public unitAction death;

    public delegate void tileAction(Tile t);
    public delegate void action();

    public Vector2Int tileCoordinate;
    public bool moved = false;
    private SpriteRenderer sprtRend;

    [Header("Stats")]
    public int mobility = 3;
    public int health;
    public int attack;
    public int attackRange = 1;
    public int defense;

    private void Awake() {
        sprtRend = GetComponent<SpriteRenderer>();
    }

    public virtual void setActive(bool active) {
        GetComponent<SpriteRenderer>().color = active ? Color.white : Color.grey;
    }

    /// <summary>
    /// sets the unit to attack other unit and starts attack Corutine
    /// </summary>
    /// <param name="other">unit to be attacked</param>
    /// <param name="attackFinished">delegate to run when attack corutine finishes</param>
    public void attackUnit(Unit other, action attackFinished) {
        StartCoroutine(attackUnitCor(other, attackFinished));
    }

    public IEnumerator attackUnitCor(Unit other, action actionFinished) {
        yield return new WaitForSeconds(0.25f);
        Vector3 targetPos = other.transform.position;
        Vector3 curPos = this.transform.position;
        yield return moveToPositionEnum(this.gameObject, targetPos, 10, false);
        yield return moveToPositionEnum(this.gameObject, curPos, 10, false);

        other.takeDamage(this, actionFinished);
    }

    /// <summary>
    /// takes damage from other unit based off of their stats
    /// </summary>
    /// <param name="other"></param>
    public virtual void takeDamage(Unit other, action actionFinished) {
		int damage = Mathf.Max(other.attack - this.defense, 0); //Don't take negative damage
		health -= damage;
        print(this.name + " took " + damage + " from "
            + other.name);

        StartCoroutine(shake.screenshake(this.gameObject, 2, 1));
        StartCoroutine(checkHealth(actionFinished));
    }

	//Changes stats other than health; Kenji tried changing health outside of combat but that does bad things
	//Going below zero can also cause bad things
	public virtual void changeStats(int mobility, int attack, int attackRange, int defense) {
		this.mobility = Mathf.Max(0, this.mobility + mobility);
		this.attack = Mathf.Max(0, this.attack + attack);
		this.attackRange = Mathf.Max(0, this.attackRange + attackRange);
		this.defense = Mathf.Max(0, this.defense + defense);
	}

    public IEnumerator checkHealth(action actionFinished) {
        yield return new WaitForSeconds(0.5f);
        if (health <= 0) {
            move.Invoke(this); //unlinks old tile from this unit
            death.Invoke(this);
        }

        if (actionFinished != null) {
            actionFinished.Invoke();
            actionFinished = null;
        }
    }

    /// <summary>
    /// sets this unit to move towards tile and starts move corrutine
    /// </summary>
    /// <param name="dest">Tile unit is moving towards</param>
    /// <param name="movementFinished">delegate to run when movement corrutine is finsihed</param>
    public virtual void moveToTile(Tile dest, action movementFinished) {
        //move.Invoke(this); //unlinks old tile from this unit
        //transform.position = dest.transform.position;
        //dest.setUnit(this);
        //move += dest.unsetUnit;
        tileCoordinate = dest.tileCoordinate;
        StartCoroutine(moveToTileCor(dest,movementFinished));
    }

    public IEnumerator moveToTileCor(Tile dest, action actionFinished) {

		Vector3 singleAxisPosition = transform.position;
		singleAxisPosition.x = dest.transform.position.x;

        move.Invoke(this); //unlinks old tile from this unit
        dest.setUnit(this);

        yield return moveToPositionEnum(singleAxisPosition, 10, false);
		yield return moveToPositionEnum(dest.transform.position, 10, false);

        move += dest.unsetUnit;

        if (actionFinished != null) {
            actionFinished.Invoke();
        }
    }

    private IEnumerator moveToPositionEnum(GameObject targetObject, Vector3 targetPos, float speed, bool lerp) {
        while (Vector3.Distance(targetObject.transform.position, targetPos) > 0.1f) {
            if (lerp) {
                targetObject.transform.position = Vector3.Lerp(targetObject.transform.position, targetPos, Time.deltaTime * speed);
            } else {
                targetObject.transform.position = Vector3.MoveTowards(targetObject.transform.position, targetPos, Time.deltaTime * speed);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator moveToPositionEnum(Vector3 targetPos, float speed, bool lerp) {
        yield return moveToPositionEnum(this.gameObject, targetPos, speed, lerp);
    }

    /// <summary>
    /// Gets the tile that this Unit is occupying
    /// </summary>
    /// <returns></returns>
    public Tile getTileOccupied() {
        return GridManager.instance.tileArray[tileCoordinate.x, tileCoordinate.y];
    }
}
