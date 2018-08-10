using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

//GameControl : GameState : GameLogic : GameData : MonoBehavior
public class GameControl : GameLogic {

    public static GameControl control; // this allows you to use the script from any scene where the object is present.

    #region Variables From Swipe
    Dictionary<Swipe,Vector2> cardinalDirections = new Dictionary<Swipe,Vector2>()
{
        { Swipe.Up,         CardinalDirection.Up                 },
        { Swipe.Down,         CardinalDirection.Down             },
        { Swipe.Right,         CardinalDirection.Right             },
        { Swipe.Left,         CardinalDirection.Left             },
        { Swipe.UpRight,     CardinalDirection.UpRight             },
        { Swipe.UpLeft,     CardinalDirection.UpLeft             },
        { Swipe.DownRight,     CardinalDirection.DownRight         },
        { Swipe.DownLeft,     CardinalDirection.DownLeft         }
    };
    Touch t;
    Transform toRotate;

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentPos;
    Vector2 swipeVelocity;
    Swipe swipeDirection;
    float dpcm;
    float swipeStartTime;
    float swipeEndTime;
    bool gotCast;
    bool hitSomething;


    float minSwipeLength = 0.5f;
    float moveCm;
    float swipeCm;
    bool useEightDirections = true;

    const float eightDirAngle = 0.906f;
    const float fourDirAngle = 0.5f;
    const float defaultDPI = 72f;
    const float dpcmFactor = 2.54f;

    bool noLongerClick;

    #endregion

    void Awake() {
        float dpi = (Screen.dpi == 0) ? defaultDPI : Screen.dpi;
        dpcm = dpi / dpcmFactor;
        if (control == null) { //if there is not a control already in this scene
			control = this; // the control is this object
		} 
		WakeVals ();
        DebugGame();
        //GetSolarSystem();
    }

    void Start() {
		StartVals ();
        StartOrbitSpin();
        DisplayScore(loader.score);
		DisplayTheText ("gamestart");
    }

    void Update() {
        #if UNITY_ANDROID
            TouchInput(); // touch control
        #endif
        #if UNITY_IOS
            TouchInput(); // touch control
        #endif
        #if UNITY_EDITOR
            MouseInput(); // mouse control
		    TouchInput(); // touch control
            KeyBoardInput(); // keyboard control
        #endif
    }

#region New Direction Detection
    void GetFirstCast() {
        if (Physics.Raycast(ray,out hit,2000,layerMaskSphereCollider)) {
            Debug.DrawLine(ray.origin,hit.point);// draws the ray line
            hitSomething = true;
            print("hit something " + hit);
        } else {
            print("hit nothing " + hit);

        }
        gotCast = true;
    }

    void ResetClickEnd() {
        hitSomething = false;
        gotCast = false;
        //swipeEnded = true;
        noLongerClick = false;
        firstPressPos = currentPos;// this is to reset the move otherwise it recalculates for next frame
    }

    void DetermineSwipeDirection(Ray ray) {
        Vector2 currentSwipe = secondPressPos - firstPressPos; // vector between first and second 
        swipeCm = currentSwipe.magnitude / dpcm;
        if (swipeCm < minSwipeLength) {
            if (noLongerClick) {
                print("this is a Nothing" + swipeCm);
            } else {
                if (Physics.Raycast(ray,out hit,2000,layerPlanetMask)) {// ignore planet collider
                    if (hit.collider.gameObject.layer == 21) {// extend the ray to 1000 units
                        hit.collider.gameObject.SetActive(false);
                    }
                } else {
                    playerScript.PlayerAttack();
                }
                print("this is a click " + swipeCm);
            }
        } else {
            print("this is a swipe at the end " + swipeCm);
            swipeEndTime = Time.time;
            swipeVelocity = currentSwipe * (swipeEndTime - swipeStartTime);
            swipeDirection = GetSwipeDirByTouch(currentSwipe);
            //print(swipeDirection);
            switch (swipeDirection.ToString().ToLower()) {
                case "left":
                playerScript.PlayerBlock();
                break;
                case "right":
                playerScript.PlayerAttack();
                break;
                case "down":
                playerScript.PlayerHardHit();
                break;
                case "up":
                ExitLevel();
                break;


                default:
                break;
            }
            moveCm = 0f;

        }
    }

