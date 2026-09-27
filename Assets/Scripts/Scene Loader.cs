using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{

    [SerializeField] GameObject StartButton;
    [SerializeField] string StartScene;
    [SerializeField] string UIScene;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(StartButton);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(StartScene);
        SceneManager.LoadScene(UIScene, LoadSceneMode.Additive);
    }

}
