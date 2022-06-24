using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CSharp;

namespace GB.SUTEL.Reglas
{
    public class ResultadoMatematico : IDisposable
    {
        private readonly ArrayList _mathMembers = new ArrayList();
        private readonly Hashtable _mathMembersMap = new Hashtable();
        private CodeGeneratorTests _compiladorRoslyn;
        /// <summary>
        /// Código a compilar dinamicamente
        /// </summary>
        private StringBuilder _source = new StringBuilder();

        public ResultadoMatematico()
        {
            GetMathMemberNames();
            ReglasEvaluar = new StringBuilder();
        }

        public List<VariablesProceso> VariablesProcesos { get; set; }

        /// <summary>
        ///     Contiene elñ código que va a procesar el motor
        /// </summary>
        public string CodigoProcesar { get; set; }

        /// <summary>
        ///     Muestra los errores de compilación en el caso de existir
        /// </summary>
        public string TrazaCompilacion { get; set; }

        /// <summary>
        ///     Contiene el resultado de la evaluación de la regla
        /// </summary>
        public string ResultadoEjecucion { get; set; }

        public StringBuilder ReglasEvaluar { get; set; }
        public string CodigoC { get; set; }

        private ICodeCompiler CreateCompiler()
        {
            //Create an instance of the C# compiler   
            CodeDomProvider codeProvider;
            codeProvider = new CSharpCodeProvider();
            var compiler = codeProvider.CreateCompiler();
            return compiler;
        }

        /// <summary>
        ///     Creawte parameters for compiling
        /// </summary>
        /// <returns></returns>
        private CompilerParameters CreateCompilerParameters()
        {
            //add compiler parameters and assembly references
            var compilerParams = new CompilerParameters();
            compilerParams.CompilerOptions = "/target:library /optimize";
            compilerParams.GenerateExecutable = false;
            compilerParams.GenerateInMemory = true;
            compilerParams.IncludeDebugInformation = false;
            compilerParams.ReferencedAssemblies.Add("mscorlib.dll");
            compilerParams.ReferencedAssemblies.Add("System.dll");
            compilerParams.ReferencedAssemblies.Add("System.Windows.Forms.dll");

            //add any aditional references needed
            //            foreach (string refAssembly in code.References)
            //              compilerParams.ReferencedAssemblies.Add(refAssembly);

            return compilerParams;
        }

        /// <summary>
        ///     Writes the output to the text box on the win form
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="args"></param>
        private void WriteLine(string txt, params object[] args)
        {
            TrazaCompilacion += string.Format(txt, args) + "\r\n";
        }

        /// <summary>
        ///     Compiles the code from the code string
        /// </summary>
        /// <param name="compiler"></param>
        /// <param name="parms"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private CompilerResults CompileCode(ICodeCompiler compiler, CompilerParameters parms, string source)
        {
            //actually compile the code
            var results = compiler.CompileAssemblyFromSource(
                parms, source);

            //Do we have any compiler errors?
            if (results.Errors.Count > 0)
            {
                foreach (CompilerError error in results.Errors)
                    WriteLine("Compile Error:" + error.ErrorText);
                return null;
            }

            return results;
        }

        /// <summary>
        ///     Need to change eval string to use .NET Math library
        /// </summary>
        /// <param name="eval">evaluation expression</param>
        /// <returns></returns>
        private string RefineEvaluationString(string eval)
        {
            // look for regular expressions with only letters
            var regularExpression = new Regex("[a-zA-Z_]+");

            // track all functions and constants in the evaluation expression we already replaced
            var replacelist = new ArrayList();

            // find all alpha words inside the evaluation function that are possible functions
            var matches = regularExpression.Matches(eval);
            foreach (Match m in matches)
            {
                // if the word is found in the math member map, add a Math prefix to it
                var isContainedInMathLibrary = _mathMembersMap[m.Value.ToUpper()] != null;
                if (replacelist.Contains(m.Value) == false && isContainedInMathLibrary)
                    eval = eval.Replace(m.Value, "Math." + _mathMembersMap[m.Value.ToUpper()]);

                // we matched it already, so don't allow us to replace it again
                replacelist.Add(m.Value);
            }

            // return the modified evaluation string
            return eval;
        }

