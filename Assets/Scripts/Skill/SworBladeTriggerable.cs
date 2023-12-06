using System.Collections;
using UnityEngine;

public class SworBladeTriggerable : MonoBehaviour
{
    [HideInInspector] public int swordDamage = 100;
    [HideInInspector] public float swordRange = 5f;
    [HideInInspector] public float hitForce = 100f;

    public Transform swordHitPoint;

    private Camera fpsCamera;
    private WaitForSeconds wfs = new WaitForSeconds(0.5f);

    public void Initialize()
    {

    }

    public void Blade()
    {
        Debug.Log("Sword blade!!!");
        StartCoroutine(BladeEffect());
    }

    IEnumerator BladeEffect()
    {
        yield return wfs;
    }
}