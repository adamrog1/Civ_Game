using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashFeedback : MonoBehaviour
{
    // Potrzebujemy informacji o assecie jednostki i interwale w jakim ma byc wykonana animacja
    // Te dane sa przydzielane z poziomu panelu
    public SpriteRenderer spriteRendrer;
    [SerializeField]
    private float invisibleTime, visibleTime;

    // Dla wybrania jednotski zaczynamy animacje
    public void PlayFeedback()
    {
        if (spriteRendrer == null)
            return;
        StopFeedback();
        StartCoroutine(FlashCoroutine());
    }

    // Funckja przelacza miedzy widocznym a niewidocznym assetem jednostki
    // Robi to cyklicznie wiec wyglada jakby migalo
    // Dane o dlugosci migania pobierane sa z panelu unity
    private IEnumerator FlashCoroutine()
    {
        Color spriteColor = spriteRendrer.color;
        spriteColor.a = 0;
        spriteRendrer.color = spriteColor;
        yield return new WaitForSeconds(invisibleTime);

        spriteColor.a = 1;
        spriteRendrer.color = spriteColor;
        yield return new WaitForSeconds(visibleTime);

        StartCoroutine(FlashCoroutine());
    }

    // Wylaczamy animacje
    public void StopFeedback()
    {
        StopAllCoroutines();
        Color spriteColor = spriteRendrer.color;
        spriteColor.a = 1;
        spriteRendrer.color = spriteColor;
    }
}
