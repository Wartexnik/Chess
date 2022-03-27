using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static int turn = 0;
    public static List<GameObject> pieces = new List<GameObject>();
    public static List<Vector2> coordinates = new List<Vector2>();
    public static Vector2 enpassant;
    public static int enpassant_color;
    public GameObject pawn_w;
    public GameObject pawn_b;
    public GameObject knight_w;
    public GameObject knight_b;
    public GameObject bishop_w;
    public GameObject bishop_b;
    public GameObject rook_w;
    public GameObject rook_b;
    public GameObject queen_w;
    public GameObject queen_b;
    public GameObject king_w;
    public GameObject king_b;
    void Start()
    {
        CreateStandardPosition();
        //CreateTestPosition();
        for (int i = 0; i < pieces.Count; i++)
        {
            coordinates.Add(new Vector2(pieces[i].transform.position.x, pieces[i].transform.position.y));
        }
        foreach (GameObject go in pieces.ToList())
        {
            go.GetComponent<LegalMoves>().CalculateMoves();
            //go.GetComponent<LegalMoves>().KingInCheck(go.GetComponent<LegalMoves>().moves);
        }
        UpdateTurn();
    }
    void CreateStandardPosition()
    {
        for (int i = 0; i < 8; i++)
        {
            pieces.Add(Instantiate(pawn_w, new Vector3(-7 + i * 2, -5, 0), Quaternion.identity));
            pieces.Add(Instantiate(pawn_b, new Vector3(-7 + i * 2, 5, 0), Quaternion.identity));
        }

        pieces.Add(Instantiate(knight_w, LegalMoves.BoardToWorld(new Vector3(2, 1, 0)), Quaternion.identity));
        pieces.Add(Instantiate(knight_w, LegalMoves.BoardToWorld(new Vector3(7, 1, 0)), Quaternion.identity));
        pieces.Add(Instantiate(knight_b, LegalMoves.BoardToWorld(new Vector3(2, 8, 0)), Quaternion.identity));
        pieces.Add(Instantiate(knight_b, LegalMoves.BoardToWorld(new Vector3(7, 8, 0)), Quaternion.identity));

        pieces.Add(Instantiate(bishop_w, LegalMoves.BoardToWorld(new Vector3(3, 1, 0)), Quaternion.identity));
        pieces.Add(Instantiate(bishop_w, LegalMoves.BoardToWorld(new Vector3(6, 1, 0)), Quaternion.identity));
        pieces.Add(Instantiate(bishop_b, LegalMoves.BoardToWorld(new Vector3(3, 8, 0)), Quaternion.identity));
        pieces.Add(Instantiate(bishop_b, LegalMoves.BoardToWorld(new Vector3(6, 8, 0)), Quaternion.identity));

        pieces.Add(Instantiate(rook_w, LegalMoves.BoardToWorld(new Vector3(1, 1, 0)), Quaternion.identity));
        pieces.Add(Instantiate(rook_w, LegalMoves.BoardToWorld(new Vector3(8, 1, 0)), Quaternion.identity));
        pieces.Add(Instantiate(rook_b, LegalMoves.BoardToWorld(new Vector3(1, 8, 0)), Quaternion.identity));
        pieces.Add(Instantiate(rook_b, LegalMoves.BoardToWorld(new Vector3(8, 8, 0)), Quaternion.identity));

        pieces.Add(Instantiate(queen_w, LegalMoves.BoardToWorld(new Vector3(4, 1, 0)), Quaternion.identity));
        pieces.Add(Instantiate(queen_b, LegalMoves.BoardToWorld(new Vector3(4, 8, 0)), Quaternion.identity));

        pieces.Add(Instantiate(king_w, LegalMoves.BoardToWorld(new Vector3(5, 1, 0)), Quaternion.identity));
        pieces.Add(Instantiate(king_b, LegalMoves.BoardToWorld(new Vector3(5, 8, 0)), Quaternion.identity));
    }
    void CreateTestPosition()
    {
        pieces.Add(Instantiate(rook_b, LegalMoves.BoardToWorld(new Vector3(4, 8, 0)), Quaternion.identity));
        pieces.Add(Instantiate(bishop_w, LegalMoves.BoardToWorld(new Vector3(4, 5, 0)), Quaternion.identity));
        pieces.Add(Instantiate(king_w, LegalMoves.BoardToWorld(new Vector3(4, 4, 0)), Quaternion.identity));
    }
    public static void UpdateTurn()
    {
        foreach (GameObject go in pieces)
        {
            if (turn % 2 == 0)
            {
                if (go.GetComponent<ColorBlack>() != null)
                    go.GetComponent<MovePiece>().enabled = false;
                else if (go.GetComponent<ColorWhite>() != null)
                    go.GetComponent<MovePiece>().enabled = true;
                if (enpassant_color == 0)
                    enpassant = new Vector2(100, 100);
            }
            else if (turn % 2 == 1)
            {
                if (go.GetComponent<ColorBlack>() != null)
                    go.GetComponent<MovePiece>().enabled = true;
                else if (go.GetComponent<ColorWhite>() != null)
                    go.GetComponent<MovePiece>().enabled = false;
                if (enpassant_color == 1)
                    enpassant = new Vector2(100, 100);
            }
        }
    }
}