        private void InitializeFields()
        {
            ResultadoEjecucion = "";
            TrazaCompilacion = "";
        }

        /// <summary>
        ///     Compiles the c# into an assembly if there are no syntax errors
        /// </summary>
        /// <returns></returns>
        private CompilerResults CompileAssembly()
        {
            // create a compiler
            var compiler = CreateCompiler();
            // get all the compiler parameters
            var parms = CreateCompilerParameters();
            // compile the code into an assembly
            var results = CompileCode(compiler, parms, _source.ToString());
            return results;
        }

        private string addVariablesProceso()
        {
            var listadoVariables = new StringBuilder();
            foreach (var VARIABLE in VariablesProcesos) listadoVariables.AppendLine(VARIABLE.SalidaVariable());
            return listadoVariables.ToString();
        }


        /// <summary>
        ///     Inicializa el calculo solicitado
        /// </summary>
        public void Calcular(string ReglaNegocio)
        {
            ReglasEvaluar.AppendLine("answer;"); // Se mapea la variable que contendrá el resultado
            ReglasEvaluar.AppendLine(addVariablesProceso());
            ReglasEvaluar.AppendLine(ReglaNegocio);
            CodigoProcesar = ReglasEvaluar.ToString();

            // Blank out result fields and compile result fields
            InitializeFields();

            // change evaluation string to pick up Math class members
            var expression = RefineEvaluationString(ReglasEvaluar.ToString());

            // build the class using codedom
            BuildClass(expression);

            _compiladorRoslyn = new CodeGeneratorTests();
            _compiladorRoslyn._generatedCode = _source.ToString();
            CodigoC = _source.ToString();
            ResultadoEjecucion = Convert.ToString(_compiladorRoslyn.CallCalculatorMethod());
        }

        private void GetMathMemberNames()
        {
            // get a reflected assembly of the System assembly
            var systemAssembly = Assembly.GetAssembly(typeof(Math));
            try
            {
                //cant call the entry method if the assembly is null
                if (systemAssembly != null)
                {
                    //Use reflection to get a reference to the Math class

                    var modules = systemAssembly.GetModules(false);
                    var types = modules[0].GetTypes();

                    //loop through each class that was defined and look for the first occurrance of the Math class
                    foreach (var type in types)
                        if (type.Name == "Math")
                        {
                            // get all of the members of the math class and map them to the same member
                            // name in uppercase
                            var mis = type.GetMembers();
                            foreach (var mi in mis)
                            {
                                _mathMembers.Add(mi.Name);
                                _mathMembersMap[mi.Name.ToUpper()] = mi.Name;
                            }
                        }
                    //if the entry point method does return in Int32, then capture it and return it


                    //if it got here, then there was no entry point method defined.  Tell user about it
                }
            }
            catch (Exception ex)
            {
                TrazaCompilacion = "Error:  Se ha producido uan Excepción ejecutando la regla. " + ex;
            }
        }

