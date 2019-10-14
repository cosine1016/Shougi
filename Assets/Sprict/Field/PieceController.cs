using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sprict.Field
{
    public static class PieceController
    {
        static List<bool[]> CanMoveDirecInit; //駒の種類ごとのうごける方向をboolで記憶
        static int[] divX; //うごく方向と座標の対応を記憶
        static int[] divY;

        //CanMoveDirecInitの対応
        //      上
        // [7] [0] [1]
        // [6] [駒] [2]
        // [5] [4] [3]
        public static void init()
        {
            CanMoveDirecInit = new List<bool[]>();
            CanMoveDirecInit.Add(new bool[8] { true, true, true, true, true, true, true, true });
            CanMoveDirecInit.Add(new bool[8] { true, false, false, false, false, false, false, false });
            CanMoveDirecInit.Add(new bool[8] { true, false, false, false, true, false, false, false });
            CanMoveDirecInit.Add(new bool[8] { true, false, false, true, false, true, false, false });
            CanMoveDirecInit.Add(new bool[8] { true, false, true, true, false, true, true, false });
            CanMoveDirecInit.Add(new bool[8] { true, true, false, true, false, true, false, false });
            CanMoveDirecInit.Add(new bool[8] { false, true, false, true, false, true, false, true });
            CanMoveDirecInit.Add(new bool[8] { true, true, false, false, false, false, false, true });
            CanMoveDirecInit.Add(new bool[8] { true, false, false, true, true, true, false, false });
            CanMoveDirecInit.Add(new bool[8] { true, false, true, false, true, false, true, false });
            CanMoveDirecInit.Add(new bool[8] { true, true, false, true, true, true, false, true });
            divX = new int[8] { 0, 1, 1, 1, 0, -1, -1, -1 };
            divY = new int[8] { -1, -1, 0, 1, 1, 1, 0, -1 };
        }

        /// <summary>
        /// 駒をフィールドに生成
        /// 生成された駒は設置されておらず座標(-1,-1)にいる
        /// </summary>
        /// <param name="field">対象とするフィールド</param>
        /// <param name="side">Player1 = 1, Player2 = 2 </param>
        /// <param name="id">駒の管理番号</param>
        /// <param name="kind">駒の種類</param>
        /// <return> 変更後のフィールド</return>
        public static Field PieceSpown(Field field, int side, int id, int kind)
        {
            if (side == 1)
            {
                field.Player1.Add(new Piece(id, side, kind, CanMoveDirecInit[kind]));
            }
            else
            {
                field.Player2.Add(new Piece(id, side, kind, CanMoveDirecInit[kind]));
            }

            return field;
        }

        /// <summary>
        /// 駒を回転　(turn = 1:右回転)(turn = -1:左回転) 
        /// </summary>
        /// <param name="field">対象とするフィールド</param>
        /// <param name="id">駒の管理番号</param>
        /// <param name="turn">回転方向(turn = 1:右回転)(turn = -1:左回転) </param>
        /// <return> 変更後のフィールド</return>
        public static Field PieceRotate(Field field, int id, int turn)
        {
            Piece piece = PieceFromID(field, id);
            piece.Angle += turn;
            if (piece.Angle < 0) piece.Angle += 4;
            if (piece.Angle > 3) piece.Angle -= 4;
            return field;
        }

        /// <summary>
        /// 駒を死亡させる
        /// </summary>
        /// <param name="field">対象とするフィールド</param>
        /// <param name="id">駒の管理番号</param>
        /// <return> 変更後のフィールド</return>
        public static Field PieceDeath(Field field, int id)
        {
            Piece piece = PieceFromID(field, id);
            piece.isDeath = true ;
            return field;
        }

        /// <summary>
        /// 駒を座標(x,y)に配置（移動）
        /// </summary>
        /// <param name="field">対象とするフィールド</param>
        /// <param name="id">駒の管理番号</param>
        /// <param name="x">座標x</param>
        /// <param name="y">座標y</param>
        /// <return> 変更後のフィールド</return>
        public static Field PieceSet(Field field, int id, int x, int y)
        {
            Piece piece = PieceFromID(field, id);
            piece.PosX = x;
            piece.PosY = y;
            piece.isExist = true;                
            field.IDs[x, y] = id;
            return field;
        }

        /// <summary>
        /// プレイヤーがおこなえるすべての動作を返す
        /// </summary>
        /// <param name="field">対象とするフィールド</param>
        /// <param name="side">Player1 = 1, Player2 = 2 </param>
        /// <return> プレイヤーがおこなえるすべての動作</return>
        public static List<ActionDate> PlayerActionList(Field field, int side)
        {
            List<ActionDate> ret = new List<ActionDate>();
            List<Piece> Player = new List<Piece>();
            if (side == 1)
            {
                Player = field.Player1;
            }
            else
            {
                Player = field.Player2;
            }

            foreach(Piece item in Player)
            {
                ret.AddRange(PieceActionList(field, item));
            }

            return ret;
        }



        /// <summary>
        /// 駒が行える全ての動作を返す
        /// </summary>
        /// <param name="field">対象とするフィールド</param>
        /// <param name="piece">対象とする駒</param>
        /// <return> 駒が行える全ての動作</return>
        public static List<ActionDate> PieceActionList(Field field, Piece piece)
        {
            List<ActionDate> ret = new List<ActionDate>();
            ret.Add(new ActionDate(piece.ID, 1, 1));
            ret.Add(new ActionDate(piece.ID, 1, -1));
            for (int i = 0; i < 8; i++)
            {
                int nextX = piece.PosX + divX[i];
                int nextY = piece.PosY + divX[i];
                if(PieceCanMoveJudge(field, piece.ID, piece.Side, nextX, nextY))
                {
                    ret.Add(new ActionDate(piece.ID, 0, nextX, nextY));
                }
            }

            return ret;
        }


        /// <summary>
        /// 駒が指定された座標に移動できるか判定
        /// </summary>
        /// <param name="field">対象とするフィールド</param>
        /// <param name="id">駒の管理番号</param>
        /// <param name="side">Player1 = 1, Player2 = 2</param>
        /// <param name="x">移動する座標x</param>
        /// <param name="y">移動する座標y</param>
        /// <return> 移動できるかどうか</return>
        public static bool PieceCanMoveJudge(Field field, int id, int side, int x, int y)
        {
            Piece piece = PieceFromID(field, id);
            List<Piece> Player = new List<Piece>();
            if (side == 1)
            {
                Player = field.Player1;
            }
            else
            {
                Player = field.Player2;
            }
            if (piece.ID == -1) return false;

            bool[] direc = PieceDirecOnTurn(piece);
            int dX = x - piece.PosX;
            int dY = y - piece.PosY;
            bool flag = false;
            for(int i = 0; i < 8; i++)
            {
                if (direc[i])
                {
                    if (divX[i] == dX && divY[i] == dY) flag = true;
                }
            }
            if (!flag) return false;

            if (x < 0 || x > 2) return false;
            if (y < 0 || y > 4) return false;

            foreach (Piece item in Player)
            {
                if (x == item.PosX && y == item.PosY)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 駒がうごける方向を現在の回転を加味して返す
        /// </summary>
        /// <param name="piece">対象となる駒</param>
        /// <return> 現在のうごける方向</return>
        public static bool[] PieceDirecOnTurn(Piece piece)
        {
            bool[] ret = new bool[8];
            if (piece.Angle == 0) { 
                ret = piece.CanMoveDirec;
            }
            else { 
                for(int i = 0; i < 8; i++)
                {

                    if (i < piece.Angle * 2)
                    { 
                        ret[i] = piece.CanMoveDirec[i + 8 - piece.Angle*2];
                    }
                    else
                    {
                        ret[i] = piece.CanMoveDirec[i - piece.Angle*2];
                    }
                }

            }

            return ret;
        }

        /// <summary>
        /// IDで参照して駒を返す
        /// </summary>
        /// <param name="field">対象のフィールド</param>
        /// <param name="id">管理番号</param>
        /// <return> 指定された駒</return>
        public static Piece PieceFromID(Field field, int id)
        {
            Piece piece = new Piece(-1, -1, -1, new bool[0]);
            foreach (Piece item in field.Player1)
            {
                if (item.ID == id)
                {
                    piece = item;
                    break;
                }
            }
            foreach (Piece item in field.Player2)
            {
                if (item.ID == id)
                {
                    piece = item;
                    break;
                }
            }
            return piece;
        }
    }

}
