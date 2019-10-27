using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Sprict.AI
{
    class SearchList : IEnumerable<SearchData>
    {
        private List<SearchData> data;

        public int Count 
        {
            get => data.Count;
        }

        public SearchList()
        {
            data = new List<SearchData>();
        }

        public SearchList(SearchData item)
        {
            data = new List<SearchData> {item};
        }

        public void Push(SearchData item)
        {
            data.Add(item);
            data = data.OrderBy(c => c.score).ToList();
        }

        public SearchData PopLow()
        {
            var res = data[0];
            data.RemoveAt(0);
            return res;
        }

        public SearchData PopHigh()
        {
            var res = data.Last();
            data.RemoveAt(data.Count - 1);
            return res;
        }

        public List<SearchData> PopLowList(int num)
        {
            return data;
//            var list = new List<SearchData>();
//            if (num > data.Count) num = data.Count;
//            for (int i = 0; i < num; i++) list.Add(PopLow());
//            return list;
        }

        public List<SearchData> PopHighList(int num)
        {
            return data;
//            var list = new List<SearchData>();
//            if (num > data.Count) num = data.Count;
//            for (int i = 0; i < num; i++) list.Add(PopHigh());
//            return list;
        }

        public IEnumerator<SearchData> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}