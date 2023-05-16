using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace CoEvent
{
    //在这里决定强转的方法实现

    internal static class CoUnsafeAs 
    {

        internal static TTo As<TFrom,TTo>(ref TFrom t)
        {
#if UNITY_2021_1_OR_NEWER
            return UnsafeUtility.As<TFrom, TTo>(ref t);
#elif NETCore
            return Unsafe.As<TFrom,TTo>(ref t);
#endif


        }


    }
}
