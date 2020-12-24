using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferedBeaverImages : MonoBehaviour
{
    private RectTransform rectTransform;

    private BufferedArray<BufferedBeaverImage> bufferedArray;

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
        bufferedArray = new BufferedArray<BufferedBeaverImage>(InstantiateBufferedBeaverImage, BufferBufferedBeaverImage);
    }

    private void Start()
    {
        bufferedArray.UpdatePooledObjects(20);
        bufferedArray.UpdatePooledObjects(1);
        //bufferedArray.UpdatePooledObjects(0);
    }

    private BufferedBeaverImage InstantiateBufferedBeaverImage()
    {
        return new BufferedBeaverImage(Instantiate(GameCanvas.beaverImage, rectTransform));
    }

    private void BufferBufferedBeaverImage(BufferedBeaverImage bufferedBeaverImage, bool state)
    {
        bufferedBeaverImage.gameObject.SetActive(state);
    }

    private class BufferedBeaverImage : BufferedRectTransform
    {
        public BufferedBeaverImage(GameObject gameObject) : base(gameObject)
        {

        }
    }
}
