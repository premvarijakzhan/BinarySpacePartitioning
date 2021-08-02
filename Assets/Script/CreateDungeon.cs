using System.Collections.Generic;
using UnityEngine;

public class CreateDungeon : MonoBehaviour
{
    public int mapWidth = 50;
    public int mapDepth = 50; //the z-direction / height of the map
    public int scale = 2;

    Leaf root;

    //1 is wall 0 is empty place 
    byte[,] map;

    List<Vector2> corridors = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        map = new byte[mapWidth, mapDepth];
        for (int z = 0; z < mapDepth; z++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                map[x, z] = 1;
            }
        }

        root = new Leaf(0, 0, mapWidth, mapDepth, scale);
        //root.Split();
        // root.Draw();
        BSP(root, 6);
        AddCorridors();
        AddRandomCorridors(10);
        DrawMap();

        void AddRandomCorridors(int numHalls)
        {
            for (int i = 0; i < numHalls; i++)
            {
                int startX = Random.Range(5, mapWidth - 5);
                int startZ = Random.Range(5, mapDepth - 5);
                int endPos = Random.Range(5, mapWidth - 5);

                if (Random.Range(0, 100) < 50)
                {
                    line(startX, startZ, endPos, startZ);
                }
                else
                {
                    line(startX, startZ, startX, endPos);
                }
            }
        }



        //EACH CALL WE MAKE TO BSP NEEDS A LEAF THUS CALLING L
        //THIS NEEDS A STOPPING CONDITION SO IT WONT CRASH
        //HENCE THE NUMBER OF SPLITDEPTH (SPLIT DEPTH) WHICH REDUCES EACH ITERATION AND STOPS
        //ONCE REACHING 0
        //SPLITDEPTH IS NUMBERS OF LEVELS THAT WE ARE CREATING
        //RECURSIVE METHOD

        void BSP(Leaf l, int splitDepth)
        {
            if (l == null) return;
            if (splitDepth <= 0)
            {
                //l.Draw(0);
                l.Draw(map);
                corridors.Add(new Vector2(l.xPos + l.width / 2, l.zPos + l.depth / 2));


                return;
            }
            // l.Draw();
            //if (l.Split(splitDepth))

            if (l.Split())
            {
                BSP(l.leftChild, splitDepth - 1);
                BSP(l.rightChild, splitDepth - 1);
            }
            else
            {
                //l.Draw(0);
                l.Draw(map);
                corridors.Add(new Vector2(l.xPos + l.width / 2, l.zPos + l.depth / 2));

            }
        }

        void AddCorridors()
        {   //i is 1 because we are starting from the second leaf 
            for (int i = 1; i < corridors.Count; i++)
            {
                if ((int)corridors[i].x == (int)corridors[i - 1].x || (int)corridors[i].y == (int)corridors[i - 1].y)
                {

                    line((int)corridors[i].x, (int)corridors[i].y, (int)corridors[i - 1].x, (int)corridors[i - 1].y);
                }
                else
                {
                    //vertical
                    line((int)corridors[i].x, (int)corridors[i].y, (int)corridors[i].x, (int)corridors[i - 1].y);
                    line((int)corridors[i].x, (int)corridors[i].y, (int)corridors[i - 1].x, (int)corridors[i].y);
                }
            }
        }


        ///Leaf left = new Leaf(.....)
        ///Leaf right = new Leaf(.....)
        ///left.Draw()
        ///right.Draw()
        ///

        // Leaf left = new Leaf(0, 0, mapWidth / 2, mapDepth, scale);
        // Leaf right = new Leaf(mapWidth / 2, 0, mapWidth / 2, mapDepth, scale);



        //get the value of the map with percentage so that it wont fix value as the
        //varibale is not fixed and able to change thus calculate it with percentage

        //int leaf1Depth = Random.Range(10, 25);
        /*
        int leaf1Depth = Random.Range((int)(mapDepth * 0.1f), ((int)(mapDepth * 0.7f)));
        Leaf left = new Leaf(0, 0, mapWidth, leaf1Depth, scale);
        Leaf right = new Leaf(0, leaf1Depth, mapWidth, mapDepth - leaf1Depth, scale);


        left.Draw();
        right.Draw();
        */
    }

    //nested forloop
    //to loop through the entire map
    void DrawMap()
    {
        for (int z = 0; z < mapDepth; z++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (map[x, z] == 1)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    cube.transform.position = new Vector3(x * scale, 10, z * scale);
                    cube.transform.localScale = new Vector3(scale, scale, scale);
                }
                else if (map[x, z] == 2)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    cube.transform.position = new Vector3(x * scale, 10, z * scale);
                    cube.transform.localScale = new Vector3(scale, scale, scale);
                    cube.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 0, 0));
                }
            }
        }

    }

    //Adapted Bresenham's line algorithm
    //https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
    public void line(int x, int y, int x2, int y2)
    {
        int w = x2 - x;
        int h = y2 - y;
        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
        if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
        if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
        int longest = Mathf.Abs(w);
        int shortest = Mathf.Abs(h);
        if (!(longest > shortest))
        {
            longest = Mathf.Abs(h);
            shortest = Mathf.Abs(w);
            if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
            dx2 = 0;
        }
        int numerator = longest >> 1;
        for (int i = 0; i <= longest; i++)
        {
            // map[x, y] = 2;
            map[x, y] = 0;
            numerator += shortest;
            if (!(numerator < longest))
            {
                numerator -= longest;
                x += dx1;
                y += dy1;
            }
            else
            {
                x += dx2;
                y += dy2;
            }
        }
    }
}
