using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DarkAttributes.Core.Tests
{
    [TestClass]
    public class SyntaxTreeProcessorTests
    {
        [TestMethod]
        [Description("Single [Serializable] attribute")]
        [DeploymentItem(@"Samples\SingleAttribute.txt", "Samples")]
        public void SingleAttribute()
        {
            var parseResult = ParseFile(@"Samples\SingleAttribute.txt");

            Assert.AreEqual(1, parseResult.AttributeSpans.Count);
            var startPosition = parseResult.SourceCode.IndexOf("[Serializable]", StringComparison.Ordinal);
            Assert.AreEqual(startPosition, parseResult.AttributeSpans.Single().Start);
        }

        [TestMethod]
        [Description("A custom attribute that is declared in the inspected code")]
        [DeploymentItem(@"Samples\CustomAttribute.txt", "Samples")]
        public void CustomAttribute()
        {
            var parseResult = ParseFile(@"Samples\CustomAttribute.txt");
            
            Assert.AreEqual(1, parseResult.AttributeSpans.Count);
            var startPosition = parseResult.SourceCode.IndexOf("[Test]", StringComparison.Ordinal);
            Assert.AreEqual(startPosition, parseResult.AttributeSpans.Single().Start);
        }

        [TestMethod]
        [Description("Two different attributes")]
        [DeploymentItem(@"Samples\TwoAttributes.txt", "Samples")]
        public void TwoAttributes()
        {
            var parseResult = ParseFile(@"Samples\TwoAttributes.txt");
            
            Assert.AreEqual(2, parseResult.AttributeSpans.Count);
        }

        [TestMethod]
        [Description("Two different attributes in a single list. " +
                     "One attribute list should be returned - because entire list " +
                     "with brackets, commas, args should be coloured.")]
        [DeploymentItem(@"Samples\TwoAttributesInCommonBrackets.txt", "Samples")]
        public void TwoAttributesInCommonBrackets()
        {
            var parseResult = ParseFile(@"Samples\TwoAttributesInCommonBrackets.txt");
            
            Assert.AreEqual(1, parseResult.AttributeSpans.Count);
        }

        [TestMethod]
        [Description("Should also see inlined attributes, not only those that have dedicated lines")]
        [DeploymentItem(@"Samples\ThreeInlinedAttributes.txt", "Samples")]
        public void ThreeInlinedAttributes()
        {
            var parseResult = ParseFile(@"Samples\ThreeInlinedAttributes.txt");
            
            Assert.AreEqual(3, parseResult.AttributeSpans.Count);
        }

        [TestMethod]
        [Description("Should ignore comments")]
        [DeploymentItem(@"Samples\CommentedAttribute.txt", "Samples")]
        public void CommentedAttribute()
        {
            var parseResult = ParseFile(@"Samples\CommentedAttribute.txt");
            
            Assert.AreEqual(0, parseResult.AttributeSpans.Count);
        }

        [TestMethod]
        [Description("Attribute can be declared somewhere else, not inside the inspected code. " +
                     "But it's still the attribute that has to be coloured.")]
        [DeploymentItem(@"Samples\UnknownAttribute.txt", "Samples")]
        public void UnknownAttribute()
        {
            var parseResult = ParseFile(@"Samples\UnknownAttribute.txt");
            
            Assert.AreEqual(1, parseResult.AttributeSpans.Count);
        }

        [TestMethod]
        [Description("Need to colour the entire attribute - with brackets and args")]
        [DeploymentItem(@"Samples\CustomAttributeWithArgs.txt", "Samples")]
        public void CustomAttributeWithArgs()
        {
            var parseResult = ParseFile(@"Samples\CustomAttributeWithArgs.txt");
            
            Assert.AreEqual(1, parseResult.AttributeSpans.Count);

            var attribute = parseResult.AttributeSpans.Single();
            var attributeString = "[Test(1, IntProperty = 2)]";
            var startPosition = parseResult.SourceCode.IndexOf(attributeString, StringComparison.Ordinal);
            var length = attributeString.Length;
            Assert.AreEqual(startPosition, attribute.Start);
            Assert.AreEqual(length, attribute.Length);
        }

        private ParseResult ParseFile(string path)
        {
            Assert.IsTrue(File.Exists(path), $"deployment failed: {path} did not get deployed");

            var csCode = File.ReadAllText(path);
            var syntaxTree = CSharpSyntaxTree.ParseText(csCode);

            var syntaxTreeProcessor = new SyntaxTreeProcessor();
            var testResult = new ParseResult
            {
                SourceCode = csCode,
                AttributeSpans = syntaxTreeProcessor.GetAttributeLists(syntaxTree).ToList()
            };
            return testResult;
        }
        
        class ParseResult
        {
            public string SourceCode { get; set; }
            public List<TextSpan> AttributeSpans { get; set; }
        }
    }
}
