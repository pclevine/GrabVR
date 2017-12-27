using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTriggeredAnimation : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		other.gameObject.GetComponent<Animation>.Play ();
	}

}
