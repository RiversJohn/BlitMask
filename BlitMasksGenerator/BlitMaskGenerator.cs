/* Copyright © 2023 - Juho Veli-Matti Joensuu
 * 
 * All Rights Reserved.
 */

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlitMaskGenerators
{
    [Generator]
    public class BlitMaskGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            //if ( !Debugger.IsAttached ) Debugger.Launch();

            var compilation = context.Compilation;

            string templatesFolder = @"E:\Repositories\BlitMasks\BlitMasksGenerator\Templates\";
            string[] templatePaths = Directory.GetFiles(templatesFolder, "*.cs");

            var templateSyntaxTrees = new List<SyntaxTree>();

            foreach ( var templatePath in templatePaths )
            {
                if ( templatePath.Contains("BlitMaskConstantsTemplate") )
                {
                    string fileContents = File.ReadAllText(templatePath, Encoding.UTF8);
                    SourceText templateCode = SourceText.From(fileContents);
                    SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(templateCode, CSharpParseOptions.Default, templatePath, context.CancellationToken);
                    templateSyntaxTrees.Add(syntaxTree);
                }
            }

            List<int> SupportedBitSizes = new List<int>() { 8, 16, 32, 64 };
            Dictionary<string, string> pathAndSourceCode = new Dictionary<string, string>();

            foreach ( SyntaxTree syntaxTree in templateSyntaxTrees )
            {
                var rootNode = (CompilationUnitSyntax)syntaxTree.GetRoot(context.CancellationToken);

                foreach ( int bitSize in SupportedBitSizes )
                {

                    Dictionary<string, string> renameMap = new Dictionary<string, string>
                    {
                        { "BlitMaskConstantsTemplate", $"BlitMaskConstants{bitSize}" },
                        { "BlitMaskTemplate", $"BlitMask{bitSize}" },
                        { "None", $"None{bitSize}" },
                        { "Everything", $"Everything{bitSize}" }
                    };

                    var rewriter = new RenamingRewriter(renameMap.Keys.ToArray(), renameMap.Values.ToArray());

                    var newRoot = (CompilationUnitSyntax)rewriter.Visit(rootNode);

                    string sourceCode = newRoot.ToFullString();

                    if ( syntaxTree.FilePath.Contains("BlitMaskConstantsTemplate") )
                    {
                        string onePattern = $"X{(int)(bitSize / 4)}";

                        sourceCode = sourceCode
                            .Replace(@"0x00000000", GenerationHelpers.GetHexValueForZero(bitSize))
                            .Replace(@"0xFFFFFFFF", GenerationHelpers.GetHexValueForMax(bitSize))
                            .Replace(@"0x00000001", "0x" + 1.ToString(onePattern));
                    }

                    pathAndSourceCode.Add(templatesFolder + Path.GetFileNameWithoutExtension(syntaxTree.FilePath) + bitSize.ToString() + ".log", sourceCode);
                }
            }

            foreach ( var path in pathAndSourceCode.Keys )
            {
                using var fs = File.OpenWrite(path);
                {
                    string sourceCode = pathAndSourceCode[path];
                    fs.Write(Encoding.UTF8.GetBytes(sourceCode), 0, sourceCode.Length);
                }
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
    }

    public class RenamingRewriter : CSharpSyntaxRewriter
    {
#pragma warning disable CS8603 // Possible null reference return.
        private string[] oldNames;
        private string[] newNames;

        public RenamingRewriter(string[] oldNames, string[] newNames)
        {
            // Check that both arrays have the same length
            if ( oldNames.Length != newNames.Length )
            {
                throw new ArgumentException("Error: The number of old names and new names must match.");
            }

            this.oldNames = oldNames;
            this.newNames = newNames;
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            // Loop through all the old names and check if they match the class name
            for ( int i = 0; i < oldNames.Length; i++ )
            {
                if ( node.Identifier.Text == oldNames[i] )
                {
                    // Replace it with the corresponding new name
                    node = node.ReplaceToken(node.Identifier, SyntaxFactory.Identifier(newNames[i]));
                    break; // Exit the loop once a match is found
                }
            }

            // Visit any nested classes
            return base.VisitClassDeclaration(node);
        }

        public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node)
        {
            // Loop through all the old names and check if they match the class name
            for ( int i = 0; i < oldNames.Length; i++ )
            {
                if ( node.Identifier.Text == oldNames[i] )
                {
                    // Replace it with the corresponding new name
                    node = node.ReplaceToken(node.Identifier, SyntaxFactory.Identifier(newNames[i]));
                    break; // Exit the loop once a match is found
                }
            }

            // Visit any nested classes
            return base.VisitStructDeclaration(node);
        }

        public override SyntaxNode VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            // Loop through all the old names and check if they match the class name
            for ( int i = 0; i < oldNames.Length; i++ )
            {
                if ( node.Identifier.Text == oldNames[i] )
                {
                    // Replace it with the corresponding new name
                    node = node.ReplaceToken(node.Identifier, SyntaxFactory.Identifier(newNames[i]));
                    break; // Exit the loop once a match is found
                }
            }

            // Visit any nested classes
            return base.VisitVariableDeclarator(node);
        }

        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            // Loop through all the old names and check if they match the identifier name
            for ( int i = 0; i < oldNames.Length; i++ )
            {
                if ( node.Identifier.Text == oldNames[i] )
                {
                    // Replace it with the corresponding new name
                    node = node.ReplaceToken(node.Identifier, SyntaxFactory.Identifier(newNames[i]));
                    break; // Exit the loop once a match is found
                }
            }

            // Visit any other identifier names
            return base.VisitIdentifierName(node);
        }
#pragma warning restore CS8603 // Possible null reference return.
    }
}
