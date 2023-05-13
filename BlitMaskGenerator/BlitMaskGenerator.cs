/* Copyright © 2023 - Juho Veli-Matti Joensuu
 * 
 * See the attached LICENSE.md for licensing details (MIT licensing)
 */

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BlitMaskGenerators
{
    [Generator]
    public class BlitMaskGenerator : ISourceGenerator
    {
        private const string _templateTypeName = "uint";

        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            //if ( !System.Diagnostics.Debugger.IsAttached ) System.Diagnostics.Debugger.Launch();

            string templatesFolder = @"E:\Repositories\BlitMask\BlitMaskGenerator\Templates\";
            string[] templatePaths = Directory.GetFiles(templatesFolder, "*.cs");

            if ( !context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.projectdir", out var projectDirectory) )
            {
                throw new KeyNotFoundException("Could not find project directory");
            }

            bool compilingUnitTests = projectDirectory.Contains("BlitMaskTests");

            templatePaths = compilingUnitTests ?
                templatePaths.Where(path => path.Contains("BlitMaskUnitTestTemplate")).ToArray() :
                templatePaths.Where(path => !path.Contains("BlitMaskUnitTestTemplate")).ToArray();

            var templateSourceCode = new Dictionary<string, string>();

            foreach ( var templatePath in templatePaths )
            {
                string fileContents = File.ReadAllText(templatePath);

                templateSourceCode.Add(templatePath, fileContents);
            }

            List<int> SupportedBitSizes = new List<int>() { 32, 64 };

            Dictionary<string, string> pathAndSourceCode = new Dictionary<string, string>();

            foreach ( var template in templateSourceCode )
            {
                foreach ( int bitSize in SupportedBitSizes )
                {
                    var rootNode = (CompilationUnitSyntax)CSharpSyntaxTree.ParseText(template.Value, CSharpParseOptions.Default, template.Key, Encoding.UTF8).GetRoot(context.CancellationToken);

                    Dictionary<string, string> renameMap = new Dictionary<string, string>
                    {
                        { "BlitMaskConstantsTemplate", $"BlitMaskConstants{bitSize}" },
                        { "BlitMaskExtensionsTemplate", $"BlitMaskExtensions{bitSize}" },
                        { "BlitMaskTemplate", $"BlitMask{bitSize}" },
                        { "BlitMaskUnitTestTemplate", $"BlitMaskUnitTests{bitSize}" },
                        { "ToUInt32", $"{GenerationHelpers.GetConvertMethodName(bitSize)}" },
                        { "None", $"None{bitSize}" },
                        { "Everything", $"Everything{bitSize}" }
                    };

                    if ( GenerationHelpers.MapBitSizeToType(bitSize) is var typeString && _templateTypeName != typeString )
                    {
                        var typeRewriter = new TypeChangingRewriter(_templateTypeName, typeString);

                        rootNode = (CompilationUnitSyntax)typeRewriter.Visit(rootNode);
                    }

                    var renamingRewriter = new IdentifierRenamingRewriter(renameMap);

                    rootNode = (CompilationUnitSyntax)renamingRewriter.Visit(rootNode);

                    string sourceCode = rootNode.ToFullString();

                    sourceCode = sourceCode
                        .Replace(@"0x00000000", GenerationHelpers.GetHexValueForZero(bitSize))
                        .Replace(@"0xFFFFFFFF", GenerationHelpers.GetHexValueForMax(bitSize))
                        .Replace("1U", GenerationHelpers.GetValueForOne(bitSize));

                    string fileName = Path.GetFileNameWithoutExtension(template.Key.Replace("Template", $"{bitSize.ToString()}"));
                    string fileHint = fileName + ".g.cs";

                    pathAndSourceCode.Add(fileHint, sourceCode);
                }
            }

            foreach ( var sourceTextKey in pathAndSourceCode.Keys )
            {
                context.AddSource(sourceTextKey, pathAndSourceCode[sourceTextKey]);
            }
        }
    }

    public class GenerationHelpers
    {
        public static string GetHexValueForZero(int bitSize)
        {
            switch ( bitSize )
            {
                case 8: return "0x00";
                case 16: return "0x0000";
                case 32: return "0x00000000";
                case 64: return "0x0000000000000000";
                default: throw new ArgumentException($"Unsupported bit size: {bitSize}", nameof(bitSize));
            }
        }

        public static string GetHexValueForMax(int bitSize)
        {
            switch ( bitSize )
            {
                case 8: return "0xFF";
                case 16: return "0xFFFF";
                case 32: return "0xFFFFFFFF";
                case 64: return "0xFFFFFFFFFFFFFFFF";
                default: throw new ArgumentException($"Unsupported bit size: {bitSize}", nameof(bitSize));
            }
        }

        /// <summary>
        /// TODO; Since byte and ushort don't have a literal suffix or any other apparent
        /// way to assign them to variables for the purposes of Roslyn type identification
        /// Need another way to make the compiled code to not use casts, symbolic setting with SyntaxReceiver maybe?
        /// Sure is great they wanted to "preserve" characters o.9
        /// </summary>
        /// <param name="bitSize"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetValueForOne(int bitSize)
        {
            switch ( bitSize )
            {
                case 8: return "0b00000001";
                case 16: return "0x0001";
                case 32: return "1U";
                case 64: return "1UL";
                default: throw new ArgumentException($"Unsupported bit size: {bitSize}", nameof(bitSize));
            }
        }

        public static string GetConvertMethodName(int bitSize)
        {
            switch ( bitSize )
            {
                case 8:
                    return "ToByte";
                case 16:
                    return "ToUInt16";
                case 32:
                    return "ToUInt32";
                case 64:
                    return "ToUInt64";
                default:
                    throw new ArgumentException("Invalid bit size", nameof(bitSize));
            }
        }

        public static string MapBitSizeToType(int bitSize)
        {
            switch ( bitSize )
            {
                case 8: return "byte";
                case 16: return "ushort";
                case 32: return "uint";
                case 64: return "ulong";
                default: throw new ArgumentException($"Unsupported bit size: {bitSize}", nameof(bitSize));
            }
        }
    }

    /// <summary>
    /// Replaces Types identified by string, from old to new type, targets System.Convert.To operator / Variable declarations / Constructor declarations
    /// </summary>
    public class TypeChangingRewriter : CSharpSyntaxRewriter
    {
#pragma warning disable CS8603 // Possible null reference return.
        private string _oldTypeName;
        private string _newTypeName;

        public TypeChangingRewriter(string templateType, string replaceWithType)
        {
            this._oldTypeName = templateType;
            this._newTypeName = replaceWithType;
        }

        public override SyntaxNode? VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
        {
            if ( node.Type.ToString() == _oldTypeName )
            {
                var newType = SyntaxFactory.ParseTypeName(_newTypeName).WithTriviaFrom(node.Type);

                node = node.Update(node.AttributeLists,
                    node.Modifiers,
                    node.ImplicitOrExplicitKeyword,
                    node.OperatorKeyword,
                    newType,
                    node.ParameterList,
                    node.Body,
                    node.ExpressionBody,
                    node.SemicolonToken);
            }

            return base.VisitConversionOperatorDeclaration(node);
        }

        /// <summary>
        /// Rewrite ParameterListSyntax node to replace the old type with the new type
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override SyntaxNode VisitParameterList(ParameterListSyntax node)
        {
            var newParameterList = SyntaxFactory.ParameterList().WithTriviaFrom(node);

            foreach ( var parameter in node.Parameters )
            {
                if ( parameter.Type?.ToString() == _oldTypeName )
                {
                    var newParameter = parameter
                        .WithType(SyntaxFactory.ParseTypeName(_newTypeName).WithTriviaFrom(parameter.Type))
                        .WithIdentifier(parameter.Identifier)
                        .WithTriviaFrom(parameter);

                    // Insert whitespace since SyntaxFactory.ParseTypeName doesn't preserve leading trivia.
                    var leadingTrivia = SyntaxFactory.Whitespace(" ");
                    newParameter = newParameter.WithLeadingTrivia(newParameter.GetLeadingTrivia().Add(leadingTrivia));

                    newParameterList = newParameterList.AddParameters(newParameter);
                }
                else
                {
                    newParameterList = newParameterList.AddParameters(parameter);
                }
            }

            node = newParameterList;

            return base.VisitParameterList(node);
        }

        public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            var parameterList = node.ParameterList;

            var newParameterList = SyntaxFactory.ParameterList();

            foreach ( var parameter in parameterList.Parameters )
            {
                if ( parameter.Type?.ToString() == _oldTypeName )
                {
                    var newParameter = parameter
                        .WithType(SyntaxFactory.ParseTypeName(_newTypeName).WithTriviaFrom(parameter.Type))
                        .WithIdentifier(parameter.Identifier)
                        .WithTriviaFrom(parameter);

                    newParameterList = newParameterList.AddParameters(newParameter);
                }
                else
                {
                    newParameterList = newParameterList.AddParameters(parameter);
                }
            }

            node = node.WithParameterList(newParameterList);

            return base.VisitConstructorDeclaration(node);
        }

        public override SyntaxNode VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            if ( node.Type.ToString() == _oldTypeName )
            {
                var newType = SyntaxFactory.ParseTypeName(_newTypeName)
                    .WithTriviaFrom(node.Type);

                var newVariables = node.Variables.Select(
                    variable => SyntaxFactory.VariableDeclarator(variable.Identifier)
                        .WithInitializer(variable.Initializer is null ? null :
                        SyntaxFactory.EqualsValueClause(variable.Initializer.Value)
                        .WithTriviaFrom(node))
                );

                node = SyntaxFactory.VariableDeclaration(newType,
                    SyntaxFactory.SeparatedList(newVariables));
            }

            return base.VisitVariableDeclaration(node);
        }
