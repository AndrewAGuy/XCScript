using System.Text;
using XCScript.Execution;

namespace XCScript.Arguments
{
    internal class ExecutableArgument : IArgument
    {
        public Executable Value { get; private set; } = new Executable();

        public object Literal
        {
            get
            {
                return this.Value;
            }
        }

        public object Evaluate(Engine context)
        {
            this.Value.Execute(context);
            return null;
        }

        public override string ToString()
        {
            return new StringBuilder().Append('<')
                .Append(this.Value.ToString()).Append('>').ToString();
        }
    }
}
