using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public Text txtScore;
	public GameObject fruitPool;
	public GameObject pointer;
	public GameObject fruitSpawner;
	public GameObject floor;
	public Text comboTxt;

	private int score = 0;
	private int combo = 0;
	// Use this for initialization

	public void setScore(int score) {
		this.score = score;
	}

	public int getScore() {
		return score;
	}

	public void setCombo(int combo) {
		this.combo = combo;
	}

	public int getCombo() {
		return combo;
	}
	
	// Update is called once per frame
	void Update () {
		txtScore.text = "Score: " + score;
		comboTxt.text = "Combo: " + combo;
	}
}
