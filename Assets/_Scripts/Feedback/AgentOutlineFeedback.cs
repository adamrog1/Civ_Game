using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentOutlineFeedback : MonoBehaviour
{
    [SerializeField]
    public Renderer outlineRender;
    private Material material;

    [SerializeField]
    private bool toogleOutline = true, changeColor=false;

    [SerializeField]
    private Color colorToChange;
    private Color originalColor;

    private void Start()
    {
        material = outlineRender.material;
        originalColor = material.GetColor("_Color");
    }

    private void ApplyChanges(bool val)
    {
        if (toogleOutline) material.SetInt("_Outline", val ? 1 : 0);
        if (changeColor) material.SetColor("_Color", val ? colorToChange : originalColor);
    }

    public void Select()
    {
        ApplyChanges(true);
    }

    public void Deselect()
    {
        ApplyChanges(false);
    }
}
