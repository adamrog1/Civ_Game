using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoProvider : MonoBehaviour
{
    // Info provider to panel na ktorym widnieje obrazek wybranego elementu gry i jego nazwa
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    public Sprite Image => spriteRenderer.sprite;
    public string NameToDisplay => gameObject.name;
}
