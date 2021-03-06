using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesController : MonoBehaviour {

	//[SerializeField] Transform puzzleHolder;
    public static PiecesController piecesController;
    public Transform puzzleSphere;
    public Transform puzzleCollider;
    public Transform puzzleSolvedPosition;
    public Transform menu;
    public List<Transform> solved;


    Ray ray;
	RaycastHit hit;
	Transform mainCameraTransform;
	float touchTimer;
    int layerMaskSphereCollider = 1 << 17;
    float spinMultiplier = 1f;
    bool puzzleSolved;

    void Awake() {
        if (piecesController == null) { //if there is not a control already in this scene
            piecesController = this; // the control is this object
        }
    }

    void Start() {
        mainCameraTransform = Camera.main.transform;
        layerMaskSphereCollider = ~layerMaskSphereCollider;
        solved = new List<Transform>();
        menu = transform.GetChild(0);
    }

    public Transform GetPuzzle() {
        return puzzleSphere;
    }

    public void RegisterPuzzle(Transform _puzzleSphere, Transform _puzzleCollider, Transform _puzzleSolvedPosition) {
        puzzleSolvedPosition = _puzzleSolvedPosition;
        puzzleCollider = _puzzleCollider;
        puzzleSphere = _puzzleSphere;
    }

	void Update(){
        if (!puzzleSolved) {
            MouseInput();
            if (Input.GetKeyUp(KeyCode.S)) {
                PuzzleSolved();
            }
        }
        if (Input.GetKeyUp(KeyCode.C)) {
            for (int i=0; i < 20; i++) {
                menu.GetChild(i).GetChild(0).GetChild(0).GetComponent<TopPieceShadow>().ChangeShadowColor();

            }
        }
    }

	void MouseInput() {

		//if (Input.GetMouseButton(0)) {// if there is any touch
		//	ray = Camera.main.ScreenPointToRay(Input.mousePosition);// cast a ray from the camera through the touch
		//	touchTimer += Time.deltaTime; // start counting this timer
		//	Vector3 mouseDirection = new Vector3(-Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);
		//	if (Input.mousePosition.x < Screen.width / 2) {
		//		IfDragged (mouseDirection.x * 10f, mouseDirection.y * 10f, true);
		//	} else {
		//		IfDragged (mouseDirection.x * 10f, mouseDirection.y * 10f, false);
		//	}
		//} else if (touchTimer < .2f && Input.GetMouseButtonUp(0)) {
		//	IfClicked();
		//} else {
		//	touchTimer = 0f;
		//}
	}

	void IfClicked() {
		if (Physics.Raycast (ray, out hit, 2000,layerMaskSphereCollider)) { // extend the ray to 1000 units
			Debug.DrawLine (ray.origin, hit.point);// draws the ray line
			if (hit.collider.gameObject.layer == 21) {// if it's on the puzzle piece layer
                TopPiece clickedOnPiece = hit.transform.GetComponent<TopPiece>();
                if (clickedOnPiece.toPuzzle) {//if it's already going to the puzzle
                    clickedOnPiece.MoveBackToHolder();// move it back to the holder
                } else {//if not going towards the puzzle
                    if (puzzleCollider.GetComponent<ColliderScript>().SelectedPiece != null) {//if there is something selected
                        if (!puzzleCollider.GetComponent<ColliderScript>().SelectedPiece.GetComponent<PuzzleSlot>().havePiece) {// if the slot is empty
                            clickedOnPiece.MoveToPuzzle(puzzleCollider.GetComponent<ColliderScript>().SelectedPiece); // move the piece to the puzzle
                        }
                    }
                }
			}
		}
	}

	void IfDragged(float directionX, float directionY,bool onTheLeft) {
		if (onTheLeft) {
			RotateObject (puzzleSphere, directionX, directionY,Space.World);
		} else {
			RotateObject (menu,0,directionY, Space.Self);
		}
	}

	public void RotateObject(Transform objectToRotate, float directionX, float directionY, Space space){
		objectToRotate.Rotate(mainCameraTransform.up * directionX,space); // rotate this object's transform by however the camera is located-up (or y axis) *speed*rotation speed, in the world's coordinates
		objectToRotate.Rotate(mainCameraTransform.right * directionY,space);// see above for right (x ) axis
	}

    public void ToggleSolvedList(Transform _piece,bool _add) {
        if (_add) {
            if (!solved.Contains(_piece)) {
                solved.Add(_piece);
            }
        } else {
            if (solved.Contains(_piece)) {
                solved.Remove(_piece);
            }
        }
        if (solved.Count == 6) {
            PuzzleSolved();
        }
        print("Toggling " + _piece.gameObject.name + _add.ToString() + solved.Count);
        //foreach (Transform wtf in solved) {
        //    print(wtf.gameObject.name);
        //}
    }

    void PuzzleSolved() {
        print("YAY!");
        puzzleSolved = true;
        puzzleCollider.localPosition = new Vector3(puzzleCollider.localPosition.x,30,puzzleCollider.localPosition.z);
        StartCoroutine(RotateSolvedPuzzle());
        StartCoroutine(MovePuzzleUpOnY());
    }

    IEnumerator RotateSolvedPuzzle() {
        while (puzzleSphere.localRotation != Quaternion.Euler(0,0,0)) {
            puzzleSphere.localRotation = Quaternion.RotateTowards(puzzleSphere.localRotation,Quaternion.Euler(0,0,0),Time.deltaTime*300f);
            yield return null;
        }
        StartCoroutine(SolvedSpin());
    }

    IEnumerator MovePuzzleUpOnY() {
        while (puzzleSphere.parent.position != puzzleSolvedPosition.position) {
            puzzleSphere.position = Vector3.MoveTowards(puzzleSphere.position,puzzleSolvedPosition.position,Time.deltaTime*10);
            yield return null;
        }
    }

    IEnumerator SolvedSpin() {
        while (puzzleSphere.localScale.y > 0.1f) {
            spinMultiplier += .2f;
            puzzleSphere.Rotate(0,50 * Time.deltaTime * spinMultiplier,0); //rotate on Y
            puzzleSphere.localScale -= new Vector3(.01f,.01f,.01f);
            yield return null;
        }
    }
}
