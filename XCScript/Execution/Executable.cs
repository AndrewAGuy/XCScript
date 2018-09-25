using System.Collections.Generic;
using System.Text;
using XCScript.Functions.Exceptions;

namespace XCScript.Execution
{
    /// <summary>
    /// Represents a collection of executable statements
    /// </summary>
    public class Executable
    {
        private List<Statement> statements = new List<Statement>();

        internal void Add(Statement statement)
        {
            statements.Add(statement);
        }

        /// <summary>
        /// Evaluate each statement in the current context
        /// </summary>
        /// <param name="context"></param>
        public void Execute(Engine context)
        {
            var i = 0;
            try
            {
                for (; i < statements.Count; ++i)
                {
                    statements[i].Execute(context);
                }
            }
            catch (ExecutionException e)
            {
                var s = statements[i];
                e.Line = s.Line;
                e.Description = s.Description;
                throw;
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
