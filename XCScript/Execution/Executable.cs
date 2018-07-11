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
        /// <param name="context"></param>
        public void Execute(Engine context)
        {
            foreach (var statement in statements)
            {
                statement.Execute(context);
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
