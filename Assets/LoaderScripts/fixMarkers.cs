using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
public class fixMarkers:MonoBehaviour {
    public float YLocation;
    GameControl control; // static control
    SolarSystem solarSystem; // new solar system
    //array to manipulate
    //public Transform[] bottomsArray;
    //public Transform[] topsArray;
    //object holder arrays
    //public Transform bottomsObjects;
    //public Transform topsObjects;
    //public Vector3 randomLocationLocalScale;
    //List<Transform> piecesLocationsList;
    void Start() {
        solarSystem = new SolarSystem();  // get solar system
        control = GameControl.control; // chache control
        //bottomsArray = new Transform[60]; // new array
        //topsArray = new Transform[60]; // new array
        //randomLocationLocalScale = new Vector3(.1f,.1f,.1f); // change this is you need the scale changed
        Action(); // do stuff
        YLocation = 4f;
    }
    void Action() {
        //populate arrays
        //for (int i = 0; i < 60; i++) {
        //    //bottomsArray[i] = bottomsObjects.GetChild(i);
        //    topsArray[i] = topsObjects.GetChild(i);
        //}
        //join tops and bottoms to create the whole piece
        //for (int i = 0; i < bottomsArray.Length; i++)
        //{
        //    bottomsArray[i].SetParent(topsArray[i].GetChild(0));
        //}
        //start looping
        foreach (GameObject _planet in solarSystem.planetList) { // for every planet
            foreach (Transform _level in _planet.transform) { // for every level
                Transform _markerHolder = _level.GetChild(1); // puzzlePlace definition
                Transform _marker = _markerHolder.GetChild(0);
                Transform _number = _markerHolder.GetChild(1);


                _marker.SetParent(_number);
                _marker.transform.localPosition = new Vector3(-0.0003f,-.003f,-.05f);
                _marker.gameObject.SetActive(false);
                
            }
        }
    }
}