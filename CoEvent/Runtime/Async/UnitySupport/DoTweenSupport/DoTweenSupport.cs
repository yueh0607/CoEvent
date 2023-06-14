
//打开这行注释以支持DoTween完成的等待

//#define CoEvent_Async_DoTween_Enable

#if CoEvent_Async_DoTween_Enable

namespace CoEvents.Async
{
    public static class DoTweenSupport
    {
        /// <summary>
        /// 有一定的GC分配，慎用
        /// </summary>
        /// <param name="tween"></param>
        /// <returns></returns>
        public static CoTask GetAwaiter(this DG.Tweening.Tween tween)
        {
            var task = CoTask.Create();
            tween.onComplete+=()=>task.SetResult();
            return task;
        }
    }
}
#endif