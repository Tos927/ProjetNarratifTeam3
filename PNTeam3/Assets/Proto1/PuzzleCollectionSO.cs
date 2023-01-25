using UnityEngine;

[CreateAssetMenu(fileName = "EncodedPuzzle", menuName = "Encoded/New Encoded Puzzle", order = 0)]
public class PuzzleCollectionSO : ScriptableObject
{
    [SerializeField] private PuzzleDataSO[] puzzleSOs;
    public PuzzleDataSO[] getPuzzleDataSOs => puzzleSOs;
}
