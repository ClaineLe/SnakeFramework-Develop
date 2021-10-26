using com.halo.framework.runtime;
using System;
using System.Collections.Generic;

namespace com.halo.framework
{
    namespace runtime
    {
        /// <summary>
        /// 任务序列
        /// </summary>
        public static class HaloTask
        {
            /// <summary>
            /// 执行任务序列
            /// 其中有一个报错会直接执行final
            /// </summary>
            /// <param name="taskList"></param>
            /// <param name="final"></param>
            public static void Run(this List<Action<Action<FrameworkStateCode>>> taskList,
                Action<FrameworkStateCode> final)
            {
                if (taskList == null)
                {
                    final?.Invoke(FrameworkStateCode.Succeed);
                    return;
                }

                var count = taskList.Count;
                if (count == 0)
                {
                    final?.Invoke(FrameworkStateCode.Succeed);
                    return;
                }

                var list = new List<SingleHaloTask>();
                for (var i = 0; i < count; i++)
                {
                    var curItem = taskList[i];
                    var cur = new SingleHaloTask(curItem, final);
                    list.Add(cur);
                }

                count = list.Count;
                for (var i = 0; i < count; i++)
                {
                    var nextIndex = i + 1;
                    if (nextIndex >= count) break;
                    var curItem = list[i];
                    var nextItem = list[nextIndex];
                    curItem.SetNextTask(nextItem);
                }

                list[0].Run();
            }

            /// <summary>
            /// 执行任务序列
            /// 如果途中有任务报错，会从头执行该任务，直到该任务成功
            /// </summary>
            /// <param name="taskList"></param>
            /// <param name="final"></param>
            public static void RunUntilSucceed(this List<Action<Action<FrameworkStateCode>>> taskList,
                Action<FrameworkStateCode> final, Action<FrameworkStateCode, Action> onTaskFail)
            {
                if (taskList == null) return;
                var count = taskList.Count;
                if (count == 0) return;
                var list = new List<SingleHaloTask>();
                for (var i = 0; i < count; i++)
                {
                    var curItem = taskList[i];
                    var cur = new SingleHaloTask(curItem, final, onTaskFail);
                    list.Add(cur);
                }

                count = list.Count;
                for (var i = 0; i < count; i++)
                {
                    var nextIndex = i + 1;
                    if (nextIndex >= count) break;
                    var curItem = list[i];
                    var nextItem = list[nextIndex];
                    curItem.SetNextTask(nextItem);
                }

                list[0].RunUntilSucceed();
            }
        }

        /// <summary>
        /// 单个任务
        /// </summary>
        public class SingleHaloTask
        {
            /// <summary>
            /// 当前任务
            /// </summary>
            private Action<Action<FrameworkStateCode>> task;

            /// <summary>
            /// 下一个
            /// </summary>
            private SingleHaloTask next;

            /// <summary>
            /// 最终任务
            /// </summary>
            private Action<FrameworkStateCode> final;

            /// <summary>
            /// 当任务出错时
            /// </summary>
            private Action<FrameworkStateCode, Action> onTaskFail;

            /// <summary>
            /// 构造方法
            /// </summary>
            /// <param name="task"></param>
            /// <param name="final"></param>
            public SingleHaloTask(Action<Action<FrameworkStateCode>> task, Action<FrameworkStateCode> final,
                Action<FrameworkStateCode, Action> onTaskFail = null)
            {
                this.task = task;
                this.final = final;
                this.onTaskFail = onTaskFail;
            }

            /// <summary>
            /// 设置下一个任务
            /// </summary>
            /// <param name="next"></param>
            public void SetNextTask(SingleHaloTask next)
            {
                this.next = next;
            }

            /// <summary>
            /// 执行
            /// </summary>
            public void Run()
            {
                task.Invoke(afterCode =>
                {
                    if (afterCode != FrameworkStateCode.Succeed)
                    {
                        final.Invoke(afterCode);
                        return;
                    }

                    if (next != null)
                    {
                        next.Run();
                        return;
                    }

                    final.Invoke(afterCode);
                });
            }

            /// <summary>
            /// 一直执行
            /// 直到成功后才执行下一个
            /// </summary>
            public void RunUntilSucceed()
            {
                task.Invoke(code =>
                {
                    if (code != FrameworkStateCode.Succeed)
                    {
                        if (onTaskFail != null)
                        {
                            onTaskFail(code, RunUntilSucceed);
                            return;
                        }

                        RunUntilSucceed();
                        return;
                    }

                    if (next != null)
                    {
                        next.RunUntilSucceed();
                        return;
                    }

                    final.Invoke(code);
                });
            }
        }
    }
}