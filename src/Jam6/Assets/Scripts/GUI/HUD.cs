using UnityEngine;
using UnityEngine.UI;

public class HUD : GUIWindow
{
    [SerializeField] private Transform lives;
    [SerializeField] private Text energyText;

    private void HandlePlayerNumLivesChange(int numLives)
    {
        if (numLives >= 0)
            lives.GetChild(numLives).gameObject.SetActive(false);
    }

    private void HandlePlayerEnergyChange(int energy)
    {
        energyText.text = (energy * 100 / Player.Singleton.MaxEnergy).ToString();
    }

    private void Awake()
    {
        for (int i = 0; i < lives.childCount; i++)
            lives.GetChild(i).gameObject.SetActive(i < Player.Singleton.NumLives);

        energyText.text = (Player.Singleton.Energy * 100 / Player.Singleton.MaxEnergy).ToString();

        Player.Singleton.OnNumLivesChange.AddListener(HandlePlayerNumLivesChange);
        Player.Singleton.OnEnergyChange.AddListener(HandlePlayerEnergyChange);
    }
}
