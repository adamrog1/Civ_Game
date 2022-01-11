using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIInfoPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private Image infoImage;

    private string selectedUnit = "";

    public void SetData(Sprite sprite, string text)
    {
        if (text.Contains("Farmer"))
        {
            selectedUnit = "Osadnik";
        }
        else {
            selectedUnit = "Wojownik";
        }
        this.nameText.text = selectedUnit;
        this.infoImage.sprite = sprite;
    }

    public void ToggleVisibility(bool val)
    {
        gameObject.SetActive(val);
    }
}
