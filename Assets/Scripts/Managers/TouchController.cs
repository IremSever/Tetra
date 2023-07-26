using UnityEngine;
using TMPro;
public class TouchController : MonoBehaviour {

    public delegate void TouchEventHandler(Vector2 swipe);
    public static event TouchEventHandler DragEvent, SwipeEvent, TapEvent;
    Vector2 touchMovement;
    public TextMeshProUGUI textDia1, textDia2;
    public bool useDia = false;
    [Range(50, 150)] public int minDragDistance = 100;
    [Range(20, 250)] public int minSwipeDistance = 50;
    float tapTimeMax = 0;
    public float tapTimeWindow = 0.1f;
    void OnTap()
    {
        if (TapEvent != null)
            TapEvent(touchMovement);
    }
    void OnDrag()
    {
        if (DragEvent != null)
            DragEvent(touchMovement);
    }
    void OnSwipeEnd()
    {
        if (DragEvent != null)
            SwipeEvent(touchMovement);
    }
    void Start()
    {
        Diagnostic("", "");
    }
    void Diagnostic(string txt1, string txt2)
    {
        textDia1.gameObject.SetActive(useDia);
        textDia2.gameObject.SetActive(useDia);
        if (textDia1 && textDia2)
        {
            textDia1.text = txt1;
            textDia2.text = txt2;
        }
    }
    string SwipeDiagnostic(Vector2 swipeMovement)
    {
        string direction = "";
        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
            direction = (swipeMovement.x >= 0) ? "right" : "left";
        else
            direction = (swipeMovement.y >= 0) ? "up" : "down";
        return direction;
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                touchMovement = Vector2.zero;
                tapTimeMax = Time.time + tapTimeWindow;
                Diagnostic("", "");
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                touchMovement += touch.deltaPosition;
                if (touchMovement.magnitude > minDragDistance)
                {
                    OnDrag();
                    Diagnostic("Drag detected", touchMovement.ToString() + " " + SwipeDiagnostic(touchMovement));
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (touchMovement.magnitude > minSwipeDistance)
                {
                    OnSwipeEnd();
                    Diagnostic("Swipe detected", touchMovement.ToString() + " " + SwipeDiagnostic(touchMovement));
                }
                else if (Time.time < tapTimeMax)
                {
                    OnTap();
                    Diagnostic("Tap detected", touchMovement.ToString() + " " + SwipeDiagnostic(touchMovement));
                }
            }
        }
    }
}
