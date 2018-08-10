using System.Collections;
using UnityEngine;

public class ColliderScript : MonoBehaviour {

	Transform myTransform;
	public Transform SelectedPiece{ get; private set;}

    void OnTriggerEnter(Collider col){
		if (col.gameObject.layer == 20) {
            SetSelectedPiece(col.transform,true);
        }
    }

    void OnTriggerExit(Collider col){
		if (col.gameObject.layer == 20) {
            SetSelectedPiece(col.transform,false);
        }
	}

    void SetSelectedPiece(Transform _selectedPiece,bool up) {
        SelectedPiece = _selectedPiece;
        Transform bottomPiece = SelectedPiece.GetChild(0).GetChild(0).GetChild(0);
        if (up) {
            bottomPiece.GetComponent<Renderer>().material.color = Color.red;
            _selectedPiece.GetChild(0).localPosition = new Vector3(0,-4,0);
            foreach (Transform child in _selectedPiece) {
                child.localScale *= 2f;
            }
            if (_selectedPiece.childCount>1) {
                _selectedPiece.GetChild(1).GetChild(0).GetComponent<TopPiece>().scaleDoubled = true;
            }
        } else {
            bottomPiece.GetComponent<Renderer>().material.color = Color.white;
            _selectedPiece.GetChild(0).localPosition = new Vector3(0,-2,0);
            foreach (Transform child in _selectedPiece) {
                child.localScale /= 2f;
            }
            if (_selectedPiece.childCount>1) {
                _selectedPiece.GetChild(1).GetChild(0).GetComponent<TopPiece>().scaleDoubled = false;
            }
            SelectedPiece = null;
        }
    }

    public Transform GetSelectedPiece(){
		return SelectedPiece;
	}
}
