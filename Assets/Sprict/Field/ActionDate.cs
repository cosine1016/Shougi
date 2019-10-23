using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Sprict.Field
{
    public class ActionDate
    {

        //id 管理番号
        //MoveOrTurn 行動の種類(MOT = 0:移動)(MOT = 1:回転)
        //Move 行動の詳細(移動:移動先の座標(x,y))
        //turn 行動の詳細(回転: 1 | -1 で右回転|左回転)
        public int ID;
        public int MoveOrTurn;
        public int MoveX;
        public int MoveY;
        public int Turn;

        public ActionDate(int _id, int _mot, int _turn)
        {
            ID = _id;
            MoveOrTurn = _mot;
            Turn = _turn;
        }
        
        public ActionDate(int _id, int _mot, int _x, int _y)
        {
            ID = _id;
            MoveOrTurn = _mot;
            MoveX = _x;
            MoveY = _y;
        }

        public ActionDate()
        {

        }

        public ActionDate Clone()
        {
            return (ActionDate)MemberwiseClone();
        }
    }
}
