namespace TrueLayerAccess.Dtos
{
    public class Result<T>
    {
        public Result()
        {

        }

        public Result(T[] results)
        {
            this.results = results;
        }

        public T[] results { get; private set; }
    }
}