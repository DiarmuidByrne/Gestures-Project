using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour {
	public GameObject pointer;

	private BoxCollider b; 
	private SphereCollider pB;

	
	void Start() {
		b = transform.GetComponent<BoxCollider>();
		pB = pointer.GetComponent<SphereCollider>();
	}

	void Update () {
		if(b.bounds.Intersects(pB.bounds)) {
			pointer.transform.position = new Vector2(transform.position.x*2, transform.position.y*2);
		}
	}
}
