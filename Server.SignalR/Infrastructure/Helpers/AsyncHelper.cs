using System;
using System.Threading;
using System.Threading.Tasks;

namespace Server.SignalR.Infrastructure.Helpers
{
    public static class AsyncHelper
    {
        private static readonly TaskFactory AppTaskFactory = new 
            TaskFactory(CancellationToken.None,
                        TaskCreationOptions.None,
                        TaskContinuationOptions.None,
                        TaskScheduler.Default);


        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AppTaskFactory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
             AppTaskFactory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }
    }
}
