using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sprict.Field
{
    public class Field
    {
        public List<Piece> Player2;
        public List<Piece> Player1;
        public int[,] IDs;

        public Field()
        {
            Player1 = new List<Piece>();
            Player2 = new List<Piece>();
            IDs = new int[3, 5] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };
        }
    }
}
