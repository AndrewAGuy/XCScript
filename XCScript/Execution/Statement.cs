namespace XCScript.Execution
{
    internal abstract class Statement
    {
        public abstract void Execute(Engine context);

        public int Line { get; private set; }

        public abstract string Description { get; }

        public Statement(int line)
        {
            this.Line = line;
        }
    }
}
