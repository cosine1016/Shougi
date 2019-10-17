using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sprict.Field
{
    public class FieldController
    {
        public Field field;
        public int TurnSide;
        static System.Random rnd;

        public FieldController()
        {
            rnd = new System.Random();
        }

        public void InitilizedRandomGame()
        {
            field = new Field();

            int PieceNumber;
            int PieceKind;
            int Rotate;
            int PiecePos;
            int PiecePosX = 0;
            int PiecePosY = 0;
            int IDsetter = 1;
            PieceNumber = 3 + rnd.Next(2);
            field = PieceController.PieceSpown(field, 1, IDsetter++, 0);
            field = PieceController.PieceSpown(field, 2, IDsetter++, 0);
            for (int i = 0; i < PieceNumber; i++)
            {
                PieceKind = rnd.Next(10) + 1;
                Rotate = rnd.Next(4);
                field = PieceController.PieceSpown(field, 1, IDsetter, PieceKind);
                field = PieceController.PieceRotate(field, IDsetter++, Rotate);
                field = PieceController.PieceSpown(field, 2, IDsetter, PieceKind);
                field = PieceController.PieceRotate(field, IDsetter++, Rotate+2);
            }
            IDsetter = 1;
            field = PieceController.PieceSet(field, IDsetter++, 1, 4);
            field = PieceController.PieceSet(field, IDsetter++, 1, 0);
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
                    if (field.IDs[PiecePosX, PiecePosY] == 0) break;
                }

                field = PieceController.PieceSet(field, IDsetter++, PiecePosX, PiecePosY);
                field = PieceController.PieceSet(field, IDsetter++, 2 - PiecePosX, 4 - PiecePosY);
            }

            TurnSide = rnd.Next(2) + 1 ;
        }

        public void ChangeSide()
        {
            TurnSide %= 2;
            TurnSide++;            
        }

        public int JudgeWinner()
        {
            if (PieceController.PieceFromID(field, 0).isDeath)
            {
                return 2;
            }
            if(PieceController.PieceFromID(field, 1).isDeath)
            {
                return 1;
            }


            return 0;
        }
    }
}
