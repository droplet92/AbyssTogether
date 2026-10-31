using System.Collections;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator Initialize()
    {
        InitializeNewGame();
        yield break;
    }

    public void InitializeNewGame()
    {
        InitializePlayer();
        InitializeHealths();
        InitializeItems();
        InitializePotions();
    }

    private void InitializePlayer()
    {
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.SetString("Deck", null);
    }
    private void InitializeHealths()
    {
        PlayerPrefs.SetInt("HpHealer", 30);
        PlayerPrefs.SetInt("HpMagician", 30);
        PlayerPrefs.SetInt("HpSwordsman", 30);
        PlayerPrefs.SetInt("HpWarrior", 30);
    }
    private void InitializeItems()
    {
        PlayerPrefs.SetInt("ItemMagicBook", 0);
        PlayerPrefs.SetInt("ItemRing", 0);
        PlayerPrefs.SetInt("ItemSword", 0);
        PlayerPrefs.SetInt("ItemNecklace", 0);
    }
    private void InitializePotions()
    {
        PlayerPrefs.SetString("Potion1", null);
        PlayerPrefs.SetString("Potion2", null);
        PlayerPrefs.SetString("Potion3", null);
    }
}
