using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EncodedPuzzle", menuName = "Encoded/New Encoded Puzzle", order = 0)]
public class PuzzleDataSO : ScriptableObject
{
    [SerializeField] private char[] lettersFirstSentence;
    public char[] getLettersFirstSentence => lettersFirstSentence;
    [SerializeField] private char[] lettersSecondSentence;
    public char[] getLettersSecondSentence => lettersSecondSentence;
    [SerializeField] private string firstSentence;
    public string getFirstSentence => firstSentence;
    [SerializeField] private string secondSentence;
    public string getSecondSentence => secondSentence;
}
