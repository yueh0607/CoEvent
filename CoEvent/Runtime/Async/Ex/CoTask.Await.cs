﻿/********************************************************************************************
 * Author : YueZhenpeng
 * Date : 2023.2.25
 * Description : 
 * 创建Async静态类以实现一些静态异步方法，比如等待，延迟等，方便用户进行使用
 */


using System;
using System.Collections.Generic;
namespace CoEvents.Async
{
    public partial class CoTask
    {
        /// <summary>
        /// 延迟指定秒数
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static CoTaskTimer Delay(float seconds)
        {
            var timer = CoTaskTimer.Create();
            timer.EndTime = seconds;
            return timer;
        }
        /// <summary>
        /// 延迟指定时间
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static CoTaskTimer Delay(TimeSpan span)
        {
            var timer = CoTaskTimer.Create();
            timer.EndTime = (float)span.TotalSeconds;
            return timer;
        }
        public static async CoTask WaitForFrame(int count = 1)
        {
            if (count <= 0)
            {
                await CompletedTask;
            }
            else
            {
                var task = CoTaskUpdate.Create();
                task.FrameCount = count;
                await task;
            }
        }


        private static CoTaskCompleted completedTask = new CoTaskCompleted();
        public static ref CoTaskCompleted CompletedTask => ref completedTask;


        /// <summary>
        /// 等待任意一个完成即可
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static CoTask WaitAny(params CoTask[] tasks)
        {
            //创建计数器
            var counterCall = CounterCall.Create();
            counterCall.ClickValue = 1;
            counterCall.OnceRecycle = true;
            //申请异步任务
            var asyncTask = CoTask.Create();

            //计数器任务绑定
            counterCall.OnClick += asyncTask.SetResult;
            //绑定异步任务到计数器
            foreach (var task in tasks)
            {

                task.OnTaskCompleted += counterCall.PlusOne;
                task.Discard();
            }
            return asyncTask;
        }
        /// <summary>
        /// 等待任意一个完成即可
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static CoTask WaitAny(List<CoTask> tasks)
        {
            //申请计数器
            var counterCall = CounterCall.Create();
            //设置触发值
            counterCall.ClickValue = 1;
            //使用一次就自动回收
            counterCall.OnceRecycle = true;
            //申请异步任务
            var asyncTask = CoTask.Create();

            //计数器任务绑定，此步骤仅第一次从池申请有GC
            counterCall.OnClick += asyncTask.SetResult;
            //绑定异步任务到计数器，同样是仅第一次存在GC
            foreach (var task in tasks)
            {
                task.OnTaskCompleted += counterCall.PlusOne;
                task.Discard();
            }
            return asyncTask;
        }

        /// <summary>
        /// 等待任意一个完成即可
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static CoTask<T> WaitAny<T>(params CoTask<T>[] tasks)
        {
            //创建计数器
            var counterCall = CounterCall<T>.Create();
            counterCall.ClickValue = 1;
            counterCall.OnceRecycle = true;
            //申请异步任务
            var asyncTask = CoTask<T>.Create();

            //计数器任务绑定
            counterCall.OnClick += (x) => { asyncTask.SetResult(x[0]); };

            //绑定异步任务到计数器
            foreach (var task in tasks)
            {
                task.OnTaskCompleted += counterCall.PlusOne;
                task.Discard();
            }
            return asyncTask;
        }

        /// <summary>
        /// 等待任意一个完成即可
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static CoTask<T> WaitAny<T>(List<CoTask<T>> tasks)
        {
            //创建计数器
            var counterCall = CounterCall<T>.Create();
            counterCall.ClickValue = 1;
            counterCall.OnceRecycle = true;
            //申请异步任务
            var asyncTask = CoTask<T>.Create();

            //计数器任务绑定
            counterCall.OnClick += (x) => { asyncTask.SetResult(x[0]); };

            //绑定异步任务到计数器
            foreach (var task in tasks)
            {
                task.OnTaskCompleted += counterCall.PlusOne;
                task.Discard();
            }
            return asyncTask;
        }

        /// <summary>
        /// 等待全部完成
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static CoTask WaitAll(params CoTask[] tasks)
        {
            //申请计数器
            var counterCall = CounterCall.Create();
            //设置触发值
            counterCall.ClickValue = tasks.Length;
            //使用一次就回收
            counterCall.OnceRecycle = true;
            //申请任务
            var asyncTask = CoTask.Create();

            //绑定结束事件，仅第一次存在GC
            counterCall.OnClick += asyncTask.SetResult;
            //绑定计数器，仅第一次存在GC
            foreach (var task in tasks)
            {
                task.Discard();
                task.OnTaskCompleted += counterCall.PlusOne;
            }

            return asyncTask;
        }

        /// <summary>
        /// 等待全部完成
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static CoTask WaitAll(List<CoTask> tasks)
        {
            //申请计数器
            var counterCall = CounterCall.Create();
            counterCall.ClickValue = tasks.Count;
            counterCall.OnceRecycle = true;
            //申请任务
            var asyncTask = CoTask.Create();

            //绑定结束事件
            counterCall.OnClick += asyncTask.SetResult;
            //绑定计数器
            foreach (var task in tasks)
            {
                task.Discard();
                task.OnTaskCompleted += counterCall.PlusOne;
            }

            return asyncTask;
        }
        /// <summary>
        /// 等待全部完成
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static CoTask<T[]> WaitAll<T>(params CoTask<T>[] tasks)
        {
            //申请计数器
            var counterCall = CounterCall<T>.Create();
            //设置触发值
            counterCall.ClickValue = tasks.Length;
            //使用一次就回收
            counterCall.OnceRecycle = true;
            //申请任务
            var asyncTask = CoTask<T[]>.Create();

            //绑定结束事件
            counterCall.OnClick += (x) => { asyncTask.SetResult(x.ToArray()); };
            //绑定计数器，仅第一次存在GC
            foreach (var task in tasks)
            {
                task.Discard();
                task.OnTaskCompleted += counterCall.PlusOne;
            }

            return asyncTask;
        }
        /// <summary>
        /// 等待全部完成
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static CoTask<T[]> WaitAll<T>(List<CoTask<T>> tasks)
        {
            //申请计数器
            var counterCall = CounterCall<T>.Create();
            //设置触发值
            counterCall.ClickValue = tasks.Count;
            //使用一次就回收
            counterCall.OnceRecycle = true;
            //申请任务
            var asyncTask = CoTask<T[]>.Create();

            //绑定结束事件
            counterCall.OnClick += (x) => { asyncTask.SetResult(x.ToArray()); };
            //绑定计数器，仅第一次存在GC
            foreach (var task in tasks)
            {
                task.Discard();
                task.OnTaskCompleted += counterCall.PlusOne;
            }
            return asyncTask;
        }




    }
}
