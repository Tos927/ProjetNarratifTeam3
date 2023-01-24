using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Circledivider : MonoBehaviour
{
    public int numberOfParts = 8;
    public float radius = 1;
    public GameObject linePrefab;
    public GameObject textPrefab;

    private void Start()
    {
        float angleStep = 360f / numberOfParts;
        for (int i = 0; i < numberOfParts; ++i)
        {
            float angle = angleStep * i;
            Vector3 linePosition = new Vector3(transform.position.x, transform.position.y, 0);

            GameObject line = Instantiate(linePrefab, linePosition, Quaternion.identity);
            RectTransform lineRect = line.GetComponent<RectTransform>();
            lineRect.transform.Rotate(new Vector3(0,0,-angle));
            lineRect.sizeDelta = new Vector2(lineRect.rect.width, GetComponent<RectTransform>().rect.height);
            line.transform.SetParent(transform);
            line.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            GameObject textPositionTest = Instantiate(textPrefab, transform.position, Quaternion.identity);
            textPositionTest.transform.SetParent(transform);
            line.transform.SetParent(transform);
            RectTransform textRect = textPositionTest.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(lineRect.rect.width * 5, GetComponent<RectTransform>().rect.height * .85f);
            textRect.transform.Rotate(new Vector3(0, 0, -angle - angleStep / 2));
        }
        CodexRotation.endCodex += FindCurrentPart;

    }

    private void FindCurrentPart()
    {
        float currentRotation = transform.eulerAngles.z;
        float angleStep = 360f / numberOfParts;
        int currentPart = (int)(currentRotation / angleStep);
        Debug.Log(currentPart);
    }
}
