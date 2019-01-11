using System;
using System.Threading.Tasks;

namespace LuhisBanking.Client
{
    public static class FunctionWrappers
    {
        public static async Task SquashExceptions(Func<Task> a)
        {
            try
            {
                await a();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}