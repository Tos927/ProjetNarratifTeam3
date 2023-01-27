using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

namespace T3
{
    public delegate void notifAction();

    public class CodexRotation : MonoBehaviour
    {
        [SerializeField] private RectTransform UI_Element;

        public notifAction startCodex;
        public notifAction endCodex;

        private Circledivider circle;
        private RectTransform rectT;
        private Vector2 centerPoint;

        private float angle;
        private float prevAngle;
        private static bool isInteractable;

        public float GetAngle()
        {
            return angle;
        }

        void Start()
        {
            circle = GetComponent<Circledivider>();
            rectT = UI_Element;
            InitEventsSystem();
            endCodex += circle.FindCurrentPart;
            TextManager.cleanEvent += () => 
                { angle = 0f; prevAngle = 0f; rectT.localEulerAngles = Vector3.zero; };
            TextManager.startPuzzleEvent += EnableCircle;
        }

        void Update()
        {
            rectT.localEulerAngles = Vector3.back * angle;
        }

        void InitEventsSystem()
        {
            EventTrigger events = UI_Element.gameObject.GetComponent<EventTrigger>();

            if (events == null)
                events = UI_Element.gameObject.AddComponent<EventTrigger>();

            if (events.triggers == null)
                events.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            EventTrigger.TriggerEvent callback = new EventTrigger.TriggerEvent();
            UnityAction<BaseEventData> functionCall = new UnityAction<BaseEventData>(PressEvent);
            callback.AddListener(functionCall);
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback = callback;

            events.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            callback = new EventTrigger.TriggerEvent();
            functionCall = new UnityAction<BaseEventData>(DragEvent);
            callback.AddListener(functionCall);
            entry.eventID = EventTriggerType.Drag;
            entry.callback = callback;

            events.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            callback = new EventTrigger.TriggerEvent();
            functionCall = new UnityAction<BaseEventData>(ReleaseEvent);//
            callback.AddListener(functionCall);
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback = callback;

            events.triggers.Add(entry);
        }

        public void PressEvent(BaseEventData eventData)
        {
            if (!isInteractable)
                return;

            Vector2 pointerPos = ((PointerEventData)eventData).position;

            centerPoint = RectTransformUtility.WorldToScreenPoint(((PointerEventData)eventData).pressEventCamera, rectT.position);
            prevAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);
            startCodex?.Invoke();
        }

        public void DragEvent(BaseEventData eventData)
        {
            if (!isInteractable)
                return;

            Vector2 pointerPos = ((PointerEventData)eventData).position;

            float newAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);
            if (Vector2.Distance(pointerPos, centerPoint) > 20f)
            {
                if (pointerPos.x > centerPoint.x)
                    angle += newAngle - prevAngle;
                else
                    angle -= newAngle - prevAngle;
            }
            prevAngle = newAngle;
        }

        public void ReleaseEvent(BaseEventData eventData)
        {
            if (!isInteractable)
                return;

            DragEvent(eventData);

            endCodex?.Invoke();
        }

        public static void EnableCircle()
        {
            isInteractable = true;
        }

        public static void DisableCircle()
        {
            isInteractable = false;
        }
    }
}