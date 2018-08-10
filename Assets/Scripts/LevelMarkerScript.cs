using UnityEngine;

public class LevelMarkerScript : MonoBehaviour {
	
    Transform myTransform;
	DataLoader loader;
    GameControl control;
    
    public Transform marker;
	string levelName;
	string levelKeyName;
	public bool amILocked;
    bool amIClickable;
    Transform myNumber;



	// Use this for initialization
	void Start () {
		loader = DataLoader.loader;
        control = GameControl.control;
        myTransform = gameObject.transform;
		
		levelName = myTransform.parent.gameObject.name;
		levelKeyName = (myTransform.parent.parent.gameObject.name + levelName).ToLower();
        myNumber = myTransform.GetChild(0);
        marker = myNumber.GetChild(0);
    }

	public void InRange() {
        if (!amILocked) {
            myNumber.GetComponent<Renderer>().material.color = Color.yellow;
            marker.gameObject.SetActive(true);
        }
        amIClickable = true;
    }

    public void OutOfRange() {
        if (!amILocked) {
            myNumber.GetComponent<Renderer>().material.color = Color.white;
            marker.gameObject.SetActive(false);
        }
        amIClickable = false;
    }

    public bool AmIClickable() {
        return amIClickable;
    }

    public void DisplayLevelName() {
        control.DisplayTheText(levelName);
    }


}
