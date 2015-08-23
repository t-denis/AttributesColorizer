//------------------------------------------------------------------------------
// <copyright file="AttributeClassifierClassificationDefinition.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace AttributesColorizer
{
    /// <summary>
    /// Classification type definition export for AttributeClassifier
    /// </summary>
    internal static class AttributeClassifierClassificationDefinition
    {
        // This disables "The field is never used" compiler's warning. Justification: the field is used by MEF.
#pragma warning disable 169

        /// <summary>
        /// Defines the "AttributeClassifier" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("AttributeClassifier")]
        private static ClassificationTypeDefinition typeDefinition;

#pragma warning restore 169
    }
}
