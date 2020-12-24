using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferedFishImages : MonoBehaviour
{
    private RectTransform rectTransform;

    private BufferedArray<BufferedFishImage> bufferedArray;

    public int count
    {
        get
        {
            return bufferedArray.bufferedCount;
        }
        set
        {
            bufferedArray.UpdatePooledObjects(value);
        }
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        bufferedArray = new BufferedArray<BufferedFishImage>(InstantiateBufferedFishImage, BufferBufferedFishImage);
    }

    private void Start()
    {
        bufferedArray.UpdatePooledObjects(20);
        bufferedArray.UpdatePooledObjects(3);
        //bufferedArray.UpdatePooledObjects(0);
    }

    private BufferedFishImage InstantiateBufferedFishImage()
    {
        return new BufferedFishImage(Instantiate(GameCanvas.fishImage, rectTransform));
    }

    private void BufferBufferedFishImage(BufferedFishImage bufferedFishImage, bool state)
    {
        bufferedFishImage.gameObject.SetActive(state);
    }

    private class BufferedFishImage : BufferedRectTransform
    {
        public BufferedFishImage(GameObject gameObject) : base(gameObject)
        {

        }
    }
}
