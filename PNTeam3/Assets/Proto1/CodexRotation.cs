using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public delegate void notifAction();

public class CodexRotation : MonoBehaviour
{
    public RectTransform UI_Element;
    public static notifAction startCodex;
    public static notifAction endCodex;

    private RectTransform rectT;
    private Vector2 centerPoint;

    private float angle;
    private float prevAngle;

    public float GetAngle()
    {
        return angle;
    }

    void Start()
    {
        rectT = UI_Element;
        InitEventsSystem();
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
        Vector2 pointerPos = ((PointerEventData)eventData).position;

        centerPoint = RectTransformUtility.WorldToScreenPoint(((PointerEventData)eventData).pressEventCamera, rectT.position);
        prevAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);
        startCodex?.Invoke();
    }

    public void DragEvent(BaseEventData eventData)
    {
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
        DragEvent(eventData);

        endCodex?.Invoke();
    }
}