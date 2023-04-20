using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpTileScript : MonoBehaviour {

    public HelpText CurrentHelpText;

	public Text HelpHeaderText;

	public Text ButtonNameText;

	public Text DescriptionText;

	public void InitializeHelpTile(HelpText currentHelpText)
    {
        if (currentHelpText == null)
        {
            return;
        }

        CurrentHelpText = currentHelpText;

        if (HelpHeaderText != null)
        {
            HelpHeaderText.text = currentHelpText.Header;
        }

        if (ButtonNameText != null)
        {
            ButtonNameText.text = currentHelpText.ButtonName;
        }

        if (DescriptionText != null)
        {
            DescriptionText.text = currentHelpText.Description;
        }
    }



}
