using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {

    const float smoothTime = .15f;

    Material mat;
    public Color targetCol;
    float smoothV;
    float percent;

    void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
        mat.shader = Shader.Find("Unlit/Color");
    }

    public void SetTargetCol(Color col)
    {
        targetCol = col;
        percent = 0;
    }

    public void SetCol(Color col)
    {
        percent = 1;
        mat.color = col;
        targetCol = col;
        smoothV = 0;
    }

	void Update () {
        percent = Mathf.SmoothDamp(percent, 1, ref smoothV, smoothTime);
        mat.color = Color.Lerp(mat.color, targetCol, percent);
	}
}
