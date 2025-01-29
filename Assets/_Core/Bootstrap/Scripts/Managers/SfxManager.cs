using UnityEngine;

public class SfxManager : AutoFieldValidator
{
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource focusSound;

    public static SfxManager Instance { get; private set; }

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

    public void PlayClickSound()
    {
        clickSound.Play();
    }
    public void PlayFocusSound()
    {
        focusSound.Play();
    }
}
