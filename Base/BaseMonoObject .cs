/******************************************************************
** BaseMonoObject.cs
** @Author       : BanMing 
** @Date         : 2020/9/27 下午6:44:47
** @Description  : 
*******************************************************************/

using UnityEngine;
using System.Collections;
using System;

public class BaseMonoObject : MonoBehaviour
{
    #region Attriube
    #endregion

    #region Unity Event

    private void OnDestroy()
    {
        Destroy();
    }

    #endregion

    #region Public Method

    public virtual void Init() { }

    public virtual void Destroy() { }

    #endregion

    #region Local Method
    #endregion
}