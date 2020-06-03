using MG.Posh.Extensions.Writes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace MG.Posh.Extensions.Progress
{

    /// <summary>
    /// A static class that extends <see cref="Cmdlet.WriteProgress(ProgressRecord)"/>.
    /// </summary>
    public static class ProgressExtensions
    {
        public static string GetCurrentItemString<T1, T2>(int current, IList<T1> itemList, Func<T1, T2> propertyFunc)
            where T2 : IConvertible
        {
            T1 item = itemList[current];
            IConvertible icon = propertyFunc(item);
            return Convert.ToString(icon);
        }
        public static string GetCurrentItemString<T1, T2>(int current, T1[] itemArray, Func<T1, T2> propertyFunc)
            where T2 : IConvertible
        {
            T1 item = itemArray[current];
            IConvertible icon = propertyFunc(item);
            return Convert.ToString(icon);
        }

        public static void PerformAsyncProgress<T1, T2>(this T1 cmdlet, IProgressKeeper keeper, IList<Task<T2>> taskList, 
            Func<T2, ErrorRecord> actionOnError,
            int threadSleep = 100)
            where T1 : Cmdlet
        {
            ProgressRecord pr = keeper.GetRecord(0);

            cmdlet.WriteProgress(pr);
            while (taskList.Count > 0)
            {
                for (int i = taskList.Count - 1; i >= 0; i--)
                {
                    Task<T2> task = taskList[i];
                    if (task.IsCompleted)
                    {
                        if ((task.IsFaulted || task.IsCanceled ) && task.Result != null)
                        {
                            cmdlet.WriteError(actionOnError(task.Result));
                        }
                        else
                        {
                            WriteExtensions.WriteError(cmdlet, "No task result was received.", typeof(InvalidOperationException), ErrorCategory.InvalidOperation, task);
                        }
                    }
                }
                Thread.Sleep(threadSleep);

                keeper.Continue(keeper.TotalCount - taskList.Count, ref pr);
                cmdlet.WriteProgress(pr);
            }
        }

        public static List<T2> PerformAsyncProgressAndReturn<T1, T2>(this T1 cmdlet, IProgressKeeper keeper, IList<Task<T2>> taskList,
            Func<Task<T2>, bool> successCondition, Func<Task<T2>, ErrorRecord> actionOnError,
            int threadSleep = 100)
            where T1 : Cmdlet
        {
            var list = new List<T2>(taskList.Count);
            ProgressRecord pr = keeper.GetRecord(0);

            cmdlet.WriteProgress(pr);
            while (taskList.Count > 0)
            {
                for (int i = taskList.Count - 1; i >= 0; i--)
                {
                    Task<T2> task = taskList[i];
                    if (task.IsCompleted)
                    {
                        if (successCondition(task))
                        {
                            list.Add(task.Result);
                        }
                        else
                        {
                            cmdlet.WriteError(actionOnError(task));
                        }
                        taskList.Remove(task);
                    }
                }
                Thread.Sleep(threadSleep);

                keeper.ContinueReverse(taskList.Count, ref pr);
                cmdlet.WriteProgress(pr);
            }
            keeper.Finish(ref pr);
            cmdlet.WriteProgress(pr);
            return list;
        }


        //public static void WriteProgress<T>(this T cmdlet, int id, int current, int total)
        //public static void WriteProgress<T>(this T cmdlet, IProgressKeeper keeper, int current)
        //{

        //}
        //private static void UpdateProgress(Cmdlet cmdlet, ProgressRecord progRec, bool enabled, int percentComplete)
        //{
        //    if (enabled)
        //    {
        //        progRec.PercentComplete = percentComplete;

        //        cmdlet.WriteProgress(progRec);
        //    }
        //}
    }
}
