using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoubleClickEventHandler : MonoBehaviour
{
    public UnityEvent OnDoubleClick;
    private int clickCount = 0;
    private float timer;
    public void Click()
    {
        timer = 0.5f;
        clickCount++;
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            clickCount = 0;
        }
        if(clickCount == 2)
        {
            OnDoubleClick?.Invoke();
            clickCount = 0;
        }
    }
}
