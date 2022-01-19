using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIInfoPanel : MonoBehaviour
{
    // Pobieramy dane o nazwie prefabu i jego assecie
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private Image infoImage;

    private string selectedUnit = "";

    // Na podstawie nazwy prefabu mozemy okreslic jaka jest jego nazwa
    
    // --- UWAGA ----
    // Nie wiem jak ustalac nazwy prefabow dynamicznie wiec wszystkie sa nazwane tak samo z kolejna cyfra na koncu
    // Przy zmianie nazwy prefabu trzeba go tutaj podac jako kolejny else if
    public void SetData(Sprite sprite, string text)
    {
        if (text.Contains("Farmer"))
        {
            selectedUnit = "Osadnik";
        }
        else if (text.Contains("Farm"))
        {
            selectedUnit = "Farma";
        }
        else if (text.Contains("Town"))
        {
            selectedUnit = "Osada";
        }
        else if (text.Contains("Lumber"))
        {
            selectedUnit = "Tartak";
        }
        else if (text.Contains("House"))
        {
            selectedUnit = "Dom";
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
