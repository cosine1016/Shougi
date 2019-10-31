﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Sprict.Field;
using Assets.Sprict.AI;

namespace Assets.Sprict.AI
{
    class Node
    {
        private Node Parent;
        private List<Node> Children;
        private SearchData Data { get; }

        public double Score { get; private set; }
        public bool IsEnd { get; } = false;
        public int Count => Children.Count;
        public Node BestChild => Children.OrderBy(c => c.Score).Last();
        public Node WorstChild => Children.OrderByDescending(c => c.Score).Last();
        
        public Node(Node parent, SearchData data)
        {
            Parent = parent;
            Data = data;
            Score = data.score;
            Children = new List<Node>();
        }
        
        public Node(Node parent, SearchData data, bool isEnd)
        {
            Parent = parent;
            Data = data;
            Score = data.score;
            Children = new List<Node>();
            IsEnd = isEnd;
        }

        public List<Node> Expand(int d)
        {
            List<ActionDate> actionable = PieceController.PlayerActionList(Data.field, Data.field.TurnSide);
            foreach (var action in actionable)
            {
                Field.Field f = Data.field.Clone();
                int judge = f.Action(action);
                var data = new SearchData(Data.field);
                if (judge == 0)
                {
                    data = new SearchData(f, Data.score + f.Score(Data.field.TurnSide));
                    Children.Add(new Node(this, data));
                }
                else
                {
                    switch (judge)
                    {
                        case 1:
                            data = new SearchData(f, Data.score + (d - 50));
                            break;
                        case 2:
                            data = new SearchData(f, Data.score + (50 - d));
                            break;
                        case 3:
                            data = new SearchData(f, Data.score);
                            break;
                    }
                    Children.Add(new Node(this, data, true));
                }
            }
            return Children.Where(c => !c.IsEnd).ToList();
        }

        public void BackProp(double score)
        {
            Score += score;
        }
    }
}