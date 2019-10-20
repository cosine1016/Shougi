using System.Collections;
using System.Collections.Generic;
using Assets.Sprict.Field;
using UnityEngine;

namespace Assets.Sprict.AI
{
    public class Rand
    {
        List<ActionDate> list;

        public Rand()
        {

        }

        public ActionDate Return(Assets.Sprict.Field.Field field)
        {
            list = PieceController.PlayerActionList(field, 2);
            System.Random r = new System.Random();
            int index = r.Next(list.Count);
            return list[index];
        }

    }
}
