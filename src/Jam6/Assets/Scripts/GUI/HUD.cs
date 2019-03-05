using UnityEngine;
using UnityEngine.UI;

public class HUD : GUIWindow
{
    [SerializeField] private Transform lives;
    [SerializeField] private Text energyText;

    private void UpdateLives(int numLives)
    {
        for (int i = 0; i < lives.childCount; i++)
            lives.GetChild(i).gameObject.SetActive(i < numLives);
    }

    private void UpdateEnergyText(int energy)
    {
        energyText.text = (energy * 100 / Player.Singleton.MaxEnergy).ToString();
    }

    private void Awake()
    {
        UpdateLives(Player.Singleton.NumLives);
        UpdateEnergyText(Player.Singleton.Energy);

        Player.Singleton.OnNumLivesChange.AddListener(UpdateLives);
        Player.Singleton.OnEnergyChange.AddListener(UpdateEnergyText);
    }
}
