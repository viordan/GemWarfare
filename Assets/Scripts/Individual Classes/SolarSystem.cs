using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SolarSystem {


	//debugVars
	protected Scene thisScene;
	protected List<GameObject> rootObjects;
	public List<GameObject> planetList;
	public List<GameObject> orbitList;
	public List<GameObject> levelsList;
	public List<List<GameObject>> planetsListWithLevels;
	public Dictionary<string, GameObject> orbitDictionary;
	public Dictionary<string, GameObject> planetDictionary;
	public Dictionary<string, GameObject> levelsDictionary;


	public SolarSystem(){
		rootObjects= new List<GameObject>();
		planetList= new List<GameObject>();
		orbitList= new List<GameObject>();
		levelsList = new List<GameObject>();
		planetsListWithLevels = new List<List<GameObject>>();
		orbitDictionary= new Dictionary<string, GameObject> ();
		planetDictionary = new Dictionary<string, GameObject> ();
		levelsDictionary= new Dictionary<string, GameObject> ();

		thisScene = SceneManager.GetActiveScene(); // get the scene
		GetSolarSystem ();
	}

	public void GetSolarSystem() {
		thisScene.GetRootGameObjects(rootObjects); // get the root objects
		levelsDictionary.Add("sun", GameControl.control.player);//default value
		foreach (GameObject obj in rootObjects) { // for every root object
			if (obj.name.ToLower().Contains("orbit")) { // if it has orbitplanet inname
				orbitDictionary.Add(obj.name, obj);
				planetDictionary.Add (obj.transform.GetChild (1).gameObject.name, obj.transform.GetChild (1).gameObject);
				levelsList = new List<GameObject>(); // list of my levels
				orbitList.Add(obj); // add it to my orbits
				planetList.Add(obj.transform.GetChild(1).gameObject); // add the the second child to my planets
				foreach (Transform level in obj.transform.GetChild(1)) { // go through planet's children
					levelsList.Add(level.gameObject); // add the levels to levels list
					levelsDictionary.Add(obj.transform.GetChild(1).gameObject.name + level.gameObject.name, level.gameObject);
				}
				planetsListWithLevels.Add(levelsList); // add my list to the the list of lists
			}
		}
	}

}
