using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerColliderNameingScript : MonoBehaviour {

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.layer == 10) {
            col.transform.GetComponent<LevelMarkerScript>().DisplayLevelName();
        }
    }

    void OnTriggerExit(Collider col) {

    }
}
