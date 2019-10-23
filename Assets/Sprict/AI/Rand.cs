using System.Collections;
using System.Collections.Generic;
using Assets.Sprict.Field;
using UnityEngine;

namespace Assets.Sprict.AI
{
    public class Rand : IAgent
    {
        List<ActionDate> list;

        public Rand()
        {

        }

        public ActionDate Search(Assets.Sprict.Field.Field field)
        {
            list = PieceController.PlayerActionList(field, 2);
            System.Random r = new System.Random();
            int index = r.Next(list.Count);
            if (list[index].MoveOrTurn == 0)
            {
                return list[index];
            }
            index = r.Next(list.Count);
            return list[index];
        }
    }
}
