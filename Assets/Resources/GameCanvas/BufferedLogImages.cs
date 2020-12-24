using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferedLogImages : MonoBehaviour
{

    private RectTransform rectTransform;

    private BufferedArray<BufferedLogImage> bufferedArray;

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
        bufferedArray = new BufferedArray<BufferedLogImage>(InstantiateBufferedLogImage, BufferBufferedLogImage);
    }

    private void Start()
    {
        bufferedArray.UpdatePooledObjects(50);
        //bufferedArray.UpdatePooledObjects(2);
        //bufferedArray.UpdatePooledObjects(0);
    }

    private BufferedLogImage InstantiateBufferedLogImage()
    {
        return new BufferedLogImage(Instantiate(GameCanvas.logImage, rectTransform));
    }

    private void BufferBufferedLogImage(BufferedLogImage bufferedLogImage, bool state)
    {
        bufferedLogImage.gameObject.SetActive(state);
    }

    private class BufferedLogImage : BufferedRectTransform
    {
        public BufferedLogImage(GameObject gameObject) : base(gameObject)
        {

        }
    }
}
