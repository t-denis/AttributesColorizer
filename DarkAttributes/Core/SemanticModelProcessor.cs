using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace DarkAttributes.Core
{
    public class SemanticModelProcessor
    {
        /// <summary>
        /// Returns attributes to darken. Uses a blacklist to filter attributes
        /// </summary>
        public IEnumerable<SyntaxNode> FilterAttributes(SemanticModel semanticModel,
            IEnumerable<AttributeListSyntax> attributes, string[] blacklist)
        {
            // Split list to separate attributes
            // check every attribute
            // if all attributes in the list should be darkened, then return the entire list.
            // if not, then return specific attributes in the list
            foreach (var attributeList in attributes)
            {
                var attributesToDarken = new List<SyntaxNode>();
                var attributesToIgnore = new List<SyntaxNode>();
                foreach (var attribute in attributeList.Attributes)
                {
                    var symbolInfo = semanticModel.GetSymbolInfo(attribute);
                    var symbol = symbolInfo.Symbol;
                    // skip unknown attributes, keep them visible
                    if (symbol == null)
                        continue;
                    // symbol is a .ctor call. Need to get the type
                    var attributeType = symbol.ContainingType;
                    if (attributeType == null)
                        // TODO: Is it possible?
                        continue;

                    var attributeName = attributeType.Name;
                    var attributeFullName = $"{attributeType.ContainingNamespace}.{attributeName}";
                    var filterResult = FilterAttribute(blacklist, attributeName, attributeFullName);
                    if (filterResult == BlacklistFilterResult.ContainedInBlacklist)
                        attributesToDarken.Add(attribute);
                    else
                        attributesToIgnore.Add(attribute);
                }
                if (!attributesToIgnore.Any())
                    yield return attributeList;
                else
                {
                    foreach (var syntaxNode in attributesToDarken)
                        yield return syntaxNode;
                }
            }
        }

        private static BlacklistFilterResult FilterAttribute(string[] blacklist, string symbolName, string symbolFullName)
        {
            foreach (var blacklistItem in blacklist)
            {
                // some blacklist elements can be empty
                if (string.IsNullOrWhiteSpace(blacklistItem))
                    continue;

                // User can specify:
                // short attribute name: Display
                // attribute name: DisplayAttribute
                // full attribute name: System.ComponentModel.DataAnnotations.DisplayAttribute
                // or a wildcard: System.ComponentModel.DataAnnotations.*
                if (MatchShortName(symbolName, blacklistItem)
                    || MatchName(symbolName, blacklistItem)
                    || MatchFullname(symbolFullName, blacklistItem)
                    || MatchWildcard(symbolFullName, blacklistItem))
                {
                    return BlacklistFilterResult.ContainedInBlacklist;
                }
            }
            return BlacklistFilterResult.NotContainedInBlacklist;
        }

        private static bool MatchShortName(string symbolName, string blacklistItem)
        {
            return $"{blacklistItem}Attribute" == symbolName;
        }

        private static bool MatchName(string symbolName, string blacklistItem)
        {
            return blacklistItem == symbolName;
        }

        private static bool MatchFullname(string symbolFullName, string blacklistItem)
        {
            return blacklistItem == symbolFullName;
        }

        private static bool MatchWildcard(string symbolFullName, string blacklistItem)
        {
            return LikeOperator.LikeString(symbolFullName, blacklistItem, CompareMethod.Text);
        }

        private enum BlacklistFilterResult
        {
            ContainedInBlacklist,
            NotContainedInBlacklist
        }
    }
}
