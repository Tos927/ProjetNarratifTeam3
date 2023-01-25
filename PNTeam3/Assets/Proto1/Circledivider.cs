using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace T3
{
    public delegate int circleEvent(int i);

    public class Circledivider : MonoBehaviour
    {
        public circleEvent findPartEvent;

        private int numberOfParts;

        [SerializeField] private GameObject linePrefab;
        [SerializeField] private GameObject textPrefab;

        private List<GameObject> lineList = new();
        private List<GameObject> textList = new();

        public void SetupCircle(int nbParts, char[] letters)
        {
            numberOfParts = nbParts;
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
                lineList.Add(line);

                GameObject textPosition = Instantiate(textPrefab, transform.position, Quaternion.identity);
                textPosition.transform.SetParent(transform);
                RectTransform textRect = textPosition.GetComponent<RectTransform>();
                textRect.sizeDelta = new Vector2(lineRect.rect.width * 5, GetComponent<RectTransform>().rect.height * .85f);
                textRect.transform.Rotate(new Vector3(0, 0, -angle - angleStep / 2));
                textPosition.GetComponent<TextMeshProUGUI>().text = letters[i].ToString();
                textList.Add(textPosition);
            }
        }

        public void FindCurrentPart()
        {
            float currentRotation = transform.eulerAngles.z;
            float angleStep = 360f / numberOfParts;
            int currentPart = (int)(currentRotation / angleStep);
            findPartEvent?.Invoke(currentPart);
            Debug.Log(currentPart);
        }

        public void ClearCircle()
        {
            for (int i = lineList.Count - 1; i >= 0; --i)
            {
                Destroy(lineList[i]);
            }

            for (int i = textList.Count - 1; i >= 0; --i)
            {
                Destroy(textList[i]);
            }

            lineList.Clear();
            textList.Clear();
        }
    }
}
