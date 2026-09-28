using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{

    [SerializeField] GameObject StartButton;
    [SerializeField] string StartScene;
    [SerializeField] string UIScene;
    public GameManager gameManager;
    [SerializeField] string Town;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(StartButton);
        SceneManager.LoadScene(UIScene, LoadSceneMode.Additive);
        //gameManager = Object.FindFirstObjectByType<GameManager>();
    }

    private void Update()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(Town, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(StartScene);
        gameManager.NewDay();
    }

}
