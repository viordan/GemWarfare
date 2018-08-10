using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSword : MonoBehaviour {


    Transform myTransform;
	// Use this for initialization
	void Start () {
        myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		   //myTransform.Rotate(new Vector3(0,0,1) * Time.deltaTime * 5f,Space.World);// see above for right (x ) axis
    }
}
