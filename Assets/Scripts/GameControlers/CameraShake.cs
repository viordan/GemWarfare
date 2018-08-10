using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    public static CameraShake instance;

    private Vector3 _originalPos;
    private float _timeAtCurrentFrame;
    private float _timeAtLastFrame;
    private float _fakeDelta;
    public static bool shaking;


    void Awake() {
        instance = this;
    }

    void Update() {
        // Calculate a fake delta time, so we can Shake while game is paused.
        _timeAtCurrentFrame = Time.realtimeSinceStartup;
        _fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
        _timeAtLastFrame = _timeAtCurrentFrame;
    }

    public static void Shake(float duration, float amount) {

        instance._originalPos = instance.gameObject.transform.localPosition;
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.cShake(duration, amount));
    }

    public IEnumerator cShake(float duration, float amount) {
        
        //float endTime = Time.time + duration;
        if (!shaking) {
            while (duration > 0) {
                //Handheld.Vibrate(); //vibrating the phone seemed like a good idea... it's kind of anoying, reserve for bosses maybe?.
                shaking = true;
                transform.localPosition = _originalPos + Random.insideUnitSphere * amount;
                duration -= _fakeDelta;
                yield return null;
            }
        }
        transform.localPosition = _originalPos;
        shaking = false;
    }
}