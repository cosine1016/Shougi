using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Sprict.Field;

namespace Assets.Sprict.AI
{
    class SearchData : IComparable<SearchData>
    {
        public double score = 0;
        public Field.Field field;

        public SearchData(Field.Field _field)
        {
            field = _field;
        }

        public SearchData(Field.Field _field, double _score)
        {
            field = _field;
            score = _score;
        }

        public int CompareTo(SearchData data)
        {
            if (score < data.score) return -1;
            else if (score > data.score) return 1;
            else return 0;
        }
    }
}
