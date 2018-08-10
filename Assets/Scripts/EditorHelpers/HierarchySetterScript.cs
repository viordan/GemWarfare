using UnityEngine;
[ExecuteInEditMode]
public class HierarchySetterScript : MonoBehaviour {

    public bool debug;
    public bool checkHierarchyConsistency;
    private GameControl control;
	private SolarSystem solarSystem;

    void Start() {
        //control = GameControl.control;
		solarSystem = new SolarSystem ();

    }
    void Update() {
        DebugSetHierarchyAll();
        if (checkHierarchyConsistency) {
            CheckHierarchyConsistency();
        }
    }

    public void CheckHierarchyConsistency() {
		foreach (GameObject orbit in solarSystem.orbitList) {
            foreach (Transform child in orbit.transform) {
                if (child.gameObject.name.ToLower().Contains("playerlocation")) {
                    child.SetAsFirstSibling();
                    child.gameObject.SetActive(false);
                }
            }
        }

		foreach (GameObject planet in solarSystem.planetList) { //for each planet 
            foreach (Transform level in planet.transform) { // for each level
                foreach (Transform child in level) {
                    if (child.gameObject.name.ToLower().Contains("enemies")) {
                        child.SetAsFirstSibling();
                    } else if (child.gameObject.name.ToLower().Contains("marker")) {
                        child.SetSiblingIndex(1);
                    } else if (child.gameObject.name.ToLower().Contains("breadbrumbs")) {
                        child.SetSiblingIndex(2);
                    } else {
                        child.SetAsLastSibling();
                    }
                }
            }
        }
    }


    public void DebugSetHierarchyAll() {

		foreach (GameObject planet in solarSystem.planetList) { //for each planet 
            foreach (Transform level in planet.transform) { // for each level
                if (debug) { //if in debug mode
                    level.gameObject.SetActive(true); //enable all levels
                    if (level.GetChild(0) != null) { //if it has a first child 
                        level.GetChild(0).gameObject.SetActive(true); //enable enemies
                        foreach (Transform encounter in level.GetChild(0))
                        {
                            encounter.gameObject.SetActive(true);

                            foreach (Transform enemyHolder in encounter)
                            { // for each enemyholder in enemies
                                enemyHolder.gameObject.SetActive(true);// enable 
                                foreach (Transform enemy in enemyHolder)
                                { // for each enemy in enemyholder
                                    enemy.gameObject.SetActive(true);
                                }
                            }
                        }
                    }
                    if (level.GetChild(1) != null) level.GetChild(1).gameObject.SetActive(true); // if it has a marker turn it on
                    if (level.GetChild(2) != null) { // if there are breadcrumbs
                        level.GetChild(2).gameObject.SetActive(true); // turn it on 
                        foreach (Transform breadCrumb in level.GetChild(2)) { // for each breadCrumb in breadCrumbs
                            //breadCrumb.gameObject.SetActive(true); // enable breadcrumb
                        }
                    }
                } else {
                    level.gameObject.SetActive(false); //disable all levels
                    if (level.GetChild(0) != null) { //if it has a first child 
                        level.GetChild(0).gameObject.SetActive(false); //disable enemies
                        foreach (Transform encounter in level.GetChild(0))
                        {
                            foreach (Transform enemyHolder in encounter)
                            { // for each enemyholder in enemies
                                enemyHolder.gameObject.SetActive(true);// enable enemy holder
                                enemyHolder.GetChild(0).gameObject.SetActive(false);
                                enemyHolder.GetChild(1).gameObject.SetActive(true);
                                enemyHolder.GetChild(1).GetChild(2).gameObject.SetActive(false);
                            }
                        }
                    }
                    if (level.GetChild(1) != null) level.GetChild(1).gameObject.SetActive(true); // if it has a marker turn it on
                    if (level.GetChild(2) != null) { // if there are breadcrumbs
                        level.GetChild(2).gameObject.SetActive(false); // turn it on 
                        foreach (Transform breadCrumb in level.GetChild(2)) { // for each breadCrumb in breadCrumbs
                            //breadCrumb.gameObject.SetActive(false); // enable breadcrumb
                        }
                    }
                }
            }
        }
    }
}
