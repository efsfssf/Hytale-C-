using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.UI
{
	// Token: 0x02000827 RID: 2087
	public class DocGen
	{
		// Token: 0x06003A70 RID: 14960 RVA: 0x00084954 File Offset: 0x00082B54
		public static string GenerateMediaWikiPage()
		{
			DocGen.<>c__DisplayClass0_0 CS$<>8__locals1;
			CS$<>8__locals1.doc = new XmlDocument();
			CS$<>8__locals1.doc.Load("../../Documentation.xml");
			CS$<>8__locals1.propertyTypes = new HashSet<Type>();
			CS$<>8__locals1.enumTypes = new HashSet<Type>();
			CS$<>8__locals1.pageStr = "This is a generated list of UI elements accessible by markup. More information about the User Interface framework can be found [[User_Interface|here]].\n\n";
			DocGen.<GenerateMediaWikiPage>g__Title|0_11("Elements", 1, ref CS$<>8__locals1);
			List<ElementClassInfo> list = new List<ElementClassInfo>(Document.ElementClassInfos.Values);
			list.Sort((ElementClassInfo a, ElementClassInfo b) => string.Compare(a.Name, b.Name, StringComparison.InvariantCulture));
			foreach (ElementClassInfo elementClassInfo in list)
			{
				string str = elementClassInfo.Constructor.DeclaringType.IsGenericType ? elementClassInfo.Constructor.DeclaringType.GetGenericTypeDefinition().FullName : elementClassInfo.Constructor.DeclaringType.FullName;
				string description = DocGen.<GenerateMediaWikiPage>g__GetXmlSummary|0_3("T:" + str, ref CS$<>8__locals1);
				DocGen.<GenerateMediaWikiPage>g__Title|0_11(elementClassInfo.Name, 2, ref CS$<>8__locals1);
				DocGen.<GenerateMediaWikiPage>g__Description|0_7(description, new bool?(elementClassInfo.AcceptsChildren), ref CS$<>8__locals1);
				DocGen.<GenerateMediaWikiPage>g__TableStart|0_8(true, "Properties", ref CS$<>8__locals1);
				foreach (MemberInfo memberInfo in DocGen.<GenerateMediaWikiPage>g__GetPublicTypeMembers|0_2(elementClassInfo.Constructor.DeclaringType))
				{
					string str2 = memberInfo.DeclaringType.IsGenericType ? memberInfo.DeclaringType.GetGenericTypeDefinition().FullName : memberInfo.DeclaringType.FullName;
					string str3 = str2 + "." + memberInfo.Name;
					string description2 = DocGen.<GenerateMediaWikiPage>g__GetXmlSummary|0_3("P:" + str3, ref CS$<>8__locals1) ?? DocGen.<GenerateMediaWikiPage>g__GetXmlSummary|0_3("F:" + str3, ref CS$<>8__locals1);
					Type type = null;
					MemberInfo memberInfo2 = memberInfo;
					MemberInfo memberInfo3 = memberInfo2;
					FieldInfo fieldInfo = memberInfo3 as FieldInfo;
					if (fieldInfo == null)
					{
						PropertyInfo propertyInfo = memberInfo3 as PropertyInfo;
						if (propertyInfo != null)
						{
							type = propertyInfo.PropertyType;
						}
					}
					else
					{
						type = fieldInfo.FieldType;
					}
					DocGen.<GenerateMediaWikiPage>g__AddDataOrEnumType|0_6(type, ref CS$<>8__locals1);
					DocGen.<GenerateMediaWikiPage>g__TableRow|0_10(memberInfo.Name, DocGen.<GenerateMediaWikiPage>g__GetName|0_4(type), description2, ref CS$<>8__locals1);
				}
				DocGen.<GenerateMediaWikiPage>g__TableEnd|0_9(ref CS$<>8__locals1);
				List<Tuple<string, string>> list2 = DocGen.<GenerateMediaWikiPage>g__GetEventCallbacks|0_1(elementClassInfo.Constructor.DeclaringType, ref CS$<>8__locals1);
				bool flag = list2.Count > 0;
				if (flag)
				{
					DocGen.<GenerateMediaWikiPage>g__TableStart|0_8(false, "Event Callbacks", ref CS$<>8__locals1);
					foreach (Tuple<string, string> tuple in list2)
					{
						DocGen.<GenerateMediaWikiPage>g__TableRow|0_10(tuple.Item1, null, tuple.Item2, ref CS$<>8__locals1);
					}
					DocGen.<GenerateMediaWikiPage>g__TableEnd|0_9(ref CS$<>8__locals1);
				}
			}
			DocGen.<GenerateMediaWikiPage>g__Title|0_11("Property Types", 1, ref CS$<>8__locals1);
			List<Type> list3 = new List<Type>(CS$<>8__locals1.propertyTypes);
			list3.Sort((Type a, Type b) => string.Compare(a.Name, b.Name, StringComparison.InvariantCulture));
			foreach (Type type2 in list3)
			{
				string description3 = DocGen.<GenerateMediaWikiPage>g__GetXmlSummary|0_3("T:" + type2.FullName, ref CS$<>8__locals1);
				DocGen.<GenerateMediaWikiPage>g__Title|0_11(type2.Name, 2, ref CS$<>8__locals1);
				DocGen.<GenerateMediaWikiPage>g__Description|0_7(description3, null, ref CS$<>8__locals1);
				DocGen.<GenerateMediaWikiPage>g__TableStart|0_8(true, "Properties", ref CS$<>8__locals1);
				foreach (FieldInfo fieldInfo2 in type2.GetFields())
				{
					bool flag2 = !fieldInfo2.IsPublic;
					if (!flag2)
					{
						string description4 = DocGen.<GenerateMediaWikiPage>g__GetXmlSummary|0_3("F:" + type2.FullName + "." + fieldInfo2.Name, ref CS$<>8__locals1);
						DocGen.<GenerateMediaWikiPage>g__TableRow|0_10(fieldInfo2.Name, DocGen.<GenerateMediaWikiPage>g__GetName|0_4(fieldInfo2.FieldType), description4, ref CS$<>8__locals1);
					}
				}
				foreach (PropertyInfo propertyInfo2 in type2.GetProperties())
				{
					bool flag3 = propertyInfo2.GetSetMethod() == null || !propertyInfo2.GetSetMethod().IsPublic;
					if (!flag3)
					{
						string description5 = DocGen.<GenerateMediaWikiPage>g__GetXmlSummary|0_3("P:" + type2.FullName + "." + propertyInfo2.Name, ref CS$<>8__locals1);
						DocGen.<GenerateMediaWikiPage>g__TableRow|0_10(propertyInfo2.Name, DocGen.<GenerateMediaWikiPage>g__GetName|0_4(propertyInfo2.PropertyType), description5, ref CS$<>8__locals1);
					}
				}
				DocGen.<GenerateMediaWikiPage>g__TableEnd|0_9(ref CS$<>8__locals1);
			}
			DocGen.<GenerateMediaWikiPage>g__Title|0_11("Enums", 1, ref CS$<>8__locals1);
			List<Type> list4 = new List<Type>(CS$<>8__locals1.enumTypes);
			list4.Sort((Type a, Type b) => string.Compare(a.Name, b.Name, StringComparison.InvariantCulture));
			foreach (Type type3 in CS$<>8__locals1.enumTypes)
			{
				string description6 = DocGen.<GenerateMediaWikiPage>g__GetXmlSummary|0_3("T:" + type3.FullName, ref CS$<>8__locals1);
				DocGen.<GenerateMediaWikiPage>g__Title|0_11(type3.Name, 2, ref CS$<>8__locals1);
				DocGen.<GenerateMediaWikiPage>g__Description|0_7(description6, null, ref CS$<>8__locals1);
				DocGen.<GenerateMediaWikiPage>g__TableStart|0_8(false, "Properties", ref CS$<>8__locals1);
				foreach (string text in Enum.GetNames(type3))
				{
					string description7 = DocGen.<GenerateMediaWikiPage>g__GetXmlSummary|0_3("F:" + type3.FullName + "." + text, ref CS$<>8__locals1);
					DocGen.<GenerateMediaWikiPage>g__TableRow|0_10(text, null, description7, ref CS$<>8__locals1);
				}
				DocGen.<GenerateMediaWikiPage>g__TableEnd|0_9(ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.pageStr;
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x00084FE5 File Offset: 0x000831E5
		[CompilerGenerated]
		internal static string <GenerateMediaWikiPage>g__Link|0_0(string p)
		{
			return string.Concat(new string[]
			{
				"[[#",
				p,
				"|",
				p,
				"]]"
			});
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x00085014 File Offset: 0x00083214
		[CompilerGenerated]
		internal static List<Tuple<string, string>> <GenerateMediaWikiPage>g__GetEventCallbacks|0_1(Type type, ref DocGen.<>c__DisplayClass0_0 A_1)
		{
			List<Tuple<string, string>> list = new List<Tuple<string, string>>();
			foreach (FieldInfo fieldInfo in type.GetFields())
			{
				bool flag = !fieldInfo.IsPublic;
				if (!flag)
				{
					bool flag2 = (fieldInfo.FieldType.IsGenericType && fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(Action<>)) || fieldInfo.FieldType == typeof(Action);
					if (flag2)
					{
						string item = DocGen.<GenerateMediaWikiPage>g__GetXmlSummary|0_3("F:" + fieldInfo.DeclaringType.FullName + "." + fieldInfo.Name, ref A_1);
						list.Add(Tuple.Create<string, string>(fieldInfo.Name, item));
					}
				}
			}
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				bool flag3 = propertyInfo.GetSetMethod() == null || !propertyInfo.GetSetMethod().IsPublic;
				if (!flag3)
				{
					bool flag4 = (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Action<>)) || propertyInfo.PropertyType == typeof(Action);
					if (flag4)
					{
						string item2 = DocGen.<GenerateMediaWikiPage>g__GetXmlSummary|0_3("P:" + propertyInfo.DeclaringType.FullName + "." + propertyInfo.Name, ref A_1);
						list.Add(Tuple.Create<string, string>(propertyInfo.Name, item2));
					}
				}
			}
			return list;
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x000851CC File Offset: 0x000833CC
		[CompilerGenerated]
		internal static List<MemberInfo> <GenerateMediaWikiPage>g__GetPublicTypeMembers|0_2(Type type)
		{
			List<MemberInfo> list = new List<MemberInfo>();
			UIMarkupElementAttribute customAttribute = type.GetCustomAttribute(false);
			bool flag = customAttribute == null;
			List<MemberInfo> result;
			if (flag)
			{
				result = list;
			}
			else
			{
				foreach (MemberInfo memberInfo in type.GetMembers(BindingFlags.Instance | BindingFlags.Public))
				{
					UIMarkupPropertyAttribute customAttribute2 = memberInfo.GetCustomAttribute<UIMarkupPropertyAttribute>();
					bool flag2 = customAttribute2 != null && (customAttribute.ExposeInheritedProperties || memberInfo.DeclaringType == type);
					if (flag2)
					{
						bool flag3 = memberInfo is FieldInfo || memberInfo is PropertyInfo;
						if (flag3)
						{
							list.Add(memberInfo);
						}
					}
				}
				result = list;
			}
			return result;
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x00085278 File Offset: 0x00083478
		[CompilerGenerated]
		internal static string <GenerateMediaWikiPage>g__GetXmlSummary|0_3(string nodePath, ref DocGen.<>c__DisplayClass0_0 A_1)
		{
			XmlNode xmlNode = A_1.doc.SelectSingleNode("//member[@name='" + nodePath + "']");
			bool flag = xmlNode != null;
			string result;
			if (flag)
			{
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("summary");
				bool flag2 = xmlNode2 == null;
				if (flag2)
				{
					result = null;
				}
				else
				{
					string text = xmlNode2.InnerText.Trim();
					text = Regex.Replace(text, "\\s+", " ");
					result = text;
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x000852F0 File Offset: 0x000834F0
		[CompilerGenerated]
		internal static string <GenerateMediaWikiPage>g__GetName|0_4(Type type)
		{
			type = (Nullable.GetUnderlyingType(type) ?? type);
			bool flag = type == typeof(int);
			string result;
			if (flag)
			{
				result = "Integer";
			}
			else
			{
				bool flag2 = type == typeof(string);
				if (flag2)
				{
					result = "String";
				}
				else
				{
					bool flag3 = type == typeof(double);
					if (flag3)
					{
						result = "Double";
					}
					else
					{
						bool flag4 = type == typeof(bool);
						if (flag4)
						{
							result = "Boolean";
						}
						else
						{
							bool flag5 = type == typeof(float);
							if (flag5)
							{
								result = "Float";
							}
							else
							{
								bool flag6 = type == typeof(long);
								if (flag6)
								{
									result = "Long";
								}
								else
								{
									bool flag7 = type == typeof(decimal);
									if (flag7)
									{
										result = "Decimal";
									}
									else
									{
										bool flag8 = type == typeof(UInt32Color);
										if (flag8)
										{
											result = "[[User_Interface#Color|Color]]";
										}
										else
										{
											bool flag9 = type == typeof(UIPath);
											if (flag9)
											{
												result = "UI Path (String)";
											}
											else
											{
												bool flag10 = type == typeof(UIFontName);
												if (flag10)
												{
													result = "Font Name (String)";
												}
												else
												{
													string text = DocGen.<GenerateMediaWikiPage>g__Link|0_0(type.Name) ?? "";
													bool flag11 = type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>) || type.GetGenericTypeDefinition() == typeof(IList<>));
													if (flag11)
													{
														string name = type.GetGenericArguments()[0].Name;
														text = "List<" + DocGen.<GenerateMediaWikiPage>g__Link|0_0(name) + ">";
													}
													else
													{
														bool isArray = type.IsArray;
														if (isArray)
														{
															string name2 = type.GetElementType().Name;
															text = DocGen.<GenerateMediaWikiPage>g__Link|0_0(name2) + "[]";
														}
													}
													string text2 = text;
													bool flag12 = type == typeof(PatchStyle);
													if (flag12)
													{
														text2 += " / String";
													}
													result = text2;
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
			return result;
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x00085528 File Offset: 0x00083728
		[CompilerGenerated]
		internal static void <GenerateMediaWikiPage>g__ScanDataType|0_5(Type type, ref DocGen.<>c__DisplayClass0_0 A_1)
		{
			foreach (FieldInfo fieldInfo in type.GetFields())
			{
				bool flag = !fieldInfo.IsPublic;
				if (!flag)
				{
					DocGen.<GenerateMediaWikiPage>g__AddDataOrEnumType|0_6(fieldInfo.FieldType, ref A_1);
				}
			}
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				bool flag2 = propertyInfo.GetSetMethod() == null || !propertyInfo.GetSetMethod().IsPublic;
				if (!flag2)
				{
					DocGen.<GenerateMediaWikiPage>g__AddDataOrEnumType|0_6(propertyInfo.PropertyType, ref A_1);
				}
			}
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x000855CC File Offset: 0x000837CC
		[CompilerGenerated]
		internal static void <GenerateMediaWikiPage>g__AddDataOrEnumType|0_6(Type type, ref DocGen.<>c__DisplayClass0_0 A_1)
		{
			type = (Nullable.GetUnderlyingType(type) ?? type);
			bool flag = type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>) || type.GetGenericTypeDefinition() == typeof(IList<>));
			if (flag)
			{
				type = type.GetGenericArguments()[0];
			}
			bool isArray = type.IsArray;
			if (isArray)
			{
				type = type.GetElementType();
			}
			bool isEnum = type.IsEnum;
			if (isEnum)
			{
				bool flag2 = !A_1.enumTypes.Contains(type);
				if (flag2)
				{
					A_1.enumTypes.Add(type);
				}
			}
			else
			{
				bool flag3 = type.GetCustomAttribute<UIMarkupDataAttribute>() != null;
				if (flag3)
				{
					bool flag4 = !A_1.propertyTypes.Contains(type);
					if (flag4)
					{
						A_1.propertyTypes.Add(type);
						DocGen.<GenerateMediaWikiPage>g__ScanDataType|0_5(type, ref A_1);
					}
				}
			}
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x000856B4 File Offset: 0x000838B4
		[CompilerGenerated]
		internal static void <GenerateMediaWikiPage>g__Description|0_7(string description, bool? children = null, ref DocGen.<>c__DisplayClass0_0 A_2)
		{
			bool flag = children != null;
			if (flag)
			{
				A_2.pageStr = A_2.pageStr + "Accepts child elements: ''" + (children.Value ? "Yes" : "No") + "''\n\n";
			}
			bool flag2 = description != null;
			if (flag2)
			{
				A_2.pageStr = A_2.pageStr + description + "\n\n";
			}
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x0008571C File Offset: 0x0008391C
		[CompilerGenerated]
		internal static void <GenerateMediaWikiPage>g__TableStart|0_8(bool hasType = true, string title = "Properties", ref DocGen.<>c__DisplayClass0_0 A_2)
		{
			A_2.pageStr = A_2.pageStr + "''' " + title + " '''\n";
			A_2.pageStr += "{| class=\"wikitable\"\n";
			A_2.pageStr += "|-\n";
			A_2.pageStr += "! scope=\"col\"| Name\n";
			if (hasType)
			{
				A_2.pageStr += "! scope=\"col\"| Type\n";
			}
			A_2.pageStr += "! scope=\"col\"| Description\n";
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x000857B9 File Offset: 0x000839B9
		[CompilerGenerated]
		internal static void <GenerateMediaWikiPage>g__TableEnd|0_9(ref DocGen.<>c__DisplayClass0_0 A_0)
		{
			A_0.pageStr += "|}\n";
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x000857D4 File Offset: 0x000839D4
		[CompilerGenerated]
		internal static void <GenerateMediaWikiPage>g__TableRow|0_10(string name, string type, string description = null, ref DocGen.<>c__DisplayClass0_0 A_3)
		{
			A_3.pageStr += "|-\n";
			A_3.pageStr = A_3.pageStr + "| '''" + name + "'''\n";
			bool flag = type != null;
			if (flag)
			{
				A_3.pageStr = A_3.pageStr + "| " + type + "\n";
			}
			A_3.pageStr = A_3.pageStr + "| " + description + "\n";
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x00085854 File Offset: 0x00083A54
		[CompilerGenerated]
		internal static void <GenerateMediaWikiPage>g__Title|0_11(string str, int level, ref DocGen.<>c__DisplayClass0_0 A_2)
		{
			switch (level)
			{
			case 1:
				A_2.pageStr = A_2.pageStr + "== " + str + " ==\n";
				break;
			case 2:
				A_2.pageStr = A_2.pageStr + "=== " + str + " ===\n";
				break;
			case 3:
				A_2.pageStr = A_2.pageStr + "==== " + str + " ====\n";
				break;
			default:
				throw new Exception("Invalid level");
			}
		}
	}
}
