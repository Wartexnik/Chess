using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovePiece : MonoBehaviour
{
    private bool isDragging;
    public Vector2 pos;
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;
    public GameObject moveHint;
    public GameObject squareBorder;
    public GameObject faded;
    public GameObject promotion_w;
    public GameObject promotion_b;
    public GameSystem gameSystem;
    float time;
    Vector3 positionSnap;
    List<GameObject> hintClones = new List<GameObject>();
    public int index;
    public void OnMouseDown()
    {
        pos = new Vector2(transform.position.x, transform.position.y);
        foreach (Vector2 move in gameObject.GetComponent<LegalMoves>().moves)
        {
            hintClones.Add(Instantiate(moveHint, new Vector3(move.x, move.y, -2), Quaternion.identity));
            moveHint.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
        faded = Instantiate(gameObject, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        faded.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        transform.localScale = new Vector2 (1.2f, 1.2f);
        GetComponent<SpriteRenderer>().sortingOrder = 3;
        isDragging = true;
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }
    public void OnMouseUp()
    {
        squareBorder.transform.position = new Vector3(-26, 11, 0);
        foreach (GameObject clone in hintClones)
        {
            Destroy(clone);
        }
        Destroy(faded);
        transform.localScale = new Vector2(1f, 1f);
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        isDragging = false;
        transform.position = positionSnap;
        if (GetComponent<LegalMoves>().moves.Contains(new Vector2(transform.position.x, transform.position.y)))
        {
            if ((GetComponent<PiecePawn>()!=null)&&Mathf.Abs(transform.position.y-pos.y)==4)
            {
                if (GetComponent<ColorWhite>() != null)
                {
                    GameSystem.enpassant = new Vector2(transform.position.x, transform.position.y-2);
                    GameSystem.enpassant_color = 0;
                }
                if (GetComponent<ColorBlack>() != null)
                {
                    GameSystem.enpassant = new Vector2(transform.position.x, transform.position.y+2);
                    GameSystem.enpassant_color = 1;
                }
            }
            if ((GetComponent<PiecePawn>()!=null)&&transform.position.x==GameSystem.enpassant.x && transform.position.y == GameSystem.enpassant.y)
            {
                audioSource.PlayOneShot(audioClipArray[1]);
                if (GetComponent<ColorWhite>() != null)
                {
                    index = GameSystem.coordinates.IndexOf(new Vector2(transform.position.x, transform.position.y-2));
                }
                if (GetComponent<ColorBlack>() != null)
                {
                    index = GameSystem.coordinates.IndexOf(new Vector2(transform.position.x, transform.position.y+2));
                }
                Destroy(GameSystem.pieces[index]);
                GameSystem.coordinates.RemoveAt(index);
                GameSystem.pieces.RemoveAt(index);
            }
            if ((GetComponent<PiecePawn>() != null)&&(transform.position.y==LegalMoves.BoardToWorld(8)||transform.position.y==LegalMoves.BoardToWorld(1)))
            {
                if (GetComponent<ColorWhite>() != null)
                {
                    promotion_w.transform.position = new Vector3(transform.position.x, LegalMoves.BoardToWorld(6.5f), -4);
                }
                if (GetComponent<ColorBlack>() != null)
                {
                    promotion_b.transform.position = new Vector3(transform.position.x, LegalMoves.BoardToWorld(2.5f), -4);
                }
                StartCoroutine(Promote(gameObject));
            }
            if ((GetComponent<PieceKing>() != null) && Mathf.Abs(transform.position.x-pos.x)>=4)
            {
                if (transform.position.x>pos.x)
                {
                    Vector3 temp_pos_old = new Vector3(transform.position.x + 2, transform.position.y);
                    Vector3 temp_pos_new = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
                    GameSystem.pieces[GameSystem.coordinates.IndexOf(temp_pos_old)].GetComponent<MovePiece>().pos = temp_pos_new;
                    GameSystem.pieces[GameSystem.coordinates.IndexOf(temp_pos_old)].transform.position = temp_pos_new;
                    GameSystem.coordinates[GameSystem.coordinates.IndexOf(temp_pos_old)] = temp_pos_new;
                }
                if (transform.position.x<pos.x)
                {
                    Vector3 temp_pos_old = new Vector3(transform.position.x - 4, transform.position.y);
                    Vector3 temp_pos_new = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
                    GameSystem.pieces[GameSystem.coordinates.IndexOf(temp_pos_old)].GetComponent<MovePiece>().pos = temp_pos_new;
                    GameSystem.pieces[GameSystem.coordinates.IndexOf(temp_pos_old)].transform.position = temp_pos_new;
                    GameSystem.coordinates[GameSystem.coordinates.IndexOf(temp_pos_old)] = temp_pos_new;
                }
            }
            GameSystem.turn += 1;
            GameSystem.UpdateTurn();
            index = GameSystem.coordinates.IndexOf(new Vector2(transform.position.x, transform.position.y));
            if (index != -1)
            {
                Destroy(GameSystem.pieces[index]);
                GameSystem.coordinates.RemoveAt(index);
                GameSystem.pieces.RemoveAt(index);
                audioSource.PlayOneShot(audioClipArray[1]);
            }
            else
            {
                audioSource.PlayOneShot(audioClipArray[0]);
            }
            GameSystem.coordinates[GameSystem.coordinates.IndexOf(pos)] = new Vector2(transform.position.x, transform.position.y);
            
            if (gameObject.GetComponent<PiecePawn>() != null)
            {
                gameObject.GetComponent<PiecePawn>().moved = true;
            }
            if (gameObject.GetComponent<PieceKing>() != null)
            {
                gameObject.GetComponent<PieceKing>().moved = true;
            }
            if (gameObject.GetComponent<PieceRook>() != null)
            {
                gameObject.GetComponent<PieceRook>().moved = true;
            }
            foreach (GameObject go in GameSystem.pieces)
            {
                go.GetComponent<LegalMoves>().CalculateMoves();
                //if (go.GetComponent<ColorWhite>() != GetComponent<ColorWhite>() && go.GetComponent<ColorBlack>() != GetComponent<ColorBlack>())
                //    go.GetComponent<LegalMoves>().KingInCheck(go.GetComponent<LegalMoves>().moves);
            }
        }
        else
        {
            transform.position = new Vector3(pos.x, pos.y, 0);
        }
    }
    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
            positionSnap = new Vector3(2 * Mathf.Floor(transform.position.x / 2) + 1, 2 * Mathf.Floor(transform.position.y / 2) + 1, 0);
            if (LegalMoves.WorldToBoard(positionSnap).x>=1 && LegalMoves.WorldToBoard(positionSnap).x <= 8 && LegalMoves.WorldToBoard(positionSnap).y >= 1 && LegalMoves.WorldToBoard(positionSnap).y <= 8)
            {
                squareBorder.transform.position = positionSnap;
            }
            else
            {
                squareBorder.transform.position = new Vector3(-26, 11, 0);
            }
        }
    }
    IEnumerator Promote(GameObject pawn)
    {
        while (ChoosePromotion.promotion == 0)
        {
            yield return null;
        }
        promotion_w.transform.position = new Vector3(-38, 2, -4);
        promotion_b.transform.position = new Vector3(-32, 2, -4);
        index = GameSystem.pieces.IndexOf(pawn);
        if (pawn.GetComponent<ColorWhite>()!=null)
        {
            if (ChoosePromotion.promotion == 1)
                GameSystem.pieces[index] = Instantiate(gameSystem.GetComponent<GameSystem>().queen_w, new Vector3(pawn.transform.position.x, pawn.transform.position.y, 0), Quaternion.identity);
            if (ChoosePromotion.promotion == 2)
                GameSystem.pieces[index] = Instantiate(gameSystem.GetComponent<GameSystem>().knight_w, new Vector3(pawn.transform.position.x, pawn.transform.position.y, 0), Quaternion.identity);
            if (ChoosePromotion.promotion == 3)
                GameSystem.pieces[index] = Instantiate(gameSystem.GetComponent<GameSystem>().rook_w, new Vector3(pawn.transform.position.x, pawn.transform.position.y, 0), Quaternion.identity);
            if (ChoosePromotion.promotion == 4)
                GameSystem.pieces[index] = Instantiate(gameSystem.GetComponent<GameSystem>().bishop_w, new Vector3(pawn.transform.position.x, pawn.transform.position.y, 0), Quaternion.identity);
        }
        if (pawn.GetComponent<ColorBlack>() != null)
        {
            if (ChoosePromotion.promotion == 1)
                GameSystem.pieces[index] = Instantiate(gameSystem.GetComponent<GameSystem>().queen_b, new Vector3(pawn.transform.position.x, pawn.transform.position.y, 0), Quaternion.identity);
            if (ChoosePromotion.promotion == 2)
                GameSystem.pieces[index] = Instantiate(gameSystem.GetComponent<GameSystem>().knight_b, new Vector3(pawn.transform.position.x, pawn.transform.position.y, 0), Quaternion.identity);
            if (ChoosePromotion.promotion == 3)
                GameSystem.pieces[index] = Instantiate(gameSystem.GetComponent<GameSystem>().rook_b, new Vector3(pawn.transform.position.x, pawn.transform.position.y, 0), Quaternion.identity);
            if (ChoosePromotion.promotion == 4)
                GameSystem.pieces[index] = Instantiate(gameSystem.GetComponent<GameSystem>().bishop_b, new Vector3(pawn.transform.position.x, pawn.transform.position.y, 0), Quaternion.identity);
        }
        Destroy(pawn);
        GameSystem.pieces[index].GetComponent<LegalMoves>().CalculateMoves();
        GameSystem.pieces[index].GetComponent<MovePiece>().pos = new Vector2(GameSystem.pieces[index].transform.position.x, GameSystem.pieces[index].transform.position.y);
        ChoosePromotion.promotion = 0;
        GameSystem.UpdateTurn();
    }
}
