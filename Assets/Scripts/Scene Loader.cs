using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{

    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject TutOn;
    [SerializeField] GameObject TutOff;
    [SerializeField] GameObject Controls;
    [SerializeField] GameObject Bg;
    [SerializeField] string StartScene;
    [SerializeField] string UIScene;
    //public GameManager gameManager;
    [SerializeField] string Town;

    public AudioSource SoundEffectPlayer;
    [SerializeField] AudioClip Button;

    void Awake()
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
        SoundEffectPlayer.clip = Button;
        SoundEffectPlayer.Play();


        SceneManager.LoadScene(Town);
        SceneManager.LoadScene(UIScene, LoadSceneMode.Additive);
        
        
    }

    public void GoHome()
    {
        Debug.Log("gohome");
        PlayButtonSE();
        SceneManager.LoadScene("Title Screen");
    }

    public void Exit()
    {
        PlayButtonSE();
        Application.Quit();
        Debug.Log("Quit");
    }

    public void ControlsOn()
    {
        TutOff.SetActive(true);
        TutOn.SetActive(false);
        Controls.SetActive(true);
        Bg.SetActive(true);
        EventSystem.current.SetSelectedGameObject(TutOff);

        PlayButtonSE();
    }

    public void ControlsOff()
    {
        TutOff.SetActive(false);
        TutOn.SetActive(true);
        Controls.SetActive(false);
        Bg.SetActive(false);
        EventSystem.current.SetSelectedGameObject(TutOn);

        PlayButtonSE();
    }

    public void PlayButtonSE()
    {
        SoundEffectPlayer.clip = Button;
        SoundEffectPlayer.Play();
    }
}
