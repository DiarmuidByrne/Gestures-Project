using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FruitSpawner : MonoBehaviour {
	private List<GameObject> spawners = new List<GameObject>();
	public GameObject fruitPool;
	private const float MAX_TIME = 5f;
	private const float MIN_TIME = 1.5f;
	private bool timerActive = false;
	private float timeToSpawn;

	// Controls when powerups can be played
	private int combo = 0;
	private bool powerupReady = false;
	private bool timeStarted = false;
	private float powerUpTime = 0;


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
				} else  {
					// Spawn in middle
					int xForce = Random.Range(750, 750);
					int yForce = Random.Range(3000, 3250);
					currentFruit.GetComponent<Rigidbody>().AddForce(new Vector3(xForce, yForce, 0), ForceMode.Impulse);

				}
			}
		}
	}

	public void setCombo(int combo) {
		this.combo = combo;
	}

	public int getCombo() {
		return combo;
	}

	public bool isPowerupReady() {
		return powerupReady;
	}

	public void setPowerUpReady(bool ready) {
		powerupReady = ready;
	}
}
