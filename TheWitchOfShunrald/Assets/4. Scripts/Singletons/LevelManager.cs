using System.Collections;
using CihanAkpınar;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Image loadingBar;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Button continueGameButton;
     public GameObject loadingCanvas, gameCanvas;
    public static LevelManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void Start()
    {
        AudioManager.Instance.PlayMusicWithFade(AudioManager.Instance.mainMenuAudio);
        if (PlayerPrefs.GetInt("lastGame", 1) > 1)
        {
            continueGameButton.interactable = true;
        }
    }
    public IEnumerator LoadingGame(int sceneIndex)
    {
        loadingCanvas.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            Debug.Log("oyun başlıyor");
            loadingBar.fillAmount = operation.progress;
            loadingText.text = "Loading %" + ((int)(loadingBar.fillAmount * 100)).ToString();
            yield return null;
        }
        Debug.Log("oyun başladı");
        loadingCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        GameManager.instance.GameStart();
        if (sceneIndex!=7)
        {
            AudioManager.Instance.PlayMusicWithFade(AudioManager.Instance.dungeonAtmosphereAudios);
        }
        else
        {
            AudioManager.Instance.PlayMusicWithFade(AudioManager.Instance.cemeteryAtmosphereAudios);
        }
    }
    public void RestartLevel()
    {
        StartCoroutine(LoadingGame(SceneManager.GetActiveScene().buildIndex));
    }
    public void NewGame()
    {
        PlayerPrefs.SetInt("lastGame", 1);
        PlayerPrefs.SetInt("balance", 0);
        SceneManager.LoadScene(2);
    }
    public void ContinueGame()
    {
        if(PlayerPrefs.GetInt("lastGame") + 2 < SceneManager.sceneCountInBuildSettings)
            StartCoroutine(LoadingGame(PlayerPrefs.GetInt("lastGame")+2));
        else
            StartCoroutine(LoadingGame(PlayerPrefs.GetInt("lastGame")+1));
    }
    public void LoadArena()
    {
        StartCoroutine(LoadArenaa());
    }
    private IEnumerator LoadArenaa()
    {
        using (var www = new UnityEngine.Networking.UnityWebRequest("http://www.google.com"))
        {
            // İsteği başlatırız
            yield return www.SendWebRequest();

            // İsteğin sonucunu kontrol ederiz
            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError ||
                www.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
            {
                // İnternet bağlantısı yok
                Debug.Log("İnternet bağlantısı yok");
            }
            else
            {
                // İnternet bağlantısı var
                StartCoroutine(LoadingGame(1));
            }
        }
    }
}
