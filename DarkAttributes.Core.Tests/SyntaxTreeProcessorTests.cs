using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis.CSharp;
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
            var csCode = GetCode(@"Samples\SingleAttribute.txt");
            var syntaxTree = CSharpSyntaxTree.ParseText(csCode);

            var syntaxTreeProcessor = new SyntaxTreeProcessor();
            var attributes = syntaxTreeProcessor.GetAttributeLists(syntaxTree).ToList();

            Assert.AreEqual(1, attributes.Count);
            var startPosition = csCode.IndexOf("[Serializable]", StringComparison.Ordinal);
            Assert.AreEqual(startPosition, attributes.Single().Start);
        }

        [TestMethod]
        [Description("A custom attribute that is declared in the inspected code")]
        [DeploymentItem(@"Samples\CustomAttribute.txt", "Samples")]
        public void CustomAttribute()
        {
            var csCode = GetCode(@"Samples\CustomAttribute.txt");
            var syntaxTree = CSharpSyntaxTree.ParseText(csCode);

            var syntaxTreeProcessor = new SyntaxTreeProcessor();
            var attributes = syntaxTreeProcessor.GetAttributeLists(syntaxTree).ToList();

            Assert.AreEqual(1, attributes.Count);
            var startPosition = csCode.IndexOf("[Test]", StringComparison.Ordinal);
            Assert.AreEqual(startPosition, attributes.Single().Start);
        }

        [TestMethod]
        [Description("Two different attributes")]
        [DeploymentItem(@"Samples\TwoAttributes.txt", "Samples")]
        public void TwoAttributes()
        {
            var csCode = GetCode(@"Samples\TwoAttributes.txt");
            var syntaxTree = CSharpSyntaxTree.ParseText(csCode);

            var syntaxTreeProcessor = new SyntaxTreeProcessor();
            var attributes = syntaxTreeProcessor.GetAttributeLists(syntaxTree).ToList();

            Assert.AreEqual(2, attributes.Count);
        }

        [TestMethod]
        [Description("Two different attributes in a single list. " +
                     "One attribute list should be returned - because entire list " +
                     "with brackets, commas, args should be coloured.")]
        [DeploymentItem(@"Samples\TwoAttributesInCommonBrackets.txt", "Samples")]
        public void TwoAttributesInCommonBrackets()
        {
            var csCode = GetCode(@"Samples\TwoAttributesInCommonBrackets.txt");
            var syntaxTree = CSharpSyntaxTree.ParseText(csCode);

            var syntaxTreeProcessor = new SyntaxTreeProcessor();
            var attributes = syntaxTreeProcessor.GetAttributeLists(syntaxTree).ToList();

            Assert.AreEqual(1, attributes.Count);
        }

        [TestMethod]
        [Description("Should also see inlined attributes, not only those that have dedicated lines")]
        [DeploymentItem(@"Samples\ThreeInlinedAttributes.txt", "Samples")]
        public void ThreeInlinedAttributes()
        {
            var csCode = GetCode(@"Samples\ThreeInlinedAttributes.txt");
            var syntaxTree = CSharpSyntaxTree.ParseText(csCode);

            var syntaxTreeProcessor = new SyntaxTreeProcessor();
            var attributes = syntaxTreeProcessor.GetAttributeLists(syntaxTree).ToList();

            Assert.AreEqual(3, attributes.Count);
        }

        [TestMethod]
        [Description("Should ignore comments")]
        [DeploymentItem(@"Samples\CommentedAttribute.txt", "Samples")]
        public void CommentedAttribute()
        {
            var csCode = GetCode(@"Samples\CommentedAttribute.txt");
            var syntaxTree = CSharpSyntaxTree.ParseText(csCode);

            var syntaxTreeProcessor = new SyntaxTreeProcessor();
            var attributes = syntaxTreeProcessor.GetAttributeLists(syntaxTree).ToList();

            Assert.AreEqual(0, attributes.Count);
        }

        [TestMethod]
        [Description("Attribute can be declared somewhere else, not inside the inspected code. " +
                     "But it's still the attribute that has to be coloured.")]
        [DeploymentItem(@"Samples\UnknownAttribute.txt", "Samples")]
        public void UnknownAttribute()
        {
            var csCode = GetCode(@"Samples\UnknownAttribute.txt");
            var syntaxTree = CSharpSyntaxTree.ParseText(csCode);

            var syntaxTreeProcessor = new SyntaxTreeProcessor();
            var attributes = syntaxTreeProcessor.GetAttributeLists(syntaxTree).ToList();

            Assert.AreEqual(1, attributes.Count);
        }

        [TestMethod]
        [Description("Need to colour the entire attribute - with brackets and args")]
        [DeploymentItem(@"Samples\CustomAttributeWithArgs.txt", "Samples")]
        public void CustomAttributeWithArgs()
        {
            var csCode = GetCode(@"Samples\CustomAttributeWithArgs.txt");
            var syntaxTree = CSharpSyntaxTree.ParseText(csCode);

            var syntaxTreeProcessor = new SyntaxTreeProcessor();
            var attributes = syntaxTreeProcessor.GetAttributeLists(syntaxTree).ToList();

            Assert.AreEqual(1, attributes.Count);

            var attribute = attributes.Single();
            var attributeString = "[Test(1, IntProperty = 2)]";
            var startPosition = csCode.IndexOf(attributeString, StringComparison.Ordinal);
            var length = attributeString.Length;
            Assert.AreEqual(startPosition, attribute.Start);
            Assert.AreEqual(length, attribute.Length);
        }

        

        [NotNull]
        private static string GetCode([NotNull] string file)
        {
            Assert.IsTrue(File.Exists(file), $"deployment failed: {file} did not get deployed");

            var csCode = File.ReadAllText(file);
            return csCode;
        }
    }
}
