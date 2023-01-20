using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject letterPanel, consequencesVisualPanel, consequencesTextPanel;
    [SerializeField]
    private float DelayToSwitchConsequences = 5;
    private Coroutine temp;
    public void Consequences()
    {
        temp = StartCoroutine(StartConsequences());
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


    public void OpenCloseSettings(GameObject settingsPanel)
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
}
