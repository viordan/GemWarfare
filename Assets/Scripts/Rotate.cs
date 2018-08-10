using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
	//this will rotate an object to face the player.  Was used for the original 3d Text labels, now used for the sun position rotator

	void Update () {
		if (PlayerScript.player != null) {
			gameObject.transform.LookAt (2 * transform.position - PlayerScript.player.transform.position);
		}
    }
}
