using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfLevelScript : MonoBehaviour {

    public DataLoader loader;
    public Button closeButton;
    public Text deaths;
    public Text enemiesKilled;
    public Text orbsEarned;
    public Text puzzlePieces;
    public CanvasGroup myCanvasGroup;
    public GameObject myCanvas;

    void Start() {
        RefreshValues();
    }

    private void OnEnable() {
        RefreshValues();
    }

    public void RefreshValues() {
		loader = DataLoader.loader;
        deaths.text = loader.howManyTimesPlayerHasDiedAtThisLevel.ToString() + " out of " + loader.howManyTimesPlayerHasDied.ToString();
        enemiesKilled.text = loader.howManyEnemiesKilledAtThisLevel.ToString() + " out of " + loader.howManyEnemiesKilled.ToString();
        orbsEarned.text = loader.howManyOrbsEarnedInThisLevel.ToString() + " out of " + loader.howManyOrbsEarnedInGame.ToString();
        puzzlePieces.text = loader.howManyPiecesSoFar.ToString() + " out of " + loader.howManyPiecesInTotal.ToString();
    }

}
