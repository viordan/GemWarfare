using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMoonLogic : MonoBehaviour {
    //public static OrbitLogic orbit;
    public bool spin;
    private float mySpeed;
	private Transform myTransform;


	void Start(){
        spin = true;
		myTransform = transform; //chache transform, this seems dumb but prevents a lookup, this saves cycles
		mySpeed = Random.Range (.05f, .1f);// weeeeeeeee speed
	}


	void Update(){
		if (spin){//weeeee!!!!!!!!!
			myTransform.Rotate(0,50 * Time.deltaTime*mySpeed, 0 ); //self explanitory
		}
	}

    public void SetSpin() {// this is to start/stop the weeeeeee!!!
        if (spin) { spin = false; } else spin = true;
    }
}
