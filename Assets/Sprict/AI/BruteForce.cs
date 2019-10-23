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
                var queue = new PriorityQueue<SearchData>(new SearchData(field, act));
                for (int d = 0; d < depth; d++)
                {
                    List<SearchData> datas = queue.DequeueList(numAdoptNode);
                    queue = new PriorityQueue<SearchData>();
                    foreach (var data in datas)
                    {
                        List<ActionDate> actionable = PieceController.PlayerActionList(data.field, data.field.TurnSide);
                        foreach (var action in actionable)
                        {
                            Field.Field f = data.field.Clone();
                            int judge = f.Action(action);
                            switch (judge)
                            {
                                case 0:
                                    queue.Enqueue(new SearchData(f, action.Clone(), data.score + f.Score(f.TurnSide) / (d + 1)));
                                    break;
                                case 1:
                                    result.Add(new SearchData(f, action.Clone(), data.score - depth * 1000 / (d + 1)));
                                    break;
                                case 2:
                                    result.Add(new SearchData(f, action.Clone(), data.score + depth * 100 / (d + 1)));
                                    break;
                                case 3:
                                    result.Add(new SearchData(f, action.Clone(), 0));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                var queueMax = queue.OrderBy(c => c.score).Last().score;
                // TODO: 返り血を変えよう
                if (result.Count > 0)
                {
                    var resMax = result.OrderBy(c => c.score).Last().score;
                    return queueMax > resMax ? (idx, queueMax) : (idx, resMax);
                }
                else
                {
                    return (idx, queueMax);
                }
            });
        }
    }
}
