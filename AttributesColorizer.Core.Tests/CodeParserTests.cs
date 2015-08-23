using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AttributesColorizer.Core.Tests
{
    [TestClass]
    public class CodeParserTests
    {
        [TestMethod]
        [DeploymentItem(@"Samples\SingleAttribute.txt", "Samples")]
        public void SingleAttribute()
        {
            var csCode = GetCode(@"Samples\SingleAttribute.txt");
            var syntaxTree = CSharpSyntaxTree.ParseText(csCode);

            var codeParser = new CodeParser();
            var attributes = codeParser.GetAttributes(syntaxTree).ToList();

            Assert.AreEqual(1, attributes.Count);
            var startPosition = csCode.IndexOf("[Serializable]", StringComparison.Ordinal);
            Assert.AreEqual(startPosition, attributes.Single().Start);

        }

        [TestMethod]
        [DeploymentItem(@"Samples\CustomAttribute.txt", "Samples")]
        public void CustomAttribute()
        {
            string file = @"Samples\CustomAttribute.txt";
            Assert.IsTrue(File.Exists(file), $"deployment failed: {file} did not get deployed");

            var csCode = File.ReadAllText(file);
            var syntaxTree = CSharpSyntaxTree.ParseText(csCode);

            var codeParser = new CodeParser();
            var attributes = codeParser.GetAttributes(syntaxTree).ToList();

            Assert.AreEqual(1, attributes.Count);
            var startPosition = csCode.IndexOf("[Test]", StringComparison.Ordinal);
            Assert.AreEqual(startPosition, attributes.Single().Start);
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
