using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour {
	public GameObject spawner;
	public GameObject uiPanel;
	public GameObject gameOverPanel;
	public GameObject instructionsPanel;

	private bool tutEnabled = false;

	void Start () {
		// Pause time before game start
		Time.timeScale = 0;
		showMainMenu();
		hideUI();
	}

	public void startGame() {
		hideMainMenu();
		showUI();
		Time.timeScale = 1f;
	}

	public void gameOver() {
		hideUI();
		showGameOverMenu();
	}

	public void hideUI() {
		uiPanel.GetComponent<CanvasGroup>().alpha = 0;
	}

	public void showUI() {
		uiPanel.GetComponent<CanvasGroup>().alpha = 1;
	}

	public void showMainMenu() {
		hideGameOverMenu();
		hideInstructions();
		// Reset all scores
		spawner.GetComponent<FruitSpawner>().clearScores();
		// Show main menu context
		GetComponent<CanvasGroup>().alpha = 1;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		GetComponent<CanvasGroup>().interactable = true;
	}

	public void hideMainMenu() {
		GetComponent<CanvasGroup>().alpha = 0;
		GetComponent<CanvasGroup>().blocksRaycasts = false;
		GetComponent<CanvasGroup>().interactable = false;
	}

	public void showInstructions() {
		tutEnabled = true;
		hideMainMenu();
		instructionsPanel.GetComponent<CanvasGroup>().alpha = 1;
		instructionsPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
		instructionsPanel.GetComponent<CanvasGroup>().interactable = true;

	}

	public void hideInstructions() {
		instructionsPanel.GetComponent<CanvasGroup>().alpha = 0;
		instructionsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
		instructionsPanel.GetComponent<CanvasGroup>().interactable = false;

	}

	public void hideGameOverMenu() {
		gameOverPanel.GetComponent<CanvasGroup>().alpha = 0;
		gameOverPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameOverPanel.GetComponent<CanvasGroup>().interactable = false;
	}

	public void showGameOverMenu() {
		gameOverPanel.GetComponent<CanvasGroup>().alpha = 1;
		gameOverPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameOverPanel.GetComponent<CanvasGroup>().interactable = true;
	}

	public void setTutEnabled(bool tut) {
		this.tutEnabled = tut;
	}

	public bool getTutEnabled() {
		return tutEnabled;
	}
}
