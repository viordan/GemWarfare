using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopPiece :AbstractPiece {

	Transform myTransform; // cached transform
    PiecesController piecesController;
	public bool toPuzzle{ get; set;} //heading toPuzzle or away from puzzle
    bool iMLonely;
	Coroutine movePiece; // the move coroutine
	Coroutine spinPiece; // the move coroutine
    float spinSpeed=1f;

    Transform myOriginalParent; // cache my parent

	//PuzzleScript puzzleScript; //the puzzle's script
    public Transform matchToBottom;
    public Transform matchToCenter;
    Transform myShadow;
    public bool scaleDoubled;

	void OnEnable(){
        piecesController = PiecesController.piecesController;
        myTransform = transform;//cache my transform
        matchToBottom = myTransform.GetChild(0);// get matchBottom
        matchToCenter = myTransform.parent;// get matchCenter
        myShadow = myTransform.parent.parent.GetChild(0); ;
        if (myTransform.parent.rotation != myShadow.rotation) {
            myTransform.parent.rotation = myShadow.rotation;
        }
        //puzzleScript = puzzle.GetComponent<PuzzleScript> ();// define puzzle script
        //StopAllCoroutines();
        ParentToChild(myTransform, matchToCenter);//make the center my parent
        spinPiece = StartCoroutine(SpinPiece()); // start spinning 
    }

    void ToggleScale(Transform _selectedPiece,bool towardsPuzzle) {
        if (towardsPuzzle) {
            if (!scaleDoubled) {
                _selectedPiece.localScale *= 2f;
                scaleDoubled = true;
            }
        } else{
            if (scaleDoubled) {
                _selectedPiece.localScale /= 2f;
                scaleDoubled = false;
            }
        }
    }


    public void SetMyOriginalParent(Transform _newParent) {
        myOriginalParent = _newParent;
    }
	

	void EvaluateSlot(Transform _slot){ // eval the slot
        if (myId == _slot.GetComponent < PuzzleSlot >().bottomPieceID) {
            //print("myID = " + myId + " holderID = " + _slot.GetComponent<PuzzleSlot>().bottomPieceID);
			piecesController.ToggleSolvedList (myTransform,true); // ? toggle from list shouldn't eval do this?
		}
	}

    void ToggleParents() {
        if (myTransform.parent == matchToCenter) {
            ParentToChild(matchToCenter, myTransform); 
            ParentToChild(myTransform, matchToBottom);
        } else if (myTransform.parent == matchToBottom) {
            ParentToChild(matchToBottom, myTransform);
            ParentToChild(myTransform, matchToCenter);
        } else {
            print("Something went wrong, I don't know who my parent is");
        }
    }

    void ParentToChild(Transform _parent, Transform _child) {  
        _child.SetParent(_parent.parent);// set the child on the same node level as the parent 
        _parent.SetParent(_child);// parent to the child
    }

    void ToggleDirrection() {// every time this is run toggle bool// every time you click it it changes direction
        ToggleParents();//toggle between center and bottom
        StopAllCoroutines();
        if (toPuzzle) {
            toPuzzle = false;
        } else {
            toPuzzle = true;
        }
    }

    public void MoveToPuzzle(Transform _slotToMoveTo) {
        ToggleDirrection();// change the direction 
        ToggleScale(myTransform.parent,true);
        _slotToMoveTo.GetComponent<PuzzleSlot>().havePiece = true; // the slot has a piece now
        myTransform.parent.parent = _slotToMoveTo; // parent to the selected slot
        
        //EvaluateSlot(_slotToMoveTo); // evaluate the choice
        movePiece = StartCoroutine(MovePiece(_slotToMoveTo)); // delegate the coroutine with new heading
    }

    public void MoveBackToHolder() {
        ToggleDirrection();// change the direction 
        ToggleScale(myTransform.parent,false);
        myTransform.parent.parent.GetComponent<PuzzleSlot>().havePiece = false; // remove myself from the slot that I am in
        myTransform.parent.parent = myOriginalParent; // make my original parent my parent
        //print(myTransform.parent.parent.childCount);
        if (!myTransform.parent.parent.GetChild(0).gameObject.activeSelf) {
            iMLonely=true;
        }
        piecesController.ToggleSolvedList(myTransform,false); // remove from solved list since it's going back to the inventory
        movePiece = StartCoroutine(MovePiece(myOriginalParent)); // start moving with new destination
    }


    IEnumerator MovePiece(Transform _destination) { // move the piece coroutine
        while (Vector3.Distance(_destination.position,myTransform.parent.position) > 0.1f) {
            myTransform.parent.position = Vector3.MoveTowards(myTransform.parent.position,_destination.position,Time.deltaTime * 20f);
            yield return null;
        }
        StopAllCoroutines();
        if (iMLonely) {
            matchToCenter.gameObject.SetActive(false);
        } else {
            if (!toPuzzle) {
                StartCoroutine(RotatePiece(_destination.GetChild(0))); // rotate the piece coroutine start, no delegate needed as we don't need to address it.
            } else {
                StartCoroutine(RotatePiece(_destination)); // rotate the piece coroutine start, no delegate needed as we don't need to address it.
            }
        }
    }

	IEnumerator RotatePiece(Transform _destination){// the rotate coroutine
        while (myTransform.parent.rotation != _destination.rotation) {// while my parent's rotation doesn't match the destination
            myTransform.parent.rotation = Quaternion.RotateTowards(myTransform.parent.rotation,_destination.rotation,Time.deltaTime * 300f);
            yield return null;
        }
        if (!toPuzzle) {
            StopAllCoroutines();
            spinPiece = StartCoroutine(SpinPiece());
        } else {
            EvaluateSlot(_destination);
        }
	}

    IEnumerator SpinPiece() {// spin should only be called back in the inventory, hence center is parent
        while (true) {// while this coroutine is active
            matchToCenter.Rotate(0,50 * Time.deltaTime * spinSpeed,0); //rotate on Y
            yield return null;// pause till next frame
        }
    }

    private void OnDisable() {// disable all coroutines
        StopAllCoroutines();
    }

}
