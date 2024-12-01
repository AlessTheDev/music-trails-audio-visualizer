using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

class DotData
{
    public float X { get; private set; }
    public float Y { get; private set; }
    public RectTransform Obj { get; private set; }
    public ConnectionData Connection { get; set; }

    public DotData(float x, float y, RectTransform obj)
    {
        X = x;
        Y = y;
        Obj = obj;
        Connection = null;
    }
}

class ConnectionData
{
    private RectTransform a;
    private RectTransform b;
    public RectTransform Connection { get; private set; }

    public ConnectionData(RectTransform a, RectTransform b, RectTransform connection)
    {
        this.a = a;
        this.b = b;
        this.Connection = connection;
    }

    public Vector2 A => !a.IsDestroyed() ? a.anchoredPosition : Vector2.zero;

    public Vector2 B => !b.IsDestroyed() ? b.anchoredPosition : Vector2.zero;
}

public class Graph : MonoBehaviour
{
    public Color32 graphColor = Color.white;

    private readonly List<DotData> dots = new();

    private float t = 0;

    private float xOffset;
    private float xOffsetTarget;

    private GraphWrapper _wrapper;
    public string graphName = "";

    public void BaseStart()
    {
        _wrapper = GraphWrapper.Instance;
    }

    public void BaseUpdate()
    {
        if (GameManager.Instance.IsPaused) return;
        t += Time.deltaTime;
        UpdateDotPositions();
        xOffset = Mathf.Lerp(xOffset, xOffsetTarget, Time.deltaTime * _wrapper.movementSpeed * _wrapper.unitDistance);
    }

    public void AddValue(float value)
    {
        DotData dot = new(
            t,
            value,
            CreateDot()
        );

        GraphDot graphDot = dot.Obj.GetComponent<GraphDot>();
        graphDot.Value = value;
        graphDot.Time = t;
        graphDot.Name = graphName;

        if (dots.Count != 0)
        {
            dot.Connection = new ConnectionData(dots.Last().Obj, dot.Obj, CreateConnection());
        }

        float dotPos = _wrapper.unitDistance * dot.X;
        if (dotPos > _wrapper.graphContainer.sizeDelta.x)
        {
            xOffsetTarget = dotPos - _wrapper.graphContainer.sizeDelta.x + 100;
        }
        dots.Add(dot);
        _wrapper.TrySetMaxValue(graphName, value);
    }

    private RectTransform CreateDot()
    {
        RectTransform dot = Instantiate(_wrapper.dotPrefab, _wrapper.graphContainer);
        dot.gameObject.GetComponent<Image>().color = graphColor;

        dot.sizeDelta = Vector2.one * _wrapper.dotSize;

        return dot;
    }

    public void UpdateDotPositions()
    {
        float graphHeight = _wrapper.graphContainer.sizeDelta.y - _wrapper.dotSize;

        List<DotData> toRemove = new();

        for (int i = 0; i < dots.Count; i++)
        {

            DotData dot = dots[i];

            bool visible = _wrapper.focusList.Count == 0 || _wrapper.focusList.Contains(graphName);
            dot.Obj.gameObject.SetActive(visible);
            dot.Connection?.Connection.gameObject.SetActive(visible);

            Vector2 dotPos = new Vector2(
                _wrapper.unitDistance * dot.X - xOffset,
                (dot.Y / _wrapper.MaxValue) * graphHeight
            );

            if (_wrapper.deleteOutsideobjects && dotPos.x < -100)
            {
                toRemove.Add(dot);
            }

            dot.Obj.anchoredPosition = dotPos;

            if (dot.Connection != null)
            {
                UpdateConnection(dot.Connection);
            }
        }

        for (int i = 0; i < toRemove.Count; i++)
        {
            DotData dot = toRemove[i];

            if (dot.Connection != null)
            {
                Destroy(dot.Connection.Connection.gameObject);
            }
            Destroy(dot.Obj.gameObject);
            dot.Connection = null;
            dots.Remove(dot);
        }
    }

    // NOTE: I tried with LineRenderer but since we are on a canvas all the calculations got messed up
    private RectTransform CreateConnection()
    {
        // Create the line
        GameObject connection = new GameObject("Connection", typeof(Image));
        connection.transform.SetParent(_wrapper.graphContainer, false);
        Color c = graphColor;
        c.a = 0.5f;
        connection.GetComponent<Image>().color = c;
        return connection.GetComponent<RectTransform>();
    }

    private void UpdateConnection(ConnectionData connectionData)
    {
        Vector2 a = connectionData.A;
        Vector2 b = connectionData.B;
        RectTransform connection = connectionData.Connection;

        connection.anchorMin = Vector2.zero;
        connection.anchorMax = Vector2.zero;

        Vector2 direction = (a - b).normalized;
        float distance = Vector2.Distance(a, b);

        connection.sizeDelta = new Vector2(
            distance,
            _wrapper.dotSize / 4
        );

        connection.anchoredPosition = a - direction * distance / 2;
        connection.anchoredPosition += new Vector2(connection.sizeDelta.y, _wrapper.dotSize / 2);

        // Rotate the line
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        connection.localEulerAngles = Vector3.forward * angle;
    }
}
