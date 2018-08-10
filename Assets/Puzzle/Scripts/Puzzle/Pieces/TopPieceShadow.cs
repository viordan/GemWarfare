using System.Collections;
using UnityEngine;

public class TopPieceShadow :AbstractPiece, IShadow {

	Transform myTransform; // cached transform
    Coroutine spinPiece; // the move coroutine
    Transform matchToCenter;
    float spinSpeed=1f;

    void OnEnable(){
        myTransform = transform;//cache my transform
        matchToCenter = myTransform.parent;// get matchCenter
        spinPiece = StartCoroutine(SpinPiece()); // start spinning 
    }

    IEnumerator SpinPiece() {// spin should only be called back in the inventory, hence center is parent
        while (true) {// while this coroutine is active
            matchToCenter.Rotate(0,50 * Time.deltaTime * spinSpeed,0); //rotate on Y
            yield return null;// pause till next frame
        }
    }

    private void OnDisable() {// disable all coroutines
        StopAllCoroutines();
    }

    public void ChangeShadowColor() {
        myTransform.GetComponent<Renderer>().material.color = new Color32(0,200,0,100);
        myTransform.GetChild(0).GetChild(0).GetChild(0).GetComponent<BottomPieceShadow>().ChangeShadowColor();
    }
}
