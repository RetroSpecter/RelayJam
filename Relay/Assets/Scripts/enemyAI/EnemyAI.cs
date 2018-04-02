using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EnemyAI keeps track of enemyUnit positions and logic
// each unit, on their turn, will randomly move to an availble space
public class EnemyAI : MonoBehaviour {

    protected UnitManager unitManager;
    [HideInInspector] public List<Unit> enemyUnits;
    protected Tile selectedUnit;
    [HideInInspector] public Queue<Unit> currentActiveUnits;

    public delegate void turnAction(bool yourTurn);
    public turnAction switchTurn;

    void Awake() {
        unitManager = GetComponent<UnitManager>();
        enemyUnits = new List<Unit>(FindObjectsOfType<EnemyUnit>());

        foreach (Unit u in enemyUnits) {
            u.death += ((x) => enemyUnits.Remove(x));
            u.death += unitManager.unitDeath;
        }

        currentActiveUnits = new Queue<Unit>();
    }

    /// <summary>
    /// sets all enemyUnits to active and starts the enemies turn
    /// </summary>
    public void setUnitsToActive() {
        foreach (Unit us in enemyUnits) {
            currentActiveUnits.Enqueue(us);
            us.setActive(true);
            us.moved = false;
        }
        startEnemyTurn();
    }

    /// <summary>
    /// starts the enemies turn. Will eventually decide the enemy order
    /// </summary>
    public void startEnemyTurn() {
        enemyTurn();
    }

    /// <summary>
    /// starts the next enemies turn
    /// if all enemies have had their turn, switches to the player phase
    /// </summary>
    public void enemyTurn() {
        if (currentActiveUnits.Count > 0) {
            Unit currentUnit = currentActiveUnits.Dequeue();
            enemyTakeTurn(currentUnit);
        } else {
            switchTurn.Invoke(true);
        }
    }

    /// <summary>
    /// enemy decision logic
    /// </summary>
    /// <param name="u"></param>
    public virtual void enemyTakeTurn(Unit u) {
        Tile t = GridManager.instance.tileArray[u.tileCoordinate.x, u.tileCoordinate.y];
        List<Tile> inRangeTiles = new List<Tile>(GridManager.instance.getTilesInRangePassable(t, u.mobility));

        for (int i = 0; i < inRangeTiles.Count; i++) {
            if (!unitManager.tileAvailable(t, inRangeTiles[i])) {
                inRangeTiles.Remove(inRangeTiles[i]);
                i--;
            }
        }

        int randPos = Random.Range(0, inRangeTiles.Count);
        unitManager.moveUnit(t.currentUnit, inRangeTiles[randPos], (() => {
            enemyTurn();
            unitManager.endUnitTurn(t);
        }));
    }
}
