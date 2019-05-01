using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utility.Time;

public class BalloonCatAnimation : MonoBehaviour
{
    [Range(0.001f, 0.999f)]
    public float SinScale = 0.2f;
    private float _delta;
    private float startY;

    private void Awake()
    {
        startY = transform.position.y;
    }

    private void Update()
    {
        var pos = transform.position;
        _delta += Time.deltaTime;
        var move = Mathf.Sin(_delta) * SinScale;
        pos.y = startY + move;
        transform.position = pos;
    }
}
