
using UnityEngine;

public class MainMenu : GUIWindow
{
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject creditPage;

    public void StartLevel()
    {
        GameManager.Singleton.StartLevel();
    }

    public void ShowCredits()
    {
        mainPage.SetActive(false);
        creditPage.SetActive(true);
    }

    public void BackToMain()
    {
        mainPage.SetActive(true);
        creditPage.SetActive(false);
    }

    private void Awake()
    {
        BackToMain();
    }
}
