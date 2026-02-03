using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using HytaleClient.Graphics;

namespace HytaleClient.Interface.UI.Markup
{
	// Token: 0x02000843 RID: 2115
	internal class DocumentParser : TextParser
	{
		// Token: 0x06003AC8 RID: 15048 RVA: 0x00087F70 File Offset: 0x00086170
		public DocumentParser(string data, string sourcePath) : base(data, sourcePath)
		{
		}

		// Token: 0x06003AC9 RID: 15049 RVA: 0x00087F7C File Offset: 0x0008617C
		public static Document Parse(string data, string sourcePath)
		{
			DocumentParser documentParser = new DocumentParser(data, sourcePath);
			Document document = new Document(documentParser);
			for (;;)
			{
				documentParser.TryEatWhitespaceOrComment();
				bool flag = documentParser.IsEOF();
				if (flag)
				{
					break;
				}
				bool flag2 = documentParser.TryEatNamedExpressionDeclaration(document.RootNode, document.RootNode.Data);
				if (!flag2)
				{
					bool flag3 = documentParser.TryEatNamedDocumentDeclaraction(document);
					if (!flag3)
					{
						DocumentNode documentNode;
						bool flag4 = documentParser.TryEatNodeDefinition(out documentNode);
						if (flag4)
						{
							documentNode.ParentNode = document.RootNode;
							document.RootNode.Data.Children.Add(documentNode);
						}
						else
						{
							bool flag5 = !documentParser.IsEOF();
							if (flag5)
							{
								goto Block_5;
							}
							Debug.Assert(documentParser._spanStack.Count == 0, "Span stack should be empty");
						}
					}
				}
			}
			return document;
			Block_5:
			throw new TextParser.TextParserException("Expected end of file", new TextParserSpan(documentParser.Cursor, documentParser.Cursor, documentParser));
		}

		// Token: 0x06003ACA RID: 15050 RVA: 0x00088070 File Offset: 0x00086270
		public bool TryEatWhitespaceOrComment()
		{
			bool flag = base.IsEOF();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = false;
				do
				{
					char c = this.Data[this.Cursor];
					char c2 = (this.Cursor < this.Data.Length - 1) ? this.Data[this.Cursor + 1] : ' ';
					bool flag3 = c == ' ' || c == '\n';
					bool flag4 = c == '/' && c2 == '*';
					bool flag5 = c == '/' && c2 == '/';
					bool flag6 = !flag3 && !flag4 && !flag5;
					if (flag6)
					{
						break;
					}
					flag2 = true;
					bool flag7 = flag4;
					if (flag7)
					{
						this.EatMultilineComment();
					}
					else
					{
						bool flag8 = flag5;
						if (flag8)
						{
							this.EatLineEndComment();
						}
						else
						{
							this.Cursor++;
						}
					}
				}
				while (!base.IsEOF());
				result = flag2;
			}
			return result;
		}

		// Token: 0x06003ACB RID: 15051 RVA: 0x00088164 File Offset: 0x00086364
		private void EatMultilineComment()
		{
			int startCursor = base.PushSpan();
			base.Eat("/*");
			for (;;)
			{
				bool flag = base.IsEOF();
				if (flag)
				{
					break;
				}
				bool flag2 = base.TryEat("*/");
				if (flag2)
				{
					goto Block_2;
				}
				this.Cursor++;
			}
			throw new TextParser.TextParserException("Encountered end of file while parsing comment", base.PopSpan(startCursor));
			Block_2:
			base.PopSpan(startCursor);
		}

		// Token: 0x06003ACC RID: 15052 RVA: 0x000881D0 File Offset: 0x000863D0
		private void EatLineEndComment()
		{
			base.Eat("//");
			bool flag2;
			do
			{
				bool flag = base.IsEOF();
				if (flag)
				{
					break;
				}
				char c = this.Data[this.Cursor];
				this.Cursor++;
				flag2 = (c == '\n');
			}
			while (!flag2);
		}

