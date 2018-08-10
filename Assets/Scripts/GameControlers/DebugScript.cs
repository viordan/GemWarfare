using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugScript : MonoBehaviour {
    private Scene thisScene;
    private List<GameObject> rootObjects;
    public List<GameObject> planetList;
    public List<GameObject> orbitList;
    public Dictionary<string, GameObject> levelsDictionary;


	// Use this for initialization
	void Start () {
        rootObjects= new List<GameObject>();
        levelsDictionary = new Dictionary<string, GameObject>();
        planetList = new List<GameObject>();
        orbitList = new List<GameObject>();
        thisScene = SceneManager.GetActiveScene();
        thisScene.GetRootGameObjects(rootObjects);
        foreach (GameObject obj in rootObjects) {
            if (obj.name.ToLower().Contains("orbitplanet")) {
                orbitList.Add(obj);
                planetList.Add(obj.transform.GetChild(1).gameObject);
                foreach (Transform level in obj.transform.GetChild(1)) {
                    levelsDictionary.Add(obj.transform.GetChild(1).name + level.gameObject.name, level.gameObject);
                }
            }
        }
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
