using System.Threading.Tasks;

namespace WebApplication1.Server
{
    public static class AsyncTupleFunctions
    {
        public static async Task<(T1, T2)> Convert<T1, T2>((T1, Task<T2>) tuple)
        {
            var (obj, task) = tuple;
            var taskRes = await task;
            return (obj, taskRes);
        }
    }
}