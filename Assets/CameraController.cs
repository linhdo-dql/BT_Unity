using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private DollyZoomExample dollyZoom;
    private ProCamera2DCinematics cinematics;
    private ProCamera2DShake shake;
    public static CameraController instance;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {   
        dollyZoom = GetComponent<DollyZoomExample>();
        cinematics = GetComponent<ProCamera2DCinematics>();
        shake = GetComponent<ProCamera2DShake>();
        StartCoroutine(CameraAction());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator CameraAction()
    {
        cinematics.Play();
        yield return new WaitForSeconds(5);
        cinematics.CinematicTargets.ForEach(t => {
            var bt = t.TargetTransform.GetComponent<BehaviorTree>();
            if (bt != null)
            {
                bt.enabled = true;
            }
        });
    }

    public void Shake()
    {
        shake.Shake(0);
    }
}
