using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextData : MonoBehaviour {

    public static TextData text;
    private Dictionary<string, string> altText;

    void Awake()
    {
        if (text == null)
        { //if there is not a control already in this scene
            DontDestroyOnLoad(gameObject); // don't destroy this game object
            text = this; // the control is this object
        }
        else if (text != this)
        { // if the control is some other instance other than this 
            Destroy(gameObject); //kill that instance
        }
		DefineText ();
    }

    public void DisplayText(string _textToDisplay) {

    }

	private void DefineText(){
		// when calling this method, include keyword +name in method

		//string travelingTo="Traveling To Planet ";
		string arrivedAt = "Arrived At ";
		string keyWordArrived = "arrived";


		altText = new Dictionary<string, string>()
		{
			{"gamestart", "Game Starting!"},
			{keyWordArrived+"alpha", arrivedAt+"alpha"},
			{keyWordArrived+"beta", arrivedAt+"beta"},
			{keyWordArrived+"gamma", arrivedAt+"gamma"},
			{keyWordArrived+"delta", arrivedAt+"delta"},
			{keyWordArrived+"epsilon", arrivedAt+"epsilon"},
			{keyWordArrived+"level1", arrivedAt+"Level 1"},
			{keyWordArrived+"level2", arrivedAt+"Level 2"},
			{keyWordArrived+"level3", arrivedAt+"Level 3"},
			{keyWordArrived+"level4", arrivedAt+"Level 4"},
			{keyWordArrived+"level5", arrivedAt+"Level 5"},
			{keyWordArrived+"level6", arrivedAt+"Level 6"},
			{keyWordArrived+"level7", arrivedAt+"Level 7"},
			{keyWordArrived+"level8", arrivedAt+"Level 8"},
			{keyWordArrived+"level9", arrivedAt+"Level 9"},
			{keyWordArrived+"level10", arrivedAt+"Level 10"},
			{"alpha", "Alpha"},
			{"beta", "Beta"},
			{"gamma", "Gamma"},
			{"delta", "Delta"},
			{"epsilon", "Epsilon"},
			{"level1", "Level 1"},
			{"level2", "Level 2"},
			{"level3", "Level 3"},
			{"level4", "Level 4"},
			{"level5", "Level 5"},
			{"level6", "Level 6"},
			{"level7", "Level 7"},
			{"level8", "Level 8"},
			{"level9", "Level 9"},
			{"level10", "Level 10"},

		}; 
	
	}

	public string GetText(string key)
    {
        string temp = "";
        altText.TryGetValue(key, out temp);
        return temp;
    }
}
