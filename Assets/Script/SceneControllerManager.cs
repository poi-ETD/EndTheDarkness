using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneControllerManager : MonoBehaviour
{
    private static SceneControllerManager instance;

    public static SceneControllerManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public IEnumerator SwitchScene(string sceneName)
    {
        Debug.Log("a");
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("b");
        yield return StartCoroutine(LoadScene(sceneName));
        Debug.Log("c");
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneByName(sceneName);

        SceneManager.SetActiveScene(newScene);
    }
}