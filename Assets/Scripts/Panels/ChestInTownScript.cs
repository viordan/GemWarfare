using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestInTownScript : MonoBehaviour {

	private Transform myTransform;
	public Text moneyInTheBank;
	public Text moneyInHand;
	public DataLoader loader;
	public GameObject myCanvas;
	public CanvasGroup myCanvasGroup;
	public Button closeButton;

	// Use this for initialization
	void Start () {
		loader = DataLoader.loader;
		myTransform = transform;
		UpDateLabels ();
		myCanvas = myTransform.GetChild (0).gameObject;
		myCanvasGroup = myTransform.GetChild (0).GetComponent<CanvasGroup> ();
	}

	public void SomethingClicked(string buttonName){
		if (buttonName == "deposit") {
			Deposit ();
		} else if (buttonName == "withdraw") {
			WithDraw ();
		} else if (buttonName == "update") {
			UpDateLabels ();
		}
	
	}

	public void Deposit(){
		loader.moneyInTheBank += loader.score;
		loader.score = 0;
		UpDateLabels ();
		loader.SaveData (loader.saveFileName);
	}

	public void WithDraw(){
		loader.score +=loader.moneyInTheBank;
		loader.moneyInTheBank = 0;
		UpDateLabels ();
		loader.SaveData (loader.saveFileName);
	}

	public void UpDateLabels(){
		moneyInHand.text = loader.score.ToString ();
		GameControl.control.DisplayScore (loader.score);
		moneyInTheBank.text = loader.moneyInTheBank.ToString ();
	}

}
