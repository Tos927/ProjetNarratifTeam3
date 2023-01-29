using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject letterPanel, consequencesVisualPanel, consequencesTextPanel;
    [SerializeField]
    private float DelayToSwitchConsequences = 5;
    private Coroutine temp;
    public bool isStarted = false;
    public GameObject languageSettings;

    [SerializeField] private GameObject creditsScreen;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private bool fadeIn = false, fadeOut = false;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "DialogueTest")
        {
            StartCoroutine(FadeCoroutine());
        }
    }
    #region Fade

    public void ShowUI()
    {
        fadeIn = true;
    }

    public void HideUI()
    {
        fadeOut = true;
    }
    public void Fade()
    {
        StartCoroutine(FadeCoroutine());
    }

    public IEnumerator FadeCoroutine()
    {
        ShowUI();
        yield return new WaitForSeconds(1);
        HideUI();
        yield return new WaitForSeconds(1);
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            SceneManager.LoadScene("DialogueTest");
        }
    }

    #endregion

    
    public void Update()
    {
        if (isStarted)
        {
            languageSettings.SetActive(false);
        }

        if (fadeIn)
        {
            if (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime;
                if (canvasGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }
        if (fadeOut)
        {
            if (canvasGroup.alpha >= 0)
            {
                canvasGroup.alpha -= Time.deltaTime;
                if (canvasGroup.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }
    public void Consequences()
    {
        temp = StartCoroutine(StartConsequences());
    }

    IEnumerator StartConsequences() 
    {
        if (SceneManager.GetActiveScene().name == "DialogueTest")
        {
            consequencesVisualPanel.SetActive(true);
            yield return new WaitForSeconds(DelayToSwitchConsequences);
            consequencesTextPanel.SetActive(true);
            consequencesVisualPanel.SetActive(false);
        }
    }

    public void StopCoroutine()
    {
        StopCoroutine(temp);
    }

    public void StartGame()
    {
        isStarted = true;   
    }

    public void OpenCloseSettings(GameObject settingsPanel)
    {
        if (SceneManager.GetActiveScene().name == "DialogueTest")
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }
}
