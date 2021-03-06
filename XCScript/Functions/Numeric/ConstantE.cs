﻿using System;
using XCScript.Arguments;

namespace XCScript.Functions.Numeric
{
    internal class ConstantE : IFunction
    {
        public string Keyword
        {
            get
            {
                return "e";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            return Math.E;
        }
    }
}
