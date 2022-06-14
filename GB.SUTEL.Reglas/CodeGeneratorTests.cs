using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GB.SUTEL.Reglas
{
     public class CodeGeneratorTests: IDisposable
    {
        public CodeGeneratorTests()
        {
        }

        public string _generatedCode { get; set; }
        private CSharpCompilation _compilation;
        private Assembly _generatedAssembly;
        /// <summary>
        /// Mensaje de error
        /// </summary>
        public string MensajeErr { get; set; }

        public double CallCalculatorMethod()
        {
            CreateCompilation();
            CompileAndLoadAssembly();
            var calculatorType = _generatedAssembly.GetType("ExpressionEvaluator.Calculator"); //Calculator.Calculator
             var calculatorInstance = Activator.CreateInstance(calculatorType);
            var calculateMethod = calculatorType.GetTypeInfo().GetDeclaredMethod("Calculate"); //AddIntegers
             var calculationResult = calculateMethod.Invoke(calculatorInstance, null);
            //Assert.IsType(typeof(int), calculationResult);
            return (double)calculationResult;
        }

        private void CreateCompilation()
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(_generatedCode);
            string assemblyName = Guid.NewGuid().ToString();
            var references = GetAssemblyReferences();
            var compilation = CSharpCompilation.Create(
                assemblyName,
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            _compilation = compilation;
        }

        private static IEnumerable<MetadataReference> GetAssemblyReferences()
        {
            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location)
                // A bit hacky, if you need it
                //MetadataReference.CreateFromFile(Path.Combine(typeof(object).GetTypeInfo().Assembly.Location, "..", "mscorlib.dll")),
            };
            return references;
        }

        private void CompileAndLoadAssembly()
        {
            using (var ms = new MemoryStream())
            {
                var result = _compilation.Emit(ms);
                ThrowExceptionIfCompilationFailure(result);
                ms.Seek(0, SeekOrigin.Begin);
                //var assembly = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(ms);
                var assembly = Assembly.Load(ms.ToArray());
//#if NET46
//                // Different in full .Net framework
//                var assembly = Assembly.Load(ms.ToArray());
//#endif
                _generatedAssembly = assembly;
            }
        }

        private void ThrowExceptionIfCompilationFailure(EmitResult result)
        {
            if (!result.Success)
            {
                var compilationErrors = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error)
                    .ToList();
                if (compilationErrors.Any())
                {
                    var firstError = compilationErrors.First();
                    var errorNumber = firstError.Id;
                    var errorDescription = firstError.GetMessage();
                    var firstErrorMessage = errorNumber + ":" + errorDescription;
                    throw new Exception("Compilation failed, first error is: {firstErrorMessage}");
                }
            }
        }

        public void Dispose()
        {
            _generatedCode = null;
            _compilation = null;
            _generatedAssembly = null;
    }
    }
}
