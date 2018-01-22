using System.Collections.Generic;
using System.Text;

namespace XCScript.Execution
{
    /// <summary>
    /// Represents a collection of executable statements
    /// </summary>
    public class Executable
    {
        private List<IStatement> statements = new List<IStatement>();

        internal void Add(IStatement statement)
        {
            statements.Add(statement);
        }

        /// <summary>
        /// Evaluate each statement in the current context
        /// </summary>
        /// <param name="globals"></param>
        public void Execute(Dictionary<string, object> globals)
        {
            foreach (var statement in statements)
            {
                statement.Execute(globals);
            }
        }

        /// <summary>
        /// Returns code that would be interpreted to the exact same executable
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var str = new StringBuilder();
            var last = statements.Count - 1;
            for (var i = 0; i < last; ++i)
            {
                str.Append(statements[i].ToString());
                str.Append('\n');
            }
            str.Append(statements[last].ToString());
            return str.ToString();
        }
    }
}
