using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
public class fixSwordRunes : MonoBehaviour {

    GameControl control; // static control
    SolarSystem solarSystem; // new solar system
    //array to manipulate
    //public Transform[] bottomsArray;
    public Transform[] topsArray;
    public Transform[] runeArray;
    //object holder arrays
    //public Transform bottomsObjects;
    public Transform topsObjects;
    public Transform runeHolders;
    public Vector3 randomLocationLocalScale;
    //List<Transform> piecesLocationsList;
    void Start() {
        solarSystem = new SolarSystem();  // get solar system
        control = GameControl.control; // chache control
        runeArray = new Transform[10]; // new array
        topsArray = new Transform[10]; // new array
        randomLocationLocalScale = new Vector3(.1f,.1f,.1f); // change this is you need the scale changed
        Action(); // do stuff
    }
    void Action() {
        //populate arrays
        for (int i = 0; i < 10; i++) {
            runeArray[i] = runeHolders.GetChild(i);
            topsArray[i] = topsObjects.GetChild(i);
        }
        //join tops and bottoms to create the whole piece
        for (int i = 0; i < topsArray.Length; i++) {
            topsArray[i].SetParent(runeArray[i]);
            topsArray[i].localPosition = new Vector3(0,0,0);
            topsArray[i].localScale /= 200;
            topsArray[i].localRotation = Quaternion.Euler(0,0,0);
        }
        //start looping
        //foreach (GameObject _planet in solarSystem.planetList) { // for every planet
        //    foreach (Transform _level in _planet.transform) { // for every level
        //        if (_level.childCount > 3) { // if it contains more than 3 children (some don't have stuff)... 
        //            Transform _puzzlePlace = _level.GetChild(3); // puzzlePlace definition
        //            foreach (Transform _randomLocation in _puzzlePlace) { // for every random location
        //                if (!_randomLocation.gameObject.activeSelf) { // if it's not active, activate
        //                    _randomLocation.gameObject.SetActive(true);
        //                }
        //                GameObject _randomPiece = Instantiate(topsArray[(int)Random.Range(0,59)].gameObject); // get a random piece from the array
        //                _randomPiece.transform.position = _randomLocation.position; // position the piece at location
        //                _randomPiece.transform.rotation = _randomLocation.rotation; // rotate the piece as location
        //                _randomPiece.transform.SetParent(_randomLocation); // set parent
        //                _randomLocation.localScale = randomLocationLocalScale; // now that the hierarchy is correct, scale accordingly
        //                _randomLocation.GetComponent<MeshRenderer>().enabled = false; // if the mesh renderer is enabled, disable that shit
        //            }
        //        }
        //    }
        //}
    }
}
