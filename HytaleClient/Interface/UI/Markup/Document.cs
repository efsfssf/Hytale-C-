using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using HytaleClient.Interface.UI.Elements;

namespace HytaleClient.Interface.UI.Markup
{
	// Token: 0x02000841 RID: 2113
	public class Document
	{
		// Token: 0x06003AB4 RID: 15028 RVA: 0x00085F79 File Offset: 0x00084179
		public Document(TextParser parser)
		{
			this.RootNode = new DocumentNode
			{
				SourceSpan = new TextParserSpan(0, parser.Data.Length, parser)
			};
		}

		// Token: 0x06003AB5 RID: 15029 RVA: 0x00085FB4 File Offset: 0x000841B4
		static Document()
		{
			foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
			{
				bool flag = !type.IsClass || type.IsAbstract || !typeof(Element).IsAssignableFrom(type);
				if (!flag)
				{
					UIMarkupElementAttribute customAttribute = type.GetCustomAttribute(false);
					bool flag2 = customAttribute == null;
					if (!flag2)
					{
						ElementClassInfo elementClassInfo = new ElementClassInfo
						{
							Name = type.Name,
							AcceptsChildren = customAttribute.AcceptsChildren
						};
						foreach (ConstructorInfo constructorInfo in type.GetConstructors())
						{
							ParameterInfo[] parameters = constructorInfo.GetParameters();
							bool flag3 = parameters.Length != 2;
							if (!flag3)
							{
								bool flag4 = parameters[0].ParameterType != typeof(Desktop);
								if (!flag4)
								{
									bool flag5 = parameters[1].ParameterType != typeof(Element);
									if (!flag5)
									{
										elementClassInfo.Constructor = constructorInfo;
										break;
									}
								}
							}
						}
						bool flag6 = elementClassInfo.Constructor == null;
						if (flag6)
						{
							throw new Exception(type.FullName + " has no constructor matching requirements for elements");
						}
						foreach (MemberInfo memberInfo in type.GetMembers(BindingFlags.Instance | BindingFlags.Public))
						{
							UIMarkupPropertyAttribute customAttribute2 = memberInfo.GetCustomAttribute<UIMarkupPropertyAttribute>();
							bool flag7 = customAttribute2 != null && (customAttribute.ExposeInheritedProperties || memberInfo.DeclaringType == type);
							if (flag7)
							{
								FieldInfo fieldInfo = memberInfo as FieldInfo;
								bool flag8 = fieldInfo != null;
								if (flag8)
								{
									elementClassInfo.PropertyTypes.Add(fieldInfo.Name, fieldInfo.FieldType);
								}
								else
								{
									PropertyInfo propertyInfo = memberInfo as PropertyInfo;
									bool flag9 = propertyInfo != null;
									if (flag9)
									{
										elementClassInfo.PropertyTypes.Add(propertyInfo.Name, propertyInfo.PropertyType);
									}
								}
							}
						}
						Document.ElementClassInfos.Add(type.Name, elementClassInfo);
					}
				}
			}
		}

		// Token: 0x06003AB6 RID: 15030 RVA: 0x000861DC File Offset: 0x000843DC
		public UIFragment Instantiate(Desktop desktop, Element root)
		{
			Document.<>c__DisplayClass5_0 CS$<>8__locals1;
			CS$<>8__locals1.desktop = desktop;
			CS$<>8__locals1.fragment = new UIFragment();
			CS$<>8__locals1.fragment.RootElements = Document.<Instantiate>g__Walk|5_1(this.RootNode.Data.Children, root, ref CS$<>8__locals1);
			return CS$<>8__locals1.fragment;
		}

		// Token: 0x06003AB7 RID: 15031 RVA: 0x00086230 File Offset: 0x00084430
		public void ResolveProperties(IUIProvider provider)
		{
			Document.<>c__DisplayClass6_0 CS$<>8__locals1;
			CS$<>8__locals1.context = new ResolutionContext(provider);
			Document.<ResolveProperties>g__Walk|6_2(this.RootNode, ref CS$<>8__locals1);
		}

		// Token: 0x06003AB8 RID: 15032 RVA: 0x0008625C File Offset: 0x0008445C
		public T ResolveNamedValue<T>(IUIProvider provider, string name)
		{
			ResolutionContext context = new ResolutionContext(provider);
			T result;
			bool flag = !this.RootNode.TryResolve<T>(context, this.RootNode.Data.LocalNamedExpressions[name], out result);
			if (flag)
			{
				throw new Exception("Could not resolve expession for root named value " + name);
			}
			return result;
		}

