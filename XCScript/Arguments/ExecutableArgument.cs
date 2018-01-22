using System.Collections.Generic;
using System.Text;
using XCScript.Execution;

namespace XCScript.Arguments
{
    internal class ExecutableArgument : IArgument
    {
        public Executable Value { get; private set; } = new Executable();

        public object Evaluate(Dictionary<string, object> globals)
        {
            this.Value.Execute(globals);
            return null;
        }

        public override string ToString()
        {
            return new StringBuilder().Append('<')
                .Append(this.Value.ToString()).Append('>').ToString();
        }
    }
}