        /// <summary>
        ///     Runs the Calculate method in our on-the-fly assembly
        /// </summary>
        /// <param name="results"></param>
        private void RunCode(CompilerResults results)
        {
            var executingAssembly = results.CompiledAssembly;
            try
            {
                //cant call the entry method if the assembly is null
                if (executingAssembly != null)
                {
                    var assemblyInstance = executingAssembly.CreateInstance("ExpressionEvaluator.Calculator");
                    //Use reflection to call the static Main function

                    var modules = executingAssembly.GetModules(false);
                    var types = modules[0].GetTypes();

                    //loop through each class that was defined and look for the first occurrance of the entry point method
                    foreach (var type in types)
                    {
                        var mis = type.GetMethods();
                        foreach (var mi in mis)
                            if (mi.Name == "Calculate")
                            {
                                var result = mi.Invoke(assemblyInstance, null);
                                ResultadoEjecucion = result.ToString();
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                TrazaCompilacion = "Error:  Se ha producido uan Excepción ejecutando la regla. " + ex;
            }
        }


        private CodeMemberField FieldVariable(string fieldName, string typeName, MemberAttributes accessLevel)
        {
            var field = new CodeMemberField(typeName, fieldName);
            field.Attributes = accessLevel;
            return field;
        }

        private CodeMemberField FieldVariable(string fieldName, Type type, MemberAttributes accessLevel)
        {
            var field = new CodeMemberField(type, fieldName);
            field.Attributes = accessLevel;
            return field;
        }

        /// <summary>
        ///     Very simplistic getter/setter properties
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="internalName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private CodeMemberProperty MakeProperty(string propertyName, string internalName, Type type)
        {
            var myProperty = new CodeMemberProperty();
            myProperty.Name = propertyName;
            myProperty.Comments.Add(
                new CodeCommentStatement(string.Format("The {0} property is the returned result", propertyName)));
            myProperty.Attributes = MemberAttributes.Public;
            myProperty.Type = new CodeTypeReference(type);
            myProperty.HasGet = true;
            myProperty.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), internalName)));

            myProperty.HasSet = true;
            myProperty.SetStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), internalName),
                    new CodePropertySetValueReferenceExpression()));

            return myProperty;
        }

        /// <summary>
        ///     Main driving routine for building a class
        /// </summary>
        private void BuildClass(string expression)
        {
            // need a string to put the code into
            _source = new StringBuilder();
            var sw = new StringWriter(_source);

            //Declare your provider and generator
            var codeProvider = new CSharpCodeProvider();
            var generator = codeProvider.CreateGenerator(sw);
            var codeOpts = new CodeGeneratorOptions();

            var myNamespace = new CodeNamespace("ExpressionEvaluator");
            myNamespace.Imports.Add(new CodeNamespaceImport("System"));
            //myNamespace.Imports.Add(new CodeNamespaceImport("System.Windows.Forms"));

            //Build the class declaration and member variables			
            var classDeclaration = new CodeTypeDeclaration();
            classDeclaration.IsClass = true;
            classDeclaration.Name = "Calculator";
            classDeclaration.Attributes = MemberAttributes.Public;
            classDeclaration.Members.Add(FieldVariable("answer", typeof(double), MemberAttributes.Private));

            //default constructor
            var defaultConstructor = new CodeConstructor();
            defaultConstructor.Attributes = MemberAttributes.Public;
            defaultConstructor.Comments.Add(new CodeCommentStatement("Default Constructor for class", true));
            defaultConstructor.Statements.Add(new CodeSnippetStatement("//TODO: implement default constructor"));
            classDeclaration.Members.Add(defaultConstructor);

            //property
            classDeclaration.Members.Add(MakeProperty("Answer", "answer", typeof(double)));

            //Our Calculate Method
            var myMethod = new CodeMemberMethod();
            myMethod.Name = "Calculate";
            myMethod.ReturnType = new CodeTypeReference(typeof(double));
            myMethod.Comments.Add(new CodeCommentStatement("Calculate an expression", true));
            myMethod.Attributes = MemberAttributes.Public;
            myMethod.Statements.Add(new CodeAssignStatement(new CodeSnippetExpression("Answer"),
                new CodeSnippetExpression(expression)));
            //>>Retorno de la variable Answer
            //myMethod.Statements.Add(
            //    new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(),
            //        "Answer")));
            //>>Retorno ajsutado variable Answer
            myMethod.Statements.Add(new
                CodeSnippetExpression("return Double.Parse(this.Answer.ToString(), new System.Globalization.CultureInfo(\"en-US\"));"));

            classDeclaration.Members.Add(myMethod);
            //write code
            myNamespace.Types.Add(classDeclaration);
            generator.GenerateCodeFromNamespace(myNamespace, sw, codeOpts);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        /// <summary>
        /// Finaliza el objeto
        /// </summary>
        public void Dispose()
        {
            _mathMembers.Clear();
            _mathMembersMap.Clear();
            _compiladorRoslyn.Dispose();
            _compiladorRoslyn = null;
        }
    }
}
