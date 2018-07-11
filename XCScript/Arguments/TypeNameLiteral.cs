namespace XCScript.Arguments
{
    internal class TypeNameLiteral : IArgument
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
            return this.Value;
        }

        public override string ToString()
        {
            return "/" + this.Value;
        }
    }
}
