using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class JointMovement : MonoBehaviour {
	public GameObject myo = null;
	public GameObject spawner;
	public GameObject powerUpCanvas;
	public GameObject mainMenuPanel;
	public GameObject tutorialPanel;
	//public GameObject tutorialPanel;
	public Button play, instructions, mainMenu, btnTutorial;
	public Text scoreText;
	private Vector2 referenceVector;
	// Prevent buttons being pressed if the game has begun
	private bool gameStarted = false;
	// This compensates for the position of the Myo armband.
	// When set to true, the current position will center the Myo on screen
	private bool updateReference = false;
	private float powerupTimer = 5f;
	private bool timerActive = false;
	private Pose _lastPose = Pose.Unknown;

	void Update () {
		if (timerActive) {
			powerupTimer -= Time.deltaTime;
			Debug.Log(powerupTimer);
		}
		if(powerupTimer <= 0) {
			powerupTimer = 5f;
			Time.timeScale = 1.1f;
			powerUpCanvas.GetComponent<CanvasGroup>().alpha = 0;

			timerActive = false;
            spawner.GetComponent<FruitSpawner>().setPowerUpReady(false);
		}
		updateReference = false;

		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();

		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;

			if (thalmicMyo.pose == Pose.DoubleTap) {
				updateReference = true;

				ExtendUnlockAndNotifyUserAction(thalmicMyo);
			}

			if (thalmicMyo.pose == Pose.FingersSpread) {
				BoxCollider2D playCollider = play.GetComponent<BoxCollider2D>();
				SphereCollider sc = transform.GetComponent<SphereCollider>();

				// If in position of start button
				if(playCollider.bounds.Intersects(sc.bounds)) {
					// Start game
					mainMenuPanel.GetComponent<MainMenuScript>().startGame();
				}

				BoxCollider2D tutCollider = btnTutorial.GetComponent<BoxCollider2D>();

				// IF in position of tutorial button
				if(tutCollider.bounds.Intersects(sc.bounds) && !gameStarted) {
					mainMenuPanel.GetComponent<MainMenuScript>().showInstructions();
				}

				// If in position of main menu button, and game is over
				bool gameOver = spawner.GetComponent<FruitSpawner>().getGameOver();
				bool tutEnabled = mainMenuPanel.GetComponent<MainMenuScript>().getTutEnabled();
				BoxCollider2D mainMenuColl = tutorialPanel.transform.GetChild(0).GetComponent<BoxCollider2D>();
				if(mainMenuColl.bounds.Intersects(sc.bounds)) {
					if (gameOver || tutEnabled) {
						mainMenuPanel.GetComponent<MainMenuScript>().showMainMenu();
					}
				}
			}

			if (thalmicMyo.pose == Pose.Fist) {
				if(spawner.GetComponent<FruitSpawner>().isPowerupReady()) {
					Time.timeScale = .75f;
					// PowerUp Timer start
					powerupTimer -= Time.deltaTime;
					// Make canvas visible for effect
					scoreText.color = Color.white;
					timerActive = true;
					spawner.GetComponent<FruitSpawner>().setPowerUpReady(false);
					spawner.GetComponent<FruitSpawner>().setPowerCombo(0);

					powerUpCanvas.GetComponent<CanvasGroup>().alpha = 1;
				}
			}
		}
		if (Input.GetKeyDown("r")) {
			updateReference = true;
		}

		if (updateReference) {
			referenceVector = new Vector2(myo.transform.forward.x*-1 *200, myo.transform.forward.y * 70);
        }
		// Here the anti-roll and yaw rotations are applied to the myo Armband's forward direction to yield
		// the orientation of the joint.
		transform.position = new Vector2((myo.transform.forward.x*-1 *200) - referenceVector.x, myo.transform.forward.y * 70 - referenceVector.y);
	}

	void ExtendUnlockAndNotifyUserAction(ThalmicMyo myo) {
		ThalmicHub hub = ThalmicHub.instance;

		if (hub.lockingPolicy == LockingPolicy.Standard) {
			myo.Unlock(UnlockType.Timed);
		}

		myo.NotifyUserAction();
	}
}
