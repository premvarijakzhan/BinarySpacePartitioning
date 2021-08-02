using UnityEngine;

//the node
public class Leaf
{

    public int xPos;
    public int zPos;
    public int width;
    public int depth;
    int scale;
    int roomMin = 5;


    public Leaf leftChild;
    public Leaf rightChild;

    public Leaf(int xpos, int zpos, int width, int depth, int scale)
    {
        xPos = xpos;
        zPos = zpos;
        this.width = width;
        this.depth = depth;
        this.scale = scale;

    }


    public bool Split()
    {
        if (width <= roomMin || depth <= roomMin) return false;

        bool splitHorizontal = Random.Range(0, 100) < 50;
        if (width > depth && width / depth >= 1.2)
            splitHorizontal = false;
        else if (depth > width && depth / width >= 1.2)
            splitHorizontal = true;

        int max = (splitHorizontal ? depth : width) - roomMin;

        if (max <= roomMin) return false;


        //if (Random.Range(0, 100) < 50)
        if (splitHorizontal)
        {

            // int leaf1Depth = Random.Range((int)(depth * 0.1f), ((int)(depth * 0.7f)));
            int leaf1Depth = Random.Range(roomMin, max);
            leftChild = new Leaf(xPos, zPos, width, leaf1Depth, scale);
            rightChild = new Leaf(xPos, zPos + leaf1Depth, width, depth - leaf1Depth, scale);

        }
        else
        {
            //int leaf1Width = Random.Range((int)(width * 0.1f), ((int)(width * 0.7f)));
            int leaf1Width = Random.Range(roomMin, max);
            leftChild = new Leaf(xPos, zPos, leaf1Width, depth, scale);
            rightChild = new Leaf(xPos + leaf1Width, zPos, width - leaf1Width, depth, scale);

        }

        //leftChild.Draw(level);
        //rightChild.Draw(level);


        return true;

        /*
        int leaf1Depth = Random.Range((int)(mapWidth * 0.1f), ((int)(mapDepth * 0.7f)));
        Leaf left = new Leaf(0, 0, mapWidth, leaf1Depth, scale);
        Leaf right = new Leaf(0, leaf1Depth, mapWidth, mapDepth - leaf1Depth, scale);
        */

    }

    public void Draw(byte[,] map)
    {
        /*
        Color color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

        for (int x = xPos; x < width + xPos; x++)
        {
            for (int z = zPos; z < depth + zPos; z++)
            {

                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(x * scale, 0, z * scale);
                cube.transform.localScale = new Vector3(scale, scale, scale);


                cube.GetComponent<Renderer>().material.SetColor("_Color", color);
            }
        }
        */

        int wallSize = Random.Range(1, 4);

        for (int x = xPos + wallSize; x < width + xPos - wallSize; x++)
        {
            for (int z = zPos + wallSize; z < depth + zPos - wallSize; z++)
            {
                map[x, z] = 0;
            }
        }
    }
}
