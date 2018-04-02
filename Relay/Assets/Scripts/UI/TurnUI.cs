using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnUI : MonoBehaviour {

    public Text text;
    public Image background;

    public delegate void turnAction();

    [Space]
    public Color playerColor;
    public Color enemyColor;

    private void Start() {
        text.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
    }

    public void displayUI(string displayText, bool playerColor, float length, turnAction startTurn) {
        StopAllCoroutines();
        StartCoroutine(displayUICor(displayText, playerColor, length, startTurn));
    }

    private IEnumerator displayUICor(string displayText, bool playerTurn, float length, turnAction startTurn) {
        text.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
        text.text = displayText;
        background.color = playerTurn ? this.playerColor : this.enemyColor;
        yield return new WaitForSeconds(length);
        text.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
        if (startTurn != null)
            startTurn.Invoke();
    }
}
