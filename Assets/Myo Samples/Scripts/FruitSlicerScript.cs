using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FruitSlicerScript : MonoBehaviour {
	private GameObject mainCamera;

	private Text scoreText;
	private GameObject fruitPool;
	private GameObject fruitSpawner;
	private GameObject pointer;
	private SphereCollider pColl;
	private GameObject floor;

	private bool hit = false;
	private bool active = true;
	private bool respawn = false;
	private int score;
	private int bestCombo = 0;

	private MeshCollider coll;
	// Use this for initialization
	void Start () {
		
		mainCamera = GameObject.Find("Main Camera");
		scoreText = mainCamera.GetComponent<CameraScript>().txtScore;
		pointer = mainCamera.GetComponent<CameraScript>().pointer;
		fruitSpawner = mainCamera.GetComponent<CameraScript>().fruitSpawner;
		fruitPool = mainCamera.GetComponent<CameraScript>().fruitPool;
		pColl = pointer.GetComponent<SphereCollider>();
		floor = mainCamera.GetComponent<CameraScript>().floor;
	}
	
	// Update is called once per frame
	void Update () {
		if(respawn) {
			returnToPool();
		}

		coll = transform.GetComponent<MeshCollider>();
		if (coll.bounds.Intersects(pColl.bounds)) {
			// Pointer hit fruit, return to fruit pool
			hit = true;
		}

		if (coll.bounds.Intersects(floor.GetComponent<BoxCollider>().bounds)) {
			returnToPool();
			fruitSpawner.GetComponent<FruitSpawner>().setCombo(0);
			fruitSpawner.GetComponent<FruitSpawner>().addFruitDropped();
			fruitSpawner.GetComponent<FruitSpawner>().setPowerCombo(0);
			scoreText.color = Color.white;
		}

		if (hit && active && transform.parent != fruitPool.transform) {
			//StartCoroutine(FadeTo(transform, 1.0f, 1.0f));
			// Prevent fruit from being hit again
			active = false;
			returnToPool();
			// Add 10 to the score
			fruitSpawner.GetComponent<FruitSpawner>().setScore(fruitSpawner
				.GetComponent<FruitSpawner>().getScore() + 10);

			// Set Combo
			// Increase combo by 1
			int combo = fruitSpawner.GetComponent<FruitSpawner>().getCombo();
			combo += 1;
			fruitSpawner.GetComponent<FruitSpawner>().setCombo(combo);
			int bestCombo = fruitSpawner.GetComponent<FruitSpawner>().getBestCombo();

			if(combo > bestCombo) {
				fruitSpawner.GetComponent<FruitSpawner>().setBestCombo(combo);
			}

			// Set PowerCombo
			int powerCombo = fruitSpawner.GetComponent<FruitSpawner>().getPowerCombo();
			powerCombo +=1;
			fruitSpawner.GetComponent<FruitSpawner>().setPowerCombo(powerCombo);

			if(powerCombo % 5 == 0) {
				// Set powerup to ready
				scoreText.color = Color.red;
				fruitSpawner.GetComponent<FruitSpawner>().setPowerUpReady(true);
			}
		}

		if(transform.position.y < - 100) {
			returnToPool();
			fruitSpawner.GetComponent<FruitSpawner>().addFruitDropped();
			fruitSpawner.GetComponent<FruitSpawner>().setCombo(0);
			scoreText.color = Color.white;
			fruitSpawner.GetComponent<FruitSpawner>().setPowerCombo(0);

		}
	}

	public void returnToPool() {
		respawn = false;
		// Return fruit to fruitpool
		transform.SetParent(fruitPool.transform);
		active = true;
		transform.position = fruitPool.transform.position;
		transform.GetComponent<Rigidbody>().isKinematic = true;
	}

	public bool toRespawn() {
		return respawn;
	}

	public void toRespawn(bool r) {
		this.respawn = r;
	}
}