		// Token: 0x06003AB9 RID: 15033 RVA: 0x000862B4 File Offset: 0x000844B4
		public bool TryResolveNamedValue<T>(IUIProvider provider, string name, out T value)
		{
			ResolutionContext context = new ResolutionContext(provider);
			Expression expression;
			bool flag = !this.RootNode.Data.LocalNamedExpressions.TryGetValue(name, out expression);
			bool result;
			if (flag)
			{
				value = default(T);
				result = false;
			}
			else
			{
				result = this.RootNode.TryResolve<T>(context, expression, out value);
			}
			return result;
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x00086308 File Offset: 0x00084508
		[CompilerGenerated]
		internal static object <Instantiate>g__CloneMarkupPropertyValue|5_0(object value)
		{
			bool flag = value == null;
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Type type = value.GetType();
				bool flag2 = type.IsValueType || type == typeof(string);
				if (flag2)
				{
					result = value;
				}
				else
				{
					bool flag3 = type.GetCustomAttribute<UIMarkupDataAttribute>() == null;
					if (flag3)
					{
						result = value;
					}
					else
					{
						object obj = Activator.CreateInstance(type);
						FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						foreach (FieldInfo fieldInfo in fields)
						{
							fieldInfo.SetValue(obj, Document.<Instantiate>g__CloneMarkupPropertyValue|5_0(fieldInfo.GetValue(value)));
						}
						result = obj;
					}
				}
			}
			return result;
		}

		// Token: 0x06003ABB RID: 15035 RVA: 0x000863B4 File Offset: 0x000845B4
		[CompilerGenerated]
		internal static List<Element> <Instantiate>g__Walk|5_1(List<DocumentNode> nodes, Element parent, ref Document.<>c__DisplayClass5_0 A_2)
		{
			List<Element> list = new List<Element>();
			foreach (DocumentNode documentNode in nodes)
			{
				ConstructorInfo constructor = documentNode.ElementClassInfo.Constructor;
				object[] array = new object[2];
				array[0] = A_2.desktop;
				Element element = (Element)constructor.Invoke(array);
				element.Name = documentNode.Name;
				Type type = element.GetType();
				foreach (KeyValuePair<string, object> keyValuePair in documentNode.Data.PropertyValues)
				{
					object value = Document.<Instantiate>g__CloneMarkupPropertyValue|5_0(keyValuePair.Value);
					FieldInfo field = type.GetField(keyValuePair.Key);
					bool flag = field != null;
					if (flag)
					{
						field.SetValue(element, value);
					}
					else
					{
						type.GetProperty(keyValuePair.Key).SetValue(element, value);
					}
				}
				list.Add(element);
				bool flag2 = documentNode.Name != null && !A_2.fragment.ElementsByName.ContainsKey(documentNode.Name);
				if (flag2)
				{
					A_2.fragment.ElementsByName.Add(documentNode.Name, element);
				}
				bool acceptsChildren = documentNode.ElementClassInfo.AcceptsChildren;
				if (acceptsChildren)
				{
					Document.<Instantiate>g__Walk|5_1(documentNode.Data.Children, element, ref A_2);
				}
				if (parent != null)
				{
					parent.AddFromMarkup(element);
				}
			}
			return list;
		}

		// Token: 0x06003ABC RID: 15036 RVA: 0x00086578 File Offset: 0x00084778
		[CompilerGenerated]
		internal static void <ResolveProperties>g__ApplyInsert|6_0(DocumentNode node, DocumentNode.NodeData insert)
		{
			foreach (DocumentNode documentNode in insert.Children)
			{
				documentNode.ParentNode = node;
				node.Data.Children.Add(documentNode);
			}
			foreach (KeyValuePair<string, Expression> keyValuePair in insert.LocalNamedExpressions)
			{
				node.Data.LocalNamedExpressions[keyValuePair.Key] = keyValuePair.Value;
			}
			foreach (KeyValuePair<string, Expression> keyValuePair2 in insert.PropertyExpressions)
			{
				node.Data.PropertyExpressions[keyValuePair2.Key] = keyValuePair2.Value;
			}
		}

		// Token: 0x06003ABD RID: 15037 RVA: 0x0008669C File Offset: 0x0008489C
		[CompilerGenerated]
		internal static DocumentNode <ResolveProperties>g__FindNodeByName|6_1(DocumentNode parent, string name)
		{
			foreach (DocumentNode documentNode in parent.Data.Children)
			{
				bool flag = documentNode.Name == name;
				if (flag)
				{
					return documentNode;
				}
				DocumentNode documentNode2 = Document.<ResolveProperties>g__FindNodeByName|6_1(documentNode, name);
				bool flag2 = documentNode2 != null;
				if (flag2)
				{
					return documentNode2;
				}
			}
			return null;
		}

