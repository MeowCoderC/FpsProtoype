using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.Serialization;

namespace HyperGame.Script.Gameplay.Input
{
     [RequireComponent(typeof(RectTransform))]
     public class EnhancedOnScreenTouchPad : OnScreenControl,
          IPointerDownHandler,
          IPointerUpHandler,
          IDragHandler
     {
          [InputControl(layout = "Vector2")]
          [FormerlySerializedAs("controlPath")]
          [SerializeField]
          private string internalControlPath;

          [SerializeField] private bool         useEventTrigger = false;
          private                  EventTrigger eventTrigger;
          private                  CanvasGroup  canvasGroup;

          protected override string controlPathInternal { get => internalControlPath; set => internalControlPath = value; }

          private        Vector2 prevDelta;
          private        Vector2 dragInput;
          private static Vector2 touchInput;
          private int? touchId = null;

          void Start()
          {
               
               canvasGroup  = GetComponent<CanvasGroup>();
               eventTrigger = GetComponent<EventTrigger>();
               
               SetupListeners();
               canvasGroup.alpha = 0;
          }

          //Setup events;
          void SetupListeners()
          {
               if (!useEventTrigger || !eventTrigger) return;
 
               var pointerDownCallBack = new EventTrigger.TriggerEvent();

               pointerDownCallBack.AddListener(data => { PointerDownHandle((PointerEventData)data); });

               eventTrigger.triggers.Add(new EventTrigger.Entry
                    { callback = pointerDownCallBack, eventID = EventTriggerType.PointerDown });

               var dragCallback = new EventTrigger.TriggerEvent();

               dragCallback.AddListener(data => { DragHandle((PointerEventData)data); });

               eventTrigger.triggers.Add(
                    new EventTrigger.Entry { callback = dragCallback, eventID = EventTriggerType.Drag });

               var pointerUpCallback = new EventTrigger.TriggerEvent();

               pointerUpCallback.AddListener(data => { PointerUpHandle((PointerEventData)data); });

               eventTrigger.triggers.Add(new EventTrigger.Entry
                    { callback = pointerUpCallback, eventID = EventTriggerType.PointerUp, });
          }

          public void OnPointerDown(PointerEventData data)
          {
               if (useEventTrigger) return;

               PointerDownHandle(data);
          }

          public void OnDrag(PointerEventData data)
          {
               if (useEventTrigger) return;

               DragHandle(data);
          }

          public void OnPointerUp(PointerEventData eventData)
          {
               if (useEventTrigger) return;

               PointerUpHandle(eventData);
          }

          void PointerDownHandle(PointerEventData data)
          {
               if (touchId.HasValue) return;
               
               var evData = data;
               touchId = data.pointerId;
               data.Use();
               prevDelta         = dragInput = evData.position;
          }

          void DragHandle(PointerEventData data)
          {
               if (!touchId.HasValue || data.pointerId != touchId.Value) return;
               
               data.Use();
               dragInput  = data.position;
               touchInput = (dragInput - prevDelta);
               SendValueToControl(touchInput);
               prevDelta = dragInput;
          }

          void PointerUpHandle(PointerEventData data)
          {
               if (!touchId.HasValue || data.pointerId != touchId.Value) return;
               
               touchId = null;
               touchInput = Vector2.zero;
               SentDefaultValueToControl();
          }
     }
}
