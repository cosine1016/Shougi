using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sprict.Field
{
    public class Field
    {
        public List<Piece> Player1;
        public List<Piece> Player2;
        public int[,] IDs;
        public int TurnSide;
        public int TurnNumber;
        public bool isEnd;

        public Field()
        {
            Player1 = new List<Piece>();
            Player2 = new List<Piece>();
            IDs = new int[3, 5] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };
            TurnNumber++;
        }

        public Field Clone()
        {
            var ids = new int[3, 5];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    ids[i, j] = IDs[i, j];
                }
            }
            return new Field()
            {
                Player1 = Player1.Select(p => p.Clone()).ToList(),
                Player2 = Player2.Select(p => p.Clone()).ToList(),
                IDs = ids,
                TurnSide = TurnSide,
                isEnd = isEnd
            };
        }

        public double Score(int TurnSide)
        {
            double score1 = Player1.Count(p => !p.isDeath);
            double score2 = Player2.Count(p => !p.isDeath);
            double scoreMin1 = PieceController.PlayerActionList(this, 1).Count;
            double scoreMin2 = PieceController.PlayerActionList(this, 2).Count;
            score1 += scoreMin1 / 10;
            score2 += scoreMin2 / 10;
            return TurnSide == 1 ? score1 - score2 : score2 - score1;
        }

        public void InitilizedRandomGame()
        {
            int PieceNumber;
            int PieceKind;
            int Rotate;
            int PiecePos;
            int PiecePosX = 0;
            int PiecePosY = 0;
            int IDsetter = 1;
            System.Random rnd = new System.Random();
            PieceNumber = 3 + rnd.Next(2);
            PieceController.PieceSpown(this, 1, IDsetter++, 0);
            PieceController.PieceSpown(this, 2, IDsetter++, 0);
            for (int i = 0; i < PieceNumber; i++)
            {
                PieceKind = rnd.Next(10) + 1;
                Rotate = rnd.Next(4);
                PieceController.PieceSpown(this, 1, IDsetter, PieceKind);
                PieceController.PieceRotate(this, IDsetter++, Rotate);
                PieceController.PieceSpown(this, 2, IDsetter, PieceKind);
                PieceController.PieceRotate(this, IDsetter++, Rotate + 2);
            }
            IDsetter = 1;
            PieceController.PieceSet(this, IDsetter++, 1, 4);
            PieceController.PieceSet(this, IDsetter++, 1, 0);
            for (int i = 0; i < PieceNumber; i++)
            {
                while (true)
                {
                    PiecePos = rnd.Next(5);
                    switch (PiecePos)
                    {
                        case 0:
                            PiecePosX = 0;
                            PiecePosY = 4;
                            break;
                        case 1:
                            PiecePosX = 0;
                            PiecePosY = 3;
                            break;
                        case 2:
                            PiecePosX = 1;
                            PiecePosY = 3;
                            break;
                        case 3:
                            PiecePosX = 2;
                            PiecePosY = 3;
                            break;
                        case 4:
                            PiecePosX = 2;
                            PiecePosY = 4;
                            break;
                    }
                    if (this.IDs[PiecePosX, PiecePosY] == 0) break;
                }

                PieceController.PieceSet(this, IDsetter++, PiecePosX, PiecePosY);
                PieceController.PieceSet(this, IDsetter++, 2 - PiecePosX, 4 - PiecePosY);
            }

            TurnSide = rnd.Next(2) + 1;
        }

        public int Action(ActionDate action)
        {
            Piece CurrentPiece = PieceController.PieceFromID(this, action.ID);
            if (action.MoveOrTurn == 0)
            {
                int enemyid = IDs[action.MoveX, action.MoveY];
                if (enemyid > 0)
                {
                    PieceController.PieceDeath(this, enemyid);
                }
                PieceController.PieceSet(this, CurrentPiece.ID, action.MoveX, action.MoveY);
                if (JudgeWinner() != 0)
                {
                    isEnd = true;
                }
            }
            else
            {
                PieceController.PieceRotate(this, CurrentPiece.ID, action.Turn);
            }
            ChangeSide();
            return JudgeWinner();
        }

        public void ChangeSide()
        {
            TurnSide %= 2;
            TurnSide++;
            TurnNumber++;
        }

        public int JudgeWinner()
        {
            if (PieceController.PieceFromID(this, 1).isDeath)
            {
                isEnd = true;
                return 2;
            }
            if (PieceController.PieceFromID(this, 2).isDeath)
            {
                isEnd = true;
                return 1;
            }

            if (Player1.Where(item => item.ID > 2).Any(item => !item.isDeath))
            {
                return 0;
            }
            if (Player2.Where(item => item.ID > 2).Any(item => !item.isDeath))
            {
                return 0;
            }
            Piece p1 = PieceController.PieceFromID(this, 1), p2 = PieceController.PieceFromID(this, 2);
            int dx = p2.PosX - p1.PosX, dy = p2.PosY - p1.PosY;
            if (dx * dx <= 1 && dy * dy <= 1)
            {
                return 0;
            }
            isEnd = true;
            return 3;
        }
    }
}
