using CodeAnalysis;

namespace CodeAnalysisTesting.PascalIntConst;

public enum LexemeType
{
    ConstKeyword = 1,
    IntegerKeyword,
    Colon,
    Equal,
    Number,
    InvalidSymbols,
    Sign,
    Semicolon,
    Identifier,
    Separator,
    UnexpectedSymbol,
}

public class Lexer() : Lexer<LexemeType>(new LexemeUtils(), LexemeEaters.Eaters);

public static class LexemeEaters
{
    public static LexemeEater<LexemeType>[] Eaters =
    [
        TryEatEqual,
        TryEatSemicolon,
        TryEatSign,
        TryEatColon,
        TryEatConstKeyword,
        TryEatIntegerKeyword,
        TryEatNumber,
        TryEatIdentifier,
        TryEatSeparator,
        TryEatInvalidSymbols,
    ];

    private static LexemeType? TryEatInvalidSymbols(Eater eater)
    {
        return eater.EatWhile(sym => sym != ':' && sym != ';' && sym != '-' && sym != '+' && sym != '=')
            ? LexemeType.InvalidSymbols
            : null;
    }

    private static LexemeType? TryEatNumber(Eater eater)
    {
        return eater.EatWhile(char.IsDigit) ? LexemeType.Number : null;
    }

    private static LexemeType? TryEatIdentifier(Eater eater)
    {
        if (!eater.Eat(IsIdentifierHead)) return null;
        eater.EatWhile(IsIdentifierTail);

        return LexemeType.Identifier;
    }

    private static bool IsIdentifierHead(char sym)
    {
        return char.IsLetter(sym);
    }

    private static bool IsIdentifierTail(char sym)
    {
        return char.IsLetterOrDigit(sym);
    }

    private static LexemeType? TryEatConstKeyword(Eater eater)
    {
        return eater.Eat("const") ? LexemeType.ConstKeyword : null;
    }

    private static LexemeType? TryEatIntegerKeyword(Eater eater)
    {
        return eater.Eat("integer") ? LexemeType.IntegerKeyword : null;
    }

    private static LexemeType? TryEatColon(Eater eater)
    {
        return eater.Eat(':') ? LexemeType.Colon : null;
    }

    private static LexemeType? TryEatEqual(Eater eater)
    {
        return eater.Eat('=') ? LexemeType.Equal : null;
    }

    private static LexemeType? TryEatSemicolon(Eater eater)
    {
        return eater.Eat(';') ? LexemeType.Semicolon : null;
    }

    private static LexemeType? TryEatSign(Eater eater)
    {
        return eater.Eat('+') || eater.Eat('-') ? LexemeType.Sign : null;
    }

    private static LexemeType? TryEatSeparator(Eater eater)
    {
        return eater.EatWhile(IsSeparator) ? LexemeType.Separator : null;
    }

    private static bool IsSeparator(char sym, char? nextSym)
    {
        // TODO fix
        return char.IsSeparator(sym)
               || $"{sym}" == Environment.NewLine
               || (nextSym is { } n && $"{sym}{n}" == Environment.NewLine);
    }
}

public class LexemeUtils : ILexemeUtils<LexemeType>
{
    public LexemeType UnexpectedSymbol()
    {
        return LexemeType.UnexpectedSymbol;
    }

    public bool IsIgnorableLexeme(LexemeType lexeme)
    {
        return lexeme is LexemeType.Separator;
    }

    public bool RemoveInvalidLexeme(LexemeType lexeme)
    {
        return true;
    }

    public bool IsInvalidLexeme(LexemeType lexeme)
    {
        return lexeme is LexemeType.UnexpectedSymbol;
    }

    public string LexemeMissingValue(LexemeType lexeme)
    {
        return lexeme switch
        {
            LexemeType.ConstKeyword => "const",
            LexemeType.IntegerKeyword => "integer",
            LexemeType.Identifier => "identifier",
            LexemeType.Colon => ":",
            LexemeType.Equal => "=",
            LexemeType.Semicolon => ";",
            LexemeType.Number => "1234",
            LexemeType.Sign => "-",
            _ => throw new ArgumentException(
                $"${LexemeType.UnexpectedSymbol} and ${LexemeType.Separator} are invalid arguments"),
        };
    }
}