using UnityEngine;

public class MarkerColliderScrpit:MonoBehaviour {

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.layer == 10) {
            col.transform.GetComponent<LevelMarkerScript>().InRange();
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.gameObject.layer == 10) {
            col.transform.GetComponent<LevelMarkerScript>().OutOfRange();
        }
    }
}