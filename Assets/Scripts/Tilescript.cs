using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilescript : MonoBehaviour
{
    public List<Tilescript> neighbors = new List<Tilescript>();

    //if keeping track of weights
    //public List<int> weights = new List<int>();

    public float distance;
    public Tilescript backPointer;
    public bool hasVisited;

    public GameObject[] borders;

    public List<Tilescript> getNeighbors()
    {
        return neighbors;
    }

    public void clear()
    {
        distance = float.MaxValue;
        hasVisited = false;
        backPointer = null;

        borders[0].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.black);
        borders[1].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.black);
        borders[2].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.black);
        borders[3].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.black);
    }

    public void setDistance(float distance_in)
    {
        distance = distance_in;
    }

    public float getDistance()
    {
        return distance;
    }

    public void setVisited(bool visited_in)
    {
        hasVisited = visited_in;
    }

    public bool getVisited()
    {
        return hasVisited;
    }

    public void setBackPointer(Tilescript backPlace)
    {
        backPointer = backPlace;
    }

    public Tilescript getBackPointer()
    {
        return backPointer;
    }

    public void setColor(Color theColor)
    {
        borders[0].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", theColor);
        borders[1].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", theColor);
        borders[2].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", theColor);
        borders[3].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", theColor);
    }
}
