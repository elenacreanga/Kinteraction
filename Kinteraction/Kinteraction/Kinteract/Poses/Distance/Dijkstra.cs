using System;
using System.Collections.Generic;
using Microsoft.Kinect;

namespace Kinteraction.Kinteract.Poses.Distance
{
    public class Dijkstra
    {
        public List<string> CalculateDistance(JointType from, JointType to)
        {
            var graph = new BodyGraph().Graph;
            //initialize neighbors using predefined dictionary
            foreach (var node in graph)
            {
                node.Neighbors = new List<Neighbor>();
                foreach (var neighbor in node.DistanceDict)
                {
                    var newNeightbor = new Neighbor();
                    foreach (var graphNode in graph)
                        if (graphNode.Name == neighbor.Key)
                        {
                            newNeightbor.Node = graphNode;
                            newNeightbor.Distance = neighbor.Value.Count;
                            node.Neighbors.Add(newNeightbor);
                            break;
                        }
                }
            }
            foreach (var node in graph)
            {
                if (node.Name == from.ToString())
                {
                    TransverNode(node);
                }
            }

            foreach (var node in graph)
            {
                var fromNameNode = from.ToString();
                if (node.Name.Equals(fromNameNode))
                {
                    if (node.DistanceDict.ContainsKey(to.ToString()))
                    {
                        var result = new List<string> {node.Name};
                        result.AddRange(node.DistanceDict[to.ToString()]);
                        return result;
                    }

                    break;
                }
            }
            throw new Exception("Should return distance");
        }

        private void TransverNode(Node node)
        {
            if (!node.Visited)
            {
                node.Visited = true;
                foreach (var neighbor in node.Neighbors)
                {
                    TransverNode(neighbor.Node);
                    var neighborName = neighbor.Node.Name;
                    var neighborDistance = neighbor.Distance;
                    //compare neighbours dictionary with current dictionary
                    //update current dictionary as required
                    foreach (var key in neighbor.Node.DistanceDict.Keys)
                    {
                        if (key == node.Name) continue;
                        var neighborKeyDistance = neighbor.Node.DistanceDict[key].Count;
                        if (node.DistanceDict.ContainsKey(key))
                        {
                            var currentDistance = node.DistanceDict[key].Count;
                            if (neighborKeyDistance + neighborDistance < currentDistance)
                            {
                                var nodeList = new List<string>();
                                nodeList.AddRange(neighbor.Node.DistanceDict[key].ToArray());
                                nodeList.Insert(0, neighbor.Node.Name);
                                node.DistanceDict[key] = nodeList;
                            }
                        }
                        else
                        {
                            var nodeList = new List<string>();
                            nodeList.AddRange(neighbor.Node.DistanceDict[key].ToArray());
                            nodeList.Insert(0, neighbor.Node.Name);
                            node.DistanceDict.Add(key, nodeList);
                        }
                    }
                }
            }
        }
    }
}