using System;
using System.Collections.Generic;
using System.Reflection;
using HytaleClient.Graphics;

namespace HytaleClient.Interface.UI.Markup
{
	// Token: 0x02000845 RID: 2117
	public class Expression
	{
		// Token: 0x06003AE2 RID: 15074 RVA: 0x0008968C File Offset: 0x0008788C
		static Expression()
		{
			foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
			{
				bool flag = type.GetCustomAttribute<UIMarkupDataAttribute>() != null;
				if (flag)
				{
					Expression.RegisteredDataTypes.Add(type.Name, type);
				}
			}
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x00089869 File Offset: 0x00087A69
		private Expression(TextParserSpan span)
		{
			this.SourceSpan = span;
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x0008987A File Offset: 0x00087A7A
		public Expression(decimal value, TextParserSpan span) : this(span)
		{
			this.DecimalValue = new decimal?(value);
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x00089891 File Offset: 0x00087A91
		public Expression(bool value, TextParserSpan span) : this(span)
		{
			this.BooleanValue = new bool?(value);
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x000898A8 File Offset: 0x00087AA8
		public Expression(string value, TextParserSpan span) : this(span)
		{
			this.StringValue = value;
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x000898BA File Offset: 0x00087ABA
		public Expression(Expression.Identifier value, TextParserSpan span) : this(span)
		{
			this.IdentifierValue = value;
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x000898CC File Offset: 0x00087ACC
		public Expression(UInt32Color value, TextParserSpan span) : this(span)
		{
			this.ColorValue = new UInt32Color?(value);
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x000898E3 File Offset: 0x00087AE3
		public Expression(Expression.ExpressionDictionary dictionary, TextParserSpan span) : this(span)
		{
			this.ObjectDictionary = dictionary;
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x000898F5 File Offset: 0x00087AF5
		public Expression(DocumentNode node, TextParserSpan span) : this(span)
		{
			this.DocumentNode = node;
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x00089908 File Offset: 0x00087B08
		public Expression(Expression.ExpressionOperator op, Expression left, Expression right, TextParserSpan span) : this(span)
		{
			this.Operator = op;
			bool flag = op >= Expression.ExpressionOperator.Multiply && op <= Expression.ExpressionOperator.Subtract;
			if (flag)
			{
				this.OperatorCategory = Expression.ExpressionOperatorCategory.Arithmetic;
			}
			else
			{
				bool flag2 = op >= Expression.ExpressionOperator.EqualTo && op <= Expression.ExpressionOperator.LessThanOrEqualTo;
				if (flag2)
				{
					this.OperatorCategory = Expression.ExpressionOperatorCategory.Comparison;
				}
				else
				{
					bool flag3 = op >= Expression.ExpressionOperator.And && op <= Expression.ExpressionOperator.Or;
					if (flag3)
					{
						this.OperatorCategory = Expression.ExpressionOperatorCategory.BooleanLogic;
					}
					else
					{
						bool flag4 = op >= Expression.ExpressionOperator.MemberAccess;
						if (flag4)
						{
							this.OperatorCategory = Expression.ExpressionOperatorCategory.Other;
						}
					}
				}
			}
			this.Left = left;
			this.Right = right;
		}

		// Token: 0x06003AEC RID: 15084 RVA: 0x0008999C File Offset: 0x00087B9C
		public static bool TryResolvePath(string relativePath, string basePath, out string resolvedPath)
		{
			resolvedPath = null;
			List<string> list = new List<string>(basePath.Split(new char[]
			{
				'/'
			}));
			list.RemoveAt(list.Count - 1);
			foreach (string text in relativePath.Split(new char[]
			{
				'/'
			}))
			{
				bool flag = text == "..";
				if (flag)
				{
					bool flag2 = list.Count == 0;
					if (flag2)
					{
						return false;
					}
					list.RemoveAt(list.Count - 1);
				}
				else
				{
					list.Add(text);
				}
			}
			resolvedPath = string.Join("/", list);
			return true;
		}

		// Token: 0x04001B18 RID: 6936
		public static readonly Dictionary<string, Type> RegisteredDataTypes = new Dictionary<string, Type>();

		// Token: 0x04001B19 RID: 6937
		public static readonly Dictionary<string, Expression.ExpressionOperator> Operators = new Dictionary<string, Expression.ExpressionOperator>
		{
			{
				",",
				Expression.ExpressionOperator.Sequence
			},
			{
				"*",
				Expression.ExpressionOperator.Multiply
			},
			{
				"/",
				Expression.ExpressionOperator.Divide
			},
			{
				"+",
				Expression.ExpressionOperator.Add
			},
			{
				"-",
				Expression.ExpressionOperator.Subtract
			},
			{
				"==",
				Expression.ExpressionOperator.EqualTo
			},
			{
				"!=",
				Expression.ExpressionOperator.NotEqualTo
			},
			{
				">",
				Expression.ExpressionOperator.GreaterThan
			},
			{
				"<",
				Expression.ExpressionOperator.LessThan
			},
			{
				"<=",
				Expression.ExpressionOperator.GreaterThanOrEqualTo
			},
			{
				">=",
				Expression.ExpressionOperator.LessThanOrEqualTo
			},
			{
				"&&",
				Expression.ExpressionOperator.And
			},
			{
				"||",
				Expression.ExpressionOperator.Or
			},
			{
				".",
				Expression.ExpressionOperator.MemberAccess
			}
		};

		// Token: 0x04001B1A RID: 6938
		public const int SequenceOperatorPrecedence = 1;

		// Token: 0x04001B1B RID: 6939
		public static readonly Dictionary<string, int> OperatorPrecedences = new Dictionary<string, int>
		{
			{
				",",
				1
			},
			{
				"||",
				2
			},
			{
				"&&",
				3
			},
			{
				"!=",
				4
			},
			{
				"==",
				4
			},
			{
				"<",
				5
			},
			{
				">",
				5
			},
			{
				"<=",
				5
			},
			{
				">=",
				5
			},
			{
				"+",
				6
			},
			{
				"-",
				6
			},
			{
				"*",
				7
			},
			{
				"/",
				7
			},
			{
				".",
				8
			}
		};

		// Token: 0x04001B1C RID: 6940
		public TextParserSpan SourceSpan;

		// Token: 0x04001B1D RID: 6941
		public readonly decimal? DecimalValue;

		// Token: 0x04001B1E RID: 6942
		public readonly bool? BooleanValue;

		// Token: 0x04001B1F RID: 6943
		public readonly string StringValue;

		// Token: 0x04001B20 RID: 6944
		public readonly Expression.Identifier IdentifierValue;

		// Token: 0x04001B21 RID: 6945
		public readonly UInt32Color? ColorValue;

		// Token: 0x04001B22 RID: 6946
		public readonly Expression.ExpressionDictionary ObjectDictionary;

		// Token: 0x04001B23 RID: 6947
		public readonly DocumentNode DocumentNode;

		// Token: 0x04001B24 RID: 6948
		public readonly Expression.ExpressionOperator Operator;

		// Token: 0x04001B25 RID: 6949
		public readonly Expression.ExpressionOperatorCategory OperatorCategory;

		// Token: 0x04001B26 RID: 6950
		public readonly Expression Left;

		// Token: 0x04001B27 RID: 6951
		public readonly Expression Right;

		// Token: 0x02000D1C RID: 3356
		public class ExpressionResolutionException : TextParser.TextParserException
		{
			// Token: 0x060064DA RID: 25818 RVA: 0x0021019E File Offset: 0x0020E39E
			public ExpressionResolutionException(string message, Expression expression, Expression.ExpressionResolutionException inner = null) : base(message, expression.SourceSpan, inner)
			{
				this.Expression = expression;
			}

			// Token: 0x040040C7 RID: 16583
			public readonly Expression Expression;
		}

		// Token: 0x02000D1D RID: 3357
		public class ExpressionDictionary
		{
			// Token: 0x040040C8 RID: 16584
			public string TypeName;

			// Token: 0x040040C9 RID: 16585
			public readonly List<Expression> SpreadReferences = new List<Expression>();

			// Token: 0x040040CA RID: 16586
			public readonly Dictionary<string, Expression> Entries = new Dictionary<string, Expression>();

			// Token: 0x040040CB RID: 16587
			public DocumentNode ScopeNode;
		}

		// Token: 0x02000D1E RID: 3358
		public enum ExpressionOperatorCategory
		{
			// Token: 0x040040CD RID: 16589
			None,
			// Token: 0x040040CE RID: 16590
			Arithmetic,
			// Token: 0x040040CF RID: 16591
			Comparison,
			// Token: 0x040040D0 RID: 16592
			BooleanLogic,
			// Token: 0x040040D1 RID: 16593
			Other
		}

		// Token: 0x02000D1F RID: 3359
		public enum ExpressionOperator
		{
			// Token: 0x040040D3 RID: 16595
			None,
			// Token: 0x040040D4 RID: 16596
			Sequence,
			// Token: 0x040040D5 RID: 16597
			Multiply,
			// Token: 0x040040D6 RID: 16598
			Divide,
			// Token: 0x040040D7 RID: 16599
			Add,
			// Token: 0x040040D8 RID: 16600
			Subtract,
			// Token: 0x040040D9 RID: 16601
			EqualTo,
			// Token: 0x040040DA RID: 16602
			NotEqualTo,
			// Token: 0x040040DB RID: 16603
			GreaterThan,
			// Token: 0x040040DC RID: 16604
			LessThan,
			// Token: 0x040040DD RID: 16605
			GreaterThanOrEqualTo,
			// Token: 0x040040DE RID: 16606
			LessThanOrEqualTo,
			// Token: 0x040040DF RID: 16607
			And,
			// Token: 0x040040E0 RID: 16608
			Or,
			// Token: 0x040040E1 RID: 16609
			MemberAccess
		}

		// Token: 0x02000D20 RID: 3360
		public class Identifier
		{
			// Token: 0x060064DC RID: 25820 RVA: 0x002101D6 File Offset: 0x0020E3D6
			public Identifier(string prefix, string text)
			{
				this.Prefix = prefix;
				this.Text = text;
			}

			// Token: 0x040040E2 RID: 16610
			public readonly string Prefix;

			// Token: 0x040040E3 RID: 16611
			public readonly string Text;
		}
	}
}
