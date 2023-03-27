using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager currentGameManager;

    public Session session;

    private void Awake()
    {
        if (currentGameManager != null)
        {
            if (currentGameManager != this)
            {
                Destroy(this);
            }

        }

        currentGameManager = this;

        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        if (EventManager.OnSessionListUpdate != null)
        {
            EventManager.OnSessionListUpdate.Invoke(FileSystem.GetAllSessionNames().ToArray());
        }
    }

    private void OnEnable()
    {
        EventManager.OnSessionSelected += LoadSelectedSessionScene;
    }

    private void OnDisable()
    {
        EventManager.OnSessionSelected -= LoadSelectedSessionScene;
    }

    void LoadSelectedSessionScene(Session selectedSession)
    {
        session = selectedSession;

        SceneManager.sceneLoaded += SceneLoaded;

        SceneManager.LoadScene(1);

    }

    int p = 0;
    void SceneLoaded(Scene s, LoadSceneMode mode)
    {
        if (session != null)
        {

            p++;

            //Debug.Log("p is " + p);

            Manager manager = FindObjectOfType<Manager>();
            if (manager != null)
            {
                manager.StartSession(session);
            }
            else
            {
                Debug.Log("No Manager was found could not start session");
            }
        }

        p = 0;

        SceneManager.sceneLoaded -= SceneLoaded;

    }

    public const string MenuSceneName = "MenuScene";
    public static void ReturnToMainMenu()
    {

        if (currentGameManager != null)
        {
            currentGameManager.session = null;
            Destroy(currentGameManager.gameObject);
        }

        SceneManager.LoadScene(MenuSceneName);

    }
}
