using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextManager : MonoBehaviour
{
    [SerializeField] private char[] lettersFirstSentence;
    [SerializeField] private char[] lettersSecondSentence;
    [SerializeField] private string firstSentence;
    [SerializeField] private string secondSentence;
    [SerializeField] private float animDuration; 
    [SerializeField] private TextMeshProUGUI firstSentenceUI;
    [SerializeField] private TextMeshProUGUI secondSentenceUI;

    private TextMeshProUGUI textUITest;
    private char firstAnswer;
    private char secondAnswer;

    private void Start()
    {
        InitText();
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
        string[] words = text.Split(' ');
        for (int i = 0; i < words.Length; ++i)
        {
            char[] wordChars = words[i].ToCharArray();
            for (int j = 0; j < wordChars.Length; ++j)
            {
                int k = UnityEngine.Random.Range(j, wordChars.Length);
                (wordChars[j], wordChars[k]) = (wordChars[k], wordChars[j]);
            }
            words[i] = new string(wordChars);
        }
        string randomizedString = string.Join(" ", words);
        return randomizedString;
    }

    private string stoc(string text, char[] letters)
    {
        string[] words = text.Split(' ');
        for (int i = 0; i < words.Length; ++i)
        {
            char[] wordChars = words[i].ToCharArray();
            for (int j = 0; j < wordChars.Length; ++j)
            {
                wordChars[j] = letters[wordChars[j] % letters.Length];
            }
            words[i] = new string(wordChars);
        }
        string encodedString = string.Join(" ", words);
        return encodedString;
    }

    public void UncoverSentences()
    {
        Tween a = firstSentenceUI.DOText(firstSentence, animDuration, true, ScrambleMode.Uppercase);
        a.onComplete += () =>
        { Tween b = secondSentenceUI.DOText(secondSentence, animDuration, true, ScrambleMode.Uppercase); };
    }

    private void InitText()
    {
        firstSentenceUI.text = stoc(firstSentence, lettersFirstSentence);
        secondSentenceUI.text = stoc(secondSentence,lettersSecondSentence);

        firstAnswer = ParseText(firstSentenceUI.text);
        secondAnswer = ParseText(secondSentenceUI.text);

        Debug.Log($"Les lettres a trouve sont : {firstAnswer} et {secondAnswer}");
    }
}