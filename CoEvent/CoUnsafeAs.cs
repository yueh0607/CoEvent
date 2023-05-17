
using Unity.Collections.LowLevel.Unsafe;
namespace CoEvent
{
    //在这里决定强转的方法实现

    internal static class CoUnsafeAs
    {

        internal static TTo As<TFrom, TTo>(ref TFrom t)
        {
#if UNITY_2020_1_OR_NEWER
            return UnsafeUtility.As<TFrom, TTo>(ref t);
#elif NETCORE
            return Unsafe.As<TFrom,TTo>(ref t);
#else

        #error Unsupported platform!(请手动实现Unsafe.As或切换到支持的平台上)

#endif
        }


    }
}
