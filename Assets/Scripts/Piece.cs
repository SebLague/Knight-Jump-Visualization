using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    public event System.Action<Piece> OnSelected;
    public event System.Action<Piece> OnDropped;
    public event System.Action<Piece> OnMoved;
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    public void SetPosition(Vector2 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, -.01f);
    }

    private void OnMouseDown()
    {
        if (OnSelected != null)
        {
            OnSelected(this);
        }
    }

    private void OnMouseUp()
    {
        if (OnDropped != null)
        {
            OnDropped(this);
        }
    }

    private void OnMouseDrag()
    {
        SetPosition(cam.ScreenToWorldPoint(Input.mousePosition));
      
        if (OnMoved != null)
        {
            OnMoved(this);
        }
    }
}
