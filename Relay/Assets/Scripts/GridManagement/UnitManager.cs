using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UnitManager handles actions for Units such as unit attacking
// and unit movement
public class UnitManager : MonoBehaviour {

    private GridManager gm;

    void Start() {
        gm = GetComponent<GridManager>();
        gm.buildGrid(GetComponent<PlayerInputManager>());
    }

    /// <summary>
    /// Takes a unit and moves it to a new tile
    /// </summary>
    /// <param name="selectedUnit">unit we want to move</param>
    /// <param name="t">tile we will move the unit to</param>
    public void moveUnit(Unit selectedUnit, Tile t, Unit.action action) {
        Unit u = selectedUnit;
        selectedUnit.moveToTile(t, action);
        gm.resetTiles(); // resets tile colors
        u.moved = true;
    }

    public void moveUnit(Unit selectedUnit, Tile t) {
        moveUnit(selectedUnit, t, null);

    }

    /// <summary>
    /// Returns whether a tile is available for a unit to move to
    /// </summary>
    /// <param name="selectedUnit">unit that wants to move</param>
    /// <param name="t"> tile it wants to move to</param>
    /// <returns></returns>
    public bool tileAvailable(Tile selectedUnit, Tile t) {
        return !t.occupied()  || t.currentUnit == selectedUnit.currentUnit;
    }

    /// <summary>
    /// sets the attacker to attack the defnder 
    /// </summary>
    /// <param name="attaker">unit attacking</param>
    /// <param name="defender">unit defending</param>
    public void attackUnit(Unit attaker, Unit defender, Unit.action action) {
        attaker.attackUnit(defender, action);
    }

    public void attackUnit(Unit attaker, Unit defender) {
        attackUnit(defender, null);
    }

    /// <summary>
    /// Ends a unit's turn and resets the tiles
    /// </summary>
    /// <param name="u"></param>
    public void endUnitTurn(Tile u) {
        u.currentUnit.setActive(false);
        gm.resetTiles();
    }

    /// <summary>
    /// kills a unit
    /// </summary>
    /// <param name="u">unit that is dead</param>
    public void unitDeath(Unit u) {
        u.death -= this.unitDeath;
        Destroy(u.gameObject);
    }
}
