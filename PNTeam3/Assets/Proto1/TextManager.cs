using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextManager : MonoBehaviour
{
    private TextMeshProUGUI textUI;

    private void Start()
    {
        textUI = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        string originalText = textUI.text;
        textUI.text = RandomizeString(textUI.text);
        char charTofind = ParseText(textUI.text);
        Debug.Log($"Most Frequent Char is : {charTofind}");
    }

    private char ParseText(string text)
    {
        var mostFrequent = text.Where(c => c != ' ')
                                .GroupBy(c => c)
                                .OrderByDescending(g => g.Count())
                                .First()
                                .Key;
        return mostFrequent;
    }

    private string RandomizeString(string text)
    {
        char[] arrayToRandomize = text.ToCharArray();
        int length = text.Length;
        while (length > 1)
        {
            length--;
            int rand = UnityEngine.Random.Range(0, length + 1);
            (arrayToRandomize[rand], arrayToRandomize[length]) = (arrayToRandomize[length], arrayToRandomize[rand]);
        }
        string randomizedString = new string(arrayToRandomize);
        return randomizedString;
    }
}