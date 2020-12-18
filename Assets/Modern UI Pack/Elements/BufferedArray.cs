using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BufferedArray<T> where T : BufferedObject
{
    public List<T> bufferedObjects = new List<T>();

    public T this[int index]
    {
        get
        {
            return bufferedObjects[index];
        }
    }

    public int Count
    {
        get
        {
            return bufferedObjects.Count;
        }
    }

    private int _bufferedCount;
    public int bufferedCount
    {
        get
        {
            return _bufferedCount;
        }
        set
        {
            _bufferedCount = value;
        }
    }

    private Func<T> TConstructor;
    private Action<T, bool> bufferFunction;

    public BufferedArray(Func<T> TConstructor, Action<T, bool> bufferFunction)
    {
        this.TConstructor = TConstructor;
        this.bufferFunction = bufferFunction;
    }

    public void Add(T bufferedObject)
    {
        bufferedObjects.Add(bufferedObject);
    }
    public void AddRange(List<T> bufferedObjects)
    {
        this.bufferedObjects.AddRange(bufferedObjects);
    }
    public void Remove(T bufferedObject)
    {
        bufferedObjects.Remove(bufferedObject);
    }
    public void RemoveAt(int index)
    {
        bufferedObjects.RemoveAt(index);
    }

    public int IndexOf(T bufferedObject)
    {
        return bufferedObjects.IndexOf(bufferedObject);
    }

    public void Buffer(T bufferedObject)
    {
        bufferedObjects.Remove(bufferedObject);
        _bufferedCount--;
        bufferedObjects.Add(bufferedObject);
        bufferFunction.Invoke(bufferedObject, false);
    }
    public void BufferRange(List<T> bufferedObjects)
    {
        for (int i = 0; i < bufferedObjects.Count; i++)
        {
            this.bufferedObjects.Remove(bufferedObjects[i]);
            bufferFunction.Invoke(bufferedObjects[i], false);
        }
        _bufferedCount -= bufferedObjects.Count;
        this.bufferedObjects.AddRange(bufferedObjects);
    }
    public void BufferRange(T[] bufferedObjects)
    {
        for (int i = 0; i < bufferedObjects.Length; i++)
        {
            this.bufferedObjects.Remove(bufferedObjects[i]);
            bufferFunction.Invoke(bufferedObjects[i], false);
        }
        _bufferedCount -= bufferedObjects.Length;
        this.bufferedObjects.AddRange(bufferedObjects);
    }

    public void BufferAt(int index)
    {
        var bufferObject = bufferedObjects[index];
        Buffer(bufferObject);
    }

    public void UpdatePooledObjects(int count)
    {
        UpdatePooledObject(bufferedObjects, ref _bufferedCount, count, TConstructor, bufferFunction);
    }

    public List<T> GetUnusedPooledObjects(int count)
    {
        return GetUnusedPooledObjects(bufferedObjects, ref _bufferedCount, count, TConstructor, bufferFunction);
    }








    private static void UpdatePooledObject(List<T> pooledList, ref int bufferedCount, int newCount, Func<T> TConstructor, Action<T, bool> bufferFunction)
    {
        var allocatedBufferSize = pooledList.Count - newCount;

        if (allocatedBufferSize >= 0)
        {
            //has enough buffer space
            if (bufferedCount > newCount)
            {
                for (int i = newCount; i < bufferedCount; i++)
                {
                    bufferFunction.Invoke(pooledList[i], false);
                }

            }
            else
            {
                for (int i = bufferedCount; i < newCount; i++)
                {
                    bufferFunction.Invoke(pooledList[i], true);
                }
            }

        }
        else
        {
            //need more buffer space
            allocatedBufferSize = allocatedBufferSize * -1;

            for (int i = bufferedCount; i < pooledList.Count; i++)
            {
                bufferFunction.Invoke(pooledList[i], true);
            }

            for (int i = 0; i < allocatedBufferSize; i++)
            {
                pooledList.Add(TConstructor.Invoke());
            }
        }

        bufferedCount = newCount;
    }
    private static List<T> GetUnusedPooledObjects(List<T> pooledList, ref int bufferedCount, int newCount, Func<T> TConstructor, Action<T, bool> bufferFunction)
    {

        if (bufferedCount + newCount > pooledList.Count)
        {
            for (int i = bufferedCount; i < pooledList.Count; i++)
            {
                bufferFunction.Invoke(pooledList[i], true);
            }

            int difference = (bufferedCount + newCount) - pooledList.Count;
            for (int i = 0; i < difference; i++)
            {
                pooledList.Add(TConstructor.Invoke());
            }

        }
        else
        {
            for (int i = bufferedCount; i < bufferedCount + newCount; i++)
            {
                bufferFunction.Invoke(pooledList[i], true);
            }
        }

        var newPooledList = pooledList.GetRange(bufferedCount, newCount);
        bufferedCount = bufferedCount + newCount;

        return newPooledList;
    }
}
