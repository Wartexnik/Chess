using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LegalMoves : MonoBehaviour
{
    public List<Vector2> moves = new List<Vector2>();
    public List<Vector2> moves_saved = new List<Vector2>();
    public List<List<Vector2>> moves_list_saved = new List<List<Vector2>>();
    public Vector2 coordinates, coordinates_new, pos;
    GameObject savedPiece;
    bool savedPieceDestroyed = false;
    int index;
    Vector2[] moves_knight = { new Vector2(1, 2), new Vector2(2, 1), new Vector2(-1, 2), new Vector2(2, -1), new Vector2(1, -2), new Vector2(-2, 1), new Vector2(-1, -2), new Vector2(-2, -1) };
    public void CalculateMoves()
    {
        moves.Clear();
        coordinates = WorldToBoard(new Vector2(transform.position.x, transform.position.y));
        if (GetComponent<PiecePawn>() != null)
        {
            if (GetComponent<ColorWhite>() != null)
            {
                if (!GameSystem.coordinates.Contains(BoardToWorld(coordinates + new Vector2(0, 1))) && (coordinates.y + 1) <= 8)
                {
                    moves.Add(BoardToWorld(coordinates + new Vector2(0, 1)));
                    if (!GameSystem.coordinates.Contains(BoardToWorld(coordinates + new Vector2(0, 2))) && !gameObject.GetComponent<PiecePawn>().moved)
                    {
                        moves.Add(BoardToWorld(coordinates + new Vector2(0, 2)));
                    }
                }
                index = GameSystem.coordinates.IndexOf(BoardToWorld(coordinates + new Vector2(1, 1)));
                if ((index != -1 && GameSystem.pieces[index].GetComponent<ColorBlack>() != null) || (WorldToBoard(GameSystem.enpassant) == coordinates + new Vector2(1, 1) && GameSystem.enpassant_color == 1))
                {
                    moves.Add(BoardToWorld(coordinates + new Vector2(1, 1)));
                }
                index = GameSystem.coordinates.IndexOf(BoardToWorld(coordinates + new Vector2(-1, 1)));
                if ((index != -1 && GameSystem.pieces[index].GetComponent<ColorBlack>() != null) || (WorldToBoard(GameSystem.enpassant) == coordinates + new Vector2(-1, 1) && GameSystem.enpassant_color == 1))
                {
                    moves.Add(BoardToWorld(coordinates + new Vector2(-1, 1)));
                }
            }
            if (GetComponent<ColorBlack>() != null)
            {
                if (!GameSystem.coordinates.Contains(BoardToWorld(coordinates + new Vector2(0, -1))) && (coordinates.y - 1) >= 1)
                {

                    moves.Add(BoardToWorld(coordinates + new Vector2(0, -1)));
                    if (!GameSystem.coordinates.Contains(BoardToWorld(coordinates + new Vector2(0, -2))) && !gameObject.GetComponent<PiecePawn>().moved)
                    {
                        moves.Add(BoardToWorld(coordinates + new Vector2(0, -2)));
                    }
                }
                index = GameSystem.coordinates.IndexOf(BoardToWorld(coordinates + new Vector2(1, -1)));
                if ((index != -1 && GameSystem.pieces[index].GetComponent<ColorWhite>() != null) || (WorldToBoard(GameSystem.enpassant) == coordinates + new Vector2(1, -1) && GameSystem.enpassant_color == 0))
                {
                    moves.Add(BoardToWorld(coordinates + new Vector2(1, -1)));
                }
                index = GameSystem.coordinates.IndexOf(BoardToWorld(coordinates + new Vector2(-1, -1)));
                if ((index != -1 && GameSystem.pieces[index].GetComponent<ColorWhite>() != null) || (WorldToBoard(GameSystem.enpassant) == coordinates + new Vector2(-1, -1) && GameSystem.enpassant_color == 0))
                {
                    moves.Add(BoardToWorld(coordinates + new Vector2(-1, -1)));
                }
            }
        }
        if (GetComponent<PieceKnight>() != null)
        {
            foreach (Vector2 move in moves_knight)
            {
                coordinates_new = coordinates + move;
                index = GameSystem.coordinates.IndexOf(BoardToWorld(coordinates_new));
                if (((index == -1) || (index != -1) && GameSystem.pieces[index].GetComponent<ColorWhite>() != GetComponent<ColorWhite>() && GameSystem.pieces[index].GetComponent<ColorBlack>() != GetComponent<ColorBlack>()) && coordinates_new.x >= 1 && coordinates_new.x <= 8 && coordinates_new.y >= 1 && coordinates_new.y <= 8)
                {
                    moves.Add(BoardToWorld(coordinates_new));
                }
            }
        }
        if (GetComponent<PieceRook>() != null)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 7; i++)
                {
                    switch (j)
                    {
                        case 0:
                            coordinates_new = coordinates + new Vector2(i + 1, 0);
                            break;
                        case 1:
                            coordinates_new = coordinates + new Vector2(-i - 1, 0);
                            break;
                        case 2:
                            coordinates_new = coordinates + new Vector2(0, i + 1);
                            break;
                        case 3:
                            coordinates_new = coordinates + new Vector2(0, -i - 1);
                            break;
                    }
                    index = GameSystem.coordinates.IndexOf(BoardToWorld(coordinates_new));
                    if (((index == -1) || (index != -1) && GameSystem.pieces[index].GetComponent<ColorWhite>() != GetComponent<ColorWhite>() && GameSystem.pieces[index].GetComponent<ColorBlack>() != GetComponent<ColorBlack>()) && coordinates_new.x >= 1 && coordinates_new.x <= 8 && coordinates_new.y >= 1 && coordinates_new.y <= 8)
                    {
                        moves.Add(BoardToWorld(coordinates_new));
                        if (index != -1)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        if (GetComponent<PieceBishop>() != null)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 7; i++)
                {
                    switch (j)
                    {
                        case 0:
                            coordinates_new = coordinates + new Vector2(i + 1, i + 1);
                            break;
                        case 1:
                            coordinates_new = coordinates + new Vector2(-i - 1, -i - 1);
                            break;
                        case 2:
                            coordinates_new = coordinates + new Vector2(-i - 1, i + 1);
                            break;
                        case 3:
                            coordinates_new = coordinates + new Vector2(i + 1, -i - 1);
                            break;
                    }
                    index = GameSystem.coordinates.IndexOf(BoardToWorld(coordinates_new));
                    if (((index == -1) || (index != -1) && GameSystem.pieces[index].GetComponent<ColorWhite>() != GetComponent<ColorWhite>() && GameSystem.pieces[index].GetComponent<ColorBlack>() != GetComponent<ColorBlack>()) && coordinates_new.x >= 1 && coordinates_new.x <= 8 && coordinates_new.y >= 1 && coordinates_new.y <= 8)
                    {
                        moves.Add(BoardToWorld(coordinates_new));
                        if (index != -1)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        if (GetComponent<PieceQueen>() != null)
        {
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 7; i++)
                {
                    switch (j)
                    {
                        case 0:
                            coordinates_new = coordinates + new Vector2(i + 1, i + 1);
                            break;
                        case 1:
                            coordinates_new = coordinates + new Vector2(-i - 1, -i - 1);
                            break;
                        case 2:
                            coordinates_new = coordinates + new Vector2(-i - 1, i + 1);
                            break;
                        case 3:
                            coordinates_new = coordinates + new Vector2(i + 1, -i - 1);
                            break;
                        case 4:
                            coordinates_new = coordinates + new Vector2(i + 1, 0);
                            break;
                        case 5:
                            coordinates_new = coordinates + new Vector2(0, -i - 1);
                            break;
                        case 6:
                            coordinates_new = coordinates + new Vector2(0, i + 1);
                            break;
                        case 7:
                            coordinates_new = coordinates + new Vector2(-i - 1, 0);
                            break;
                    }
                    index = GameSystem.coordinates.IndexOf(BoardToWorld(coordinates_new));
                    if (((index == -1) || (index != -1) && GameSystem.pieces[index].GetComponent<ColorWhite>() != GetComponent<ColorWhite>() && GameSystem.pieces[index].GetComponent<ColorBlack>() != GetComponent<ColorBlack>()) && coordinates_new.x >= 1 && coordinates_new.x <= 8 && coordinates_new.y >= 1 && coordinates_new.y <= 8)
                    {
                        moves.Add(BoardToWorld(coordinates_new));
                        if (index != -1)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        if (GetComponent<PieceKing>() != null)
        {
            for (int j = 0; j < 8; j++)
            {
                switch (j)
                {
                    case 0:
                        coordinates_new = coordinates + new Vector2(0, 1);
                        break;
                    case 1:
                        coordinates_new = coordinates + new Vector2(1, 1);
                        break;
                    case 2:
                        coordinates_new = coordinates + new Vector2(1, 0);
                        break;
                    case 3:
                        coordinates_new = coordinates + new Vector2(1, -1);
                        break;
                    case 4:
                        coordinates_new = coordinates + new Vector2(0, -1);
                        break;
                    case 5:
                        coordinates_new = coordinates + new Vector2(-1, -1);
                        break;
                    case 6:
                        coordinates_new = coordinates + new Vector2(-1, 0);
                        break;
                    case 7:
                        coordinates_new = coordinates + new Vector2(-1, 1);
                        break;
                }
                index = GameSystem.coordinates.IndexOf(BoardToWorld(coordinates_new));
                if (((index == -1) || (index != -1) && GameSystem.pieces[index].GetComponent<ColorWhite>() != GetComponent<ColorWhite>() && GameSystem.pieces[index].GetComponent<ColorBlack>() != GetComponent<ColorBlack>()) && coordinates_new.x >= 1 && coordinates_new.x <= 8 && coordinates_new.y >= 1 && coordinates_new.y <= 8)
                {
                    moves.Add(BoardToWorld(coordinates_new));
                }
            }
            if (!GetComponent<PieceKing>().moved)
            {
                if (!GameSystem.coordinates.Contains(BoardToWorld(coordinates + new Vector2(1, 0))) && !GameSystem.coordinates.Contains(BoardToWorld(coordinates + new Vector2(2, 0))) && GameSystem.coordinates.Contains(BoardToWorld(coordinates + new Vector2(3, 0))))
                {
                    index = GameSystem.coordinates.IndexOf(BoardToWorld(coordinates + new Vector2(3, 0)));
                    if ((GameSystem.pieces[index].GetComponent<ColorWhite>()==GetComponent<ColorWhite>()|| GameSystem.pieces[index].GetComponent<ColorBlack>() == GetComponent<ColorBlack>()) && GameSystem.pieces[index].GetComponent<PieceRook>() && !GameSystem.pieces[index].GetComponent<PieceRook>().moved)
                    {
                        moves.Add(BoardToWorld(coordinates + new Vector2(2, 0)));
                    }
                }
                if (!GameSystem.coordinates.Contains(BoardToWorld(coordinates + new Vector2(-1, 0))) && !GameSystem.coordinates.Contains(BoardToWorld(coordinates + new Vector2(-2, 0))) && !GameSystem.coordinates.Contains(BoardToWorld(coordinates + new Vector2(-3, 0))) && GameSystem.coordinates.Contains(BoardToWorld(coordinates + new Vector2(-4, 0))))
                {
                    index = GameSystem.coordinates.IndexOf(BoardToWorld(coordinates + new Vector2(-4, 0)));
                    if ((GameSystem.pieces[index].GetComponent<ColorWhite>() == GetComponent<ColorWhite>() || GameSystem.pieces[index].GetComponent<ColorBlack>() == GetComponent<ColorBlack>()) && GameSystem.pieces[index].GetComponent<PieceRook>() && !GameSystem.pieces[index].GetComponent<PieceRook>().moved)
                    {
                        moves.Add(BoardToWorld(coordinates + new Vector2(-2, 0)));
                    }
                }
            }
        }
    }
    public static float WorldToBoard(float coordinates)
    {
        return coordinates * 0.5f + 4.5f;
    }
    public static Vector2 WorldToBoard(Vector2 coordinates)
    {
        return new Vector2(coordinates.x * 0.5f + 4.5f, coordinates.y * 0.5f + 4.5f);
    }
    public static Vector3 WorldToBoard(Vector3 coordinates)
    {
        return new Vector3(coordinates.x * 0.5f + 4.5f, coordinates.y * 0.5f + 4.5f, coordinates.z);
    }
    public static float BoardToWorld(float coordinates)
    {
        return (coordinates - 4.5f) * 2f;
    }
    public static Vector2 BoardToWorld(Vector2 coordinates)
    {
        return new Vector2((coordinates.x - 4.5f) * 2f, (coordinates.y - 4.5f) * 2f);
    }
    public static Vector3 BoardToWorld(Vector3 coordinates)
    {
        return new Vector3((coordinates.x - 4.5f) * 2f, (coordinates.y - 4.5f) * 2f, coordinates.z);
    }
    public void KingInCheck(List<Vector2> moves)
    {
        moves_list_saved.Clear();
        foreach (GameObject go in GameSystem.pieces)
        {
            if (go.GetComponent<ColorWhite>() != GetComponent<ColorWhite>() && go.GetComponent<ColorBlack>() != GetComponent<ColorBlack>())
            {
                moves_list_saved.Add(go.GetComponent<LegalMoves>().moves);
            }
        }
        pos = new Vector2(transform.position.x, transform.position.y);
        foreach (Vector2 move in moves.ToList())
        {
            transform.position = new Vector3(move.x, move.y, 0);
            index = GameSystem.coordinates.IndexOf(new Vector2(transform.position.x, transform.position.y));
            if (index != -1)
            {

                savedPiece = GameSystem.pieces[index];
                moves_saved = savedPiece.GetComponent<LegalMoves>().moves;
                Destroy(savedPiece);
                GameSystem.coordinates.RemoveAt(index);
                GameSystem.pieces.RemoveAt(index);
                savedPieceDestroyed = true;
            }
            GameSystem.coordinates[GameSystem.coordinates.IndexOf(pos)] = new Vector2(transform.position.x, transform.position.y);
            foreach (GameObject go in GameSystem.pieces)
            {
                if (go.GetComponent<ColorWhite>() != GetComponent<ColorWhite>() && go.GetComponent<ColorBlack>() != GetComponent<ColorBlack>())
                {
                    go.GetComponent<LegalMoves>().CalculateMoves();
                }

            }
            for (int i = 0; i < GameSystem.pieces.Count; i++)
            {
                if (GameSystem.pieces[i].GetComponent<PieceKing>() != null && ((GameSystem.pieces[i].GetComponent<ColorWhite>() == GetComponent<ColorWhite>()) || (GameSystem.pieces[i].GetComponent<ColorBlack>() == GetComponent<ColorBlack>())))
                {

                    foreach (GameObject go in GameSystem.pieces)
                    {
                        if (go.GetComponent<LegalMoves>().moves.Contains(GameSystem.coordinates[i]))
                        {
                            if (moves.Contains(move))
                            {
                                moves.Remove(move);
                            }
                        }
                    }
                }
            }
            GameSystem.coordinates[GameSystem.coordinates.IndexOf(new Vector2(transform.position.x, transform.position.y))] = new Vector2(pos.x, pos.y);
            transform.position = new Vector3(pos.x, pos.y, 0);
            if (savedPieceDestroyed)
            {
                MonoBehaviour[] comps = savedPiece.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour c in comps)
                {
                    c.enabled = true;
                }
                Instantiate(savedPiece, new Vector3(move.x, move.y, 0), Quaternion.identity);
                
                GameSystem.coordinates.Add(new Vector2(move.x, move.y));
                GameSystem.pieces.Add(savedPiece);
                moves_list_saved.Add(moves_saved);
                print("W");
            }
            savedPieceDestroyed = false;
        }
        int k = 0;
        foreach (GameObject go in GameSystem.pieces)
        {
            if (go.GetComponent<ColorWhite>() != GetComponent<ColorWhite>() && go.GetComponent<ColorBlack>() != GetComponent<ColorBlack>())
            {
                go.GetComponent<LegalMoves>().moves = moves_list_saved[k];
                k++;
            }
        }
        GameSystem.UpdateTurn();

    }
    
}
