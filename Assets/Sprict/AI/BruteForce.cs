using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Sprict.Field;

namespace Assets.Sprict.AI
{
    class BruteForce : IAgent
    {
        private int depth;
        private int numAdoptNode;

        public BruteForce(int _depth, int _numAdoptNode)
        {
            depth = _depth - 1;
            numAdoptNode = _numAdoptNode;
        }

        public ActionDate Search(Field.Field field)
        {
            List<ActionDate> actionable = PieceController.PlayerActionList(field, field.TurnSide);
            var tasks = actionable.Select((a, i) => NodeSearch(field.Clone(), a, i));
            var t = Task.WhenAll(tasks);
            t.Wait();
            UnityEngine.Debug.Log(t.Result.OrderBy(p => p.Item2).Last().Item2.ToString());
            return actionable[t.Result.OrderBy(p => p.Item2).Last().Item1];
        }

        private Task<(int, double)> NodeSearch(Field.Field field, ActionDate act, int idx)
        {
            return Task.Run(() =>
            {
                List<Node> tmpList = new List<Node>();
                List<Node> searchList = new List<Node>();
                Field.Field f = field.Clone();
                int judge = f.Action(act);
                Node root = new Node(null, new SearchData(f, f.Score(f.TurnSide)));
                switch (judge)
                {
                    case 0:
                        searchList.AddRange(root.Expand(0));
                        break;
                    case 1: return (idx, -10);
                    case 2: return (idx, 1000);
                    case 3: return (idx, 0);
                }

                for (int d = 1; d < depth+1; d++)
                {
                    foreach (var node in searchList)
                    {
                        tmpList.AddRange(node.Expand(d));
                    }
                    searchList.AddRange(tmpList);
                    tmpList.Clear();
                }
                return (idx, root.Score);
            });
        }

        private Task<(int, double)> QueueSearch(Field.Field field, ActionDate act, int idx)
        {
            return new Task<(int, double)>(() => {
                var result = new List<SearchData>();
                var list = new SearchList(new SearchData(field));
                for (int d = 0; d < depth; d++)
                {
                    List<SearchData> datas = (d % 2 == 0) ? list.PopLowList(numAdoptNode) : list.PopHighList(numAdoptNode);
                    list = new SearchList();
                    foreach (var data in datas)
                    {
                        List<SearchData> tmpList = new List<SearchData>();
                        List<ActionDate> actionable = (d == 0) ? new List<ActionDate> {act} : PieceController.PlayerActionList(data.field, data.field.TurnSide);
                        foreach (var action in actionable)
                        {
                            Field.Field f = data.field.Clone();
                            int judge = f.Action(action);
                            if (judge == 0)
                            {
                                var tmp = new SearchData(f, data.score + f.Score(data.field.TurnSide) / (d + 1));
                                if (d % 2 == 0) list.Push(tmp);
                                else tmpList.Add(tmp);
                            }
                            else if (judge == 1)
                            {
                                result.Add(new SearchData(f, data.score - depth * 100 / (d + 1) / 5));
                            }
                            else if (judge == 2)
                            {
                                result.Add(new SearchData(f, data.score + depth * 100 / (d + 1) / 5));
                            }
                            else if (judge == 3)
                            {
                                result.Add(new SearchData(f, 0));
                            }
                        }
                        if (d % 2 != 1) continue;
                        // ここでCPUにとって最悪の手を人間の手として取り出す
                        var bestData = tmpList.OrderBy(c => c.score).First();
                        list.Push(bestData);
                    }
                }
                if (result.Count > 0 && list.Count > 0)
                {
                    var listMax = list.Last().score;
                    var resMax = result.OrderBy(c => c.score).Last().score;
                    return listMax > resMax ? (idx, listMax) : (idx, resMax);
                }
                if (result.Count == 0 && list.Count > 0)
                {
                    var listMax = list.Last().score;
                    return (idx, listMax);
                }
                if (result.Count <= 0 || list.Count != 0) throw new InvalidOperationException();
                {
                    var resMax = result.OrderBy(c => c.score).Last().score;
                    return (idx, resMax);
                }
            });
        }
    }
}
