using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;

namespace T3
{
    public class TextManager : MonoBehaviour
    {
        [SerializeField] private PuzzleCollectionSO puzzles;
        [SerializeField] private float animDuration;
        [SerializeField] private TextMeshProUGUI firstSentenceUI;
        [SerializeField] private TextMeshProUGUI secondSentenceUI;
        [SerializeField] private Circledivider[] circledividers;
        public Circledivider[] getCircledivider => circledividers;

        private TextMeshProUGUI textUITest;
        private int indexTrack;
        private int firstAnswer;
        private int secondAnswer;
        private int firstInput = -1;
        private int secondInput = -1;

        private void Start()
        {
            circledividers[0].findPartEvent += x => { return firstInput = x; };
            circledividers[0].findPartEvent += x => { CheckInput(); return x; };
            circledividers[1].findPartEvent += x => { return secondInput = x; };
            circledividers[1].findPartEvent += x => { CheckInput(); return x; };
            InitText(0);
        }

        private int ParseText(string text, char[] letters)
        {
            var mostFrequent = text.Where(c => c != ' ')
                                    .GroupBy(c => c)
                                    .OrderByDescending(g => g.Count())
                                    .First()
                                    .Key;
            int answer = 0;
            for (int i = 0; i < letters.Length; ++i)
            {
                if (mostFrequent == letters[i])
                {
                    answer = i;
                    Debug.Log($"La lettre a trouver est : {mostFrequent} ({answer})");
                    return answer;
                }
            }

            return answer;
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

        public void UncoverSentences(int index)
        {
            Tween a = firstSentenceUI.DOText(puzzles.getPuzzleDataSOs[index].getFirstSentence, animDuration, true, ScrambleMode.Uppercase);
            a.onComplete += () =>
            { Tween b = secondSentenceUI.DOText(puzzles.getPuzzleDataSOs[index].getSecondSentence, animDuration, true, ScrambleMode.Uppercase); };
        }

        private void InitText(int index)
        {
            indexTrack = index;
            firstSentenceUI.text = stoc(puzzles.getPuzzleDataSOs[index].getFirstSentence, puzzles.getPuzzleDataSOs[index].getLettersFirstSentence);
            secondSentenceUI.text = stoc(puzzles.getPuzzleDataSOs[index].getSecondSentence, puzzles.getPuzzleDataSOs[index].getLettersSecondSentence);

            firstAnswer = ParseText(firstSentenceUI.text, puzzles.getPuzzleDataSOs[index].getLettersFirstSentence);
            secondAnswer = ParseText(secondSentenceUI.text, puzzles.getPuzzleDataSOs[index].getLettersSecondSentence);

            circledividers[0].SetupCircle(puzzles.getPuzzleDataSOs[index].getLettersFirstSentence.Length, puzzles.getPuzzleDataSOs[index].getLettersFirstSentence);
            circledividers[1].SetupCircle(puzzles.getPuzzleDataSOs[index].getLettersSecondSentence.Length, puzzles.getPuzzleDataSOs[index].getLettersSecondSentence);
        }

        private void CheckInput()
        {
            //if (firstAnswer != firstInput)
            //{
            //    Debug.Log("First answer incorrect");
            //    return;
            //}
            //Debug.Log("first answer correct");
            //if (secondAnswer != secondInput)
            //{
            //    Debug.Log("Second answer incorrect");
            //    return;
            //}
            //Debug.Log("Second answer correct");
            if (firstAnswer == firstInput && secondAnswer == secondInput)
            {
                UncoverSentences(indexTrack);
                Debug.Log("Correct");
                return;
            }
            Debug.Log("Incorrect");
        }
    }
}