using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpGradeTrigger : InteractableBase

{
    public GameManager gm;
    // Start is called before the first frame update
    public override void BeginInteract()
    {
        ShowText("Hold E to Interact");
    }

    public override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            gm.ActivateUpGrade();
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            gm.ActivateUpGrade();
        }

    }

    public override void EndInteract()
    {
        HideText();
    }
}
