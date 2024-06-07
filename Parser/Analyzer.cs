using CodeAnalysis;

namespace CodeAnalysisTesting.PascalIntConst;

public class Analyzer() : Analyzer<LexemeType, State>(new LexemeUtils(), new StateMachine());

internal class StateMachine : IStateMachine<LexemeType, State>
{
    public State EntryState { get; } = new()
    {
        LexemeType = null
    };

    public IEnumerable<State> ExitStates { get; } =
    [
        new State
        {
            LexemeType = LexemeType.Semicolon,
        }
    ];

    public IEnumerable<Transition<LexemeType, State>> Transitions { get; } =
    [
        new Transition<LexemeType, State>
        {
            Input = new State
            {
                LexemeType = null,
            },
            Output = new State
            {
                LexemeType = LexemeType.ConstKeyword,
            },
            BoundaryLexemes =
            [
                LexemeType.Identifier,
            ],
            CanLexemesBeSkipped = true,
            NotFoundPriority = _ => 1,
        },
        new Transition<LexemeType, State>
        {
            Input = new State
            {
                LexemeType = LexemeType.ConstKeyword,
            },
            Output = new State
            {
                LexemeType = LexemeType.Identifier,
            },
            BoundaryLexemes =
            [
                LexemeType.Colon,
            ],
            CanLexemesBeSkipped = true,
            NotFoundPriority = _ => 1,
        },
        new Transition<LexemeType, State>
        {
            Input = new State
            {
                LexemeType = LexemeType.Identifier,
            },
            Output = new State
            {
                LexemeType = LexemeType.Colon,
            },
            BoundaryLexemes =
            [
                LexemeType.IntegerKeyword,
            ],
            CanLexemesBeSkipped = true,
            NotFoundPriority = _ => 1,
        },
        new Transition<LexemeType, State>
        {
            Input = new State
            {
                LexemeType = LexemeType.Colon,
            },
            Output = new State
            {
                LexemeType = LexemeType.IntegerKeyword,
            },
            BoundaryLexemes =
            [
                LexemeType.Equal,
            ],
            CanLexemesBeSkipped = true,
            NotFoundPriority = _ => 1,
        },
        new Transition<LexemeType, State>
        {
            Input = new State
            {
                LexemeType = LexemeType.IntegerKeyword,
            },
            Output = new State
            {
                LexemeType = LexemeType.Equal,
            },
            BoundaryLexemes =
            [
                LexemeType.Number,
            ],
            CanLexemesBeSkipped = true,
            NotFoundPriority = _ => 1,
        },
        new Transition<LexemeType, State>
        {
            Input = new State
            {
                LexemeType = LexemeType.Equal,
            },
            Output = new State
            {
                LexemeType = LexemeType.Number,
            },
            BoundaryLexemes =
            [
                LexemeType.Semicolon,
            ],
            CanLexemesBeSkipped = true,
            NotFoundPriority = _ => 1,
        },
        new Transition<LexemeType, State>
        {
            Input = new State
            {
                LexemeType = LexemeType.Number,
            },
            Output = new State
            {
                LexemeType = LexemeType.Semicolon,
            },
            BoundaryLexemes =
            [
                // LexemeType.Semicolon,
            ],
            CanLexemesBeSkipped = true,
            NotFoundPriority = _ => 1,
        },
        // new Transition<LexemeType, State>
        // {
        //     Input = new State
        //     {
        //         LexemeType = LexemeType.StringLiteral,
        //     },
        //     Output = new State
        //     {
        //         LexemeType = LexemeType.Semicolon,
        //     },
        //     BoundaryLexemes =
        //     [
        //         LexemeType.ConstKeyword,
        //         // LexemeType.Identifier,
        //         // LexemeType.Colon,
        //         // LexemeType.Ampersand,
        //         // LexemeType.StrKeyword,
        //         // LexemeType.Equal,
        //         // LexemeType.StringLiteral,
        //     ],
        //     CanLexemesBeSkipped = true,
        //     NotFoundPriority = _ => 1,
        // },
        // new Transition<LexemeType, State>
        // {
        //     Input = new State
        //     {
        //         LexemeType = LexemeType.Equal,
        //     },
        //     Output = new State
        //     {
        //         LexemeType = LexemeType.UnterminatedStringLiteral,
        //     },
        //     BoundaryLexemes =
        //     [
        //         LexemeType.Semicolon,
        //     ],
        //     CanLexemesBeSkipped = true,
        //     NotFoundPriority = _ => 1,
        // },
        // new Transition<LexemeType, State>
        // {
        //     Input = new State
        //     {
        //         LexemeType = LexemeType.UnterminatedStringLiteral,
        //     },
        //     Output = new State
        //     {
        //         LexemeType = LexemeType.Semicolon,
        //     },
        //     BoundaryLexemes =
        //     [
        //         LexemeType.ConstKeyword,
        //         // LexemeType.Identifier,
        //         // LexemeType.Colon,
        //         // LexemeType.Ampersand,
        //         // LexemeType.StrKeyword,
        //         // LexemeType.Equal,
        //         // LexemeType.StringLiteral,
        //     ],
        //     CanLexemesBeSkipped = true,
        //     NotFoundPriority = _ => 1,
        // },
    ];
}

public class State : IState<LexemeType>
{
    public LexemeType? LexemeType { get; init; }

    public bool Equals(IState<LexemeType>? other)
    {
        if (other is not State otherState) return false;
        return LexemeType == otherState.LexemeType;
    }
}
