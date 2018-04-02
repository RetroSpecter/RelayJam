using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnManager : MonoBehaviour
{

    private PlayerInputManager playerInput;
    private EnemyAI enemyInput;

    void Start() {
        playerInput = GetComponent<PlayerInputManager>();
        enemyInput = GetComponent<EnemyAI>();

        playerInput.switchTurn += switchTurn;
        enemyInput.switchTurn += switchTurn;

        playerInput.setUnitsToActive();
    }

    // Move to not here
    public void switchTurn(bool yourTurn) {
        if (yourTurn) {
            playerInput.setUnitsToActive();
        } else {
            enemyInput.setUnitsToActive();
        }
        yourTurn = !yourTurn;
    }
}
