using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsScala;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Test
{
    public static class TestFramework
    {

        public static void TestCode(string testName, string cSharp, string expectedOutput)
        {
            TestCode(testName, new[] { cSharp }, new[] { expectedOutput });
        }
        public static void TestCode(string testName, string cSharp, IEnumerable<string> expectedOutput)
        {
            TestCode(testName, new[] { cSharp }, expectedOutput);
        }
        public static void TestCode(string testName, IEnumerable<string> cSharp, string expectedOutput)
        {
            TestCode(testName, cSharp, new[] { expectedOutput });
        }
        public static void TestCode(string testName, IEnumerable<string> cSharp, IEnumerable<string> expectedOutput, params string[] extraTranslation)
        {
            var dir = Path.Combine(Path.GetTempPath(), "CSSCALA", testName);

            if (Directory.Exists(dir))
                foreach (var existing in Directory.GetFiles(dir, "*.scala", SearchOption.AllDirectories))
                {
                    File.SetAttributes(existing, FileAttributes.Normal); //clear read only flag so we can delete it
                    File.Delete(existing);
                }

            Console.WriteLine("Parsing into " + dir);

            var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location); //https://roslyn.codeplex.com/discussions/541557
            var compilation = CSharpCompilation.Create(testName, options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)) //dll so we don't require a main method
                .AddReferences(
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "mscorlib.dll")),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.dll")),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Core.dll"))
                )
                .AddSyntaxTrees(cSharp.Select(o => CSharpSyntaxTree.ParseText(o)));

            CsScala.Program.Go(compilation, dir, extraTranslation);

            Func<string, string> strip = i => Regex.Replace(i, "[\r\n \t]+", " ").Trim();

            var scalaFilesFromDisk = Directory.GetFiles(dir, "*.scala", SearchOption.AllDirectories)
                .Where(o => Path.GetFileName(o) != "Constructors.scala")
                .Select(File.ReadAllText)
                .Select(strip)
                .OrderBy(o => o)
                .ToList();

            var expectedOutputStripped = expectedOutput
                .Select(strip)
                .OrderBy(o => o)
                .ToList();

            Assert.AreEqual(expectedOutputStripped.Count, scalaFilesFromDisk.Count, "Incorrect number of files in " + dir);

            for (int i = 0; i < expectedOutputStripped.Count; i++)
            {
                if (expectedOutputStripped[i] != scalaFilesFromDisk[i])
                {
                    var err = new StringBuilder();

                    err.AppendLine("Code different");
                    err.AppendLine("---------------Expected----------------");
                    err.AppendLine(expectedOutputStripped[i].Ellipse(1024));
                    err.AppendLine("---------------Actual----------------");
                    err.AppendLine(scalaFilesFromDisk[i].Ellipse(1024));

                    var at = DifferentAt(expectedOutputStripped[i], scalaFilesFromDisk[i]);
                    err.AppendLine("Different at " + at);

                    var sub = at - 15;
                    if (sub > 0)
                    {

                        err.AppendLine("---------------Expected after " + sub + "----------------");
                        err.AppendLine(expectedOutputStripped[i].SubstringSafe(sub, 30));
                        err.AppendLine("---------------Actual after " + sub + "----------------");
                        err.AppendLine(scalaFilesFromDisk[i].SubstringSafe(sub, 30));
                    }
                    throw new Exception(err.ToString());
                }

            }
        }

        private static int DifferentAt(string p1, string p2)
        {
            for (int i = 0; i < p1.Length && i < p2.Length; i++)
            {
                if (p1[i] != p2[i])
                    return i;
            }

            return Math.Min(p1.Length, p2.Length);
        }

        public static void ExpectException(Action a, string exceptionText)
        {
            try
            {
                a();
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains(exceptionText))
                    return;
                else
                    throw new Exception("Expected exception that contains " + exceptionText, ex);
            }

            throw new Exception("Expected exception, but got none");
        }
    }
}
