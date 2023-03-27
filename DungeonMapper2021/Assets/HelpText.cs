using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpText : MonoBehaviour {

	public string Header;
	public string ButtonName;
	public string Description;


	public void ShowHelp()
    {
        if (EventManager.OnDisplayHelp != null)
        {
            EventManager.OnDisplayHelp.Invoke(this);
        }
    }

    public void EndShowHelp()
    {
        if (EventManager.OnEndDisplayHelp != null)
        {
            EventManager.OnEndDisplayHelp.Invoke(this);
        }
    }

    private void OnDisable()
    {
        EndShowHelp();
    }
}
