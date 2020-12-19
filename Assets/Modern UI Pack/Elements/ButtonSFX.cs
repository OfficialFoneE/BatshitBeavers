using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSFX : MonoBehaviour
{

    private EventTrigger eventTrigger;

    private void Awake()
    {
        eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }
    }

    private void Start()
    {


        EventTrigger.Entry onPointerEnter = null;
        EventTrigger.Entry onPointerClick = null;

        for (int i = 0; i < eventTrigger.triggers.Count; i++)
        {
            if (eventTrigger.triggers[i].eventID == EventTriggerType.PointerEnter)
            {
                onPointerEnter = eventTrigger.triggers[i];
            }
            if (eventTrigger.triggers[i].eventID == EventTriggerType.PointerClick)
            {
                onPointerClick = eventTrigger.triggers[i];
            }
        }

        if (onPointerEnter == null)
        {
            onPointerEnter = new EventTrigger.Entry();
            onPointerEnter.eventID = EventTriggerType.PointerEnter;

            eventTrigger.triggers.Add(onPointerEnter);
        }
        if (onPointerClick == null)
        {
            onPointerClick = new EventTrigger.Entry();
            onPointerClick.eventID = EventTriggerType.PointerClick;

            eventTrigger.triggers.Add(onPointerClick);
        }

        onPointerEnter.callback.AddListener(delegate (BaseEventData e) { AudioSFXReferences.PlayButtonHighlight(); });
        onPointerClick.callback.AddListener(delegate(BaseEventData e) { AudioSFXReferences.PlayButtonClick(); });
    }

}
