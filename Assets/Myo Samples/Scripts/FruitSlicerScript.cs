using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FruitSlicerScript : MonoBehaviour {
	private GameObject mainCamera;

	private GameObject fruitPool;
	private GameObject fruitSpawner;
	private GameObject pointer;
	private SphereCollider pColl;
	private GameObject floor;

	private bool hit = false;
	private bool active = true;
	private bool respawn = false;
	private int score;


	private MeshCollider coll;
	// Use this for initialization
	void Start () {
		mainCamera = GameObject.Find("Main Camera");
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
			mainCamera.GetComponent<CameraScript>().setCombo(0);

		}

		if (hit && active && transform.parent != fruitPool.transform) {
			//StartCoroutine(FadeTo(transform, 1.0f, 1.0f));
			returnToPool();
			// Add 10 to the score
			mainCamera.GetComponent<CameraScript>().setScore(mainCamera
				.GetComponent<CameraScript>().getScore() + 10);
			
			// Increase combo by 1
			int combo = mainCamera.GetComponent<CameraScript>().getCombo();
			combo += 1;
			// Set Combo
			mainCamera.GetComponent<CameraScript>().setCombo(combo);

			// Prevent fruit from being hit again
			active = false;

			if(combo % 5 == 0) {
				// Set powerup to ready
				fruitSpawner.GetComponent<FruitSpawner>().setPowerUpReady(true);
			}
		}

		if(transform.position.y < - 100) {
			returnToPool();
			mainCamera.GetComponent<CameraScript>().setCombo(0);
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

	IEnumerator FadeTo(Transform fruit, float aValue, float aTime)
	{
		float alpha = fruit.GetComponent<MeshRenderer>().material.color.a;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
			fruit.GetComponent<Renderer>().material.color = newColor;
			yield return null;
		}
	}

	public bool toRespawn() {
		return respawn;
	}

	public void toRespawn(bool r) {
		this.respawn = r;
	}
}