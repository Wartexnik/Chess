using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosePromotion : MonoBehaviour
{
    public static int promotion = 0;
    public GameObject prompt;
    public Vector2 mousePosition;
    private void OnMouseDown()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Mathf.Abs(mousePosition.y) > LegalMoves.BoardToWorld(7.5f) && Mathf.Abs(mousePosition.y) <= LegalMoves.BoardToWorld(8.5f))
        {
            promotion = 1;
        }
        if (Mathf.Abs(mousePosition.y) > LegalMoves.BoardToWorld(6.5f) && Mathf.Abs(mousePosition.y) <= LegalMoves.BoardToWorld(7.5f))
        {
            promotion = 2;
        }
        if (Mathf.Abs(mousePosition.y) > LegalMoves.BoardToWorld(5.5f) && Mathf.Abs(mousePosition.y) <= LegalMoves.BoardToWorld(6.5f))
        {
            promotion = 3;
        }
        if (Mathf.Abs(mousePosition.y) <= LegalMoves.BoardToWorld(5.5f))
        {
            promotion = 4;
        }
    }
}
