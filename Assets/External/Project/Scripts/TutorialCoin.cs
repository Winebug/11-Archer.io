using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCoin : MonoBehaviour
{
    public Action<GameObject> coinTriggerd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (coinTriggerd != null)
            coinTriggerd.Invoke(gameObject);
    }

}
