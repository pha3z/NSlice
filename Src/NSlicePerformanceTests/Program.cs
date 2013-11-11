﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using NSlicePerformanceTests.Markdown;
using NSlicePerformanceTests.Tests.Cases;

namespace NSlicePerformanceTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var testSettings = new TestSettings
            {
                TestCollectionSize = 1000000,
                IterationsPerSample = 10,
                SampleCount = 50
            };
            var testCases = new ComparisonTestCases(testSettings);
            var culture = CultureInfo.InvariantCulture;

            Console.WriteLine("Rendering...");

            File.WriteAllText("report.md",
                Markdown.Markdown.Empty
                .Add(culture, "Following performance tests were performed on simple, enumerable collection generated by {0}. ",
                        "Enumerable.Range(0, collectionSize)".ToMarkdown().InlineCode())
                .Add("Partial times, longer than normalized deviation, were discarded.")
                .NewLine()
                .NewLine()
                .Add("Settings".ToMarkdown().H3())
                .NewLine()
                .NewLine()
                .Add(
                    new[]
                    {
                        string.Format(culture, "Collection size: {0:#,0}", testSettings.TestCollectionSize),
                        string.Format(culture, "Sample size: {0:#,0}", testSettings.SampleCount),
                        string.Format(culture, "Iterations per sample: {0:#,0}", testSettings.IterationsPerSample)
                    }.Select(x => x.ToMarkdown()).AsUnorderedList()
                )
                .NewLine()
                .NewLine()
                .Add("NSlice vs. LINQ".ToMarkdown().H2().NewLine())
                .Add(
                    new[]
                    {
                        testCases.GetItemsAtOddIndices,
                        testCases.Reverse,
                        testCases.ReverseAndEvenIndices,
                        testCases.MiddleTerce,
                        testCases.MiddleTerceReversed
                    }.Select(x => x.ToMarkdown(culture)).AsLines())
            .Render());
        }
    }
}
