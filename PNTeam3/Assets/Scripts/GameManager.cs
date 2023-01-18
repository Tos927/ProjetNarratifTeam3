using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject letterPanel, consequencesVisualPanel, consequencesTextPanel;
    public void Consequences()
    {
        StartCoroutine(StartConsequences());
    }

    IEnumerator StartConsequences() 
    {
        consequencesVisualPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        consequencesTextPanel.SetActive(true);
        consequencesVisualPanel.SetActive(false);
    }

}
