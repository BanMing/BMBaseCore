
// 2020.9.13 banming

using BMBaseCore;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class InstantiateTool : SingletonMono<InstantiateTool>
{
    //private class InstantateInfo
    //{
    //    public GameObject go;

    //    public int count;

    //    public Action<GameObject> callback;

    //    public int delayTimeMs;

    //    public float nextInstantiateTime;

    //    public InstantateInfo(GameObject go, int count, Action<GameObject> callback, int delayTimeMs = 10)
    //    {
    //        this.go = go;
    //        this.count = count;
    //        this.callback = callback;
    //        this.delayTimeMs = delayTimeMs;
    //        this.nextInstantiateTime = Time.time + (delayTimeMs * 1.0f / 1000.0f);
    //    }
    //}

    //private Queue<InstantateInfo> _instanctateQueque;

    #region unity event

    private void Awake()
    {
        //_instanctateQueque = new Queue<InstantateInfo>();
        DontDestroyOnLoad(this);
    }

    //private void Update()
    //{
    //    Debug.Log($"Time.deltaTime:{Time.deltaTime} time:{Time.time}");
    //    if (_instanctateQueque.Count < 1)
    //    {
    //        return;
    //    }
    //    while (_instanctateQueque.GetEnumerator().MoveNext())
    //    {
    //        InstantateInfo info = _instanctateQueque.GetEnumerator().Current;
    //        if (info.nextInstantiateTime < Time.time)
    //        {
    //            if (info.count < 1)
    //            {

    //            }
    //        }
    //    }
    //}

    #endregion

    #region public method

    //public void InstantiateByQueue(GameObject gameObject, int count, Action<GameObject> eachCompeletCallBack, int delayTimeMs = 10)
    //{
    //    _instanctateQueque.Enqueue(new InstantateInfo(gameObject, count, eachCompeletCallBack, delayTimeMs));
    //}
    public async Task InstantiateByAsync(GameObject gameObject, int count, Action<GameObject> completeCallbcak)
    {
        await InstantiateByAsync(gameObject, count, completeCallbcak, 10).ConfigureAwait(false);
    }

    public async Task InstantiateByAsync(GameObject gameObject, int count, Action<GameObject> completeCallbcak, int delayTimeMs)
    {
        for (int i = 0; i < count; i++)
        {
            if (this != null && isActiveAndEnabled)
            {
                await instantiateByAsync(gameObject, completeCallbcak, delayTimeMs).ConfigureAwait(false);
            }
        }
    }

    #endregion

    #region private method

    private async Task instantiateByAsync(GameObject gameObject, Action<GameObject> completeCallbcak, int delayTimeMs)
    {
        await Task.Delay(delayTimeMs).ConfigureAwait(false);

        if (this == null || !isActiveAndEnabled)
        {
            return;
        }

        GameObject instanceGo = Instantiate(gameObject);

        completeCallbcak?.Invoke(instanceGo);
    }
    #endregion


}
