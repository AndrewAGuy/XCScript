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
using XCScript.Parsing;
using XCScript.Parsing.Exceptions;
using XCScript.Plugins;

namespace XCScript
{
    /// <summary>
    /// The containing class for execution of scripts and command lines
    /// </summary>
    public class Engine
    {
        /// <summary>
        /// Handles message logging such as console, stderr or gui output
        /// </summary>
        /// <param name="src"></param>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        /// <param name="time"></param>
        public delegate void LogHandler(object src, string msg, int code, DateTime time);

        /// <summary>
        /// The message logging event
        /// </summary>
        public event LogHandler LogEvent;

        /// <summary>
        /// Invokes the logging event
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        public void Log(string msg, int code = 0)
        {
            LogEvent?.Invoke(this, msg, code, DateTime.Now);
        }

        /// <summary>
        /// If multiple engines exist, attach a name to them for reporting
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// For reporting purposes
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return typeof(Engine).FullName + ": " + this.Name;
        }

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

        private void AddDefaults()
        {
            globals[FKey] = functions;
            globals[PKey] = plugins;
            globals[RKey] = result;
        }

        /// <summary>
        /// 
        /// </summary>
        public Engine(bool loadAll)
        {
            AddDefaults();
            if (loadAll)
            {
                LoadAssembly(typeof(Engine).Assembly);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Engine(LoadingOptions opt)
        {
            AddDefaults();
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
                        new Foreach(),
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
                        new ConstantE(),
                        new ConstantPi(),
                        new Divide(),
                        new Equal(),
                        new Greater(),
                        new GreaterEqual(),
                        new Less(),
                        new LessEqual(),
                        new Logarithm(),
                        new Multiply(),
                        new NotEqual(),
                        new Power(),
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
        /// <param name="path"></param>
        /// <param name="executable"></param>
        /// <returns></returns>
        public Result InterpretFile(string path, out Executable executable)
        {
            var res = new Result();
            CharSource source = null;
            StreamReader text = null;
            executable = null;
            try
            {
                text = File.OpenText(path);
                source = new CharSource(text);
                executable = Evaluatable.Parse(source, functions);
                return new Result();
            }
            catch (ParsingException p)
            {
                return new Result(false, $"Caught {p.GetType().Name} in '{path}' at line {source.Line}: {p.Message}");
            }
            catch (Exception e)
            {
                return new Result(false, $"Caught {e.GetType().Name}: {e.Message}");
            }
            finally
            {
                text.Dispose();
            }
        }

        /// <summary>
        /// For command line input
        /// </summary>
        /// <param name="text"></param>
        /// <param name="executable"></param>
        /// <returns></returns>
        public Result InterpretString(string text, out Executable executable)
        {
            var res = new Result();
            var source = new CharSource(new StringReader(text));
            executable = null;
            try
            {
                executable = Evaluatable.Parse(source, functions);
                return new Result();
            }
            catch (ParsingException p)
            {
                return new Result(false, $"Caught {p.GetType().Name} at line {source.Line}: {p.Message}");
            }
            catch (Exception e)
            {
                return new Result(false, $"Caught {e.GetType().Name}: {e.Message}");
            }
        }

        /// <summary>
        /// Interprets and executes the given text source, evaluating and resetting the global results
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isPath"></param>
        /// <returns></returns>
        public Result Execute(string data, bool isPath = false)
        {
            result.Reset();
            var res = isPath ? InterpretFile(data, out var exec) : InterpretString(data, out exec);
            if (!res.Success)
            {
                return res;
            }

            try
            {
                exec.Execute(this);
            }
            catch (ExecutionException e)
            {
                res = new Result(false, $"Caught {e.GetType().Name}: {e.Message}");
            }
            // Get warnings
            return res.Append(result);
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
            catch (Exception e)
            {
                return new Result(false, "Could not load " + path
                    + ", failed with " + e.GetType().Name + ": " + e.Message);
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
