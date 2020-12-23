using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamIcons : MonoBehaviour
{

    public BufferedLogImages bufferedLogImage;
    public BufferedFishImages bufferedFishImages;
    public BufferedBeaverImages bufferedBeaverImages;

    private void Awake()
    {
        bufferedLogImage = GetComponentInChildren<BufferedLogImages>();
        bufferedFishImages = GetComponentInChildren<BufferedFishImages>();
        bufferedBeaverImages = GetComponentInChildren<BufferedBeaverImages>();
    }
}
