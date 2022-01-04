using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UIBuildSelectionHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private BuildDataSO buildData;

    public BuildDataSO BuildData { get => buildData; }

    private UIBuildButtonHandler buttonHandler;

    private UIHighlight uiHighlight;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private bool interactable = false;

    private void Awake()
    {
        buttonHandler = GetComponentInParent<UIBuildButtonHandler>();
        uiHighlight = GetComponent<UIHighlight>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(interactable == false)
        {
            buttonHandler.ResetBuildButton();
            return;
        }
        buttonHandler.PrepareBuildButton(this.buildData);
        uiHighlight.ToggleHighlight(true);
    }

    public void ToggleActive(bool val)
    {
        this.interactable = val;
        canvasGroup.alpha = val ? 1 : 0.5f;
        canvasGroup.interactable = val;
    }

    public void Reset()
    {
        uiHighlight.ToggleHighlight(false);
    }
}
