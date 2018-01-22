﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XCScript.Execution;
using XCScript.Functions;
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
        private readonly Manager plugins = new Manager();

        /// <summary>
        /// Default constructor
        /// </summary>
        public Engine()
        {
            globals["/f"] = functions;
            globals["/p"] = plugins;
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
        /// Loaded functions. Stored in <see cref="Globals"/> as /f
        /// </summary>
        public Dictionary<string, IFunction> Functions
        {
            get
            {
                return functions;
            }
        }

        /// <summary>
        /// Plugin type manager. Stored in <see cref="Globals"/> as /p
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
                        inst.Initialise(globals);
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
            var exec = Parsing.Evaluatable.Parse(reader, this.Functions);
            reader.Dispose();
            return exec;
        }

        public void Execute(TextReader reader)
        {
            Interpret(reader).Execute(this.Globals);
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