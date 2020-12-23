using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftSide : MonoBehaviour
{
    public static List<BeaverHit> beaverHits = new List<BeaverHit>();

    public static List<BeaverFishing> beaverFishing = new List<BeaverFishing>();

    private void Awake()
    {
        beaverHits.AddRange(GetComponentsInChildren<BeaverHit>(true));

        beaverFishing.AddRange(GetComponentsInChildren<BeaverFishing>(true));
    }

    private void Start()
    {
        for (int i = 0; i < beaverHits.Count; i++)
        {

        }

        for (int i = 0; i < beaverFishing.Count; i++)
        {

        }
    }
}
