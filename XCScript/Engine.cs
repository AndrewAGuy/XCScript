using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using XCScript.Execution;
using XCScript.Functions;
using XCScript.Functions.Access;
using XCScript.Functions.Control;
using XCScript.Functions.Exceptions;
using XCScript.Functions.Execution;
using XCScript.Functions.Logical;
using XCScript.Functions.Numeric;
using XCScript.Functions.Plugins;
using XCScript.Parsing.Exceptions;
using XCScript.Plugins;

namespace XCScript
{
    /// <summary>
    /// The containing class for execution of scripts and command lines
    /// </summary>
    public class Engine
    {
        private readonly Dictionary<string, object> globals = new Dictionary<string, object>();
        private readonly Dictionary<string, IFunction> functions = new Dictionary<string, IFunction>();
        private readonly Result result = new Result();
        private readonly Manager plugins = new Manager();

        /// <summary>
        /// Key for <see cref="Functions"/> in <see cref="Globals"/>
        /// </summary>
        public static string FKey
        {
            get
            {
                return "/f";
            }
        }

        /// <summary>
        /// Key for <see cref="Result"/> in <see cref="Globals"/>
        /// </summary>
        public static string RKey
        {
            get
            {
                return "/r";
            }
        }

        /// <summary>
        /// Key for <see cref="Plugins"/> in <see cref="Globals"/>
        /// </summary>
        public static string PKey
        {
            get
            {
                return "/p";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Engine(LoadingOptions opt = null)
        {
            globals[FKey] = functions;
            globals[PKey] = plugins;
            globals[RKey] = result;
            if (opt != null)
            {
                var funcs = new List<IFunction>();
                if (opt.Access)
                {
                    funcs.AddRange(new IFunction[]
                    {
                        new Delete(),
                        new Error(),
                        new Index(),
                        new Map()
                    });
                }
                if (opt.Control)
                {
                    funcs.AddRange(new IFunction[]
                    {
                        new Do(),
                        new If(),
                        new Throw(),
                        new Until(),
                        new While()
                    });
                }
                if (opt.Execution)
                {
                    funcs.AddRange(new IFunction[]
                    {
                        new Execute(),
                        new Interpret()
                    });
                }
                if (opt.Plugins)
                {
                    funcs.AddRange(new IFunction[] 
                    {
                        new Alias(),
                        new New(),
                        new Property()
                    });
                }
                if (opt.Numeric)
                {
                    funcs.AddRange(new IFunction[] 
                    {
                        new Add(),
                        new Divide(),
                        new Equal(),
                        new Greater(),
                        new GreaterEqual(),
                        new Less(),
                        new LessEqual(),
                        new Multiply(),
                        new NotEqual(),
                        new Subtract()
                    });
                }
                if (opt.Logical)
                {
                    funcs.AddRange(new IFunction[]
                    {
                        new And(),
                        new Nand(),
                        new Nor(),
                        new Not(),
                        new Or(),
                        new Xnor(),
                        new Xor()
                    });
                }
                foreach (var f in funcs)
                {
                    functions[f.Keyword] = f;
                }
            }
        }

        /// <summary>
        /// Execution context
        /// </summary>
        public Dictionary<string, object> Globals
        {
            get
            {
                return globals;
            }
        }

        /// <summary>
        /// Loaded functions. Stored in <see cref="Globals"/> as <see cref="FKey"/>
        /// </summary>
        public Dictionary<string, IFunction> Functions
        {
            get
            {
                return functions;
            }
        }

        /// <summary>
        /// Execution result. Stored in <see cref="Globals"/> as <see cref="RKey"/>
        /// </summary>
        public Result Result
        {
            get
            {
                return result;
            }
        }

        /// <summary>
        /// Plugin type manager. Stored in <see cref="Globals"/> as <see cref="PKey"/>
        /// </summary>
        public Manager Plugins
        {
            get
            {
                return plugins;
            }
        }

        private Result LoadFunctions(Assembly assy)
        {
            var res = new Result();
            var valid = assy.DefinedTypes.Where(t => typeof(IFunction).IsAssignableFrom(t) && !t.IsInterface);
            foreach (var vt in valid)
            {
                try
                {
                    var inst = vt.Assembly.CreateInstance(vt.FullName) as IFunction;
                    if (!functions.ContainsKey(inst.Keyword))
                    {
                        functions[inst.Keyword] = inst;
                    }
                    else
                    {
                        res.Messages.Add($"Function with keyword: {inst.Keyword} already exists");
                    }
                }
                catch (Exception e)
                {
                    res.Success = false;
                    res.Messages.Add($"Failed to instantiate: {vt.FullName}, threw {e.GetType().FullName}");
                }
            }
            return res;
        }

        /// <summary>
        /// Interprets a text source into an executable item
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public Executable Interpret(TextReader reader)
        {
            var exec = Parsing.Evaluatable.Parse(reader, functions);
            reader.Dispose();
            return exec;
        }

        /// <summary>
        /// Interprets and executes the given text source, evaluating and resetting the global results
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public Result Execute(TextReader reader)
        {
            try
            {
                Interpret(reader).Execute(globals);
                // Get warnings
                var res = new Result().Append(result);
                result.Reset();
                return res;
            }
            catch (ParsingException p)
            {
                // No execution to worry about
                return new Result(false, $"Caught parsing exception ({p.GetType().Name}): {p.Message}");
            }
            catch (ExecutionException e)
            {
                // Get execution warnings, then error
                var res = new Result().Append(result);
                res.Append(new Result(false, $"Caught execution exception ({e.GetType().Name}): {e.Message}"), true);
                result.Reset();
                return res;
            }
        }

        /// <summary>
        /// Loads all plugin inheriting types and functions from the assembly
        /// </summary>
        /// <param name="assy"></param>
        /// <returns></returns>
        public Result LoadAssembly(Assembly assy)
        {
            plugins.FromAssembly(assy);
            return LoadFunctions(assy);
        }

        /// <summary>
        /// Loads a file as an <see cref="Assembly"/>, then calls <see cref="LoadAssembly(Assembly)"/>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Result LoadFile(string path)
        {
            try
            {
                var assy = Assembly.LoadFile(Path.GetFullPath(path));
                return LoadAssembly(assy);
            }
            catch
            {
                return new Result(false, "Could not load " + path);
            }
        }

        /// <summary>
        /// Loads each file in <paramref name="path"/> that ends in .dll
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Result LoadDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return new Result(false, "No such directory: " + path);
            }
            var dlls = Directory.GetFiles(path).Where(f => Path.GetExtension(f).ToLower() == ".dll");
            if (dlls.Count() == 0)
            {
                return new Result();
            }

            // Success is conserved, so any good load will convert this to true
            var res = new Result() { Success = false };
            foreach (var p in dlls)
            {
                res.Append(LoadFile(p));
            }

            return res;
        }
    }
}
