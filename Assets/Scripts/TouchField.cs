using UnityEngine;
using UnityEngine.EventSystems;

public class TouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector2 TouchDistance;
    Vector2 PointerOld;
    int PointerId;
    public bool Pressed;
    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        PointerId = eventData.pointerId;
        PointerOld = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }
    void Update()
    {
        if (Pressed)
        {
            if(PointerId >= 0 && PointerId < Input.touches.Length)//on PC Doesn`t work, only on telephone
            {
                TouchDistance = Input.touches[PointerId].position - PointerOld;
                PointerOld = Input.touches[PointerId].position;
            }
        }
        else
        {
            TouchDistance = new Vector2();
        }
    }
}
