namespace XCScript.Arguments
{
    internal class NameArgument : IArgument
    {
        public string Value { get; set; }

        public object Literal
        {
            get
            {
                return this.Value;
            }
        }

        public object Evaluate(Engine context)
        {
            return context.Globals.TryGetValue(this.Value, out var val) ? val : null;
        }

        public object EvaluateChildren(Engine context)
        {
            return context.Globals.TryGetValue(this.Value, out var val) ? val : null;
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}
