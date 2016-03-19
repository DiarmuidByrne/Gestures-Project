using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class JointMovement : MonoBehaviour {
	public GameObject myo = null;
	public GameObject spawner;
	public GameObject powerUpCanvas;

	private Vector2 initialPos;
	private Vector2 referenceVector;

	// This compensates for the position of the Myo armband.
	// When set to true, the current position will center the Myo on screen
	private bool updateReference = false;
	private float powerupTimer = 5f;
	private Pose _lastPose = Pose.Unknown;
	
	void Start() {
		initialPos = transform.position;
	}

	void Update () {
		
		if(powerupTimer <= 0) {
			powerupTimer = 5f;
			Time.timeScale = 1f;
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

			if (thalmicMyo.pose == Pose.Fist) {
				if(spawner.GetComponent<FruitSpawner>().isPowerupReady()) {
					Time.timeScale = .75f;
					// PowerUp Timer start
					powerupTimer -= Time.deltaTime;
					// Make canvas visible for effect
					Debug.Log("Powerup Started");
					powerUpCanvas.GetComponent<CanvasGroup>().alpha = 1;

				}
			}
		}
		if (Input.GetKeyDown("r")) {
			updateReference = true;
		}

		if (updateReference) {
			referenceVector = new Vector2(myo.transform.forward.z *200, myo.transform.forward.y * 70);
        }

		// Here the anti-roll and yaw rotations are applied to the myo Armband's forward direction to yield
		// the orientation of the joint.
		transform.position = new Vector2((myo.transform.forward.z *200) - referenceVector.x, myo.transform.forward.y * 70 - referenceVector.y);
	}

	void ExtendUnlockAndNotifyUserAction(ThalmicMyo myo) {
		ThalmicHub hub = ThalmicHub.instance;

		if (hub.lockingPolicy == LockingPolicy.Standard) {
			myo.Unlock(UnlockType.Timed);
		}

		myo.NotifyUserAction();
	}
}
