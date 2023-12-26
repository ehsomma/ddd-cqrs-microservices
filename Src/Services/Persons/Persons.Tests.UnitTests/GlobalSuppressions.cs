// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "StyleCop.CSharp.DocumentationRules",
    "SA1600:Elements should be documented",
    Justification = "Not in test methods")]

[assembly: SuppressMessage(
    "StyleCop.CSharp.NamingRules",
    "SA1300:ElementMustBeginWithUpperCaseLetter",
    Justification = "Not for test names")]

[assembly: SuppressMessage(
    "Style",
    "IDE1006:Naming Styles",
    Justification = "Not for test names")]
[assembly: SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1200:Using directives should be placed correctly",
    Justification = "Not for test names")]

[assembly: SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Not for test names")]

[assembly: SuppressMessage(
    "Globalization",
    "CA1303:Do not pass literals as localized parameters",
    Justification = "Not for test")]
