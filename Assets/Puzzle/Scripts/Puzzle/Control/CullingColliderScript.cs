using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CullingColliderScript : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		if (col.gameObject.layer == 23) {
            if (col.transform.childCount ==1) {
                if (col.transform.GetChild(0).gameObject.activeSelf) {
                    col.transform.GetChild(0).gameObject.SetActive(false);
                }
            }else if (col.transform.childCount == 2) {
                if (col.transform.GetChild(0).gameObject.activeSelf) {
                    col.transform.GetChild(0).gameObject.SetActive(false);
                }
                if (col.transform.GetChild(1).gameObject.activeSelf) {
                    col.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }
    void OnTriggerExit(Collider col) {
        if (col.gameObject.layer == 23) {
            if (col.transform.childCount == 1) {
                if (!col.transform.GetChild(0).gameObject.activeSelf) {
                    col.transform.GetChild(0).gameObject.SetActive(true);
                }
            } else if (col.transform.childCount == 2) {
                if (!col.transform.GetChild(0).gameObject.activeSelf) {
                    col.transform.GetChild(0).gameObject.SetActive(true);
                }
                if (!col.transform.GetChild(1).gameObject.activeSelf) {
                    col.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
    }
}
