using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{

    [SerializeField] GameObject StartButton;
    [SerializeField] string StartScene;
    [SerializeField] string UIScene;
    //public GameManager gameManager;
    [SerializeField] string Town;

    void Start()
    {
       EventSystem.current.SetSelectedGameObject(StartButton);
        //
        //gameManager = Object.FindFirstObjectByType<GameManager>();
    }

    private void Update()
    {
        
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(Town);
        SceneManager.LoadScene(UIScene, LoadSceneMode.Additive);
        
        
    }

    public void GoHome()
    {
        Debug.Log("gohome");
        SceneManager.LoadScene("Title Screen");
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

}
