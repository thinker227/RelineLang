using Reline.Compilation.Symbols;
using Reline.Compilation.Syntax.Nodes;

namespace Reline.Compilation.Binding;

/// <summary>
/// A factory for symbols.
/// </summary>
internal sealed class SymbolFactory {

	private readonly IBindingContext context;



	public SymbolFactory(IBindingContext context) {
		this.context = context;
	}



	public ProgramSymbol CreateProgram(ProgramSyntax syntax) =>
		context.GetSymbol(syntax, s =>
			new ProgramSymbol(1, s.Lines.Length) { Syntax = syntax });

	public LabelSymbol CreateLabel(LabelSyntax syntax, string identifier, LineSymbol line) =>
		context.GetSymbol(syntax, s => new LabelSymbol(identifier, line) { Syntax = s });

	public AssignmentStatementSymbol CreateAssignmentStatement(AssignmentStatementSyntax syntax, IVariableSymbol? variable, IExpressionSymbol? initializer) =>
		context.GetSymbol(syntax, s => new AssignmentStatementSymbol(variable, initializer) { Syntax = s });
	public ReturnStatementSymbol CreateReturnStatement(ReturnStatementSyntax syntax, IExpressionSymbol expression, FunctionSymbol? function) =>
		context.GetSymbol(syntax, s => new ReturnStatementSymbol(expression, function) { Syntax = s });
	public ExpressionStatementSymbol CreateExpressionStatement(ExpressionStatementSyntax syntax, IExpressionSymbol expression) =>
		context.GetSymbol(syntax, s => new ExpressionStatementSymbol(expression) { Syntax = s });
	public MoveStatementSymbol CreateMoveStatement(MoveStatementSyntax syntax, IExpressionSymbol source, IExpressionSymbol target) =>
		context.GetSymbol(syntax, s => new MoveStatementSymbol(source, target) { Syntax = s });
	public SwapStatementSymbol CreateSwapStatement(SwapStatementSyntax syntax, IExpressionSymbol source, IExpressionSymbol target) =>
		context.GetSymbol(syntax, s => new SwapStatementSymbol(source, target) { Syntax = s });
	public CopyStatementSymbol CreateCopyStatement(CopyStatementSyntax syntax, IExpressionSymbol source, IExpressionSymbol target) =>
		context.GetSymbol(syntax, s => new CopyStatementSymbol(source, target) { Syntax = s });

	public UnaryExpressionSymbol CreateUnaryExpression(UnaryExpressionSyntax syntax, IExpressionSymbol expression, UnaryOperatorType operatorType) =>
		context.GetSymbol(syntax, s => new UnaryExpressionSymbol(expression, operatorType) { Syntax = s });
	public BinaryExpressionSymbol CreateBinaryExpression(BinaryExpressionSyntax syntax, IExpressionSymbol left, BinaryOperatorType operatorType, IExpressionSymbol right) =>
		context.GetSymbol(syntax, s => new BinaryExpressionSymbol(left, operatorType, right) { Syntax = s });
	public KeywordExpressionSymbol CreateKeywordExpression(KeywordExpressionSyntax syntax, KeywordExpressionType keywordType) =>
		context.GetSymbol(syntax, s => new KeywordExpressionSymbol(keywordType) { Syntax = s });
	public LiteralExpressionSymbol CreateLiteralExpression(LiteralExpressionSyntax syntax, BoundValue literal) =>
		context.GetSymbol(syntax, s => new LiteralExpressionSymbol(literal) { Syntax = s });
	public FunctionInvocationExpressionSymbol CreateFunctionInvocationExpression(FunctionInvocationExpressionSyntax syntax, IFunctionSymbol function, IReadOnlyCollection<IExpressionSymbol> arguments) =>
		context.GetSymbol(syntax, s => new FunctionInvocationExpressionSymbol(function, arguments) { Syntax = s });
	public FunctionPointerExpressionSymbol CreateFunctionPointerExpression(FunctionPointerExpressionSyntax syntax, FunctionSymbol function) =>
		context.GetSymbol(syntax, s => new FunctionPointerExpressionSymbol(function) { Syntax = s });
	public IdentifierExpressionSymbol CreateIdentifierExpression(IdentifierExpressionSyntax syntax, IIdentifiableSymbol identifier) =>
		context.GetSymbol(syntax, s => new IdentifierExpressionSymbol(identifier) { Syntax = s });

}
