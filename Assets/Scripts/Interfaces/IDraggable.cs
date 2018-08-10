using System.Collections;
using UnityEngine;

public interface IDraggable{

	void Drag (float directionX, float directionY, Ray _ray);
}
