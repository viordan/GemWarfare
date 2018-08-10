using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRotate : MonoBehaviour {

//
//	public Transform target;
//	public Transform reflection;
//	public Transform myReflection;
//
//	Transform myTransform;
//	Coroutine rotateMe;
//	public float speed;
//	bool gotThere;
//	// Use this for initialization
//	void Start () {
//		myTransform = transform;
//		speed = 100f;
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		//RotateMe ();
//		print (AngleValue ());
//
//		if (Input.GetKeyUp(KeyCode.U)) { // if you press S
//
//			rotateMe = StartCoroutine (RotateMe ());
//
//		}
//	}
//
//	IEnumerator RotateMe(){
//		while (AngleValue()>=5f){
//			float dir = 0;// get the rotation direction as 0
//			if (AngleDir (myTransform.forward, (target.position - myTransform.position), myTransform.up) == 1f) {
//				dir = 1; // if on right it is 1
//			} else {
//				dir = -1; // else it's negative
//			}
//			myTransform.RotateAround (myTransform.position, myTransform.up, dir*speed*Time.deltaTime);// this does the rotate
//			yield return null;
//		}
//	}
//
//	float AngleValue(){
//		Vector3 projection = Vector3.ProjectOnPlane (target.position, myTransform.up);
//		Vector3 myProjection = Vector3.ProjectOnPlane (myTransform.position, myTransform.up);
//		reflection.position = projection;
//		myReflection.position = myProjection;
//		//print ("my projection : " + projection);
//		//targets's y is 0 to ignore height in calculating the angle's value
//		//get the direction
//		Vector3	targetDir = (projection - myProjection).normalized;
//		//get the dot (multiplication of vectors)
//		float dot = Vector3.Dot (targetDir, myTransform.forward);
//		//return the angle value
//		return Mathf.Acos (dot) * Mathf.Rad2Deg;
//	}
//
//	float AngleDir(Vector3 forward, Vector3 targetDir, Vector3 up){ // this functions returns -1 for left 1 for right
//		Vector3 perp = Vector3.Cross (forward, targetDir);
//		float dir = Vector3.Dot (perp, up);
//		if (dir > 0f) {
//			return 1f;
//		} else if (dir < 0f) {
//			return -1f;
//		} else {
//			return 0f;
//		}
//	}
}
