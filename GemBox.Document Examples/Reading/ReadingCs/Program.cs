using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GemBox.Document;

class Program
{
    static void Main()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        Example1();
        Example2();
    }

    static void Example1()
    {
        // Load Word document from file's path.
        var document = DocumentModel.Load("Reading.docx");

        // Get Word document's plain text.
        string text = document.Content.ToString();

        // Get Word document's count statistics.
        int charactersCount = text.Replace(Environment.NewLine, string.Empty).Length;
        int wordsCount = Regex.Matches(text, @"[\S]+").Count;
        int paragraphsCount = document.GetChildElements(true, ElementType.Paragraph).Count();
        /* DocumentModel.GetPaginator method is available in .NET Framework +3.5 and .NET Core +3.0. */
        /* Currently this method is not available in .NET Standard 2.0. */
        /* int pageCount = document.GetPaginator().Pages.Count; */

        // Display file's count statistics.
        Console.WriteLine($"Characters count: {charactersCount}");
        Console.WriteLine($"     Words count: {wordsCount}");
        Console.WriteLine($"Paragraphs count: {paragraphsCount}");
        /* Console.WriteLine($"     Pages count: {pageCount}"); */
        Console.WriteLine();

        // Display file's text content.
        Console.WriteLine(text);
    }

    static void Example2()
    {
        var document = DocumentModel.Load("Reading.docx");
        using (var writer = File.CreateText("Output.txt"))
        {
            // Iterate through all Paragraph elements in the Word document.
            foreach (Paragraph paragraph in document.GetChildElements(true, ElementType.Paragraph))
            {
                // Iterate through all Run elements in the Paragraph element.
                foreach (Run run in paragraph.GetChildElements(true, ElementType.Run))
                {
                    string text = run.Text;
                    CharacterFormat format = run.CharacterFormat;

                    // Replace text with bold formatting to 'Mathematical Bold Italic' Unicode characters.
                    // For instance, "ABC" to "𝑨𝑩𝑪".
                    if (format.Bold)
                    {
                        text = string.Concat(text.Select(
                            c => c >= 'A' && c <= 'Z' ? char.ConvertFromUtf32(119847 + c) :
                                 c >= 'a' && c <= 'z' ? char.ConvertFromUtf32(119841 + c) :
                                 c.ToString()));
                    }

                    writer.Write(text);
                }

                writer.WriteLine();
            }
        }
    }
}