namespace FastIncrementalAnalysis
{
    public class Bounds<T, TDelta>
    {
        public T InitialValue { get; set; }

        public TDelta Step { get; set; }

        public T MaxValue { get; set; }
    }
}