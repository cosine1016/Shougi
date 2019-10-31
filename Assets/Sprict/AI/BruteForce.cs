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

        private Task<(int, double)> NodeSearch(Field.Field field, ActionDate act, int idx)
        {
            return Task.Run(() =>
            {
                List<Node> tmpList = new List<Node>();
                List<Node> searchList = new List<Node>();
                int judge = field.Action(act);
                // CPUのスコアをrootに格納
                Node root = new Node(null, new SearchData(field.Clone(), field.Score(2)));
                switch (judge)
                {
                    case 0:
                        // Playerのスコアに関してExpand
                        searchList.AddRange(root.Expand(0));
                        break;
                    case 1: throw new InvalidDataException();
                    case 2: return (idx, 1000);
                    case 3: return (idx, 0);
                }

                // 指定depth回木を拡張
                for (int d = 1; d < depth; d++)
                {
                    foreach (var node in searchList)
                    {
                        tmpList.AddRange(node.Expand(d));
                    }
                    searchList.AddRange(tmpList);
                    tmpList.Clear();
                }
                int side = -1;
                Node bestChild = root;
                for (int d = 0; d < depth; d++)
                {
                    if (bestChild.Count == 0) break;
                    bestChild = (side == 1) ? bestChild.BestChild : bestChild.WorstChild;
                }
                return (idx, bestChild.Score);
            });
        }
    }
}
