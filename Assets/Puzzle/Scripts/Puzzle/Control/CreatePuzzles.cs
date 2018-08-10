using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CreatePuzzles : MonoBehaviour {

	int []randomIntKey;
	Transform[] bottomsArray;
    Transform[] topsArray;
	Transform[] topsShadowsArray;
	Transform[] bottomsShadowsArray;
	[SerializeField] GameObject puzzleHolder;
	[SerializeField] GameObject menuHolder;
    [SerializeField] Transform tops;
    [SerializeField] Transform topsShadows;
    [SerializeField] Transform bottomsShadows;
    [SerializeField] Transform bottoms;

	void Start () {
		GetRandomKey (); // get a key
		CreateArrays (); // create the arrays
		ApplyRandomKeyToArrays (); // set them to key
		CreatePuzzlesInScene(); // create the puzzles (and place bottoms)
        GetTopsInHolder(); // place the tops
    }

    void GetRandomKey() {
        int[] arr = new int[60]; // new array
        for (int i = 0; i < 60; i++) {
            arr[i] = i;
        }// fill it with 0-59
        System.Random rnd = new System.Random(); // get a new random
        randomIntKey = arr.OrderBy(x => rnd.Next()).ToArray(); // sramble
    }

    void CreateArrays() {
        bottomsArray = new Transform[60];
        topsArray = new Transform[60];
        topsShadowsArray = new Transform[60];
        bottomsShadowsArray = new Transform[60];
        for (int i = 0; i < 60; i++) {
            bottomsArray[i] = bottoms.GetChild(i);
        }
        for (int i = 0; i < 60; i++) {
            topsArray[i] = tops.GetChild(i);
        }
        for (int i = 0; i < 60; i++) {
            topsShadowsArray[i] = topsShadows.GetChild(i);
        }
        for (int i = 0; i < 60; i++) {
            bottomsShadowsArray[i] = bottomsShadows.GetChild(i);
        }
    }

    void ApplyRandomKeyToArrays() {
        Transform[] newRandom = new Transform[60]; // empty new reandom
        for (int i = 0; i < bottomsArray.Length; i++) { // go through bottomsArray
            newRandom[i] = bottomsArray[randomIntKey[i]]; // scramble based on key
        }
        bottomsArray = newRandom;//assign as new bottomsArray
    }

    void CreatePuzzlesInScene() {
        for (int i = 0; i < 1; i++) {// create 10 puzzles
            GameObject puzzle = Instantiate(puzzleHolder,new Vector3(-9f+i * 20f,-5f,-7.5f),Quaternion.identity);// get the prefab and place it
            puzzle.name = "puzzle" + i.ToString();//name it to iteration
            GetBottomsInHolder(i,puzzle); //populate them
            PiecesController.piecesController.RegisterPuzzle(puzzle.transform.GetChild(0),puzzle.transform.GetChild(1),puzzle.transform.GetChild(2));
        }
    }

    void GetBottomsInHolder (int _index, GameObject puzzle){// for each puzzle 
		Transform[] bottomPieceHolder = new Transform[6]; // empty 6 holders
        for (int child = 0; child < bottomPieceHolder.Length; child++) { // get the first 6 children from the PuzzleSphere (child(0)) 
            bottomPieceHolder[child] = puzzle.transform.GetChild(0).GetChild(child);
        }
		for (int i = 0; i < bottomPieceHolder.Length; i++) {//for every bottom holder
			bottomsArray [i + (_index * 6)].position = bottomPieceHolder [i].GetChild(0).position; // get a bottom and match position to holder
            bottomsArray [i + (_index * 6)].rotation = bottomPieceHolder[i].GetChild(0).rotation; // match rotation
            bottomsArray[i + (_index * 6)].SetParent(bottomPieceHolder[i].GetChild(0)); // parent to bottom
            bottomPieceHolder[i].GetComponent<PuzzleSlot>().bottomPieceID= bottomsArray[i + (_index * 6)].GetChild(0).GetComponent<BottomPiece>().myId; // get the ID of the bottom
        }
	}

    void GetTopsInHolder() {
        Transform[] topPieceHolders = new Transform[60]; // empty 60 holders
        for (int child = 0; child < topPieceHolders.Length; child++) { // register the holders from to menu shpere
            topPieceHolders[child] = menuHolder.transform.GetChild(0).GetChild(child);
        }
        for (int i = 0; i < topPieceHolders.Length; i++) { //for every top holder
            topsShadowsArray[i].position = topPieceHolders[i].position; // get a top piece and postion it in where holder is
            topsShadowsArray[i].rotation = topPieceHolders[i].rotation; // rotate it
            topsShadowsArray[i].parent = topPieceHolders[i]; // parent it to holder
            //topsShadowsArray[i].GetChild(0).GetComponent<TopPiece>().SetMyOriginalParent(topPieceHolders[i]); //assign the Original Parent as holder (to be used on return)
        }
        for (int i = 0; i < topPieceHolders.Length; i++) { //for every top holder
            bottomsShadowsArray[i].parent = topPieceHolders[i].GetChild(0).GetChild(0).GetChild(0); // parent it to holder
            bottomsShadowsArray[i].rotation = bottomsShadowsArray[i].parent.rotation; // rotate it
            bottomsShadowsArray[i].localPosition = new Vector3(0,-1,0); // get a top piece and postion it in where holder is
            //topsShadowsArray[i].GetChild(0).GetComponent<TopPiece>().SetMyOriginalParent(topPieceHolders[i]); //assign the Original Parent as holder (to be used on return)
        }
        for (int i = 0; i < topPieceHolders.Length; i++) { //for every top holder
            topsArray[i].position = topPieceHolders[i].position; // get a top piece and postion it in where holder is
            topsArray[i].rotation = topPieceHolders[i].rotation; // rotate it
            topsArray[i].parent=topPieceHolders[i]; // parent it to holder
            topsArray[i].GetChild(0).GetComponent<TopPiece>().SetMyOriginalParent(topPieceHolders[i]); //assign the Original Parent as holder (to be used on return)
        }

    }
}
