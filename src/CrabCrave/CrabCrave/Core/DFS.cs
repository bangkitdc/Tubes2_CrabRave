using System;
using System.Collections.Generic;

class DFS 
{
    private Stack<Node> path; // holds the path that is used
    DFS(Map map) {
        // priorities: U D L R
        Stack<Node> stack = new Stack<Node>();
        this.path = new Stack<Node>();
 
        // assume
        (int currentX, int currentY) = map.getStart();

        map.map[currentX, currentY].setVisiting();
        map.map[currentX, currentY].setVisited();
        stack.Push(map.map[currentX, currentY]);
        this.path.Push(map.map[currentX, currentY]);

        // while 
        while (stack.Count != 0) {
            if (map.isUpAvailable(currentX, currentY)) {
                map.map[currentX, currentY].setLeaving();
                currentX--;
                map.map[currentX, currentY].setVisiting();
                map.map[currentX, currentY].setVisited();
                this.path.Push(map.map[currentX, currentY]);
                stack.Push(map.map[currentX, currentY]);
            } else if (map.isDownAvailable(currentX, currentY)) {
                map.map[currentX, currentY].setLeaving();
                currentX++;
                map.map[currentX, currentY].setVisiting();
                map.map[currentX, currentY].setVisited();
                this.path.Push(map.map[currentX, currentY]);
                stack.Push(map.map[currentX, currentY]);
            } else if (map.isLeftAvailable(currentX, currentY)) {
                map.map[currentX, currentY].setLeaving();
                currentY--;
                map.map[currentX, currentY].setVisiting();
                map.map[currentX, currentY].setVisited();
                this.path.Push(map.map[currentX, currentY]);
                stack.Push(map.map[currentX, currentY]);
            } else if (map.isRightAvailable(currentX, currentY)) {
                map.map[currentX, currentY].setLeaving();
                currentY++;
                map.map[currentX, currentY].setVisiting();
                map.map[currentX, currentY].setVisited();
                this.path.Push(map.map[currentX, currentY]);
                stack.Push(map.map[currentX, currentY]);
            } else {
                map.map[currentX, currentY].setLeaving();
                Node temp = stack.Pop();
                currentX = temp.x;
                currentY = temp.y;
            }
        }
    }
}