		// Token: 0x06003ABE RID: 15038 RVA: 0x00086728 File Offset: 0x00084928
		[CompilerGenerated]
		internal static void <ResolveProperties>g__Walk|6_2(DocumentNode node, ref Document.<>c__DisplayClass6_0 A_1)
		{
			bool flag = node.TemplateSetup != null;
			if (flag)
			{
				DocumentNode.NodeTemplateSetup templateSetup = node.TemplateSetup;
				node.TemplateSetup = null;
				DocumentNode.NodeData data = node.Data;
				bool flag2 = !node.ParentNode.TryResolve<DocumentNode>(A_1.context, templateSetup.NamedTemplateExpression, out node.SourceTemplateNode);
				if (flag2)
				{
					throw new Expression.ExpressionResolutionException("Could not resolve template node", templateSetup.NamedTemplateExpression, null);
				}
				node.ElementClassInfo = node.SourceTemplateNode.ElementClassInfo;
				node.Data = new DocumentNode.NodeData();
				foreach (DocumentNode documentNode in node.SourceTemplateNode.Data.Children)
				{
					DocumentNode item = documentNode.Clone(node);
					node.Data.Children.Add(item);
				}
				foreach (KeyValuePair<string, Expression> keyValuePair in node.SourceTemplateNode.Data.LocalNamedExpressions)
				{
					node.Data.LocalNamedExpressions.Add(keyValuePair.Key, keyValuePair.Value);
				}
				foreach (KeyValuePair<string, Expression> keyValuePair2 in node.SourceTemplateNode.Data.PropertyExpressions)
				{
					node.Data.PropertyExpressions.Add(keyValuePair2.Key, keyValuePair2.Value);
				}
				Document.<ResolveProperties>g__ApplyInsert|6_0(node, data);
				foreach (KeyValuePair<string, DocumentNode.NodeData> keyValuePair3 in templateSetup.Inserts)
				{
					DocumentNode documentNode2 = Document.<ResolveProperties>g__FindNodeByName|6_1(node, keyValuePair3.Key);
					bool flag3 = documentNode2 == null;
					if (flag3)
					{
						throw new TextParser.TextParserException("Could not find node named " + keyValuePair3.Key + " for insertion", node.SourceSpan);
					}
					Document.<ResolveProperties>g__ApplyInsert|6_0(documentNode2, keyValuePair3.Value);
				}
			}
			foreach (KeyValuePair<string, Expression> keyValuePair4 in node.Data.PropertyExpressions)
			{
				Type type;
				bool flag4 = !node.ElementClassInfo.PropertyTypes.TryGetValue(keyValuePair4.Key, out type);
				if (flag4)
				{
					throw new TextParser.TextParserException("Unknown property " + keyValuePair4.Key + " on node of type " + node.ElementClassInfo.Name, node.SourceSpan);
				}
				object value;
				bool flag5 = !node.TryResolveAs(A_1.context, keyValuePair4.Value, type, out value);
				if (flag5)
				{
					Type type2 = Nullable.GetUnderlyingType(type) ?? type;
					throw new Expression.ExpressionResolutionException("Could not resolve expression for property " + keyValuePair4.Key + " to type " + type2.Name, keyValuePair4.Value, null);
				}
				node.Data.PropertyValues.Add(keyValuePair4.Key, value);
			}
			bool flag6 = node.Data.Children.Count > 0 && node.ElementClassInfo != null && !node.ElementClassInfo.AcceptsChildren;
			if (flag6)
			{
				throw new TextParser.TextParserException("Node of type " + node.ElementClassInfo.Name + " can't have children.", node.SourceSpan);
			}
			foreach (DocumentNode node2 in node.Data.Children)
			{
				Document.<ResolveProperties>g__Walk|6_2(node2, ref A_1);
			}
		}

		// Token: 0x04001B0A RID: 6922
		public readonly DocumentNode RootNode;

		// Token: 0x04001B0B RID: 6923
		public readonly Dictionary<string, string> NamedDocumentReferences = new Dictionary<string, string>();

		// Token: 0x04001B0C RID: 6924
		public static readonly Dictionary<string, ElementClassInfo> ElementClassInfos = new Dictionary<string, ElementClassInfo>();
	}
}
