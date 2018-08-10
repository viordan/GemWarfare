using UnityEngine;
using System.Collections;

public class FrameCounter : MonoBehaviour {

    string label = "";
    float count;
    public GUIStyle myGuiStyle = new GUIStyle();

    IEnumerator Start() {
        GUI.depth = 2;
        while (true) {
            if (Time.timeScale == 1) {
                yield return new WaitForSeconds(0.1f);
                count = (1 / Time.deltaTime);
                label = "FPS :" + (Mathf.Round(count));
            } else {
                label = "Pause";
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void OnGUI() {
        myGuiStyle.fontSize = 40;
        myGuiStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(5, 300, 100, 25), label, myGuiStyle);
    }
}