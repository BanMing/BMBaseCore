// 2020.09.13 banming

using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    private GameObject _orgianGo;

    private List<GameObject> _pools;

    public GameObjectPool(GameObject orgianGo)
    {
        _orgianGo = orgianGo;

    }

    public void Get() { }

    public void Release() { }

}
