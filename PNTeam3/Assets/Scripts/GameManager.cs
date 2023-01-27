using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject letterPanel, consequencesVisualPanel, consequencesTextPanel;
    [SerializeField]
    private float DelayToSwitchConsequences = 5;
    private Coroutine temp;
    public bool isStarted = false;
    public GameObject languageSettings;

    public void Consequences()
    {
        temp = StartCoroutine(StartConsequences());
    }

    public void Update()
    {
        if (isStarted)
        {
            languageSettings.SetActive(false);
        }
    }

    IEnumerator StartConsequences() 
    {
        consequencesVisualPanel.SetActive(true);
        yield return new WaitForSeconds(DelayToSwitchConsequences);
        consequencesTextPanel.SetActive(true);
        consequencesVisualPanel.SetActive(false);
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
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
}
