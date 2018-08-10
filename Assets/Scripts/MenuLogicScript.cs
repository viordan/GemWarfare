using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogicScript : MonoBehaviour {

    AsyncOperation async;
    public GameObject option1;
    public GameObject option2;
    public GameObject option3;
    public GameObject theOption;
    public GameObject theCollapse;
    public GameObject theBolt;
    public float timer=0f;


    bool selectionMade;
    bool finishedTraveling;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (selectionMade) {
            theCollapse.SetActive(true);
            timer += Time.deltaTime;
            if (timer < 2) {
                theBolt.SetActive(true);
            } else if (timer >= 2) {
                MoveObject(theCollapse, theOption, 1f);
            }
            if (finishedTraveling) {
                async.allowSceneActivation = true;
            }
        }
	}

    public void Start1() {
		ColapseWave(option1,"/datafile1");
    }

    public void Start2() {
		ColapseWave(option2,"/datafile2");
    }

    public void Start3() {
		ColapseWave(option3,"/datafile3");
    }

	IEnumerator Load1(string _saveFileName) {
		DataLoader.loader.LoadData (_saveFileName);
        async = SceneManager.LoadSceneAsync("Globe");
        Debug.Log("Loading complete");
        async.allowSceneActivation = false;
        Debug.Log("Paused, do stuff to load");
        yield return async;
    }

    public void MoveObject(GameObject objectToMove, GameObject objectWhereToMove, float moveSpeedModifier) {
        if (objectToMove.transform.position != objectWhereToMove.transform.position) {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, objectWhereToMove.transform.position, Time.deltaTime * moveSpeedModifier);
        } else {
            finishedTraveling = true;
        }
    }

	public void ColapseWave(GameObject option, string _saveFileName) {
        selectionMade = true;
        theOption = option.transform.GetChild(0).gameObject;
        theCollapse = option.transform.GetChild(1).gameObject;
        theBolt = option.transform.GetChild(2).gameObject;
		StartCoroutine("Load1",_saveFileName);
    }

}
