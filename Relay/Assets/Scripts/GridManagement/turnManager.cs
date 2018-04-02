using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnManager : MonoBehaviour
{

    private PlayerInputManager playerInput;
    private EnemyAI enemyInput;
    public TurnUI turnUIDisplay;

    void Start() {
        playerInput = GetComponent<PlayerInputManager>();
        enemyInput = GetComponent<EnemyAI>();

        playerInput.switchTurn += switchTurn;
        enemyInput.switchTurn += switchTurn;

        switchTurn(true);
    }

    // Move to not here
    public void switchTurn(bool yourTurn) {
        if (yourTurn) {
            turnUIDisplay.displayUI("Player Turn", true, 1.5f, playerInput.setUnitsToActive);
        } else {
            turnUIDisplay.displayUI("Enemy Turn", false, 1.5f, enemyInput.setUnitsToActive);
        }
        yourTurn = !yourTurn;
    }
}
