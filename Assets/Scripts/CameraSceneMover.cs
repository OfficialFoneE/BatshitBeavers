using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSceneMover : MonoBehaviour
{
    private static CameraSceneMover instance;

    private static new Camera camera;
    private static Transform cameraTransform;

    private static Transform riverEnviroment;
    private static Transform repeatingTileEnviroment;

    private static Vector3 target;

    public float speed;

    private void Awake()
    {
        instance = this;

        camera = Camera.main;
        cameraTransform = camera.transform;

        riverEnviroment = FindObjectOfType<RiverEnviroment>().transform;
        repeatingTileEnviroment = FindObjectOfType<RepeatingTileEnviroment>().transform;

        this.enabled = false;
    }

    private void Update()
    {
        if(DistanceSqrd(cameraTransform.position, target) > 0.001)
        {
            var newPos = Vector3.MoveTowards(cameraTransform.position, target, speed * Time.deltaTime);

            cameraTransform.position = newPos;
        } else
        {
            instance.enabled = false;
        }
    }

    private static float DistanceSqrd(Vector3 v1, Vector3 v2)
    {
        return (v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z);
    }

    public static void TravelToRiverEnviroment()
    {
        var tempTarget = riverEnviroment.position;
        tempTarget.y += 0.140f;
        target = tempTarget;
        target.z = cameraTransform.position.z;
        instance.enabled = true;
    }

    public static void TravelToForestEnviroment()
    {
        target = repeatingTileEnviroment.position;
        target.z = cameraTransform.position.z;
        instance.enabled = true;
    }
}
