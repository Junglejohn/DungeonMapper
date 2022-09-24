using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionButtonScript : MonoBehaviour {

    public Text NameText;

    private string _assignedSessionName = "";
    public string assignedSessionName { get {
            return _assignedSessionName;
        } set {
            _assignedSessionName = value;

            if (NameText != null)
            {
                NameText.text = value;
            }
        }
    }

    //public Session assignedSession;

    public void AssignButtonValues(string sessionName)
    {
        assignedSessionName = sessionName;
        //NameText.text = sessionName;
        //assignedSession = session;
    }

    public void SelectSession()
    {
        if (EventManager.OnSessionSelected != null)
        {
            Session session = new Session(assignedSessionName);

            if (session.LoadSession())
            {
                EventManager.OnSessionSelected.Invoke(session);
            } else
            {
                if (NameText != null)
                {
                    NameText.text = "Not Found";
                }
            }

        }
    }

    public void DeleteSession()
    {
        FileSystem.GetSessionByName(assignedSessionName).DeleteSession();
        //assignedSession.DeleteSession();
        Destroy(this.gameObject);
    }

}
