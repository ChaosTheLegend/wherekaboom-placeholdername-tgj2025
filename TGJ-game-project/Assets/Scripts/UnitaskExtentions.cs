using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace DefaultNamespace
{
    public static class UnitaskExtensions
    {
        /// <summary>
        /// I forgor 💀
        /// </summary>
        /// <param name="task"></param>
        public static void Forgor(this UniTaskVoid task)
        {
            task.Forget();
        }
        
        /// <summary>
        /// I forgor 💀
        /// </summary>
        /// <param name="task"></param>
        public static void Forgor(this UniTask task)
        {
            task.Forget();
        }
        
    }
}