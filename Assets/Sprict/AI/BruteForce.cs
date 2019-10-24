using System;
using System.Collections.Generic;
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
            depth = _depth;
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

        public Task<(int, double)> NodeSearch(Field.Field field, ActionDate act, int idx)
        {
            return Task.Run(() =>
            {
                var result = new List<SearchData>();
                var list = new SearchList(new SearchData(field));
                for (int d = 0; d < depth; d++)
                {
                    List<SearchData> datas = (d % 2 == 0) ? list.PopHighList(numAdoptNode) : list.PopLowList(numAdoptNode);
                    list = new SearchList();
                    foreach (var data in datas)
                    {
                        List<SearchData> tmpList = new List<SearchData>();
                        List<ActionDate> actionable = PieceController.PlayerActionList(data.field, data.field.TurnSide);
                        // 疲れていたのでこのようなクソコードを書いてしまいました。
                        // 締め切りが先、コードが後
                        if (d == 0)
                        {
                            Field.Field f = field.Clone();
                            int judge = f.Action(act);
                            switch (judge)
                            {
                                case 0:
                                    var tmp = new SearchData(f, data.score + f.Score(data.field.TurnSide) / (d + 1));
                                    list.Push(tmp);
                                    break;
                                case 1:
                                    result.Add(new SearchData(f, data.score - depth * 100 / (d + 1)));
                                    break;
                                case 2:
                                    result.Add(new SearchData(f, data.score + depth * 100 / (d + 1)));
                                    break;
                                case 3:
                                    result.Add(new SearchData(f, 0));
                                    break;
                            }
                            continue;
                        }
                        foreach (var action in actionable)
                        {
                            Field.Field f = data.field.Clone();
                            int judge = f.Action(action);
                            switch (judge)
                            {
                                case 0:
                                    var tmp = new SearchData(f, data.score + f.Score(data.field.TurnSide) / (d + 1));
                                    if (d % 2 == 0) list.Push(tmp);
                                    else tmpList.Add(tmp);
                                    break;
                                case 1:
                                    result.Add(new SearchData(f, data.score - depth * 100 / (d + 1)));
                                    break;
                                case 2:
                                    result.Add(new SearchData(f, data.score + depth * 100 / (d + 1)));
                                    break;
                                case 3:
                                    result.Add(new SearchData(f, 0));
                                    break;
                            }
                        }
                        if (d % 2 == 1)
                        {
                            // ここでCPUにとって最悪の手を人間の手として取り出す
                            var bestData = tmpList.OrderBy(c => c.score).First();
                            list.Push(bestData);
                        }
                    }
                }
                if (result.Count > 0 && list.Count > 0)
                {
                    var listMax = list.Last().score;
                    var resMax = result.OrderBy(c => c.score).Last().score;
                    return listMax > resMax ? (idx, listMax) : (idx, resMax);
                }
                else if (result.Count == 0 && list.Count > 0)
                {
                    var listMax = list.Last().score;
                    return (idx, listMax);
                }
                else if (result.Count > 0 && list.Count == 0)
                {
                    var resMax = result.OrderBy(c => c.score).Last().score;
                    return (idx, resMax);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            });
        }
    }
}
