using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpTextArray : MonoBehaviour
{
    public HelpText[] helpTextArray;

    private bool isHelpTextAssigned(int index)
    {
        if (index >= 0 && index < helpTextArray.Length && helpTextArray[index] != null)
        {
            //Debug.Log("HelpText is assigned for index " + index);
            return true;
        } else
        {
            //Debug.Log("Not assigned HelpText for index " + index);
            return false;
        }
    }

    public void ShowHelp(int index)
    {
        if (EventManager.OnDisplayHelp != null && isHelpTextAssigned(index))
        {
            EventManager.OnDisplayHelp.Invoke(helpTextArray[index]);
        }
    }

    public void EndShowHelp(int index)
    {
        if (EventManager.OnEndDisplayHelp != null && isHelpTextAssigned(index))
        {
            EventManager.OnEndDisplayHelp.Invoke(helpTextArray[index]);
        }
    }

    public void EndShowAllHelp()
    {
        for (int i = 0; i < helpTextArray.Length; i++)
        {
            EndShowHelp(i);
        }


    }


    private void OnDisable()
    {
        EndShowAllHelp();
    }
}
