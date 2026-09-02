using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace M1.Core;

public static class QueryParser
{
	public static QueryParseResult Parse(string query, bool cleanupFormatting)
	{
		QueryParseResult queryParseResult = new QueryParseResult();
		TSql100Parser tSql100Parser = new TSql100Parser(initialQuotedIdentifiers: false);
		Dictionary<string, string> scriptRefs = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
		query = preProcessQuery(query, queryParseResult);
		query = checkQueryForScript(query, scriptRefs);
		StringReader input = new StringReader(query);
		IList<ParseError> errors;
		TSqlFragment tSqlFragment = tSql100Parser.Parse(input, out errors);
		if (errors != null && errors.Count != 0)
		{
			queryParseResult.Errors = new List<QueryParseError>();
			foreach (ParseError item in errors)
			{
				queryParseResult.Errors.Add(new QueryParseError(item.Number, item.Offset, item.Line, item.Column, item.Message));
			}
		}
		else
		{
			TSqlScript tSqlScript = tSqlFragment as TSqlScript;
			QuerySpecification querySpecification = (QuerySpecification)((SelectStatement)tSqlScript.Batches[0].Statements[0]).QueryExpression;
			for (int i = 0; i < tSqlScript.ScriptTokenStream.Count; i++)
			{
				if (tSqlScript.ScriptTokenStream[i].TokenType == TSqlTokenType.Variable)
				{
					tSqlScript.ScriptTokenStream[i].Text.StartsWith("@ExternalLink", StringComparison.CurrentCultureIgnoreCase);
				}
			}
			_ = querySpecification.WhereClause;
			foreach (TSqlBatch batch in tSqlScript.Batches)
			{
				foreach (SelectStatement statement in batch.Statements)
				{
					fillDataFromQuery((QuerySpecification)statement.QueryExpression, queryParseResult, scriptRefs, cleanupFormatting);
					queryParseResult.Into = getInto(statement);
				}
			}
		}
		return queryParseResult;
	}

	private static string preProcessQuery(string query, QueryParseResult data)
	{
		data.ExternalLinks = null;
		if (query.StartsWith("\""))
		{
			query = query.Substring(1);
		}
		if (query.EndsWith("\""))
		{
			query = query.Substring(0, query.Length - 1);
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		int num = 0;
		int num2 = 0;
		bool flag = false;
		string text = query;
		foreach (char c in text)
		{
			if (c == '"' && num == 0)
			{
				if (flag)
				{
					flag = false;
					stringBuilder.Append("@ExternalParm" + num2);
					if (data.ExternalLinks == null)
					{
						data.ExternalLinks = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
					}
					data.ExternalLinks.Add("@ExternalParm" + num2, stringBuilder2.ToString());
					stringBuilder2.Length = 0;
					num2++;
				}
				else
				{
					flag = true;
				}
			}
			else if (flag)
			{
				stringBuilder2.Append(c);
			}
			else
			{
				switch (c)
				{
				case '(':
					num++;
					break;
				case ')':
					num--;
					break;
				}
				stringBuilder.Append(c);
			}
		}
		if (flag)
		{
			stringBuilder.Append("@ExternalParm" + num2);
			if (data.ExternalLinks == null)
			{
				data.ExternalLinks = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
			}
			data.ExternalLinks.Add("@ExternalParm" + num2, stringBuilder2.ToString());
			stringBuilder2.Length = 0;
			num2++;
		}
		return stringBuilder.ToString();
	}

	private static string putScriptInQuery(string query, Dictionary<string, string> scriptRefs)
	{
		if (scriptRefs != null && scriptRefs.Count != 0)
		{
			foreach (KeyValuePair<string, string> scriptRef in scriptRefs)
			{
				query = query.Replace(scriptRef.Key, scriptRef.Value);
			}
		}
		return query;
	}

	private static string checkQueryForScript(string query, Dictionary<string, string> scriptRefs)
	{
		int num = query.IndexOf("{!");
		if (num != -1)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num2 = 0;
			int num3 = 0;
			while (num != -1)
			{
				stringBuilder.Append(query.Substring(num2, num - num2));
				num2 = num;
				num = query.IndexOf("!}", num + 2);
				stringBuilder.Append("@QueryScript" + num3);
				scriptRefs.Add("@QueryScript" + num3, query.Substring(num2, num - num2 + 2));
				num3++;
				num2 = num + 2;
				num = query.IndexOf("{!", num2);
			}
			stringBuilder.Append(query.Substring(num2));
			return stringBuilder.ToString();
		}
		return query;
	}

