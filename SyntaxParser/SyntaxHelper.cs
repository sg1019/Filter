using Antlr4.Runtime;

namespace SyntaxParser
{
    public class SyntaxHelper
    {
        public static readonly string InfinitySymbol = "#";
        public static readonly char RangeSeparator = ':';
        public static readonly string OpenSquareBracket = "[";
        public static readonly string CloseSquareBracket = "]";

        public static SyntaxParser GetSyntaxParser(string text)
        {
            var inputStream = new AntlrInputStream(text);
            var syntaxLexer = new SyntaxLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(syntaxLexer);
            var syntaxParser = new SyntaxParser(commonTokenStream);
            syntaxParser.RemoveErrorListeners();

            return syntaxParser;
        }
    }
}