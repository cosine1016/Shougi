using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sprict.Field
{
    public class Piece
    {
        public int PosX;
        public int PosY;
        public int ID;
        public int Angle;
        public int Kind;
        public int Side;
        public bool[] CanMoveDirec;
        public bool isExist;
        public bool isDeath;

        public Piece(int _ID, int _side, int _kind, bool[] _cmv)
        {
            PosX = -1;
            PosY = -1;
            ID = _ID;
            Kind = _kind;
            Side = _side;
            Angle = 0;
            CanMoveDirec = _cmv;
            isExist = false;
            isDeath = false;
        }

    }
}