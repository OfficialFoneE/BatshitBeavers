using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingTileEnviroment : MonoBehaviour
{

    private static GameObject enviromentPrefab;

    private static BufferedArray<BufferedEnviroment> bufferedEnviroment;

    private new Transform transform;

    public float startSpeed = 0.01f;
    public float queueSpeed = 1.0f;
    public float speed;
    [SerializeField] private float multiplier = 5;
    private const float width = 1.95f;

    private BufferedEnviroment lastEnviroment;

    private Transform riverEnviroment;

    private bool repeatEnviroment = true;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        bufferedEnviroment = new BufferedArray<BufferedEnviroment>(InstantiateBufferedEnviroment, BufferBufferedEnviroment);
        riverEnviroment = FindObjectOfType<RiverEnviroment>().transform;
    }

    void Start()
    {
        speed = startSpeed;

        bufferedEnviroment.UpdatePooledObjects(6);
        bufferedEnviroment.UpdatePooledObjects(4);

        bufferedEnviroment[0].position = new Vector3(0, width, 0);
        bufferedEnviroment[1].position = new Vector3(0, 0, 0);
        bufferedEnviroment[2].position = new Vector3(0, -width, 0);
        bufferedEnviroment[3].position = new Vector3(0, -width * 2, 0);

        lastEnviroment = bufferedEnviroment[3];
    }

    void Update()
    {
        if (repeatEnviroment)
        {
            for (int i = 0; i < bufferedEnviroment.bufferedCount; i++)
            {
                bufferedEnviroment[i].position.y += speed * Time.deltaTime;
                var temp = bufferedEnviroment[i].position;
                bufferedEnviroment[i].transform.localPosition = new Vector3(temp.x, temp.y, temp.z);

                if (temp.y >= width * 1.5 + 0.5)
                {
                    bufferedEnviroment[i].position = new Vector3(0, lastEnviroment.position.y - width + speed * Time.deltaTime, 0);

                    lastEnviroment = bufferedEnviroment[i];
                }
            }
        }
    }

    public void AlignWithEnviroment()
    {
        var temp = lastEnviroment.transform.position;
        temp.y -= width;
        riverEnviroment.transform.position = temp;
        repeatEnviroment = false;
    }

    private class BufferedEnviroment : BufferedObject
    {
        public Vector3 position;
        public Transform transform;

        public BufferedEnviroment(GameObject gameObject) : base(gameObject)
        {
            transform = gameObject.transform;
        }
    }

    private BufferedEnviroment InstantiateBufferedEnviroment()
    {
        return new BufferedEnviroment(Instantiate(enviromentPrefab, transform));
    }

    private void BufferBufferedEnviroment(BufferedEnviroment bufferedEnviroment, bool state)
    {
        bufferedEnviroment.gameObject.SetActive(state);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    public static void LoadResources()
    {
        enviromentPrefab = Resources.Load<GameObject>("RepeatingTileEnviroment/RepeatingTileEnviroment");
    }
}
