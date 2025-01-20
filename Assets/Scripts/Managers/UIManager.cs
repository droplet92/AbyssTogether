using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasRenderer settingsPanel;
    [SerializeField] private VideoController videoController;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip enterSound;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixerGroup sfxMixer;

    private AudioSource clickAudioSource;
    private AudioSource enterAudioSource;

    void Start()
    {
        if (canvas != null)
        {
            canvas.gameObject.SetActive(false);
            settingsPanel.gameObject.SetActive(false);

            videoController.OnAllVideoEnd += ActivateStartCanvas;
        }

        clickAudioSource = gameObject.AddComponent<AudioSource>();
        clickAudioSource.playOnAwake = false;
        clickAudioSource.clip = clickSound;
        clickAudioSource.outputAudioMixerGroup = sfxMixer;

        enterAudioSource = gameObject.AddComponent<AudioSource>();
        enterAudioSource.playOnAwake = false;
        enterAudioSource.clip = enterSound;
        enterAudioSource.outputAudioMixerGroup = sfxMixer;
        
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.SetInt("HpHealer", 30);
        PlayerPrefs.SetInt("HpMagician", 30);
        PlayerPrefs.SetInt("HpSwordsman", 30);
        PlayerPrefs.SetInt("HpWarrior", 30);
        PlayerPrefs.SetInt("ItemMagicBook", 0);
        PlayerPrefs.SetInt("ItemRing", 0);
        PlayerPrefs.SetInt("ItemSword", 0);
        PlayerPrefs.SetInt("ItemNecklace", 0);
        PlayerPrefs.SetString("Potion1", null);
        PlayerPrefs.SetString("Potion2", null);
        PlayerPrefs.SetString("Potion3", null);
    }
    private void ActivateStartCanvas()
    {
        canvas.gameObject.SetActive(true);
    }
    public void OnButtonClick()
    {
        clickAudioSource.PlayOneShot(clickSound);
    }
    public void OnButtonEnter()
    {
        enterAudioSource.PlayOneShot(enterSound);
    }
    public void OnStartGameButtonClick()
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }
    public void OnExitButtonClick()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
