using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    [Range(0, .5f)]
    public float opticalAdjustFactor;
    [Range(1,10)]
    public float opticalAdjustPower;
    [Range(0, .5f)]
    public float spacing;
    public Vector2Int startPos;

    public Color selectedCol;
    public Color oneJumpCol;
    public Color minTint;
    public Color maxTint;
    public Piece knight;
    public TextMesh textPrefab;

    Square[,] squares;
    TextMesh[,] squareText;

    Vector2Int posOld;
    Board board;

    void Start()
    {
        CreateBoard();

        knight.OnMoved += OnPieceMoved;
        knight.OnDropped += OnPieceDropped;

        knight.SetPosition(CoordToPos(startPos.x, startPos.y));
		posOld = startPos;

        Draw(startPos);
    }

    void Update()
    {
        //Draw(startPos);
        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleText();
        }
    }

    void CreateBoard()
    {
        squareText = new TextMesh[8, 8];
        squares = new Square[8, 8];

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
				Transform square = GameObject.CreatePrimitive(PrimitiveType.Quad).transform;
                square.position = CoordToPos(i, j);
                square.parent = transform;
                square.localScale = Vector3.one * (1-spacing);

                squares[i,j] = square.gameObject.AddComponent<Square>();
                squares[i, j].SetCol(Color.black);

                TextMesh newText = Instantiate(textPrefab);
                newText.transform.parent = square;
                newText.transform.localPosition = Vector3.forward * -.01f;
                squareText[i, j] = newText;

            }
        }

        board = new Board();
    }

    Vector2 CoordToPos(int x, int y)
    {
        return new Vector3(-3.5f + x, -3.5f + y);
    }

    Vector2Int PosToCoord(Vector2 pos)
    {
        int x = Mathf.Clamp(Mathf.RoundToInt(pos.x + 3.5f),0,7);
        int y = Mathf.Clamp(Mathf.RoundToInt(pos.y + 3.5f), 0, 7);
        return new Vector2Int(x, y);
    }

    void ToggleText()
    {

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                squareText[i, j].gameObject.SetActive(!squareText[i, j].gameObject.activeSelf);
            }
        }
    }

    void Draw(Vector2Int pos)
    {
		board.CalculateDistances(pos);

		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
                float percent = (board.Distances[i, j]-1) / (float)(Board.maxMovesToReachAnySquare-1);
                float smallestNeighbour = float.MaxValue;
                for (int oI = -1; oI <= 1; oI++)
                {
                    for (int oJ = -1; oJ <=1; oJ++)
                    {
                        if (oI == 0 && oJ == 0)
                        {
                            continue;
                        }
                        float neighbour = board.Distances[Mathf.Clamp(i + oI, 0, 7), Mathf.Clamp(j + oJ, 0, 7)];
                        if (neighbour < smallestNeighbour)
                        {
                            smallestNeighbour = neighbour;
                        }
                    }
                }

                float adjust = (5 - smallestNeighbour) * opticalAdjustFactor;
                percent -= Mathf.Pow(adjust,opticalAdjustPower);

                Color distanceTint = Color.Lerp(minTint, maxTint, percent) * Color.white;
                if (board.Distances[i, j] == 1)
                {
                    distanceTint = oneJumpCol;
                }

                squares[i, j].SetTargetCol(distanceTint);
				squareText[i, j].text = board.Distances[i, j].ToString();

                bool pieceOnThisSquare = i == pos.x && j == pos.y;
                if (pieceOnThisSquare)
                {
                    squares[i, j].SetTargetCol(selectedCol);
                    squareText[i, j].text = "";
                }
			}
		}

    }

    void OnPieceMoved(Piece piece)
    {
        Vector2Int hoverCoord = PosToCoord(piece.transform.position);
        if (hoverCoord != posOld)
        {
            posOld = hoverCoord;
            Draw(hoverCoord);
        }
    }

    void OnPieceDropped(Piece piece)
    {
        Vector2Int coord = PosToCoord(piece.transform.position);
        piece.SetPosition(CoordToPos(coord.x,coord.y));
    }
}
