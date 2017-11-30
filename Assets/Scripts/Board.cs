using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board {

    public const int maxMovesToReachAnySquare = 6;

    public int[,] Distances { get; private set; }

    public void CalculateDistances(Vector2Int knightPos)
    {
        Distances = new int[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Distances[i, j] = 99;
            }
        }

        Node startNode = new Node(0, knightPos, Distances);
        startNode.Propagate();
    }


    public class Node
    {
        int depth;
        Vector2Int pos;
        int[,] distances;
        List<Vector2Int> validMoves;

        public Node(int depth, Vector2Int pos, int[,] distances)
        {
            this.depth = depth;
            this.pos = pos;
            this.distances = distances;
            validMoves = new List<Vector2Int>();
        }

        public void Propagate()
        {
			AddMove(2, -1);
			AddMove(2, 1);
			AddMove(-2, 1);
			AddMove(-2, -1);

			AddMove(-1, 2);
			AddMove(1, 2);
            AddMove(-1, -2);
			AddMove(1, -2);

            foreach (Vector2Int newPos in validMoves)
            {
                Node node = new Node(depth + 1, newPos, distances);
                node.Propagate();
            }
        }

        void AddMove(int dx, int dy)
        {
            int x = pos.x + dx;
            int y = pos.y + dy;
            if (x >= 0 && x < 8 && y >= 0 && y < 8)
            {
                if (distances[x, y] > depth + 1)
                {
                    distances[x, y] = depth + 1;
                    if (depth + 1 < maxMovesToReachAnySquare)
                    {
                        validMoves.Add(new Vector2Int(x, y));
                    }

                }
            }
        }

    }

}
