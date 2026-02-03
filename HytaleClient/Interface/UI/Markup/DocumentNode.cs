using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.UI.Markup
{
	// Token: 0x02000842 RID: 2114
	public class DocumentNode
	{
		// Token: 0x1700102D RID: 4141
		// (get) Token: 0x06003ABF RID: 15039 RVA: 0x00086B38 File Offset: 0x00084D38
		public string SourcePath
		{
			get
			{
				return this.SourceSpan.Parser.SourcePath;
			}
		}

		// Token: 0x06003AC0 RID: 15040 RVA: 0x00086B4C File Offset: 0x00084D4C
		public bool TryGetNamedExpression(string name, HashSet<Expression> already, out Expression expression)
		{
			bool flag = !this.Data.LocalNamedExpressions.TryGetValue(name, out expression);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !already.Add(expression);
				if (flag2)
				{
					throw new Expression.ExpressionResolutionException("Cyclic reference detected.", expression, null);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06003AC1 RID: 15041 RVA: 0x00086B9C File Offset: 0x00084D9C
		public DocumentNode Clone(DocumentNode newParentNode)
		{
			DocumentNode documentNode = new DocumentNode
			{
				SourceSpan = this.SourceSpan,
				TemplateSetup = this.TemplateSetup,
				ElementClassInfo = this.ElementClassInfo,
				Name = this.Name,
				ParentNode = newParentNode
			};
			foreach (KeyValuePair<string, Expression> keyValuePair in this.Data.LocalNamedExpressions)
			{
				documentNode.Data.LocalNamedExpressions.Add(keyValuePair.Key, keyValuePair.Value);
			}
			foreach (KeyValuePair<string, Expression> keyValuePair2 in this.Data.PropertyExpressions)
			{
				documentNode.Data.PropertyExpressions.Add(keyValuePair2.Key, keyValuePair2.Value);
			}
			foreach (DocumentNode documentNode2 in this.Data.Children)
			{
				documentNode.Data.Children.Add(documentNode2.Clone(documentNode));
			}
			return documentNode;
		}

		// Token: 0x06003AC2 RID: 15042 RVA: 0x00086D14 File Offset: 0x00084F14
		public bool TryResolve<T>(ResolutionContext context, Expression expression, out T value)
		{
			bool flag = this.DoTryResolve<T>(context, expression, out value);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this.SourceTemplateNode != null && this.SourceTemplateNode.TryResolve<T>(context, expression, out value);
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = this.ParentNode != null && this.ParentNode.TryResolve<T>(context, expression, out value);
					result = flag3;
				}
			}
			return result;
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x00086D7C File Offset: 0x00084F7C
		public bool TryResolveAs(ResolutionContext context, Expression expression, Type type, out object value)
		{
			bool flag = this.DoTryResolveAs(context, expression, type, out value);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this.SourceTemplateNode != null && this.SourceTemplateNode.TryResolveAs(context, expression, type, out value);
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = this.ParentNode != null && this.ParentNode.TryResolveAs(context, expression, type, out value);
					result = flag3;
				}
			}
			return result;
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x00086DE8 File Offset: 0x00084FE8
		private bool DoTryResolve<T>(ResolutionContext context, Expression expression, out T value)
		{
			object obj;
			bool flag = this.TryResolveAs(context, expression, typeof(T), out obj);
			bool result;
			if (flag)
			{
				bool flag2 = typeof(T) == typeof(decimal) && obj.GetType() == typeof(int);
				if (flag2)
				{
					value = (T)((object)((int)obj));
				}
				else
				{
					value = (T)((object)obj);
				}
				result = true;
			}
			else
			{
				value = default(T);
				result = false;
			}
			return result;
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x00086E80 File Offset: 0x00085080
		private bool DoTryResolveAs(ResolutionContext context, Expression expression, Type type, out object value)
		{
			DocumentNode.<>c__DisplayClass16_0 CS$<>8__locals1;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.type = type;
			CS$<>8__locals1.expression = expression;
			CS$<>8__locals1.type = (Nullable.GetUnderlyingType(CS$<>8__locals1.type) ?? CS$<>8__locals1.type);
			bool flag = CS$<>8__locals1.expression.Operator == Expression.ExpressionOperator.MemberAccess;
			if (flag)
			{
				Expression.Identifier identifierValue = CS$<>8__locals1.expression.Left.IdentifierValue;
				bool flag2 = ((identifierValue != null) ? identifierValue.Prefix : null) == "$" && CS$<>8__locals1.expression.Right.IdentifierValue.Prefix == "@";
				if (flag2)
				{
					string sourcePath = CS$<>8__locals1.expression.SourceSpan.Parser.SourcePath;
					Document document;
					string text;
					bool flag3 = !CS$<>8__locals1.context.Provider.TryGetDocument(sourcePath, out document) || !document.NamedDocumentReferences.TryGetValue(CS$<>8__locals1.expression.Left.IdentifierValue.Text, out text);
					if (flag3)
					{
						throw new Expression.ExpressionResolutionException("Could not find a document reference named " + CS$<>8__locals1.expression.Left.IdentifierValue.Text, CS$<>8__locals1.expression.Left, null);
					}
					Document document2;
					bool flag4 = !CS$<>8__locals1.context.Provider.TryGetDocument(text, out document2);
					if (flag4)
					{
						throw new Expression.ExpressionResolutionException("Could not find a document with path " + text, CS$<>8__locals1.expression.Left, null);
					}
					Expression expression2;
					bool flag5 = !document2.RootNode.TryGetNamedExpression(CS$<>8__locals1.expression.Right.IdentifierValue.Text, CS$<>8__locals1.context.ExpressionPath, out expression2);
					if (flag5)
					{
						throw new Expression.ExpressionResolutionException("Could not find an expression named " + CS$<>8__locals1.expression.Right.IdentifierValue.Text + " in document " + text, CS$<>8__locals1.expression, null);
					}
					CS$<>8__locals1.context.ExpressionPath.Add(expression2);
					bool result = document2.RootNode.TryResolveAs(CS$<>8__locals1.context, expression2, CS$<>8__locals1.type, out value);
					CS$<>8__locals1.context.ExpressionPath.Remove(expression2);
					return result;
				}
				else
				{
					bool flag6 = CS$<>8__locals1.expression.Right.IdentifierValue.Prefix == null;
					if (flag6)
					{
						object obj;
						bool flag7 = !this.TryResolveAs(CS$<>8__locals1.context, CS$<>8__locals1.expression.Left, typeof(object), out obj);
						if (flag7)
						{
							throw new Expression.ExpressionResolutionException("Failed to resolve left-hand side of member access expression", CS$<>8__locals1.expression.Left, null);
						}
						FieldInfo field = obj.GetType().GetField(CS$<>8__locals1.expression.Right.IdentifierValue.Text);
						bool flag8 = field == null;
						if (flag8)
						{
							throw new Expression.ExpressionResolutionException("Could not find a field named " + CS$<>8__locals1.expression.Right.IdentifierValue.Text, CS$<>8__locals1.expression, null);
						}
						value = field.GetValue(obj);
						return true;
					}
				}
			}
			else
			{
				bool flag9 = CS$<>8__locals1.expression.IdentifierValue != null;
				if (flag9)
				{
					string prefix = CS$<>8__locals1.expression.IdentifierValue.Prefix;
					string text2 = prefix;
					if (!(text2 == "@"))
					{
						if (text2 == "%")
						{
							value = CS$<>8__locals1.context.Provider.GetText(CS$<>8__locals1.expression.IdentifierValue.Text, null, true);
							return true;
						}
						if (text2 == null)
						{
							bool isEnum = CS$<>8__locals1.type.IsEnum;
							if (isEnum)
							{
								try
								{
									value = Enum.Parse(CS$<>8__locals1.type, CS$<>8__locals1.expression.IdentifierValue.Text);
									return true;
								}
								catch
								{
								}
							}
						}
					}
					else
					{
						Expression expression3;
						bool flag10 = this.TryGetNamedExpression(CS$<>8__locals1.expression.IdentifierValue.Text, CS$<>8__locals1.context.ExpressionPath, out expression3);
						if (flag10)
						{
							CS$<>8__locals1.context.ExpressionPath.Add(expression3);
							bool result2 = this.TryResolveAs(CS$<>8__locals1.context, expression3, CS$<>8__locals1.type, out value);
							CS$<>8__locals1.context.ExpressionPath.Remove(expression3);
							return result2;
						}
					}
					value = null;
					return false;
				}
			}
			bool flag11 = CS$<>8__locals1.type == typeof(UInt32Color);
			if (flag11)
			{
				bool flag12 = CS$<>8__locals1.expression.ColorValue != null;
				if (flag12)
				{
					value = CS$<>8__locals1.expression.ColorValue.Value;
					return true;
				}
			}
			else
			{
				bool flag13 = CS$<>8__locals1.type == typeof(decimal);
				if (flag13)
				{
					bool flag14 = CS$<>8__locals1.expression.DecimalValue != null;
					if (flag14)
					{
						value = CS$<>8__locals1.expression.DecimalValue.Value;
						return true;
					}
					bool flag15 = CS$<>8__locals1.expression.OperatorCategory == Expression.ExpressionOperatorCategory.Arithmetic;
					if (flag15)
					{
						decimal d;
						decimal d2;
						bool flag16 = this.TryResolve<decimal>(CS$<>8__locals1.context, CS$<>8__locals1.expression.Left, out d) && this.TryResolve<decimal>(CS$<>8__locals1.context, CS$<>8__locals1.expression.Right, out d2);
						if (flag16)
						{
							switch (CS$<>8__locals1.expression.Operator)
							{
							case Expression.ExpressionOperator.Multiply:
								value = d * d2;
								return true;
							case Expression.ExpressionOperator.Divide:
								value = d / d2;
								return true;
							case Expression.ExpressionOperator.Add:
								value = d + d2;
								return true;
							case Expression.ExpressionOperator.Subtract:
								value = d - d2;
								return true;
							}
						}
					}
				}
				else
				{
					bool flag17 = CS$<>8__locals1.type == typeof(float);
					if (flag17)
					{
						decimal value2;
						bool flag18 = this.TryResolve<decimal>(CS$<>8__locals1.context, CS$<>8__locals1.expression, out value2);
						if (flag18)
						{
							value = (float)value2;
							return true;
						}
					}
					else
					{
						bool flag19 = CS$<>8__locals1.type == typeof(int);
						if (flag19)
						{
							decimal d3;
							bool flag20 = this.TryResolve<decimal>(CS$<>8__locals1.context, CS$<>8__locals1.expression, out d3);
							if (flag20)
							{
								value = (int)Math.Round(d3, MidpointRounding.AwayFromZero);
								return true;
							}
						}
						else
						{
							bool flag21 = CS$<>8__locals1.type == typeof(bool);
							if (flag21)
							{
								bool flag22 = CS$<>8__locals1.expression.BooleanValue != null;
								if (flag22)
								{
									value = CS$<>8__locals1.expression.BooleanValue.Value;
									return true;
								}
								Expression.ExpressionOperatorCategory operatorCategory = CS$<>8__locals1.expression.OperatorCategory;
								Expression.ExpressionOperatorCategory expressionOperatorCategory = operatorCategory;
								if (expressionOperatorCategory != Expression.ExpressionOperatorCategory.Comparison)
								{
									if (expressionOperatorCategory == Expression.ExpressionOperatorCategory.BooleanLogic)
									{
										bool flag24;
										bool flag25;
										bool flag23 = this.TryResolve<bool>(CS$<>8__locals1.context, CS$<>8__locals1.expression.Left, out flag24) && this.TryResolve<bool>(CS$<>8__locals1.context, CS$<>8__locals1.expression.Right, out flag25);
										if (flag23)
										{
											Expression.ExpressionOperator @operator = CS$<>8__locals1.expression.Operator;
											Expression.ExpressionOperator expressionOperator = @operator;
											if (expressionOperator == Expression.ExpressionOperator.And)
											{
												value = (flag24 && flag25);
												return true;
											}
											if (expressionOperator == Expression.ExpressionOperator.Or)
											{
												value = (flag24 || flag25);
												return true;
											}
										}
									}
								}
								else
								{
									decimal d4;
									decimal d5;
									bool flag26 = this.TryResolve<decimal>(CS$<>8__locals1.context, CS$<>8__locals1.expression.Left, out d4) && this.TryResolve<decimal>(CS$<>8__locals1.context, CS$<>8__locals1.expression.Right, out d5);
									if (flag26)
									{
										switch (CS$<>8__locals1.expression.Operator)
										{
										case Expression.ExpressionOperator.EqualTo:
											value = (d4 == d5);
											return true;
										case Expression.ExpressionOperator.NotEqualTo:
											value = (d4 != d5);
											return true;
										case Expression.ExpressionOperator.GreaterThan:
											value = (d4 > d5);
											return true;
										case Expression.ExpressionOperator.LessThan:
											value = (d4 < d5);
											return true;
										case Expression.ExpressionOperator.GreaterThanOrEqualTo:
											value = (d4 >= d5);
											return true;
										case Expression.ExpressionOperator.LessThanOrEqualTo:
											value = (d4 <= d5);
											return true;
										}
									}
								}
							}
							else
							{
								bool flag27 = CS$<>8__locals1.type == typeof(UIPath);
								if (flag27)
								{
									string text3;
									bool flag28 = this.TryResolve<string>(CS$<>8__locals1.context, CS$<>8__locals1.expression, out text3);
									if (flag28)
									{
										DocumentNode documentNode = this;
										while (documentNode.SourceTemplateNode != null)
										{
											documentNode = documentNode.SourceTemplateNode;
										}
										string value3;
										bool flag29 = !Expression.TryResolvePath(text3, documentNode.SourcePath, out value3);
										if (flag29)
										{
											throw new Expression.ExpressionResolutionException("Could not resolve relative path: " + text3, CS$<>8__locals1.expression, null);
										}
										value = new UIPath(value3);
										return true;
									}
								}
								else
								{
									bool flag30 = CS$<>8__locals1.type == typeof(UIFontName);
									if (flag30)
									{
										string text4;
										bool flag31 = this.TryResolve<string>(CS$<>8__locals1.context, CS$<>8__locals1.expression, out text4);
										if (flag31)
										{
											bool flag32 = CS$<>8__locals1.context.Provider.GetFontFamily(text4) == null;
											if (flag32)
											{
												throw new Expression.ExpressionResolutionException("Font does not exist: " + text4, CS$<>8__locals1.expression, null);
											}
											value = new UIFontName(text4);
											return true;
										}
									}
									else
									{
										bool flag33 = CS$<>8__locals1.type == typeof(char);
										if (flag33)
										{
											bool flag34 = CS$<>8__locals1.expression.StringValue != null && CS$<>8__locals1.expression.StringValue.Length == 1;
											if (flag34)
											{
												value = CS$<>8__locals1.expression.StringValue[0];
												return true;
											}
										}
										else
										{
											bool flag35 = CS$<>8__locals1.type == typeof(string);
											if (flag35)
											{
												bool flag36 = CS$<>8__locals1.expression.StringValue != null;
												if (flag36)
												{
													value = CS$<>8__locals1.expression.StringValue;
													return true;
												}
												bool flag37 = CS$<>8__locals1.expression.Operator == Expression.ExpressionOperator.Add;
												if (flag37)
												{
													string str;
													string str2;
													bool flag38 = this.TryResolve<string>(CS$<>8__locals1.context, CS$<>8__locals1.expression.Left, out str) && this.TryResolve<string>(CS$<>8__locals1.context, CS$<>8__locals1.expression.Right, out str2);
													if (flag38)
													{
														value = str + str2;
														return true;
													}
												}
											}
											else
											{
												bool flag39 = CS$<>8__locals1.type == typeof(DocumentNode);
												if (flag39)
												{
													bool flag40 = CS$<>8__locals1.expression.DocumentNode != null;
													if (flag40)
													{
														value = CS$<>8__locals1.expression.DocumentNode;
														return true;
													}
												}
												else
												{
													bool flag41 = CS$<>8__locals1.expression.ObjectDictionary != null;
													if (flag41)
													{
														bool flag42 = CS$<>8__locals1.type == typeof(Expression.ExpressionDictionary);
														if (flag42)
														{
															value = CS$<>8__locals1.expression.ObjectDictionary;
															return true;
														}
														Type type2;
														bool flag43 = !Expression.RegisteredDataTypes.TryGetValue(CS$<>8__locals1.expression.ObjectDictionary.TypeName ?? CS$<>8__locals1.type.Name, out type2);
														if (flag43)
														{
															throw new Expression.ExpressionResolutionException("Tried to resolve expression to an unregistered markup data type " + (CS$<>8__locals1.expression.ObjectDictionary.TypeName ?? CS$<>8__locals1.type.Name), CS$<>8__locals1.expression, null);
														}
														bool flag44 = type2 != CS$<>8__locals1.type;
														if (flag44)
														{
															bool flag45 = CS$<>8__locals1.type == typeof(object);
															if (!flag45)
															{
																throw new Expression.ExpressionResolutionException("Type mismatch between " + type2.FullName + " and " + CS$<>8__locals1.type.FullName, CS$<>8__locals1.expression, null);
															}
															CS$<>8__locals1.type = type2;
														}
														bool flag46 = CS$<>8__locals1.type.Name == CS$<>8__locals1.expression.ObjectDictionary.TypeName || CS$<>8__locals1.type == typeof(object) || CS$<>8__locals1.expression.ObjectDictionary.TypeName == null;
														if (flag46)
														{
															DocumentNode.<>c__DisplayClass16_1 CS$<>8__locals2;
															CS$<>8__locals2.newValue = Activator.CreateInstance(CS$<>8__locals1.type);
															DocumentNode.<DoTryResolveAs>g__ApplyRecursive|16_0(CS$<>8__locals1.expression.ObjectDictionary, this, CS$<>8__locals1.expression, ref CS$<>8__locals1, ref CS$<>8__locals2);
															value = CS$<>8__locals2.newValue;
															return true;
														}
													}
													else
													{
														bool flag47 = CS$<>8__locals1.type == typeof(PatchStyle);
														if (flag47)
														{
															UIPath texturePath;
															bool flag48 = this.TryResolve<UIPath>(CS$<>8__locals1.context, CS$<>8__locals1.expression, out texturePath);
															if (flag48)
															{
																value = new PatchStyle
																{
																	TexturePath = texturePath
																};
																return true;
															}
															UInt32Color color;
															bool flag49 = this.TryResolve<UInt32Color>(CS$<>8__locals1.context, CS$<>8__locals1.expression, out color);
															if (flag49)
															{
																value = new PatchStyle
																{
																	Color = color
																};
																return true;
															}
														}
														else
														{
															bool flag50 = CS$<>8__locals1.type == typeof(SoundStyle);
															if (flag50)
															{
																UIPath soundPath;
																bool flag51 = this.TryResolve<UIPath>(CS$<>8__locals1.context, CS$<>8__locals1.expression, out soundPath);
																if (flag51)
																{
																	value = new SoundStyle
																	{
																		SoundPath = soundPath
																	};
																	return true;
																}
															}
															else
															{
																bool flag52 = CS$<>8__locals1.type == typeof(Padding);
																if (flag52)
																{
																	decimal d6;
																	bool flag53 = this.TryResolve<decimal>(CS$<>8__locals1.context, CS$<>8__locals1.expression, out d6);
																	if (flag53)
																	{
																		value = new Padding((int)Math.Round(d6, MidpointRounding.AwayFromZero));
																		return true;
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
						}
					}
				}
			}
			value = null;
			return false;
		}

		// Token: 0x06003AC7 RID: 15047 RVA: 0x00087CF0 File Offset: 0x00085EF0
		[CompilerGenerated]
		internal static void <DoTryResolveAs>g__ApplyRecursive|16_0(Expression.ExpressionDictionary dictionary, DocumentNode scopeNode, Expression dictionaryExpression, ref DocumentNode.<>c__DisplayClass16_0 A_3, ref DocumentNode.<>c__DisplayClass16_1 A_4)
		{
			foreach (Expression expression in dictionary.SpreadReferences)
			{
				Expression.ExpressionDictionary expressionDictionary;
				bool flag = !scopeNode.TryResolve<Expression.ExpressionDictionary>(A_3.context, expression, out expressionDictionary);
				if (flag)
				{
					throw new Expression.ExpressionResolutionException("Could not resolve spread expression to type " + A_3.type.Name, A_3.expression, null);
				}
				DocumentNode.<DoTryResolveAs>g__ApplyRecursive|16_0(expressionDictionary, expressionDictionary.ScopeNode, expression, ref A_3, ref A_4);
			}
			bool flag2 = A_3.type.Name != dictionary.TypeName && A_3.type != typeof(object) && dictionary.TypeName != null;
			if (flag2)
			{
				throw new Expression.ExpressionResolutionException("Cannot resolve dictionary of type " + dictionary.TypeName + " to type " + A_3.type.Name, dictionaryExpression, null);
			}
			foreach (KeyValuePair<string, Expression> keyValuePair in dictionary.Entries)
			{
				FieldInfo field = A_3.type.GetField(keyValuePair.Key, BindingFlags.Instance | BindingFlags.Public);
				PropertyInfo property = A_3.type.GetProperty(keyValuePair.Key, BindingFlags.Instance | BindingFlags.Public);
				bool flag3 = field != null;
				Type type;
				if (flag3)
				{
					type = field.FieldType;
				}
				else
				{
					bool flag4 = property != null;
					if (!flag4)
					{
						throw new Expression.ExpressionResolutionException("Could not find field " + keyValuePair.Key + " in type " + A_3.type.Name, keyValuePair.Value, null);
					}
					type = property.PropertyType;
				}
				type = (Nullable.GetUnderlyingType(type) ?? type);
				object value;
				bool flag5 = !scopeNode.TryResolveAs(A_3.context, keyValuePair.Value, type, out value);
				if (flag5)
				{
					throw new Expression.ExpressionResolutionException("Could not resolve expression for property " + keyValuePair.Key + " to type " + type.Name, keyValuePair.Value, null);
				}
				bool flag6 = field != null;
				if (flag6)
				{
					field.SetValue(A_4.newValue, value);
				}
				else
				{
					property.SetValue(A_4.newValue, value);
				}
			}
		}

		// Token: 0x04001B0D RID: 6925
		public TextParserSpan SourceSpan;

		// Token: 0x04001B0E RID: 6926
		public ElementClassInfo ElementClassInfo;

		// Token: 0x04001B0F RID: 6927
		public string Name;

		// Token: 0x04001B10 RID: 6928
		public DocumentNode ParentNode;

		// Token: 0x04001B11 RID: 6929
		public DocumentNode.NodeTemplateSetup TemplateSetup;

		// Token: 0x04001B12 RID: 6930
		public DocumentNode SourceTemplateNode;

		// Token: 0x04001B13 RID: 6931
		public DocumentNode.NodeData Data = new DocumentNode.NodeData();

		// Token: 0x02000D17 RID: 3351
		public class NodeData
		{
			// Token: 0x040040BB RID: 16571
			public readonly Dictionary<string, Expression> LocalNamedExpressions = new Dictionary<string, Expression>();

			// Token: 0x040040BC RID: 16572
			public readonly Dictionary<string, Expression> PropertyExpressions = new Dictionary<string, Expression>();

			// Token: 0x040040BD RID: 16573
			public readonly Dictionary<string, object> PropertyValues = new Dictionary<string, object>();

			// Token: 0x040040BE RID: 16574
			public readonly List<DocumentNode> Children = new List<DocumentNode>();
		}

		// Token: 0x02000D18 RID: 3352
		public class NodeTemplateSetup
		{
			// Token: 0x040040BF RID: 16575
			public Expression NamedTemplateExpression;

			// Token: 0x040040C0 RID: 16576
			public readonly Dictionary<string, DocumentNode.NodeData> Inserts = new Dictionary<string, DocumentNode.NodeData>();
		}
	}
}
