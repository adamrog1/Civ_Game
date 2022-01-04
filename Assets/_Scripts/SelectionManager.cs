using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour, ITurnDependant
{
    FlashFeedback flashFeedback;
    SelectionIndicatorFeedback selectionFeedback;
    AgentOutlineFeedback outlineFeedback;

    public void HandleSelection(GameObject detectedColldier)
    {
        DeselectOldObject();

        if (detectedColldier == null)
            return;

        selectionFeedback = detectedColldier.GetComponent<SelectionIndicatorFeedback>();
        if (selectionFeedback != null)
            selectionFeedback.Select();

        Unit unit = detectedColldier.GetComponent<Unit>();
        if (unit != null)
        {
            if (unit.CanStillMove() == false)
                return;
        }

        flashFeedback = detectedColldier.GetComponent<FlashFeedback>();
        if (flashFeedback != null)
            flashFeedback.PlayFeedback();

        outlineFeedback = detectedColldier.GetComponent<AgentOutlineFeedback>();
        if (outlineFeedback != null)
            outlineFeedback.Select();
    }

    public void DeselectOldObject()
    {
        if (flashFeedback != null)
        {
            flashFeedback.StopFeedback();
            flashFeedback = null;
        }

        if (outlineFeedback != null)
        {
            outlineFeedback.Deselect();
            outlineFeedback = null;
        }

        if (selectionFeedback != null)
        {
            selectionFeedback.Deselect();
            selectionFeedback = null;
        }
    }

    public void WaitTurn()
    {
        DeselectOldObject();
    }
}
