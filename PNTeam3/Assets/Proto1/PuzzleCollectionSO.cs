using UnityEngine;

[CreateAssetMenu(fileName = "EncodedPuzzleCollection", menuName = "Encoded/Collection", order = 0)]
public class PuzzleCollectionSO : ScriptableObject
{
    [SerializeField] private PuzzleDataSO[] puzzleSOs;
    public PuzzleDataSO[] getPuzzleDataSOs => puzzleSOs;
}