#pragma warning restore CS8603 // Possible null reference return.
    }

    public class IdentifierRenamingRewriter : CSharpSyntaxRewriter
    {
#pragma warning disable CS8603 // Possible null reference return.
        Dictionary<string, string> nameMapping;

        /// <summary>
        /// Rewrite <see langword="class"/>, <see langword="struct"/>, Variable and Constructor identifiers with another.
        /// </summary>
        /// <param name="nameMapping">Key is the identifier to replace, with Value</param>
        public IdentifierRenamingRewriter(Dictionary<string, string> nameMapping)
        {
            this.nameMapping = nameMapping;
        }

        /// <summary>
        /// Replace a struct node identifier declaration with another, renamePair.Key will be replaced with renamePair.Value
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            foreach ( var renamePair in nameMapping )
            {
                if ( node.Identifier.Text == renamePair.Key )
                {
                    node = node.ReplaceToken(node.Identifier, SyntaxFactory.Identifier(renamePair.Value)).WithTriviaFrom(node);
                    break;
                }
            }

            return base.VisitClassDeclaration(node);
        }

        /// <summary>
        /// Replace a struct node identifier declaration with another, renamePair.Key will be replaced with renamePair.Value
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node)
        {
            foreach ( var renamePair in nameMapping )
            {
                if ( node.Identifier.Text == renamePair.Key )
                {
                    node = node.ReplaceToken(node.Identifier, SyntaxFactory.Identifier(renamePair.Value)).WithTriviaFrom(node);
                    break;
                }
            }

            return base.VisitStructDeclaration(node);
        }

        /// <summary>
        /// Replace a Constructor node identifier declaration with another, renamePair.Key will be replaced with renamePair.Value
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            foreach ( var renamePair in nameMapping )
            {
                if ( node.Identifier.Text == renamePair.Key )
                {
                    node = node.ReplaceToken(node.Identifier, SyntaxFactory.Identifier(renamePair.Value)).WithTriviaFrom(node);
                    break;
                }
            }

            return base.VisitConstructorDeclaration(node);
        }

        /// <summary>
        /// Replace a Variable node identifier declaration with another, renamePair.Key will be replaced with renamePair.Value
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override SyntaxNode VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            foreach ( var renamePair in nameMapping )
            {
                if ( node.Identifier.Text == renamePair.Key )
                {
                    node = node.ReplaceToken(node.Identifier, SyntaxFactory.Identifier(renamePair.Value)).WithTriviaFrom(node);
                    break;
                }
            }

            return base.VisitVariableDeclarator(node);
        }

        /// <summary>
        /// Replace a Identifier node identifier declaration with another, renamePair.Key will be replaced with renamePair.Value
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            foreach ( var renamePair in nameMapping )
            {
                if ( node.Identifier.Text == renamePair.Key )
                {
                    node = node.ReplaceToken(node.Identifier, SyntaxFactory.Identifier(renamePair.Value)).WithTriviaFrom(node);
                    break;
                }
            }

            return base.VisitIdentifierName(node);
        }
#pragma warning restore CS8603 // Possible null reference return.
    }
}
