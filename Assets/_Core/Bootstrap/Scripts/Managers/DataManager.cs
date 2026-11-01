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
        SaveData.StartNewGame();
    }
}