    bool DetermineIfClick(Ray _ray) {
        Vector2 currentSwipe = secondPressPos - firstPressPos; // vector between first and second 
        swipeCm = currentSwipe.magnitude / dpcm;
        if (swipeCm < minSwipeLength) {
            if (noLongerClick) {
                print("this is a Not a click" + swipeCm);

                return false;
            } else {
                return true;
               
            }
        }
        return false;
    }
    void DetectSwipe() {
        Vector2 currentMovement = currentPos - firstPressPos; // vector between first and current 
        moveCm = currentMovement.magnitude / dpcm;
        if (moveCm > minSwipeLength) { // if you exceeded minSwipeLength
            if (!noLongerClick) {
                noLongerClick = true;
            }
        }
    }

    bool IsDirection(Vector2 direction,Vector2 cardinalDirection) {
        var angle = useEightDirections ? eightDirAngle : fourDirAngle;
        return Vector2.Dot(direction,cardinalDirection) > angle;
    }

    Swipe GetSwipeDirByTouch(Vector2 currentSwipe) {
        currentSwipe.Normalize();
        var swipeDir = cardinalDirections.FirstOrDefault(dir => IsDirection(currentSwipe,dir.Value));
        return swipeDir.Key;
    }

#endregion

#region Get New Inputs
    //void GetTouchInput() {
    //    if (Input.touches.Length > 0) {
    //        t = Input.GetTouch(0);
    //        currentPos = t.position;
    //        if (!gotCast) {
    //            ray = Camera.main.ScreenPointToRay(currentPos);// cast a ray from the camera through the touch
    //            GetFirstCast();
    //        }
    //        if (hitSomething) {
    //            RotateObject(hit.transform,-Input.GetTouch(0).deltaPosition.x * .3f,Input.GetTouch(0).deltaPosition.y * .3f,Space.World);
    //        } else {
    //            RotateObject(Camera.main.transform,-Input.GetTouch(0).deltaPosition.x * .3f,-Input.GetTouch(0).deltaPosition.y * .3f,Space.World);
    //        }

    //        // Swipe/Touch started
    //        if (t.phase == TouchPhase.Began) {
    //            firstPressPos = t.position;
    //            swipeStartTime = Time.time;
    //            //swipeEnded = false;
    //            // Swipe/Touch ended
    //        } else if (t.phase == TouchPhase.Ended) {
    //            secondPressPos = t.position;
    //            DetermineSwipeDirection();
    //            ResetClickEnd();
    //        }
    //    }
    //}

    //void GetMouseInput() {
    //    // Swipe/Click started
    //    if (Input.GetMouseButton(0)) {
    //        currentPos = (Vector2)Input.mousePosition;
    //        if (!gotCast) {
    //            ray = Camera.main.ScreenPointToRay(Input.mousePosition);// cast a ray from the camera through the touch
    //            GetFirstCast();
    //        }
    //        if (hitSomething) {
    //            RotateObject(hit.transform,-Input.GetAxis("Mouse X") * 10f,Input.GetAxis("Mouse Y") * 10f,Space.World);
    //        } else {
    //            RotateObject(Camera.main.transform,-Input.GetAxis("Mouse X") * 10f,-Input.GetAxis("Mouse Y") * 10f,Space.World);
    //        }

