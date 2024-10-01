using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UiEvent
{
    public static void SetEvent(this GameObject gameObject, UIEventType uiEventType, Action<PointerEventData> action)
    {
        UiEventHandler handler = Utilities.GetOrAddComponent<UiEventHandler>(gameObject);
        handler.BindEvent(uiEventType, action);
    }
}
