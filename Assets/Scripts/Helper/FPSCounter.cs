using UnityEngine;
using TMPro;
using System;

public class FPSCounter : MonoBehaviour
{
    private float deltaTime;
    private float fps;
    public TextMeshProUGUI text;

    void Start()
    {
        deltaTime = Time.deltaTime;
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
        text.text = Mathf.Ceil(fps).ToString() + " FPS";
    }
}