    //    }
    //    if (Input.GetMouseButtonDown(0)) {
    //        firstPressPos = (Vector2)Input.mousePosition;
    //        swipeStartTime = Time.time;
    //        //swipeEnded = false;
    //        // Swipe/Click ended
    //    } else if (Input.GetMouseButtonUp(0)) {
    //        secondPressPos = (Vector2)Input.mousePosition;
    //        DetermineSwipeDirection();
    //        ResetClickEnd();
    //    }
    //}
#endregion
    public void MouseInput() {
        switch (currentPlayerState) {
            case PlayerState.inBattle:
            if (Input.GetMouseButton(1)) {
                ExitLevel();
            }
            if (Input.GetMouseButton(0)) {
                DetectSwipe();
                currentPos = (Vector2)Input.mousePosition;
            }
            if (Input.GetMouseButtonDown(0)) {
                firstPressPos = (Vector2)Input.mousePosition;
                swipeStartTime = Time.time;
                //swipeEnded = false;
                // Swipe/Click ended
            } else if (Input.GetMouseButtonUp(0)) {
                secondPressPos = (Vector2)Input.mousePosition;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);// cast a ray from the camera through the touch
                DetermineSwipeDirection(ray);
                ResetClickEnd();
            }
            break;
            case PlayerState.onTheSun://have it fall into next state for now, will introduce different behavior later
            case PlayerState.inPlanetOrbit:
            if (Input.GetMouseButton(0)) {
                currentPos = (Vector2)Input.mousePosition;
                if (!gotCast) {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);// cast a ray from the camera through the touch
                    GetFirstCast();
                }
                if (hitSomething) {
                    if (hit.collider.gameObject.layer == planetLayer) {
                        RotateObject(hit.transform,-Input.GetAxis("Mouse X") * 10f,Input.GetAxis("Mouse Y") * 10f,Space.World);
                    }
                } else {
                    RotateObject(playerTransform,Input.GetAxis("Mouse X") * 10f,-Input.GetAxis("Mouse Y") * 10f,Space.World);
                }

            }
            if (Input.GetMouseButtonDown(0)) {
                firstPressPos = (Vector2)Input.mousePosition;
                swipeStartTime = Time.time;
                //swipeEnded = false;
                // Swipe/Click ended
            } else if (Input.GetMouseButtonUp(0)) {
                secondPressPos = (Vector2)Input.mousePosition;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);// cast a ray from the camera through the touch
                if (DetermineIfClick(ray)) {
                    if (Physics.Raycast(ray,out hit,2000,layerMaskSphereCollider)) {
                        if (hit.collider.gameObject.layer == levelLayer) {
                            if (hit.transform.GetComponent<LevelMarkerScript>().AmIClickable()) {// check if clicable
                                SelectLevel(hit.collider.transform.parent.gameObject); // get the parent of the colider's parent, which is the level.
                            }
                        } else if (hit.collider.gameObject.layer == planetLayer) {
                            SelectPlanet(hit.collider.gameObject);
                        }

                    }
                }
                ResetClickEnd();
            }



            break;
            case PlayerState.puzzleSolving:
            //if (_positionX < Screen.width / 2) {
            //    RotateObject(piecesController.puzzleSphere,_directionX,_directionY,Space.World);
            //} else {
            //    RotateObject(piecesController.menu,0,_directionY,Space.Self);
            //}
            break;
            case PlayerState.menuSelection:
            if (Input.GetMouseButton(0)) {
                currentPos = (Vector2)Input.mousePosition;
                if (!gotCast) {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);// cast a ray from the camera through the touch
                    GetFirstCast();
                }
                if (hitSomething) {
                    if (hit.collider.gameObject.layer == 22) {
                        //RotateObject(piecesController.menu,0,Input.GetAxis("Mouse Y") * 10f,Space.Self);
                        piecesController.menu.Rotate(new Vector3(0,0,1) * -Input.GetAxis("Mouse Y") * 10f,Space.Self);// see above for right (x ) axis
                    }
                }

            }
            if (Input.GetMouseButtonDown(0)) {
                firstPressPos = (Vector2)Input.mousePosition;
                swipeStartTime = Time.time;
                //swipeEnded = false;
                // Swipe/Click ended
            } else if (Input.GetMouseButtonUp(0)) {
                secondPressPos = (Vector2)Input.mousePosition;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);// cast a ray from the camera through the touch
                if (DetermineIfClick(ray)) {
                    if (Physics.Raycast(ray,out hit,2000,layerMaskSphereCollider)) {
                        if (hit.collider.gameObject.layer == levelLayer) {
                            if (hit.transform.GetComponent<LevelMarkerScript>().AmIClickable()) {// check if clicable
                                SelectLevel(hit.collider.transform.parent.gameObject); // get the parent of the colider's parent, which is the level.
                            }
                        } else if (hit.collider.gameObject.layer == planetLayer) {
                            SelectPlanet(hit.collider.gameObject);
                        }

                    }
                }
                ResetClickEnd();
            }


            break;
            default:
            break;
        }
    }
   

    public void TouchInput() {
        switch (currentPlayerState) {
            case PlayerState.inBattle:
            if (Input.touches.Length > 0) {
                DetectSwipe();
                t = Input.GetTouch(0);
                currentPos = t.position;
                // Swipe/Touch started
                if (t.phase == TouchPhase.Began) {
                    firstPressPos = t.position;
                    swipeStartTime = Time.time;
                    //swipeEnded = false;
                    // Swipe/Touch ended
                } else if (t.phase == TouchPhase.Ended) {
                    secondPressPos = t.position;
                    ray = Camera.main.ScreenPointToRay(currentPos);// cast a ray from the camera through the touch
                    DetermineSwipeDirection(ray);
                    ResetClickEnd();
                }
            }
            break;
            case PlayerState.onTheSun://have it fall into next state for now, will introduce different behavior later
            case PlayerState.inPlanetOrbit:
            if (Input.touches.Length > 0) {
                t = Input.GetTouch(0);
                currentPos = t.position;
                if (!gotCast) {
                    ray = Camera.main.ScreenPointToRay(currentPos);// cast a ray from the camera through the touch
                    GetFirstCast();
                }
                if (hitSomething) {
                    if (hit.collider.gameObject.layer == planetLayer) {
                        RotateObject(hit.transform,-Input.GetTouch(0).deltaPosition.x * .3f,Input.GetTouch(0).deltaPosition.y * .3f,Space.World);
                    }
                } else {
                    RotateObject(playerTransform,Input.GetTouch(0).deltaPosition.x * .3f,-Input.GetTouch(0).deltaPosition.y * .3f,Space.World);
                }

                if (t.phase == TouchPhase.Began) {
                    firstPressPos = t.position;
                    swipeStartTime = Time.time;
                } else if (t.phase == TouchPhase.Ended) {
                    secondPressPos = t.position;
                    ray = Camera.main.ScreenPointToRay(currentPos);// cast a ray from the camera through the touch
                    if (DetermineIfClick(ray)) {
                        if (Physics.Raycast(ray,out hit,2000,layerMaskSphereCollider)) {
                            if (hit.collider.gameObject.layer == levelLayer) {
                                if (hit.transform.GetComponent<LevelMarkerScript>().AmIClickable()) {// check if clicable
                                    SelectLevel(hit.collider.transform.parent.gameObject); // get the parent of the colider's parent, which is the level.
                                }
                            } else if (hit.collider.gameObject.layer == planetLayer) {
                                SelectPlanet(hit.collider.gameObject);
                            }
                        }
                    }
                    ResetClickEnd();
                }
            }


            break;
            case PlayerState.puzzleSolving:
            //if (_positionX < Screen.width / 2) {
            //    RotateObject(piecesController.puzzleSphere,_directionX,_directionY,Space.World);
            //} else {
            //    RotateObject(piecesController.menu,0,_directionY,Space.Self);
            //}
            break;
            case PlayerState.menuSelection:

            break;
            default:
            break;
        }
    }

    public void KeyBoardInput() {
        switch (currentPlayerState) {
            case PlayerState.inBattle:
            if (Input.GetKeyUp(KeyCode.S)) { // if you press S
                playerScript.PlayerAttack();
            }
            if (Input.GetKeyUp(KeyCode.A)) { // if you press A
                playerScript.PlayerBlock();
            }
            if (Input.GetKeyUp(KeyCode.D)) { // if you press D
                playerScript.PlayerHardHit();
            }
            if (Input.GetKeyUp(KeyCode.K)) { // if you press S
                foreach (GameObject target in targetsList.ToArray()) {
                    target.GetComponent<EnemyScript>().AddjustCurrentHealth(-1000f);
                }
            }
            if (Input.GetKeyUp(KeyCode.R)) { // fake a revive
                targetsList[0].GetComponent<EnemyScript>().ReviveEnemyEvent();
            }
            break;
            case PlayerState.onTheSun://have it fall into next state for now, will introduce different behavior later
            case PlayerState.inPlanetOrbit:
            if (Input.GetKeyUp(KeyCode.Y)) {
                ToggleMenu();
            }


            break;
            case PlayerState.puzzleSolving:

            break;
            case PlayerState.menuSelection:
            if (Input.GetKeyUp(KeyCode.U)) {
                ToggleMenu();
            }
            break;
            default:
            break;
        }
    }


    //   public void MouseInput() {
    //       if (Input.GetMouseButton(0)) {// if there is any touch
    //           ray = Camera.main.ScreenPointToRay(Input.mousePosition);// cast a ray from the camera through the touch
    //           touchMouseTimer += Time.deltaTime; // start counting this timer
    //           Vector3 mouseDirection = new Vector3(-Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);
    //		IfDragged(mouseDirection.x * 10f, mouseDirection.y * 10f,Input.mousePosition.x,Input.mousePosition.y);
    //       } else if (touchMouseTimer < .2f && Input.GetMouseButtonUp(0)) {
    //           IfClicked(Input.mousePosition.x,Input.mousePosition.y);
    //       } else {
    //           dragEmpty = false;
    //           dragPlanet = false;
    //           touchMouseTimer = 0f;
    //       }
    //       if (Input.GetMouseButton(1)) {
    //           print("right clicked!");
    //           ExitLevel();
    //       }
    //   }

    //public void IfDragged(float _directionX, float _directionY, float _positionX, float _positionY){
    //       switch (currentPlayerState) {
    //           case PlayerState.inBattle:

    //           break;
    //           case PlayerState.onTheSun://have it fall into next state for now, will introduce different behavior later
    //           case PlayerState.inPlanetOrbit:
    //               if (!dragPlanet && !Physics.Raycast(ray,out hit,1000,layerMaskSphereCollider) || dragEmpty) { // extend the ray to 1000 units
    //                   dragEmpty = true;
    //                   RotateObject(playerTransform,-_directionX,-_directionY, Space.World);
    //               } else if (hit.collider.gameObject == planet && (dragPlanet || (Physics.Raycast(ray,out hit,1000,layerMaskSphereCollider) && !dragEmpty))) { // extend the ray to 1000 units
    //                   dragPlanet = true;
    //                   if (selectedPlanet != null && currentPlayerState == PlayerState.inPlanetOrbit) { // rotate the planet
    //                       RotateObject(selectedPlanet.selectedPlanet.transform,_directionX,_directionY,Space.World);
    //                   }
    //               }
    //           break;
    //           case PlayerState.puzzleSolving:
    //           if (_positionX < Screen.width / 2) {
    //               RotateObject(piecesController.puzzleSphere,_directionX,_directionY,Space.World);
    //           } else {
    //               RotateObject(piecesController.menu,0,_directionY,Space.Self);
    //           }
    //           break;
    //           case PlayerState.menuSelection:

    //           break;
    //           default:
    //           break;
    //	}
    //}

    //public void IfClicked(float _clickX, float _clickY) {
    //    switch (currentPlayerState) {
    //        case PlayerState.inBattle:
    //        print("I'm in battle");
    //            if (Physics.Raycast(ray,out hit,2000,layerPlanetMask)) {// ignore planet collider
    //                if (hit.collider.gameObject.layer == 21) {// extend the ray to 1000 units
    //                    hit.collider.gameObject.SetActive(false);
    //                }
    //            } else {
    //                if (_clickX < Screen.width / 2) {
    //                    playerScript.PlayerBlock();
    //                } else {
    //                    playerScript.PlayerAttack();
    //                }
    //            }
    //            if (Input.touchCount == 2) {
    //                playerScript.PlayerHardHit();
    //            }


    //            if (Input.touchCount == 4) {
    //                ExitLevel();
    //            }


    //        break;
    //        case PlayerState.onTheSun://have it fall into next state for now, will introduce different behavior later
    //        case PlayerState.inPlanetOrbit:
    //            if (Physics.Raycast(ray,out hit,2000,layerMaskSphereCollider)) { // extend the ray to 1000 units
    //                Debug.DrawLine(ray.origin,hit.point);// draws the ray line
    //                if (hit.collider.gameObject.layer == menuButtonLayer) {
    //                    chestInTownScript.SomethingClicked(hit.collider.gameObject.name.ToLower());
    //                }
    //                if (hit.collider.gameObject.layer == backToMenu) {
    //                    SceneManager.LoadScene("GameMenu");
    //                }
    //                if (hit.collider.gameObject.layer == levelLayer) {// if whatever colider's you hit, game object has the layer 10 and zoomOut is false ***NOTE*** why zoom out false?
    //                    if (hit.transform.GetComponent<LevelMarkerScript>().AmIClickable()) {// check if clicable
    //                        SelectLevel(hit.collider.transform.parent.gameObject); // get the parent of the colider's parent, which is the level.
    //                    }
    //                } else if (hit.collider.gameObject.layer == planetLayer) { // if you hit planet one
    //                    SelectPlanet(hit.collider.gameObject);
    //                }
    //            }
    //            break;
    //        case PlayerState.puzzleSolving:
    //            if (Physics.Raycast(ray,out hit,2000,layerMaskSphereCollider)) { // extend the ray to 1000 units
    //                Debug.DrawLine(ray.origin,hit.point);// draws the ray line
    //                if (hit.collider.gameObject.layer == 21) {// if it's on the puzzle piece layer
    //                    TopPiece clickedOnPiece = hit.transform.GetComponent<TopPiece>();
    //                    if (clickedOnPiece.toPuzzle) {//if it's already going to the puzzle
    //                        clickedOnPiece.MoveBackToHolder();// move it back to the holder
    //                    } else {//if not going towards the puzzle
    //                    if (piecesController.puzzleCollider.GetComponent<ColliderScript>().SelectedPiece != null) {//if there is something selected
    //                        if (!piecesController.puzzleCollider.GetComponent<ColliderScript>().SelectedPiece.GetComponent<PuzzleSlot>().havePiece) {// if the slot is empty
    //                            clickedOnPiece.MoveToPuzzle(piecesController.puzzleCollider.GetComponent<ColliderScript>().SelectedPiece); // move the piece to the puzzle
    //                        }
    //                    }
    //                }
    //                }
    //            }
    //        break;
    //        case PlayerState.menuSelection:

    //        break;
    //        default:
    //        break;
    //    }

    //}

}

class CardinalDirection {
    public static readonly Vector2 Up = new Vector2(0,1);
    public static readonly Vector2 Down = new Vector2(0,-1);
    public static readonly Vector2 Right = new Vector2(1,0);
    public static readonly Vector2 Left = new Vector2(-1,0);
    public static readonly Vector2 UpRight = new Vector2(1,1);
    public static readonly Vector2 UpLeft = new Vector2(-1,1);
    public static readonly Vector2 DownRight = new Vector2(1,-1);
    public static readonly Vector2 DownLeft = new Vector2(-1,-1);
}
// https://forum.unity.com/threads/swipe-in-all-directions-touch-and-mouse.165416/page-2#post-2741253
public enum Swipe {
    None,
    Up,
    Down,
    Left,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight
};

