using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsScala.Translations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsScala
{
    public static class WriteImports
    {

        static public string StandardImports =
@"
import scala.util.control._;
import scala.reflect.ClassTag;
import CsRoot._;
import anonymoustypes._;
import System.CsScala;
import System.CsRef;
import System.CsLock;
import System.CsObject;

";


        public static void Go(ScalaWriter writer)
        {
            writer.WriteLine(StandardImports);
        }
    }
}