	private static void fillDataFromQuery(QuerySpecification query, QueryParseResult data, Dictionary<string, string> scriptRefs, bool cleanupFormatting)
	{
		data.PrimaryTable = getPrimaryTable(query, cleanupFormatting);
		data.UniqueRowFilter = getUniqueRowFilter(query);
		data.Top = getTop(query, cleanupFormatting);
		data.Fields = putScriptInQuery(getFields(query, cleanupFormatting), scriptRefs);
		data.From = putScriptInQuery(getFromClause(query, cleanupFormatting), scriptRefs);
		data.Where = putScriptInQuery(getWhereClause(query, cleanupFormatting), scriptRefs);
		data.GroupBy = getGroupBy(query, cleanupFormatting);
		data.Having = getHavingClause(query, cleanupFormatting);
		data.OrderBy = getOrderBy(query, cleanupFormatting);
	}

	private static string getObjectName(SchemaObjectName name)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < name.Identifiers.Count; i++)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(".");
			}
			stringBuilder.Append(name.Identifiers[i].Value);
		}
		return stringBuilder.ToString();
	}

	private static string getInto(SelectStatement sqlStatement)
	{
		if (sqlStatement.Into != null)
		{
			return getObjectName(sqlStatement.Into);
		}
		return string.Empty;
	}

	private static string getUniqueRowFilter(QuerySpecification query)
	{
		if (query.UniqueRowFilter == UniqueRowFilter.All)
		{
			return "All";
		}
		if (query.UniqueRowFilter == UniqueRowFilter.Distinct)
		{
			return "Distinct";
		}
		return string.Empty;
	}

	private static string getTop(QuerySpecification query, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (query.TopRowFilter != null)
		{
			stringBuilder.Append(getExpressionText(query.TopRowFilter.Expression, cleanupFormatting));
			if (query.TopRowFilter.Percent)
			{
				stringBuilder.Append(" Percent");
			}
			if (query.TopRowFilter.WithTies)
			{
				stringBuilder.Append(" With Ties");
			}
		}
		return stringBuilder.ToString();
	}

	public static string BuildQueryFromResult(QueryParseResult data)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Select");
		if (!string.IsNullOrWhiteSpace(data.UniqueRowFilter))
		{
			stringBuilder.Append(" " + data.UniqueRowFilter);
		}
		if (!string.IsNullOrWhiteSpace(data.Top))
		{
			stringBuilder.Append(" Top " + data.Top);
		}
		stringBuilder.Append(" " + data.Fields);
		stringBuilder.Append(" From " + data.From);
		if (!string.IsNullOrWhiteSpace(data.Where))
		{
			stringBuilder.Append(" Where " + data.Where);
		}
		if (!string.IsNullOrWhiteSpace(data.GroupBy))
		{
			stringBuilder.Append(" Group By " + data.GroupBy);
		}
		if (!string.IsNullOrWhiteSpace(data.Having))
		{
			stringBuilder.Append(" Having " + data.Having);
		}
		if (!string.IsNullOrWhiteSpace(data.OrderBy))
		{
			stringBuilder.Append(" Order By " + data.OrderBy);
		}
		return stringBuilder.ToString();
	}

	private static string getOrderBy(QuerySpecification query, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (query.OrderByClause != null)
		{
			for (int i = 0; i < query.OrderByClause.OrderByElements.Count; i++)
			{
				getOrderByElement(query.OrderByClause.OrderByElements[i], stringBuilder, cleanupFormatting);
			}
		}
		return stringBuilder.ToString();
	}

	private static string getGroupBy(QuerySpecification query, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (query.GroupByClause != null)
		{
			for (int i = 0; i < query.GroupByClause.GroupingSpecifications.Count; i++)
			{
				getGroup(query.GroupByClause.GroupingSpecifications[i], stringBuilder, cleanupFormatting);
			}
		}
		return stringBuilder.ToString();
	}

	private static string getFields(QuerySpecification query, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < query.SelectElements.Count; i++)
		{
			getField(query.SelectElements[i], stringBuilder, cleanupFormatting);
		}
		return stringBuilder.ToString();
	}

	private static void getField(SelectElement element, StringBuilder builder, bool cleanupFormatting)
	{
		if (builder.Length != 0)
		{
			builder.Append(",");
			if (!cleanupFormatting && element.FirstTokenIndex > 0 && element.ScriptTokenStream[element.FirstTokenIndex - 1].TokenType == TSqlTokenType.WhiteSpace)
			{
				builder.Append(element.ScriptTokenStream[element.FirstTokenIndex - 1].Text);
			}
		}
		if (element is SelectScalarExpression)
		{
			SelectScalarExpression selectScalarExpression = (SelectScalarExpression)element;
			if (cleanupFormatting)
			{
				if (selectScalarExpression.ColumnName != null)
				{
					if (selectScalarExpression.ColumnName.FirstTokenIndex == selectScalarExpression.FirstTokenIndex)
					{
						builder.Append(selectScalarExpression.ColumnName.Value + "=" + getExpressionText(selectScalarExpression.Expression, cleanupFormatting));
					}
					else
					{
						builder.Append(getExpressionText(selectScalarExpression.Expression, cleanupFormatting) + " as " + selectScalarExpression.ColumnName.Value);
					}
				}
				else
				{
					builder.Append(getExpressionText(selectScalarExpression.Expression, cleanupFormatting));
				}
			}
			else
			{
				builder.Append(combineTokens(selectScalarExpression.ScriptTokenStream, selectScalarExpression.FirstTokenIndex, selectScalarExpression.Expression.FirstTokenIndex - 1));
				builder.Append(getExpressionText(selectScalarExpression.Expression, cleanupFormatting));
				builder.Append(combineTokens(selectScalarExpression.ScriptTokenStream, selectScalarExpression.Expression.LastTokenIndex + 1, selectScalarExpression.LastTokenIndex));
			}
		}
		else
		{
			if (!(element is SelectStarExpression))
			{
				throw new Exception("Unhandled SelectElement in getField - " + element.GetType().Name);
			}
			SelectStarExpression selectStarExpression = (SelectStarExpression)element;
			if (selectStarExpression.Qualifier != null)
			{
				builder.Append(getMultiPartIdentifierText(selectStarExpression.Qualifier));
				builder.Append(".");
			}
			builder.Append("*");
		}
	}

	private static void getGroup(GroupingSpecification element, StringBuilder builder, bool cleanupFormatting)
	{
		if (builder.Length != 0)
		{
			builder.Append(",");
		}
		if (element is ExpressionGroupingSpecification)
		{
			ExpressionGroupingSpecification expressionGroupingSpecification = (ExpressionGroupingSpecification)element;
			builder.Append(getExpressionText(expressionGroupingSpecification.Expression, cleanupFormatting));
			return;
		}
		throw new Exception("Unhandled GroupingSpecification in getGroup - " + element.GetType().Name);
	}

	private static void getOrderByElement(ExpressionWithSortOrder element, StringBuilder builder, bool cleanupFormatting)
	{
		if (builder.Length != 0)
		{
			builder.Append(",");
		}
		builder.Append(getExpressionText(element.Expression, cleanupFormatting));
		if (element.SortOrder == SortOrder.Ascending)
		{
			builder.Append(" Asc");
		}
		else if (element.SortOrder == SortOrder.Descending)
		{
			builder.Append(" Desc");
		}
	}

	private static string getMultiPartIdentifierText(MultiPartIdentifier id)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < id.Identifiers.Count; i++)
		{
			if (i != 0)
			{
				stringBuilder.Append(".");
			}
			stringBuilder.Append(id.Identifiers[i].Value);
		}
		return stringBuilder.ToString();
	}

	private static string getColumnReferenceExpressionText(ColumnReferenceExpression colExpr)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (colExpr.ColumnType == ColumnType.Regular)
		{
			stringBuilder.Append(getMultiPartIdentifierText(colExpr.MultiPartIdentifier));
		}
		else if (colExpr.ColumnType == ColumnType.Wildcard)
		{
			if (colExpr.MultiPartIdentifier != null)
			{
				stringBuilder.Append(getMultiPartIdentifierText(colExpr.MultiPartIdentifier));
				stringBuilder.Append(".");
			}
			stringBuilder.Append("*");
		}
		return stringBuilder.ToString();
	}

	private static string getBinaryExpressionText(BinaryExpression binaryExpr, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(getExpressionText(binaryExpr.FirstExpression, cleanupFormatting));
		if (cleanupFormatting)
		{
			if (binaryExpr.BinaryExpressionType == BinaryExpressionType.Add)
			{
				stringBuilder.Append("+");
			}
			else if (binaryExpr.BinaryExpressionType == BinaryExpressionType.Subtract)
			{
				stringBuilder.Append("-");
			}
			else if (binaryExpr.BinaryExpressionType == BinaryExpressionType.Divide)
			{
				stringBuilder.Append("/");
			}
			else
			{
				if (binaryExpr.BinaryExpressionType != BinaryExpressionType.Multiply)
				{
					throw new Exception("Unhandled BinaryExpressionType in getBinaryExpressionText - " + binaryExpr.BinaryExpressionType);
				}
				stringBuilder.Append("*");
			}
		}
		else
		{
			stringBuilder.Append(combineTokens(binaryExpr.ScriptTokenStream, binaryExpr.FirstExpression.LastTokenIndex + 1, binaryExpr.SecondExpression.FirstTokenIndex - 1));
		}
		stringBuilder.Append(getExpressionText(binaryExpr.SecondExpression, cleanupFormatting));
		return stringBuilder.ToString();
	}

	private static string getBooleanBinaryExpressionText(BooleanBinaryExpression binaryExpr, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(getBooleanExpressionText(binaryExpr.FirstExpression, cleanupFormatting));
		if (binaryExpr.BinaryExpressionType == BooleanBinaryExpressionType.And)
		{
			stringBuilder.Append(" And ");
		}
		else if (binaryExpr.BinaryExpressionType == BooleanBinaryExpressionType.Or)
		{
			stringBuilder.Append(" Or ");
		}
		stringBuilder.Append(getBooleanExpressionText(binaryExpr.SecondExpression, cleanupFormatting));
		return stringBuilder.ToString();
	}

	private static string EscapeString(string value, bool isNational)
	{
		if (isNational)
		{
			return "N'" + value.Replace("'", "''") + "'";
		}
		return "'" + value.Replace("'", "''") + "'";
	}

	private static string getLiteralText(Literal expr)
	{
		return expr.ScriptTokenStream[expr.FirstTokenIndex].Text;
	}

	private static string getExpressionText(ScalarExpression expr, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (expr is ColumnReferenceExpression)
		{
			stringBuilder.Append(getColumnReferenceExpressionText((ColumnReferenceExpression)expr));
		}
		else if (expr is Literal)
		{
			stringBuilder.Append(getLiteralText((Literal)expr));
		}
		else if (expr is BinaryExpression)
		{
			stringBuilder.Append(getBinaryExpressionText((BinaryExpression)expr, cleanupFormatting));
		}
		else if (expr is FunctionCall)
		{
			stringBuilder.Append(getFunctionCallText((FunctionCall)expr, cleanupFormatting));
		}
		else if (expr is ConvertCall)
		{
			stringBuilder.Append(getConvertCallText((ConvertCall)expr, cleanupFormatting));
		}
		else if (expr is CastCall)
		{
			stringBuilder.Append(getCastCallText((CastCall)expr, cleanupFormatting));
		}
		else if (expr is ScalarSubquery)
		{
			stringBuilder.Append("(" + getScalarSubquery((ScalarSubquery)expr, cleanupFormatting) + ")");
		}
		else if (expr is ParenthesisExpression)
		{
			stringBuilder.Append("(" + getExpressionText(((ParenthesisExpression)expr).Expression, cleanupFormatting) + ")");
		}
		else if (expr is UnaryExpression)
		{
			stringBuilder.Append(getUnaryExpressionText((UnaryExpression)expr, cleanupFormatting));
		}
		else if (expr is SearchedCaseExpression)
		{
			stringBuilder.Append(getSearchedCaseText((SearchedCaseExpression)expr, cleanupFormatting));
		}
		else
		{
			if (!(expr is VariableReference))
			{
				throw new Exception("Unhandled ScalarExpression in getExpressionText - " + expr.GetType().Name);
			}
			stringBuilder.Append(getVariableReferenceText((VariableReference)expr));
		}
		doCommentCheck(expr, stringBuilder);
		return stringBuilder.ToString();
	}

	private static string getVariableReferenceText(VariableReference expr)
	{
		return expr.Name;
	}

	private static string getSearchedCaseText(SearchedCaseExpression expr, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Case");
		for (int i = 0; i < expr.WhenClauses.Count; i++)
		{
			stringBuilder.Append(" When " + getBooleanExpressionText(expr.WhenClauses[i].WhenExpression, cleanupFormatting));
			stringBuilder.Append(" Then " + getExpressionText(expr.WhenClauses[i].ThenExpression, cleanupFormatting));
		}
		if (expr.ElseExpression != null)
		{
			stringBuilder.Append(" Else " + getExpressionText(expr.ElseExpression, cleanupFormatting));
		}
		stringBuilder.Append(" End");
		return stringBuilder.ToString();
	}

	private static string getUnaryExpressionText(UnaryExpression expr, bool cleanupFormatting)
	{
		if (expr.UnaryExpressionType == UnaryExpressionType.Negative)
		{
			return "-" + getExpressionText(expr.Expression, cleanupFormatting);
		}
		if (expr.UnaryExpressionType == UnaryExpressionType.Positive)
		{
			return "+" + getExpressionText(expr.Expression, cleanupFormatting);
		}
		if (expr.UnaryExpressionType == UnaryExpressionType.BitwiseNot)
		{
			return "~" + getExpressionText(expr.Expression, cleanupFormatting);
		}
		throw new Exception("Unhandled UnaryExpressionType in getUnaryExpressionText - " + expr.UnaryExpressionType);
	}

	private static string getScalarSubquery(ScalarSubquery subQuery, bool cleanupFormatting)
	{
		return getQueryExpressionText(subQuery.QueryExpression, cleanupFormatting);
	}

	private static string getDataTypeText(DataTypeReference dataType, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(getObjectName(dataType.Name));
		if (dataType is SqlDataTypeReference)
		{
			SqlDataTypeReference sqlDataTypeReference = (SqlDataTypeReference)dataType;
			if (sqlDataTypeReference.Parameters.Count != 0)
			{
				stringBuilder.Append("(");
				for (int i = 0; i < sqlDataTypeReference.Parameters.Count; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(getExpressionText(sqlDataTypeReference.Parameters[i], cleanupFormatting));
				}
				stringBuilder.Append(")");
			}
		}
		return stringBuilder.ToString();
	}

	private static string getCastCallText(CastCall func, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Cast(");
		stringBuilder.Append(getExpressionText(func.Parameter, cleanupFormatting));
		stringBuilder.Append(" as ");
		stringBuilder.Append(getDataTypeText(func.DataType, cleanupFormatting));
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}

	private static string getConvertCallText(ConvertCall func, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Convert(");
		stringBuilder.Append(getDataTypeText(func.DataType, cleanupFormatting));
		stringBuilder.Append(",");
		stringBuilder.Append(getExpressionText(func.Parameter, cleanupFormatting));
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}

	private static string getFunctionCallText(FunctionCall func, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(func.FunctionName.Value);
		stringBuilder.Append("(");
		for (int i = 0; i < func.Parameters.Count; i++)
		{
			if (i != 0)
			{
				if (cleanupFormatting)
				{
					stringBuilder.Append(",");
				}
				else
				{
					stringBuilder.Append(combineTokens(func.ScriptTokenStream, func.Parameters[i - 1].LastTokenIndex + 1, func.Parameters[i].FirstTokenIndex - 1));
				}
			}
			stringBuilder.Append(getExpressionText(func.Parameters[i], cleanupFormatting));
		}
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}

	private static string getWhereClause(QuerySpecification query, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (query.WhereClause != null)
		{
			stringBuilder.Append(getBooleanExpressionText(query.WhereClause.SearchCondition, cleanupFormatting));
		}
		return stringBuilder.ToString();
	}

	private static string getHavingClause(QuerySpecification query, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (query.HavingClause != null)
		{
			stringBuilder.Append(getBooleanExpressionText(query.HavingClause.SearchCondition, cleanupFormatting));
		}
		return stringBuilder.ToString();
	}

	private static string getBooleanExpressionText(BooleanExpression expr, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (expr is BooleanComparisonExpression)
		{
			stringBuilder.Append(getBooleanComparisonExpression((BooleanComparisonExpression)expr, cleanupFormatting));
		}
		else if (expr is BooleanBinaryExpression)
		{
			stringBuilder.Append(getBooleanBinaryExpressionText((BooleanBinaryExpression)expr, cleanupFormatting));
		}
		else if (expr is LikePredicate)
		{
			LikePredicate likePredicate = (LikePredicate)expr;
			stringBuilder.Append(getExpressionText(likePredicate.FirstExpression, cleanupFormatting));
			stringBuilder.Append(" Like ");
			stringBuilder.Append(getExpressionText(likePredicate.SecondExpression, cleanupFormatting));
		}
		else if (expr is BooleanParenthesisExpression)
		{
			BooleanParenthesisExpression booleanParenthesisExpression = (BooleanParenthesisExpression)expr;
			stringBuilder.Append("(");
			stringBuilder.Append(getBooleanExpressionText(booleanParenthesisExpression.Expression, cleanupFormatting));
			stringBuilder.Append(")");
		}
		else if (expr is BooleanNotExpression)
		{
			BooleanNotExpression booleanNotExpression = (BooleanNotExpression)expr;
			stringBuilder.Append(" Not " + getBooleanExpressionText(booleanNotExpression.Expression, cleanupFormatting));
		}
		else if (expr is BooleanIsNullExpression)
		{
			BooleanIsNullExpression booleanIsNullExpression = (BooleanIsNullExpression)expr;
			stringBuilder.Append(getExpressionText(booleanIsNullExpression.Expression, cleanupFormatting));
			if (booleanIsNullExpression.IsNot)
			{
				stringBuilder.Append(" Is Not Null");
			}
			else
			{
				stringBuilder.Append(" Is Null");
			}
		}
		else if (expr is InPredicate)
		{
			stringBuilder.Append(getInPredicateText((InPredicate)expr, cleanupFormatting));
		}
		else
		{
			if (!(expr is ExistsPredicate))
			{
				throw new Exception("Unhandled BooleanExpression type in getBooleanExpressionText - " + expr.GetType().Name);
			}
			stringBuilder.Append(getExistsPredicateText((ExistsPredicate)expr, cleanupFormatting));
		}
		return stringBuilder.ToString();
	}

	private static void doCommentCheck(TSqlFragment expr, StringBuilder builder)
	{
		if (expr.LastTokenIndex + 1 < expr.ScriptTokenStream.Count && expr.ScriptTokenStream[expr.LastTokenIndex + 1].TokenType == TSqlTokenType.MultilineComment)
		{
			builder.Append(expr.ScriptTokenStream[expr.LastTokenIndex + 1].Text);
		}
		if (expr.LastTokenIndex + 2 < expr.ScriptTokenStream.Count && expr.ScriptTokenStream[expr.LastTokenIndex + 1].TokenType == TSqlTokenType.WhiteSpace && expr.ScriptTokenStream[expr.LastTokenIndex + 2].TokenType == TSqlTokenType.MultilineComment)
		{
			builder.Append(expr.ScriptTokenStream[expr.LastTokenIndex + 1].Text);
			builder.Append(expr.ScriptTokenStream[expr.LastTokenIndex + 2].Text);
		}
	}

	private static string getExistsPredicateText(ExistsPredicate expr, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(" Exists ");
		if (expr.Subquery != null)
		{
			stringBuilder.Append("(" + getScalarSubquery(expr.Subquery, cleanupFormatting) + ")");
			return stringBuilder.ToString();
		}
		throw new Exception("Unhandled ExistsPredicate in getExistsPredicateText - ");
	}

	private static string getInPredicateText(InPredicate expr, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(getExpressionText(expr.Expression, cleanupFormatting));
		if (expr.NotDefined)
		{
			stringBuilder.Append(" Not In ");
		}
		else
		{
			stringBuilder.Append(" In ");
		}
		if (expr.Subquery != null)
		{
			stringBuilder.Append("(" + getScalarSubquery(expr.Subquery, cleanupFormatting) + ")");
		}
		else
		{
			if (expr.Values.Count == 0)
			{
				throw new Exception("Unhandled InPredicate in getInPredicateText - ");
			}
			stringBuilder.Append("(");
			for (int i = 0; i < expr.Values.Count; i++)
			{
				if (i != 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(getExpressionText(expr.Values[i], cleanupFormatting));
			}
			stringBuilder.Append(")");
		}
		return stringBuilder.ToString();
	}

	private static string getBooleanComparisonExpression(BooleanComparisonExpression expr, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (cleanupFormatting)
		{
			if (expr.ComparisonType == BooleanComparisonType.Equals)
			{
				stringBuilder.Append(getExpressionText(expr.FirstExpression, cleanupFormatting));
				stringBuilder.Append("=");
				stringBuilder.Append(getExpressionText(expr.SecondExpression, cleanupFormatting));
			}
			else if (expr.ComparisonType == BooleanComparisonType.GreaterThan)
			{
				stringBuilder.Append(getExpressionText(expr.FirstExpression, cleanupFormatting));
				stringBuilder.Append(">");
				stringBuilder.Append(getExpressionText(expr.SecondExpression, cleanupFormatting));
			}
			else if (expr.ComparisonType == BooleanComparisonType.GreaterThanOrEqualTo)
			{
				stringBuilder.Append(getExpressionText(expr.FirstExpression, cleanupFormatting));
				stringBuilder.Append(">=");
				stringBuilder.Append(getExpressionText(expr.SecondExpression, cleanupFormatting));
			}
			else if (expr.ComparisonType == BooleanComparisonType.LessThan)
			{
				stringBuilder.Append(getExpressionText(expr.FirstExpression, cleanupFormatting));
				stringBuilder.Append("<");
				stringBuilder.Append(getExpressionText(expr.SecondExpression, cleanupFormatting));
			}
			else if (expr.ComparisonType == BooleanComparisonType.LessThanOrEqualTo)
			{
				stringBuilder.Append(getExpressionText(expr.FirstExpression, cleanupFormatting));
				stringBuilder.Append("<=");
				stringBuilder.Append(getExpressionText(expr.SecondExpression, cleanupFormatting));
			}
			else
			{
				if (expr.ComparisonType != BooleanComparisonType.NotEqualToBrackets)
				{
					throw new Exception("Unhandled comparison type in getBooleanComparisonExpression - " + expr.ComparisonType);
				}
				stringBuilder.Append(getExpressionText(expr.FirstExpression, cleanupFormatting));
				stringBuilder.Append("<>");
				stringBuilder.Append(getExpressionText(expr.SecondExpression, cleanupFormatting));
			}
		}
		else
		{
			stringBuilder.Append(getExpressionText(expr.FirstExpression, cleanupFormatting));
			stringBuilder.Append(combineTokens(expr.ScriptTokenStream, expr.FirstExpression.LastTokenIndex + 1, expr.SecondExpression.FirstTokenIndex - 1));
			stringBuilder.Append(getExpressionText(expr.SecondExpression, cleanupFormatting));
		}
		return stringBuilder.ToString();
	}

	private static string getPrimaryTable(QuerySpecification query, bool cleanupFormatting)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		for (int i = 0; i < query.SelectElements.Count; i++)
		{
			if (!(query.SelectElements[i] is SelectStarExpression))
			{
				continue;
			}
			SelectStarExpression selectStarExpression = (SelectStarExpression)query.SelectElements[i];
			if (selectStarExpression.Qualifier != null)
			{
				text2 = getMultiPartIdentifierText(selectStarExpression.Qualifier);
				if (!string.IsNullOrWhiteSpace(text2))
				{
					break;
				}
			}
		}
		for (int j = 0; j < query.FromClause.TableReferences.Count; j++)
		{
			text = getTableOnly(query.FromClause.TableReferences[j]);
			if (!string.IsNullOrWhiteSpace(text))
			{
				break;
			}
		}
		if (!string.IsNullOrWhiteSpace(text2))
		{
			return text2;
		}
		return text;
	}

	private static string getTableOnly(TableReference table)
	{
		if (table is NamedTableReference)
		{
			return getObjectName(((NamedTableReference)table).SchemaObject);
		}
		if (table is QualifiedJoin)
		{
			QualifiedJoin qualifiedJoin = (QualifiedJoin)table;
			string tableOnly = getTableOnly(qualifiedJoin.FirstTableReference);
			if (string.IsNullOrWhiteSpace(tableOnly))
			{
				tableOnly = getTableOnly(qualifiedJoin.SecondTableReference);
			}
			return tableOnly;
		}
		if (table is QueryDerivedTable)
		{
			return getTableOnlyDerived(((QueryDerivedTable)table).QueryExpression);
		}
		return string.Empty;
	}

	private static string getTableOnlyDerived(QueryExpression expr)
	{
		if (expr is QuerySpecification)
		{
			QueryParseResult queryParseResult = new QueryParseResult();
			fillDataFromQuery((QuerySpecification)expr, queryParseResult, null, cleanupFormatting: false);
			return queryParseResult.PrimaryTable;
		}
		if (expr is BinaryQueryExpression)
		{
			BinaryQueryExpression binaryQueryExpression = (BinaryQueryExpression)expr;
			string tableOnlyDerived = getTableOnlyDerived(binaryQueryExpression.FirstQueryExpression);
			if (string.IsNullOrWhiteSpace(tableOnlyDerived))
			{
				tableOnlyDerived = getTableOnlyDerived(binaryQueryExpression.SecondQueryExpression);
			}
			return tableOnlyDerived;
		}
		if (expr is QueryParenthesisExpression)
		{
			return getTableOnlyDerived(((QueryParenthesisExpression)expr).QueryExpression);
		}
		return string.Empty;
	}

	private static string getFromClause(QuerySpecification query, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < query.FromClause.TableReferences.Count; i++)
		{
			if (i != 0 && query.FromClause.TableReferences[i] is NamedTableReference)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append(getTableReferenceText(query.FromClause.TableReferences[i], cleanupFormatting));
		}
		return stringBuilder.ToString();
	}

	private static string combineTokens(IList<TSqlParserToken> tokenStream, int start, int end)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = start; i <= end; i++)
		{
			stringBuilder.Append(tokenStream[i].Text);
		}
		return stringBuilder.ToString();
	}

	private static string getJoinType(QualifiedJoin join, bool cleanupFormatting)
	{
		if (cleanupFormatting)
		{
			QualifiedJoinType qualifiedJoinType = join.QualifiedJoinType;
			return qualifiedJoinType switch
			{
				QualifiedJoinType.Inner => " inner join ", 
				QualifiedJoinType.LeftOuter => " left outer join ", 
				QualifiedJoinType.RightOuter => " right outer join ", 
				QualifiedJoinType.FullOuter => " full outer join ", 
				_ => throw new Exception("Unhandled join type in getJoinType - " + qualifiedJoinType), 
			};
		}
		return combineTokens(join.ScriptTokenStream, join.FirstTableReference.LastTokenIndex + 1, join.SecondTableReference.FirstTokenIndex - 1);
	}

	private static string getTableReferenceText(TableReference table, bool cleanupFormatting)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (table is QualifiedJoin)
		{
			QualifiedJoin qualifiedJoin = (QualifiedJoin)table;
			stringBuilder.AppendFormat("{0}{1}{2} On {3}", getTableReferenceText(qualifiedJoin.FirstTableReference, cleanupFormatting), getJoinType(qualifiedJoin, cleanupFormatting), getTableReferenceText(qualifiedJoin.SecondTableReference, cleanupFormatting), getBooleanExpressionText(qualifiedJoin.SearchCondition, cleanupFormatting));
		}
		else if (table is NamedTableReference)
		{
			NamedTableReference namedTableReference = (NamedTableReference)table;
			stringBuilder.Append(getObjectName(namedTableReference.SchemaObject));
			if (namedTableReference.Alias != null)
			{
				stringBuilder.Append(" " + namedTableReference.Alias.Value);
			}
		}
		else
		{
			if (!(table is QueryDerivedTable))
			{
				throw new Exception("Unhandled TableReference type in getTableReferenceText - " + table.GetType().Name);
			}
			QueryDerivedTable queryDerivedTable = (QueryDerivedTable)table;
			stringBuilder.Append("(" + getQueryExpressionText(queryDerivedTable.QueryExpression, cleanupFormatting) + ")");
			if (queryDerivedTable.Alias != null)
			{
				if (queryDerivedTable.ScriptTokenStream[queryDerivedTable.LastTokenIndex - 1].TokenType == TSqlTokenType.WhiteSpace && queryDerivedTable.ScriptTokenStream[queryDerivedTable.LastTokenIndex - 2].TokenType == TSqlTokenType.As)
				{
					stringBuilder.Append(" as " + queryDerivedTable.Alias.Value);
				}
				else
				{
					stringBuilder.Append(" " + queryDerivedTable.Alias.Value);
				}
			}
		}
		return stringBuilder.ToString();
	}

	private static string getQueryExpressionText(QueryExpression queryExpr, bool cleanupFormatting)
	{
		if (queryExpr is QuerySpecification)
		{
			QueryParseResult data = new QueryParseResult();
			fillDataFromQuery((QuerySpecification)queryExpr, data, null, cleanupFormatting);
			return BuildQueryFromResult(data);
		}
		if (queryExpr is BinaryQueryExpression)
		{
			new QueryParseResult();
			StringBuilder stringBuilder = new StringBuilder();
			BinaryQueryExpression binaryQueryExpression = (BinaryQueryExpression)queryExpr;
			stringBuilder.Append(getQueryExpressionText(binaryQueryExpression.FirstQueryExpression, cleanupFormatting));
			if (binaryQueryExpression.BinaryQueryExpressionType == BinaryQueryExpressionType.Union)
			{
				stringBuilder.Append("\r Union ");
			}
			else if (binaryQueryExpression.BinaryQueryExpressionType == BinaryQueryExpressionType.Except)
			{
				stringBuilder.Append("\r Except ");
			}
			else if (binaryQueryExpression.BinaryQueryExpressionType == BinaryQueryExpressionType.Intersect)
			{
				stringBuilder.Append("\r Intersect ");
			}
			if (binaryQueryExpression.All)
			{
				stringBuilder.Append("All ");
			}
			stringBuilder.Append(getQueryExpressionText(binaryQueryExpression.SecondQueryExpression, cleanupFormatting));
			return stringBuilder.ToString();
		}
		if (queryExpr is QueryParenthesisExpression)
		{
			QueryParenthesisExpression queryParenthesisExpression = (QueryParenthesisExpression)queryExpr;
			return "(" + getQueryExpressionText(queryParenthesisExpression.QueryExpression, cleanupFormatting) + ")";
		}
		throw new Exception("Unhandled QueryExpression type in getQueryExpressionText - " + queryExpr.GetType().Name);
	}
}
