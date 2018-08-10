using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    [SerializeField] GameObject dropdownPrefab;
    public Transform myparent;
    //private GameControl control;
    private Dropdown myDropdown;
    //private HierarchySetterScript myHierarchy;
    public Text myHierarchyButton;

    public GameObject dropDownMenuCanvas;
    public CanvasGroup DropDownMenuCanvasGroup;
    public GameObject chestInTownMenu;
    public CanvasGroup chestInTownMenuGroup;
    public GameObject endOfLevelMenu;
    public CanvasGroup endOfLevelMenuGroup;
    public List<GameObject> listOfCanvases;

	private SolarSystem solarSystem;


    void Start() {
		solarSystem = new SolarSystem ();
        //control = GameControl.control;
        //myHierarchy = control.GetComponent<HierarchySetterScript>();
		for (int i = 0; i < solarSystem.planetsListWithLevels.Count; i++) {
            GameObject dropdown = (GameObject)Instantiate(dropdownPrefab);
            dropdown.transform.SetParent(myparent);
            //dropdown.transform.parent = myparent;
            dropdown.transform.localPosition = new Vector3(60, 130 - 40 * i, 0);
            myDropdown = dropdown.GetComponent<Dropdown>();
            
            List<string> optionsList = new List<string>();
			foreach (GameObject level in solarSystem.planetsListWithLevels[i]) {
                optionsList.Add(level.name);
            }
			optionsList.Insert(0, "Planet " + solarSystem.planetList[i].name.ToString());
            myDropdown.AddOptions(optionsList);
            myDropdown.name = i.ToString();
        }

        listOfCanvases = new List<GameObject>() {
            chestInTownMenu,
            endOfLevelMenu,
            dropDownMenuCanvas
        };
    }

    public void CloseTab() {
        CloseAllCanvases();
    }

    public void OpenChestInTown() {
        CloseAllCanvases();
        OpenCanvas(chestInTownMenu);
    }

    public void OpenEndOfLevel() {
        CloseAllCanvases();
        OpenCanvas(endOfLevelMenu);
    }

    public void OpenDropDownMenu() {
        CloseAllCanvases();
        OpenCanvas(dropDownMenuCanvas);
    }


    public void OpenCanvas(GameObject myCanvas) {
        if (!myCanvas.activeSelf) myCanvas.SetActive(true);
    }
    // manage submenus;
    //public IEnumerator CloseCanvas(CanvasGroup myCanvasGroup, GameObject myCanvas) {
    //    while (myCanvasGroup.alpha != 0) {
    //        myCanvasGroup.alpha -= .1f;
    //        yield return new WaitForSeconds(.01f);
    //    }
    //    myCanvas.SetActive(false);
    //}

    //public IEnumerator OpenCanvas(CanvasGroup myCanvasGroup, GameObject myCanvas) {
    //    if (!myCanvas.activeSelf) myCanvas.SetActive(true);
    //    while (myCanvasGroup.alpha != 1) {
    //        myCanvasGroup.alpha += .1f;
    //        yield return new WaitForSeconds(.01f);
    //    }
    //}

    public void CloseAllCanvases() {
        foreach (GameObject canvas in listOfCanvases) {
            if (canvas.activeSelf) { canvas.SetActive(false); }
        }
    }
}
