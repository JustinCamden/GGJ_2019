using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SplashShake : MonoBehaviour {

    RectTransform tran;

    public float strength;
    public int vibration;
    public float randomness;

    void Start()
    {
        tran = this.GetComponent<RectTransform>();

        tran.DOShakeAnchorPos(999, strength, vibration, randomness, false, false);
    }
}
