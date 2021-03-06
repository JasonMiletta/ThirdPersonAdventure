﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util_TransformManipulation :MonoBehaviour {

    #region EVENTS
    public delegate void LerpEvent();
    public static event LerpEvent OnLerpBegin;
    public static event LerpEvent OnLerpComplete;
    #endregion

	public static IEnumerator lerpObjToScale(GameObject obj, Vector3 targetScale, float duration)
    {
        float currentTime = 0.0f;
        while (currentTime <= duration)
        {
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, targetScale, currentTime/duration);
            currentTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.localScale = targetScale;
        if(targetScale == Vector3.zero)
        {
            obj.SetActive(false);
        }
        if(OnLerpComplete != null){
            OnLerpComplete();
        }
    }
    

    public static IEnumerator smoothMovement(GameObject obj, Vector3 source, Vector3 destination, float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            obj.transform.position = Vector3.Lerp(source, destination, (Time.time - startTime) / duration);
            yield return null;
        }
        obj.transform.position = destination;
        yield return null;
    }

    public IEnumerator smoothForceToPosition(Rigidbody rigidBody, Dictionary<Vector3, float> destinationToSpeedMap, float strength){
        foreach(Vector3 destination in destinationToSpeedMap.Keys){
            float duration = 5.0f;
            //destinationToSpeedMap.TryGetValue(destination, out duration);
            yield return StartCoroutine(smoothForceToPositionCoroutine(rigidBody, rigidBody.transform.position, destination, strength, duration));
        }
    }
    public IEnumerator smoothForceToPositionCoroutine(Rigidbody rigidBody, Vector3 source, Vector3 destination, float strength, float duration){
        float startTime = Time.time;
        while(Time.time < startTime + duration){
            rigidBody.AddForce((destination - source) * strength, ForceMode.Force);
            yield return null;
        }

        yield return null;
    }
}