		// Token: 0x06003ACD RID: 15053 RVA: 0x0008822C File Offset: 0x0008642C
		public bool TryEatIdentifier(string kind, out string identifier)
		{
			identifier = null;
			bool flag = base.IsEOF();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				char c = this.Data[this.Cursor];
				bool flag2 = (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
				if (flag2)
				{
					identifier = this.EatIdentifier(kind, false);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06003ACE RID: 15054 RVA: 0x00088294 File Offset: 0x00086494
		public string EatIdentifier(string kind, bool allowDigitAsFirstChar = false)
		{
			int startCursor = base.PushSpan();
			bool flag = base.IsEOF();
			if (flag)
			{
				throw new TextParser.TextParserException("Expected " + kind + ", found end of file", base.PopSpan(startCursor));
			}
			string text = "";
			while (!base.IsEOF())
			{
				char c = this.Data[this.Cursor];
				bool flag2 = (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
				if (flag2)
				{
					text += c.ToString();
					this.Cursor++;
				}
				else
				{
					bool flag3 = c >= '0' && c <= '9';
					if (flag3)
					{
						bool flag4 = text.Length == 0 && !allowDigitAsFirstChar;
						if (flag4)
						{
							throw new TextParser.TextParserException("Expected " + kind + ", found digit", base.PopSpan(startCursor));
						}
						text += c.ToString();
						this.Cursor++;
					}
					else
					{
						bool flag5 = text.Length == 0;
						if (flag5)
						{
							throw new TextParser.TextParserException(string.Format("Expected {0}, found {1}", kind, c), base.PopSpan(startCursor));
						}
						break;
					}
				}
			}
			base.PopSpan(startCursor);
			return text;
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x000883F0 File Offset: 0x000865F0
		public bool TryEatNamedExpressionDeclaration(DocumentNode scopeNode, DocumentNode.NodeData nodeData)
		{
			int cursor = this.Cursor;
			bool flag = !base.TryEat('@');
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int startCursor = base.PushSpan();
				string text = this.EatIdentifier("expression name", false);
				TextParserSpan span = base.PopSpan(startCursor);
				this.TryEatWhitespaceOrComment();
				bool flag2 = !base.TryEat('=');
				if (flag2)
				{
					this.Cursor = cursor;
					result = false;
				}
				else
				{
					this.TryEatWhitespaceOrComment();
					Expression value = this.EatExpression(scopeNode, 0);
					bool flag3 = nodeData.LocalNamedExpressions.ContainsKey(text);
					if (flag3)
					{
						throw new TextParser.TextParserException("Named expression with same key " + text + " has already been defined", span);
					}
					nodeData.LocalNamedExpressions.Add(text, value);
					this.TryEatWhitespaceOrComment();
					base.Eat(';');
					this.TryEatWhitespaceOrComment();
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x000884C8 File Offset: 0x000866C8
		private Expression EatNamedExpressionReference()
		{
			Expression expression = null;
			bool flag = base.Peek('$');
			if (flag)
			{
				int startCursor = base.PushSpan();
				base.Eat('$');
				string text = this.EatIdentifier("document name", false);
				expression = new Expression(new Expression.Identifier("$", text), base.PopSpan(startCursor));
				base.Eat('.');
			}
			int startCursor2 = base.PushSpan();
			base.Eat('@');
			Expression expression2 = new Expression(new Expression.Identifier("@", this.EatIdentifier("expression name", false)), base.PopSpan(startCursor2));
			bool flag2 = expression != null;
			Expression result;
			if (flag2)
			{
				result = new Expression(Expression.ExpressionOperator.MemberAccess, expression, expression2, new TextParserSpan(expression.SourceSpan.Start, expression2.SourceSpan.End, this));
			}
			else
			{
				result = expression2;
			}
			return result;
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x00088598 File Offset: 0x00086798
		private Expression EatNamedExpressionMemberReference()
		{
			Expression expression = null;
			bool flag = base.Peek('$');
			if (flag)
			{
				int startCursor = base.PushSpan();
				base.Eat('$');
				string text = this.EatIdentifier("document name", false);
				expression = new Expression(new Expression.Identifier("$", text), base.PopSpan(startCursor));
				base.Eat('.');
			}
			int startCursor2 = base.PushSpan();
			base.Eat('@');
			Expression expression2 = new Expression(new Expression.Identifier("@", this.EatIdentifier("expression name", false)), base.PopSpan(startCursor2));
			bool flag2 = expression != null;
			Expression expression3;
			if (flag2)
			{
				expression3 = new Expression(Expression.ExpressionOperator.MemberAccess, expression, expression2, new TextParserSpan(expression.SourceSpan.Start, expression2.SourceSpan.End, this));
			}
			else
			{
				expression3 = expression2;
			}
			while (base.TryEat('.'))
			{
				int cursor = this.Cursor;
				int startCursor3 = base.PushSpan();
				Expression expression4 = new Expression(new Expression.Identifier(null, this.EatIdentifier("member", false)), base.PopSpan(startCursor3));
				expression3 = new Expression(Expression.ExpressionOperator.MemberAccess, expression3, expression4, new TextParserSpan(expression3.SourceSpan.Start, expression4.SourceSpan.End, this));
			}
			return expression3;
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x000886D4 File Offset: 0x000868D4
		public bool TryEatNamedDocumentDeclaraction(Document doc)
		{
			int cursor = this.Cursor;
			bool flag = !base.TryEat('$');
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				string key = this.EatIdentifier("document name", false);
				this.TryEatWhitespaceOrComment();
				bool flag2 = !base.TryEat('=');
				if (flag2)
				{
					this.Cursor = cursor;
					result = false;
				}
				else
				{
					this.TryEatWhitespaceOrComment();
					int startCursor = base.PushSpan();
					string text = this.EatDoubleQuotedString();
					string value;
					bool flag3 = !Expression.TryResolvePath(text, this.SourcePath, out value);
					if (flag3)
					{
						throw new TextParser.TextParserException("Could not resolve relative path: " + text, base.PopSpan(startCursor));
					}
					base.PopSpan(startCursor);
					doc.NamedDocumentReferences.Add(key, value);
					base.Eat(';');
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06003AD3 RID: 15059 RVA: 0x000887A0 File Offset: 0x000869A0
		public bool TryEatNodeDefinition(out DocumentNode node)
		{
			node = new DocumentNode();
			int startCursor = base.PushSpan();
			int startCursor2 = base.PushSpan();
			bool flag = base.Peek('$') || base.Peek('@');
			if (flag)
			{
				Expression namedTemplateExpression = this.EatNamedExpressionReference();
				node.TemplateSetup = new DocumentNode.NodeTemplateSetup
				{
					NamedTemplateExpression = namedTemplateExpression
				};
			}
			else
			{
				int startCursor3 = base.PushSpan();
				string text;
				bool flag2 = !this.TryEatIdentifier("node type", out text);
				if (flag2)
				{
					return false;
				}
				TextParserSpan span = base.PopSpan(startCursor3);
				bool flag3 = !Document.ElementClassInfos.TryGetValue(text, out node.ElementClassInfo);
				if (flag3)
				{
					throw new TextParser.TextParserException("Unknown node type: " + text, span);
				}
			}
			node.SourceSpan = base.PopSpan(startCursor2);
			this.TryEatWhitespaceOrComment();
			bool flag4 = base.TryEat('#');
			if (flag4)
			{
				node.Name = this.EatIdentifier("node name", false);
				node.SourceSpan = base.PopSpan(startCursor);
				this.TryEatWhitespaceOrComment();
			}
			else
			{
				base.PopSpan(startCursor);
			}
			this.EatNodeDefinitionBody(node);
			return true;
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x000888C8 File Offset: 0x00086AC8
		public void EatNodeDefinitionBody(DocumentNode node)
		{
			base.Eat('{');
			this.TryEatWhitespaceOrComment();
			while (this.TryEatNamedExpressionDeclaration(node, node.Data))
			{
				this.TryEatWhitespaceOrComment();
			}
			this.EatNodeProperties(node, node.Data);
			bool flag = node.TemplateSetup != null;
			if (flag)
			{
				while (this.TryEatTemplateInsert(node))
				{
					this.TryEatWhitespaceOrComment();
				}
			}
			for (;;)
			{
				DocumentNode documentNode;
				bool flag2 = this.TryEatNodeDefinition(out documentNode);
				if (!flag2)
				{
					break;
				}
				documentNode.ParentNode = node;
				node.Data.Children.Add(documentNode);
				this.TryEatWhitespaceOrComment();
			}
			base.Eat('}');
		}

		// Token: 0x06003AD5 RID: 15061 RVA: 0x0008896C File Offset: 0x00086B6C
		private bool TryEatTemplateInsert(DocumentNode templateNode)
		{
			bool flag = !base.TryEat('#');
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				string key = this.EatIdentifier("template node name", false);
				this.TryEatWhitespaceOrComment();
				DocumentNode.NodeData nodeData = new DocumentNode.NodeData();
				base.Eat('{');
				this.TryEatWhitespaceOrComment();
				while (this.TryEatNamedExpressionDeclaration(templateNode, nodeData))
				{
					this.TryEatWhitespaceOrComment();
				}
				this.EatNodeProperties(templateNode, nodeData);
				for (;;)
				{
					DocumentNode item;
					bool flag2 = this.TryEatNodeDefinition(out item);
					if (!flag2)
					{
						break;
					}
					nodeData.Children.Add(item);
					this.TryEatWhitespaceOrComment();
				}
				base.Eat('}');
				templateNode.TemplateSetup.Inserts.Add(key, nodeData);
				result = true;
			}
			return result;
		}

		// Token: 0x06003AD6 RID: 15062 RVA: 0x00088A24 File Offset: 0x00086C24
		private void EatNodeProperties(DocumentNode scopeNode, DocumentNode.NodeData nodeData)
		{
			int cursor = this.Cursor;
			TextParserSpan span;
			for (;;)
			{
				bool flag = base.Peek('}') || base.Peek('$') || base.Peek('@') || base.Peek('#');
				if (flag)
				{
					break;
				}
				int startCursor = base.PushSpan();
				string key = this.EatIdentifier("property name or node type name", false);
				span = base.PopSpan(startCursor);
				this.TryEatWhitespaceOrComment();
				bool flag2 = !base.TryEat(':');
				if (flag2)
				{
					goto Block_5;
				}
				bool flag3 = nodeData.PropertyExpressions.ContainsKey(key);
				if (flag3)
				{
					goto Block_6;
				}
				this.TryEatWhitespaceOrComment();
				nodeData.PropertyExpressions.Add(key, this.EatExpression(scopeNode, 0));
				base.Eat(';');
				this.TryEatWhitespaceOrComment();
				cursor = this.Cursor;
			}
			return;
			Block_5:
			this.Cursor = cursor;
			return;
			Block_6:
			throw new TextParser.TextParserException("A property cannot be set twice", span);
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x00088B0C File Offset: 0x00086D0C
		public Expression EatExpression(DocumentNode scopeNode, int precedence = 0)
		{
			int startCursor = base.PushSpan();
			bool flag = base.IsEOF();
			if (flag)
			{
				throw new TextParser.TextParserException("Expected expression, found end of file", base.PopSpan(startCursor));
			}
			char c = this.Data[this.Cursor];
			bool flag2 = c == '-' || (c >= '0' && c <= '9');
			Expression expression;
			if (flag2)
			{
				decimal value = base.EatDecimal();
				expression = new Expression(value, base.PopSpan(startCursor));
			}
			else
			{
				Expression.ExpressionDictionary dictionary;
				bool flag3 = this.TryEatObjectDictionary(scopeNode, null, out dictionary);
				if (flag3)
				{
					expression = new Expression(dictionary, base.PopSpan(startCursor));
				}
				else
				{
					bool flag4 = c == '(';
					if (flag4)
					{
						base.Eat('(');
						this.TryEatWhitespaceOrComment();
						expression = this.EatExpression(scopeNode, 0);
						base.Eat(')');
						base.PopSpan(startCursor);
					}
					else
					{
						bool flag5 = c == '#';
						if (flag5)
						{
							UInt32Color value2 = this.EatColor();
							expression = new Expression(value2, base.PopSpan(startCursor));
						}
						else
						{
							bool flag6 = c == '%';
							if (flag6)
							{
								base.Eat('%');
								string text = this.EatIdentifier("i18n key part", false);
								while (base.TryEat('.'))
								{
									text = text + "." + this.EatIdentifier("i18n key part", false);
								}
								expression = new Expression(new Expression.Identifier("%", text), base.PopSpan(startCursor));
							}
							else
							{
								bool flag7 = c == '$';
								if (flag7)
								{
									base.Eat('$');
									expression = new Expression(new Expression.Identifier("$", this.EatIdentifier("document name", false)), base.PopSpan(startCursor));
								}
								else
								{
									bool flag8 = c == '@';
									if (flag8)
									{
										base.Eat('@');
										expression = new Expression(new Expression.Identifier("@", this.EatIdentifier("expression name", false)), base.PopSpan(startCursor));
									}
									else
									{
										bool value3;
										bool flag9 = this.TryEatBoolean(out value3);
										if (flag9)
										{
											expression = new Expression(value3, base.PopSpan(startCursor));
										}
										else
										{
											string value4;
											bool flag10 = this.TryEatDoubleQuotedString(out value4);
											if (flag10)
											{
												expression = new Expression(value4, base.PopSpan(startCursor));
											}
											else
											{
												string text2 = this.EatIdentifier("expression", false);
												TextParserSpan textParserSpan = base.PopSpan(startCursor);
												this.TryEatWhitespaceOrComment();
												bool flag11 = base.Peek('{');
												if (flag11)
												{
													ElementClassInfo elementClassInfo;
													bool flag12 = !Document.ElementClassInfos.TryGetValue(text2, out elementClassInfo);
													if (flag12)
													{
														throw new TextParser.TextParserException("Unknown node type: " + text2, textParserSpan);
													}
													DocumentNode documentNode = new DocumentNode
													{
														ElementClassInfo = elementClassInfo,
														ParentNode = scopeNode
													};
													this.EatNodeDefinitionBody(documentNode);
													documentNode.SourceSpan = new TextParserSpan(textParserSpan.Start, this.Cursor, this);
													expression = new Expression(documentNode, textParserSpan);
												}
												else
												{
													bool flag13 = this.TryEatObjectDictionary(scopeNode, text2, out dictionary);
													if (flag13)
													{
														expression = new Expression(dictionary, textParserSpan);
													}
													else
													{
														expression = new Expression(new Expression.Identifier(null, text2), textParserSpan);
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			this.TryEatWhitespaceOrComment();
			Expression expression2;
			bool flag14 = this.TryEatCompositeExpression(scopeNode, expression, precedence, out expression2);
			Expression result;
			if (flag14)
			{
				result = expression2;
			}
			else
			{
				result = expression;
			}
			return result;
		}

		// Token: 0x06003AD8 RID: 15064 RVA: 0x00088E38 File Offset: 0x00087038
		private bool TryEatCompositeExpression(DocumentNode scopeNode, Expression left, int precedence, out Expression result)
		{
			result = null;
			bool flag = base.IsEOF();
			bool result2;
			if (flag)
			{
				result2 = false;
			}
			else
			{
				char c = this.Data[this.Cursor];
				string text = c.ToString();
				string key = text + ((this.Cursor + 1 < this.Data.Length) ? this.Data[this.Cursor + 1].ToString() : "");
				int num;
				bool flag2 = !Expression.OperatorPrecedences.TryGetValue(key, out num);
				if (flag2)
				{
					bool flag3 = !Expression.OperatorPrecedences.TryGetValue(text, out num);
					if (flag3)
					{
						return false;
					}
				}
				bool flag4 = num <= precedence;
				if (flag4)
				{
					result2 = false;
				}
				else
				{
					base.Eat(c);
					Expression.ExpressionOperator op;
					bool flag5 = Expression.Operators.TryGetValue(key, out op);
					if (flag5)
					{
						this.Cursor++;
					}
					else
					{
						op = Expression.Operators[text];
					}
					this.TryEatWhitespaceOrComment();
					result = new Expression(op, left, this.EatExpression(scopeNode, num), new TextParserSpan(left.SourceSpan.Start, this.Cursor, this));
					Expression expression;
					bool flag6 = this.TryEatCompositeExpression(scopeNode, result, precedence, out expression);
					if (flag6)
					{
						result = expression;
					}
					result2 = true;
				}
			}
			return result2;
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x00088F8C File Offset: 0x0008718C
		private bool TryEatObjectDictionary(DocumentNode scopeNode, string typeName, out Expression.ExpressionDictionary dictionary)
		{
			DocumentParser.<>c__DisplayClass17_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			int cursor = this.Cursor;
			bool flag = !base.TryEat('(');
			bool result;
			if (flag)
			{
				dictionary = null;
				result = false;
			}
			else
			{
				Expression.ExpressionDictionary expressionDictionary = new Expression.ExpressionDictionary();
				expressionDictionary.TypeName = typeName;
				expressionDictionary.ScopeNode = scopeNode;
				Expression.ExpressionDictionary newDictionary = expressionDictionary;
				dictionary = expressionDictionary;
				CS$<>8__locals1.newDictionary = newDictionary;
				this.TryEatWhitespaceOrComment();
				bool flag2 = base.TryEat(')');
				if (flag2)
				{
					result = true;
				}
				else
				{
					while (this.<TryEatObjectDictionary>g__TryEatSpread|17_0(ref CS$<>8__locals1))
					{
						this.TryEatWhitespaceOrComment();
						bool flag3 = !base.TryEat(',');
						if (flag3)
						{
							break;
						}
						this.TryEatWhitespaceOrComment();
					}
					string text;
					TextParserSpan span;
					for (;;)
					{
						bool flag4 = CS$<>8__locals1.newDictionary.SpreadReferences.Count > 0 || CS$<>8__locals1.newDictionary.Entries.Count > 0;
						int startCursor = base.PushSpan();
						bool flag5 = !this.TryEatIdentifier("key", out text);
						if (flag5)
						{
							base.PopSpan(startCursor);
							bool flag6 = !flag4;
							if (flag6)
							{
								break;
							}
							bool flag7 = base.TryEat(')');
							if (flag7)
							{
								goto Block_7;
							}
							this.EatIdentifier("key", false);
						}
						span = base.PopSpan(startCursor);
						this.TryEatWhitespaceOrComment();
						bool flag8 = !base.Peek(':') && !flag4;
						if (flag8)
						{
							goto Block_9;
						}
						base.Eat(':');
						this.TryEatWhitespaceOrComment();
						int startCursor2 = base.PushSpan();
						Expression.ExpressionDictionary dictionary2;
						bool flag9 = this.TryEatObjectDictionary(scopeNode, null, out dictionary2);
						Expression value;
						if (flag9)
						{
							value = new Expression(dictionary2, base.PopSpan(startCursor2));
						}
						else
						{
							value = this.EatExpression(scopeNode, 1);
							base.PopSpan(startCursor2);
						}
						this.TryEatWhitespaceOrComment();
						bool flag10 = CS$<>8__locals1.newDictionary.Entries.ContainsKey(text);
						if (flag10)
						{
							goto Block_11;
						}
						CS$<>8__locals1.newDictionary.Entries.Add(text, value);
						bool flag11 = base.TryEat(')');
						if (flag11)
						{
							goto Block_12;
						}
						base.Eat(',');
						this.TryEatWhitespaceOrComment();
					}
					this.Cursor = cursor;
					dictionary = null;
					return false;
					Block_7:
					goto IL_233;
					Block_9:
					this.Cursor = cursor;
					dictionary = null;
					return false;
					Block_11:
					throw new TextParser.TextParserException("Duplicate key " + text + " in dictionary", span);
					Block_12:
					IL_233:
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x000891D4 File Offset: 0x000873D4
		public bool TryEatBoolean(out bool value)
		{
			value = false;
			bool flag = base.IsEOF();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int length = "true".Length;
				int length2 = "false".Length;
				bool flag2 = this.Cursor + length < this.Data.Length && this.Data.Substring(this.Cursor, length) == "true";
				if (flag2)
				{
					this.Cursor += length;
					value = true;
					result = true;
				}
				else
				{
					bool flag3 = this.Cursor + length2 < this.Data.Length && this.Data.Substring(this.Cursor, length2) == "false";
					if (flag3)
					{
						this.Cursor += length2;
						value = false;
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x000892B4 File Offset: 0x000874B4
		public bool EatBoolean()
		{
			int startCursor = base.PushSpan();
			bool flag = base.IsEOF();
			if (flag)
			{
				throw new TextParser.TextParserException("Expected boolean, found end of file", base.PopSpan(startCursor));
			}
			bool result;
			bool flag2 = !this.TryEatBoolean(out result);
			if (flag2)
			{
				throw new TextParser.TextParserException(string.Format("Expected boolean, found {0}", this.Data[this.Cursor]), base.PopSpan(startCursor));
			}
			base.PopSpan(startCursor);
			return result;
		}

		// Token: 0x06003ADC RID: 15068 RVA: 0x00089330 File Offset: 0x00087530
		public string EatDoubleQuotedString()
		{
			int startCursor = base.PushSpan();
			base.Eat('"');
			string text = "";
			for (;;)
			{
				bool flag = base.IsEOF();
				if (flag)
				{
					break;
				}
				char c = this.Data[this.Cursor];
				char c2 = (this.Cursor + 1 < this.Data.Length) ? this.Data[this.Cursor + 1] : ' ';
				bool flag2 = c == '\\';
				if (flag2)
				{
					bool flag3 = c2 == '\\' || c2 == '"';
					if (!flag3)
					{
						goto IL_B8;
					}
					text += c2.ToString();
					this.Cursor += 2;
				}
				else
				{
					bool flag4 = c == '"';
					if (flag4)
					{
						goto Block_6;
					}
					text += c.ToString();
					this.Cursor++;
				}
			}
			throw new TextParser.TextParserException("Encountered end of file while parsing double-quoted string", base.PopSpan(startCursor));
			IL_B8:
			throw new TextParser.TextParserException("\\ must be followed by \\ or \"", base.PopSpan(startCursor));
			Block_6:
			base.Eat('"');
			base.PopSpan(startCursor);
			return text;
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x00089454 File Offset: 0x00087654
		public bool TryEatDoubleQuotedString(out string value)
		{
			bool flag = base.IsEOF() || this.Data[this.Cursor] != '"';
			bool result;
			if (flag)
			{
				value = null;
				result = false;
			}
			else
			{
				value = this.EatDoubleQuotedString();
				result = true;
			}
			return result;
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x000894A0 File Offset: 0x000876A0
		public UInt32Color EatColor()
		{
			int startCursor = base.PushSpan();
			base.Eat('#');
			byte r;
			byte g;
			byte b;
			bool flag = !this.<EatColor>g__TryReadHexByte|22_0(out r) || !this.<EatColor>g__TryReadHexByte|22_0(out g) || !this.<EatColor>g__TryReadHexByte|22_0(out b);
			if (flag)
			{
				throw new TextParser.TextParserException("Expected color literal", base.PopSpan(startCursor));
			}
			byte a;
			bool flag2 = !this.<EatColor>g__TryReadHexByte|22_0(out a);
			if (flag2)
			{
				a = byte.MaxValue;
				bool flag3 = base.TryEat('(');
				if (flag3)
				{
					decimal num = base.EatDecimal();
					bool flag4 = num < 0m || num > 1m;
					if (flag4)
					{
						throw new TextParser.TextParserException("Alpha value must be in 0-1 range within color literal", base.PopSpan(startCursor));
					}
					a = (byte)(255m * num);
					base.Eat(')');
				}
			}
			base.PopSpan(startCursor);
			return UInt32Color.FromRGBA(r, g, b, a);
		}

		// Token: 0x06003ADF RID: 15071 RVA: 0x0008959C File Offset: 0x0008779C
		[CompilerGenerated]
		private bool <TryEatObjectDictionary>g__TryEatSpread|17_0(ref DocumentParser.<>c__DisplayClass17_0 A_1)
		{
			int num = base.PushSpan();
			bool flag = !base.TryEat("...");
			bool result;
			if (flag)
			{
				base.PopSpan(num);
				this.Cursor = num;
				result = false;
			}
			else
			{
				Expression item = this.EatNamedExpressionMemberReference();
				base.PopSpan(num);
				A_1.newDictionary.SpreadReferences.Add(item);
				result = true;
			}
			return result;
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x00089600 File Offset: 0x00087800
		[CompilerGenerated]
		private bool <EatColor>g__TryReadHexByte|22_0(out byte value)
		{
			bool flag = this.Cursor + 1 >= this.Data.Length;
			bool result;
			if (flag)
			{
				value = 0;
				result = false;
			}
			else
			{
				string s = this.Data.Substring(this.Cursor, 2);
				bool flag2 = !byte.TryParse(s, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out value);
				if (flag2)
				{
					value = 0;
					result = false;
				}
				else
				{
					this.Cursor += 2;
					result = true;
				}
			}
			return result;
		}
	}
}
