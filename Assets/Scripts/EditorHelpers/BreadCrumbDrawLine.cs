using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BreadCrumbDrawLine : MonoBehaviour {

	public GameObject breadCrumbsPath;
	public Transform toTrace;

	void Start(){
		toTrace = breadCrumbsPath.transform;
	}

	// Update is called once per frame
	void Update () {
		DebugDrawLine ();
	}

	public void DebugDrawLine() {
		//Debug.DrawLine(playerInBattlePositionRelative.transform.position, breadCrumbsPath.transform.GetChild(0).position, Color.white,Time.deltaTime,false);
		for (int j = 0; j < toTrace.childCount-1; j++) {
			Debug.DrawLine(toTrace.GetChild(j).position, toTrace.GetChild(j+1).position, Color.white, Time.deltaTime, false);
		}
	}

}
