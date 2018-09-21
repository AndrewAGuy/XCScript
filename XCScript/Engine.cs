using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using XCScript.Execution;
using XCScript.Functions;
using XCScript.Functions.Exceptions;
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
        /// 
        /// </summary>
        public static int Error => -1;

        /// <summary>
        /// 
        /// </summary>
        public static int Warning => 1;

        /// <summary>
        /// 
        /// </summary>
        public static int Information => 0;

        /// <summary>
        /// The message logging event
        /// </summary>
        public event LogHandler LogEvent;

        /// <summary>
        /// Invokes the logging event
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        public void Log(string msg, int code)
        {
            LogEvent?.Invoke(this, msg, code, DateTime.Now);
        }

        /// <summary>
        /// Default with code provided by <see cref="Information"/>
        /// </summary>
        /// <param name="msg"></param>
        public void Log(string msg)
        {
            Log(msg, Information);
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
            return typeof(Engine).FullName + (!string.IsNullOrWhiteSpace(this.Name) ? $": {this.Name}" : "");
        }

        private readonly Dictionary<string, object> globals = new Dictionary<string, object>();
        private readonly Dictionary<string, IFunction> functions = new Dictionary<string, IFunction>();
        private readonly Manager plugins = new Manager();

        /// <summary>
        /// Allows users to attach arbitrary, fast-access data for use in functions, if program structure is fixed.
        /// </summary>
        public object[] Data { get; private set; } = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadAll"></param>
        /// <param name="dataItems"></param>
        public Engine(bool loadAll = true, int dataItems = 0)
        {
            if (loadAll)
            {
                LoadAssembly(typeof(Engine).Assembly);
            }
            if (dataItems > 0)
            {
                this.Data = new object[dataItems];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        public void LoadFunction(IFunction func)
        {
            functions[func.Keyword] = func;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcs"></param>
        public void LoadFunctions(IEnumerable<IFunction> funcs)
        {
            foreach (var func in funcs)
            {
                LoadFunction(func);
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
        /// Loaded functions
        /// </summary>
        public Dictionary<string, IFunction> Functions
        {
            get
            {
                return functions;
            }
        }

        /// <summary>
        /// Plugin type manager
        /// </summary>
        public Manager Plugins
        {
            get
            {
                return plugins;
            }
        }

        private bool LoadFunctions(Assembly assy)
        {
            var res = true;
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
                        Log($"Function with keyword: {inst.Keyword} already exists", Warning);
                    }
                }
                catch (Exception e)
                {
                    res = false;
                    Log($"Failed to instantiate: {vt.FullName}, threw {e.GetType().FullName}", Error);
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
        public bool InterpretFile(string path, out Executable executable)
        {
            CharSource source = null;
            StreamReader text = null;
            executable = null;
            try
            {
                text = File.OpenText(path);
                source = new CharSource(text);
                executable = Evaluatable.Parse(source, functions);
                return true;
            }
            catch (ParsingException p)
            {
                Log($"Caught {p.GetType().Name} in '{path}' at line {source.Line}: {p.Message}", Error);
                return false;
            }
            catch (Exception e)
            {
                Log($"Caught {e.GetType().Name}: {e.Message}", Error);
                return false;
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
        public bool InterpretString(string text, out Executable executable)
        {
            var source = new CharSource(new StringReader(text));
            executable = null;
            try
            {
                executable = Evaluatable.Parse(source, functions);
                return true;
            }
            catch (ParsingException p)
            {
                Log($"Caught {p.GetType().Name} at line {source.Line}: {p.Message}", Error);
                return false;
            }
            catch (Exception e)
            {
                Log($"Caught {e.GetType().Name}: {e.Message}", Error);
                return false;
            }
        }

        /// <summary>
        /// Interprets and executes the given text source, evaluating and resetting the global results
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isPath"></param>
        /// <returns></returns>
        public bool Execute(string data, bool isPath = false)
        {
            if (!(isPath ? InterpretFile(data, out var exec) : InterpretString(data, out exec)))
            {
                return false;
            }

            try
            {
                exec.Execute(this);
            }
            catch (ExecutionException e)
            {
                Log($"Caught {e.GetType().Name}: {e.Message}", Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Loads all plugin inheriting types and functions from the assembly
        /// </summary>
        /// <param name="assy"></param>
        /// <returns></returns>
        public bool LoadAssembly(Assembly assy)
        {
            plugins.FromAssembly(assy);
            return LoadFunctions(assy);
        }

        /// <summary>
        /// Loads a file as an <see cref="Assembly"/>, then calls <see cref="LoadAssembly(Assembly)"/>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool LoadFile(string path)
        {
            try
            {
                var assy = Assembly.LoadFile(Path.GetFullPath(path));
                return LoadAssembly(assy);
            }
            catch (Exception e)
            {
                Log($"Could not load '{path}', failed with {e.GetType().Name}: {e.Message}", Error);
                return false;
            }
        }

        /// <summary>
        /// Loads each file in <paramref name="path"/> that ends in .dll
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool LoadDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Log($"No such directory: {path}", Warning);
                return false;
            }
            var dlls = Directory.GetFiles(path).Where(f => Path.GetExtension(f).ToLower() == ".dll");
            if (dlls.Count() == 0)
            {
                return true;
            }

            // Success is conserved, so any good load will convert this to true
            var res = false;
            foreach (var p in dlls)
            {
                res |= LoadFile(p);
            }

            return res;
        }
    }
}
