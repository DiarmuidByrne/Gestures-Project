using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FruitSpawner : MonoBehaviour {
	private List<GameObject> spawners = new List<GameObject>();
	public GameObject fruitPool;
	public GameObject mainMenu;
	// Displays results when game is over
	public Text scoreText, comboText;
	public Text finalScoreText, finalComboText;
	public GameObject gameOverPanel;

	private const float MAX_TIME = 3f;
	private const float MIN_TIME = 1.5f;
	private bool timerActive = false;
	private float timeToSpawn;
	public int fruitsDropped = 0;

	private int score = 0;
	// Controls when powerups can be played
	// Keept track of overall combo since the last fruit was missed
	private int combo = 0;
	private int bestCombo = 0;
	// Keeps track of the combo since the last time a powerup was used
	private int powerCombo = 0;
	private bool powerupReady = false;
	// Game over
	private bool gameOver = false;

	void Start () {
		foreach(Transform child in transform) {
			spawners.Add(child.gameObject);
		}

		for(int i = 0; i <= 15; i++) {
			GameObject tmp1 = Instantiate(Resources.Load("Apple_Full") as GameObject);
			GameObject tmp2 = Instantiate(Resources.Load("Banana_Full") as GameObject);
			GameObject tmp3 = Instantiate(Resources.Load("Orange_Full") as GameObject);

			// Start each fruit in the pool. 
			// They will be picked from here when a spawner is activated
			tmp1.transform.GetComponent<FruitSlicerScript>().toRespawn(true);
			tmp2.transform.GetComponent<FruitSlicerScript>().toRespawn(true);
			tmp3.transform.GetComponent<FruitSlicerScript>().toRespawn(true);
		}
	}

	void Update() {
		scoreText.text = "Score: " + score;
		comboText.text = "Combo: " + combo;

		if(fruitsDropped == 10 ) {
			// Game over
			Time.timeScale = 0;
			mainMenu.GetComponent<MainMenuScript>().gameOver();
			finalScoreText.text = "Final Score: " + score;
			finalComboText.text = "Best Combo: " + bestCombo;
			gameOver = true;
		}
		// If no timer active, start timer
		if (!timerActive) {
			timeToSpawn = Random.Range(MIN_TIME, MAX_TIME);
			timerActive = true;
		}
		timeToSpawn -= Time.deltaTime;

		if (timeToSpawn <= 0) {
			timerActive = false;

			// Spawn random (Weighted) fruit in a random spawner
			// 1. Pick Random fruit
			int randomFruit = Random.Range(0, fruitPool.transform.childCount);
			// 2. Add to Random spawn location
			int spawnerIndex = Random.Range(0, spawners.Count);

			GameObject randomSpawner = transform.GetChild(spawnerIndex).gameObject;
			GameObject currentFruit = fruitPool.transform.GetChild(randomFruit).gameObject;

			currentFruit.transform.SetParent(randomSpawner.transform);
			currentFruit.transform.position = currentFruit.transform.parent.position;
			currentFruit.transform.GetComponent<Rigidbody>().isKinematic = false;

			if(randomSpawner.name.Contains("SpawnerBottom")) {
				// Add force to fruit spawning from below the screen.
				// If they spawn from the left, the force will have a right bias
				// Right spawning fruit will have a left bias
				if(randomSpawner.name.EndsWith("R")) {
					// Add random force with left bias
					int xForce = (Random.Range(0, 1000) * -1);
					int yForce = Random.Range(3000, 3250);
					currentFruit.GetComponent<Rigidbody>().AddForce(new Vector3(xForce, yForce, 0), ForceMode.Impulse);
				}

				else if(randomSpawner.name.EndsWith("L")) {
					// Add random force with right bias
					int xForce = Random.Range(0, 1000);
					int yForce = Random.Range(3000, 3250);
					currentFruit.GetComponent<Rigidbody>().AddForce(new Vector3(xForce, yForce, 0), ForceMode.Impulse);
				} 
				else  {
					// Spawn in middle
					int xForce = Random.Range(750, 750);
					int yForce = Random.Range(3000, 3250);
					currentFruit.GetComponent<Rigidbody>().AddForce(new Vector3(xForce, yForce, 0), ForceMode.Impulse);

				}
			}
		}
	}

	public void clearScores() {
		score = combo = powerCombo = fruitsDropped = 0;
	}

	public void setCombo(int combo) {
		this.combo = combo;
	}

	public int getCombo() {
		return combo;
	}

	public int getPowerCombo() {
		return powerCombo;
	}

	public void setPowerCombo(int powerCombo) {
		this.powerCombo = powerCombo;
	}

	public bool isPowerupReady() {
		return powerupReady;
	}

	public int getScore() {
		return score;
	}

	public void setScore(int score) {
		this.score = score;
	}

	public int getBestCombo() {
		return bestCombo;
	}

	public void setBestCombo(int bestCombo) {
		this.bestCombo = bestCombo;
	}

	public void setPowerUpReady(bool ready) {
		powerupReady = ready;
	}

	public void addFruitDropped() {
		this.fruitsDropped += 1;
	}

	public bool getGameOver() {
		return gameOver;
	}

	public void setGameOver(bool gameOVer) {
		this.gameOver = gameOver;
	}
}
