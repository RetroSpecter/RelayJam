using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sets AI behavior such that sets up passive behavior to enemy units.
// If an ally unit is within an enemy unit's range, it will attack them.
// if there is more than one target, the enemy unit, on their turn, will 
// attack the unit it can do the most damage to.
public class PassiveAI : EnemyAI{

    public override void enemyTakeTurn(Unit u) {
        Tile t = u.getTileOccupied();
        List<Tile> inRangeTiles = new List<Tile>(GridManager.instance.getTilesInRangePassable(t, u.mobility));
        List<Unit> heroesInRange = getHeroesInRange(inRangeTiles, u);

        for (int i = 0; i < inRangeTiles.Count; i++) {
            if (!unitManager.tileAvailable(t, inRangeTiles[i])) {
                inRangeTiles.Remove(inRangeTiles[i]);
                i--;
            }
        }

        if (heroesInRange.Count > 0) {
            Tile closest = closestTile(t, inRangeTiles, heroesInRange[0].getTileOccupied());
            Unit targetUnit = heroesInRange[0];
            unitManager.moveUnit(u, closest, (() => {
                unitManager.attackUnit(u, targetUnit, (() => {
                    startEnemyTurn();
                    unitManager.endUnitTurn(closest);
                }));
            }));
        } else {
            unitManager.moveUnit(u, t, (() => {
                startEnemyTurn();
                unitManager.endUnitTurn(t);
            }));
        }
        
    }

    public Unit highestPriorityUnit(List<Unit> heroesInRange) {
        Unit target = null;
        return target;
    }

    public List<Unit> getHeroesInRange(List<Tile> inRangeTiles, Unit attacker) {
        List<Unit> heroes = new List<Unit>();

        foreach (Tile t in inRangeTiles) {
            if  (t.occupied() && t.currentUnit is HeroUnit) {
                List<Tile> tileNeighbors = GridManager.instance.getTileNeighbors(t);
                foreach (Tile t2 in tileNeighbors) {
                    if (inRangeTiles.Contains(t2) && !t2.occupied()) {
                        heroes.Add(t.currentUnit);
                        break;
                    }
                }
            }
        }
        heroes.Sort((x,y) => { return compareTargets(x,y, attacker); });
        return heroes;
    }

    public int compareTargets(Unit a, Unit b, Unit attacker) {
        int damageDealtToA = (attacker.attack - a.defense);
        int damageDealtToB = (attacker.attack - b.defense);
        if (a.health <= damageDealtToA) {
            return -1;
        } else if (b.health <= damageDealtToA) {
            return 1;
        }

        return damageDealtToB - damageDealtToA;
    }

    public Tile closestTile(Tile startTile, List<Tile> inRangeTiles, Tile targetTile) {
        Tile closest = null;
        float shortestDistnaceToDest = Mathf.Infinity;
        float shortestDistanceFromStart = Mathf.Infinity;

        foreach (Tile t in inRangeTiles) {
            float distanceToTarget = GridManager.instance.getTileDistances(t, targetTile);
            float distanceFromStart = GridManager.instance.getTileDistances(startTile, t);

            if (shortestDistnaceToDest > distanceToTarget || (shortestDistnaceToDest == distanceToTarget
                        && shortestDistanceFromStart > distanceFromStart))
            {
                shortestDistnaceToDest = distanceToTarget;
                shortestDistanceFromStart = distanceFromStart;
                closest = t;
            } 
        }

        return closest;
    }
}
