﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Build.Locator;

namespace CsScala
{
    public static class EntryPoint
    {
        public static void Main(string[] args)
        {
            try
            {

                Console.WriteLine("C# to Scala Converter\nSee http://github.com/FizzerWL/CsScala for full info and documentation.\n\n");

                if (args.Length == 0 || args.Any(o => o == "-?" || o == "--help" || o == "/?"))
                {
                    //Print usage
                    Console.WriteLine(
@"

Usage:
    csscala.exe  /sln:<path to solution file> [options] 


Options available:

	/out:<output directory>
		Directory to write scala files to.  If not specified, output will be written to the current working directory.

	/config:<configuration>
		The configuration within the passed solution file to use. (Debug, Release, etc.)

	/projects:<comma-delimited list of project names>
		If you don't want to convert all projects in the passed solution, you can provide a list of project names.  Only the projects named here will be converted.

	/extraTranslation:<path to xml file>
		Defines extra conversion parameters for use with this project.  See Translations.xml for examples.

	/define:<symbol>
		Adds extra pre-processor #define symbols to add to the project before building.
");
                    return;
                }

                var sourceFiles = new List<string>();
                var outDir = Directory.GetCurrentDirectory();
                var extraTranslations = new List<string>();
                string pathToSolution = null;
                string config = null;
                string projects = null;
                string[] extraDefines = new string[] { };

                foreach (var arg in args)
                {
                    if (arg.StartsWith("/extraTranslation:"))
                        extraTranslations.AddRange(arg.Substring(18).Split(';').Select(File.ReadAllText));
                    else if (arg.StartsWith("/out:"))
                        outDir = arg.Substring(5);
                    else if (arg.StartsWith("/sln:"))
                        pathToSolution = arg.Substring(5);
                    else if (arg.StartsWith("/config:"))
                        config = arg.Substring(8);
                    else if (arg.StartsWith("/projects:"))
                        projects = arg.Substring(10);
                    else if (arg.StartsWith("/define:"))
                        extraDefines = arg.Substring(8).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    else
                        throw new Exception("Invalid argument: " + arg);
                }

                if (pathToSolution == null)
                    throw new Exception("/sln parameter not passed");


                FixMsbuild();


                var workspace = MSBuildWorkspace.Create();

                var solution = workspace.OpenSolutionAsync(pathToSolution).Result;

                foreach (var diag in workspace.Diagnostics)
                    Console.WriteLine("Diagnostic: " + diag);

                var projectsList = solution.Projects.ToList();

                Console.WriteLine("All projects in solution: " + string.Join(", ", solution.Projects.Select(o => o.Name)));

                if (projects != null)
                    TrimList(projectsList, projects);

                if (extraDefines.Length > 0)
                    projectsList = projectsList.Select(p => p.WithParseOptions(new CSharpParseOptions(preprocessorSymbols:
                        p.ParseOptions.PreprocessorSymbolNames
                        .Concat(extraDefines.Where(z => z.StartsWith("-") == false))
                        .Except(extraDefines.Where(z => z.StartsWith("-")).Select(z => z.Substring(1)))
                        .ToArray())
                        )).ToList();

                foreach (var project in projectsList)
                {
                    Console.WriteLine("Converting project " + project.Name + "...");
                    var sw = Stopwatch.StartNew();
                    Program.Go(project.GetCompilationAsync().Result, outDir, extraTranslations);
                    Console.WriteLine("Finished project " + project.Name + " in " + sw.Elapsed);
                }

                Environment.ExitCode = 0;
            }
            catch (AggregateException agex)
            {
                Environment.ExitCode = 1;

                Exception ex = agex;
                while (ex is AggregateException)
                    ex = ((AggregateException)ex).InnerException;

                Console.WriteLine("\nException:");
                Console.WriteLine(ex);
            }
            catch (Exception ex)
            {
                Environment.ExitCode = 1;

                Console.WriteLine("\nException:");
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Workaround from https://github.com/dotnet/roslyn/issues/26029
        /// </summary>
        private static void FixMsbuild()
        {
             // Locates all of the instances of Visual Studio 2017 on the machine with MSBuild.
            var instances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
            if (!instances.Any())
                throw new Exception("No Visual Studio instances found.");


            Console.WriteLine("Visual Studio intances: " + string.Join(", ", instances.Select(o => o.Name + " -" + o.Version + " " + o.MSBuildPath)));


            //Console.WriteLine("Visual Studio intances:");

            //foreach (var instance in instances)
            //{
            //    Console.WriteLine($"  - {instance.Name} - {instance.Version}");
            //    Console.WriteLine($"    {instance.MSBuildPath}");
            //    Console.WriteLine();
            //}

            // We register the first instance that we found. This will cause MSBuildWorkspace to use the MSBuild installed in that instance.
            // Note: This has to be registered *before* creating MSBuildWorkspace. Otherwise, the MEF composition used by  MSBuildWorkspace will fail to compose.
            var registeredInstance = instances.ElementAt(0); //TODO: How do we know which to choose?
            MSBuildLocator.RegisterInstance(registeredInstance);

            Console.WriteLine($"Registered visual studio: {registeredInstance.Name} - {registeredInstance.Version}");
        }

        private static void TrimList(List<Project> projectsList, string projectsCsv)
        {
            var orig = projectsList.ToList();

            projectsList.Clear();
            foreach(var name in projectsCsv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                projectsList.Add(orig.Single(o => o.Name == name));
        }

    }
}
