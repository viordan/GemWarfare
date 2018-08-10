using System.Collections;
using UnityEngine;

public class BottomPieceShadow :AbstractPiece, IShadow{

    public void ChangeShadowColor() {
        print("changing my color");
        transform.GetComponent<Renderer>().material.color = new Color32(0,200,0,100);
    }
}
