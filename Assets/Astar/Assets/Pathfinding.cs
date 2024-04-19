using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    /*
    1. Create an Open and Closed list of Nodes
    2. Add the current (start) Node to the open set. 
    3. While openSet.count is greater than 0 (meaning there are open nodes left to the target)
    4. Iterate through the nodes and perform a series of checks to determine the course of action
    */

    /* public Transform seeker, target;
 */
    PathRequestManager requestManager;
    Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();
        requestManager = GetComponent<PathRequestManager>();
    }

    /* void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindPath(seeker.position, target.position);
        }
    } */

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            //We're removing the currentNode from the openSet, and adding it to the closed set. This is because we don't want to be able to 'rediscover' it as a node we haven't been to before. 
            closedSet.Add(currentNode);


            //Here we're checking if the currentNode we're on is actually the target Node. If it is, we'll call the retracePath method, which works bachwards to regenerate the path that was discovered to the target. 
            if (currentNode == targetNode)
            {
                sw.Stop();
                print("Path found" + sw.ElapsedMilliseconds + " ms");
                pathSuccess = true;
                break;
            }

            //In GetNeighbours we're creating a List of Nodes that are around the currentNode in a smaller radius around it. We're then checking if that neighbour is walkable (i.e. an obstacle or not) or if it is in the closedSet, which will call the continue that will skip the rest of the logic for this particualr neighbour, and move on to the next one. 
            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                //We'll then check whether the path to the neighbour is shorter than the old path, or whether the openSet already contains the neighbour node. We'll first calculate the g- and hCost, and set the parent node (the parent is the node that it came from) on the neighbour as the currentNode. 
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessing(waypoints, pathSuccess);
    }

    //This method will ensure that we can work backwards through our list of nodes, and thereby shape the specific path from start to target. However, this method works by going from the target to the start, and then setting this list of nodes as the path in the Grid class. 
    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        grid.path = path;
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    //We're adding some different values here, which essentially just returns some measurement of the cost of moving either horizontally, vertically or diagonally (diagonally is more expensive?)
    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);
    }
}
