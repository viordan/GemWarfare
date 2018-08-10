﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {


	public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
	public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.


	public bool zoomIn = false;
    public bool zoomOut = false;
    public Vector3 endCamera;
    public Vector3 startCamera;
    // Use this for initialization
    void Start () {
        //endCamera = new Vector3(46, 8, 76);
        //startCamera = new Vector3(-500, 500, -500);
		startCamera = new Vector3(71, 46, 95);
		endCamera = new Vector3(200, -300, -1000);
    }
	
	// Update is called once per frame
	void Update () {
        
			//// If there are two touches on the device...
			//if (Input.touchCount == 2)
			//{
			//	// Store both touches.
			//	Touch touchZero = Input.GetTouch(0);
			//	Touch touchOne = Input.GetTouch(1);

			//	// Find the position in the previous frame of each touch.
			//	Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			//	Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			//	// Find the magnitude of the vector (the distance) between the touches in each frame.
			//	float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			//	float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			//	// Find the difference in the distances between each frame.
			//	float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			//	// If the camera is orthographic...
			//if (Camera.main.orthographic)
			//	{
			//		// ... change the orthographic size based on the change in distance between the touches.
			//	Camera.main.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

			//		// Make sure the orthographic size never drops below zero.
			//	Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, 0.1f);
			//	}
			//	else
			//	{
			//		// Otherwise change the field of view based on the change in distance between the touches.
			//		Camera.main.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

			//		// Clamp the field of view to make sure it's between 0 and 180.
			//		Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 0.1f, 179.9f);
			//	}
			//}



		//zoomin
        if (Input.GetKeyDown(KeyCode.Z))
        { 
            zoomIn = true;
        }
        else if (Input.GetKeyUp(KeyCode.Z)) {
            zoomIn = false;
        }
            if (zoomIn==true)
        { 
            transform.position = Vector3.Lerp(transform.position, endCamera, Time.deltaTime*2);
        }
        if (Input.GetKeyDown(KeyCode.X))
        { 
            zoomOut = true;
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            zoomOut = false;
        }
        if (zoomOut == true)
        { 
            transform.position = Vector3.Lerp(transform.position, startCamera, Time.deltaTime * 2);
        }

    }
}
