using System.Collections.Generic;

namespace XCScript.Execution
{
    internal interface IStatement
    {
        void Execute(Dictionary<string, object> globals);
    }
}
