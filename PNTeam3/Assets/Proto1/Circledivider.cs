using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circledivider : MonoBehaviour
{
    public int numberOfParts = 8;
    public float radius = 1;
    public GameObject linePrefab;
    public GameObject textPrefab;

    private void Start()
    {
        float angleStep = 360f / numberOfParts;
        for (int i = 0; i <= numberOfParts; i++)
        {
            float angle = angleStep * i;
            Vector3 linePosition = new Vector3(transform.position.x, transform.position.y, 0);

            GameObject line = Instantiate(linePrefab, linePosition, Quaternion.identity);
            line.GetComponent<RectTransform>().transform.Rotate(new Vector3(0,0,angle));
            line.transform.SetParent(transform);

            CodexRotation.endCodex += FindCurrentPart;
        }
    }

    private void FindCurrentPart()
    {
        float currentRotation = transform.eulerAngles.z;
        float angleStep = 360f / numberOfParts;
        int currentPart = (int)(currentRotation / angleStep);
        Debug.Log(currentPart);
    }
}
