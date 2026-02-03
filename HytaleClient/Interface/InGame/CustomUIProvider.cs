using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using HytaleClient.Audio;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.Interface.InGame
{
	// Token: 0x0200087E RID: 2174
	internal class CustomUIProvider : Disposable, IUIProvider
	{
		// Token: 0x06003D6A RID: 15722 RVA: 0x0009CF54 File Offset: 0x0009B154
		private static void GetFieldOrProperty(Type parentType, string name, out FieldInfo fieldInfo, out PropertyInfo propertyInfo)
		{
			MemberInfo[] member = parentType.GetMember(name, BindingFlags.Instance | BindingFlags.Public);
			bool flag = member.Length != 1;
			if (flag)
			{
				fieldInfo = null;
				propertyInfo = null;
			}
			else
			{
				fieldInfo = (member[0] as FieldInfo);
				propertyInfo = (member[0] as PropertyInfo);
			}
		}

		// Token: 0x06003D6B RID: 15723 RVA: 0x0009CF98 File Offset: 0x0009B198
		public void ApplyCommands(CustomUICommand[] commands, Element layer)
		{
			foreach (CustomUICommand customUICommand in commands)
			{
				Element element = null;
				List<string> list = null;
				bool flag = customUICommand.Selector != null;
				if (flag)
				{
					CustomUIProvider.ResolveSelector(customUICommand.Selector, layer, out element, out list);
					bool flag2 = element == null;
					if (flag2)
					{
						throw new Exception("Selected element in CustomUI command was not found. Selector: " + customUICommand.Selector);
					}
				}
				switch (customUICommand.Type)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				{
					bool flag3 = list != null;
					if (flag3)
					{
						throw new Exception(string.Format("Custom UI {0} command cannot be applied on a property. Selector:  Selector: {1}", customUICommand.Type, customUICommand.Selector));
					}
					bool flag4 = customUICommand.Type == null || customUICommand.Type == 1;
					bool flag5 = customUICommand.Type == 1 || customUICommand.Type == 3;
					bool flag6 = flag5;
					Document document;
					if (flag6)
					{
						try
						{
							document = DocumentParser.Parse(customUICommand.Text, "");
							document.ResolveProperties(this);
						}
						catch
						{
							throw new Exception(string.Format("Failed to parse or resolve document for Custom UI {0} command. Selector: {1}", customUICommand.Type, customUICommand.Selector));
						}
					}
					else
					{
						bool flag7 = !this.Interface.InGameCustomUIProvider.TryGetDocument(customUICommand.Text, out document);
						if (flag7)
						{
							throw new Exception(string.Format("Could not find document {0} for Custom UI {1} command. Selector: {2}", customUICommand.Text, customUICommand.Type, customUICommand.Selector));
						}
					}
					UIFragment uifragment = document.Instantiate(layer.Desktop, null);
					bool flag8 = flag4;
					if (flag8)
					{
						bool flag9 = element != null && !element.GetType().GetCustomAttribute<UIMarkupElementAttribute>().AcceptsChildren;
						if (flag9)
						{
							throw new Exception(string.Format("CustomUI {0} command's selected element doesn't accept children. Selector: {1}", customUICommand.Type, customUICommand.Selector));
						}
						Element element2 = element ?? layer;
						foreach (Element child in uifragment.RootElements)
						{
							element2.Add(child, -1);
						}
					}
					else
					{
						bool flag10 = element == null;
						if (flag10)
						{
							throw new Exception(string.Format("CustomUI {0} command needs a selected element", customUICommand.Type));
						}
						Element parent = element.Parent;
						foreach (Element element3 in uifragment.RootElements)
						{
							parent.Add(element3, element);
							element = element3;
						}
					}
					break;
				}
				case 4:
				{
					bool flag11 = element == null;
					if (flag11)
					{
						throw new Exception("CustomUI Remove command needs a selected element");
					}
					bool flag12 = list != null;
					if (flag12)
					{
						throw new Exception("CustomUI Remove command can't be applied on a property. Selector: " + customUICommand.Selector);
					}
					element.Parent.Remove(element);
					break;
				}
				case 5:
				{
					bool flag13 = element == null;
					if (flag13)
					{
						throw new Exception("CustomUI Set command needs a selected element");
					}
					JToken jtoken;
					try
					{
						jtoken = ((JObject)BsonHelper.FromBson(customUICommand.Data))["0"];
					}
					catch (JsonReaderException)
					{
						throw new Exception("CustomUI command data is not valid JSON. Selector: " + customUICommand.Selector);
					}
					bool flag14 = list != null && list.Count > 0;
					if (flag14)
					{
						FieldInfo fieldInfo;
						PropertyInfo propertyInfo;
						CustomUIProvider.GetFieldOrProperty(element.GetType(), list[0], out fieldInfo, out propertyInfo);
						bool flag15 = fieldInfo == null && propertyInfo == null;
						if (flag15)
						{
							throw new Exception("CustomUI Set command selector doesn't match a markup property. Selector: " + customUICommand.Selector);
						}
						MemberInfo element4 = (fieldInfo != null) ? fieldInfo : propertyInfo;
						bool flag16 = element4.GetCustomAttribute<UIMarkupPropertyAttribute>() == null;
						if (flag16)
						{
							throw new Exception("CustomUI Set command selector doesn't match a markup property. Selector: " + customUICommand.Selector);
						}
						Type type = ((fieldInfo != null) ? fieldInfo.FieldType : null) ?? propertyInfo.PropertyType;
						object obj = element;
						for (int j = 1; j < list.Count; j++)
						{
							bool flag17 = obj == null;
							if (flag17)
							{
								throw new Exception("CustomUI Set command selector doesn't match a markup property. Selector: " + customUICommand.Selector);
							}
							obj = (((fieldInfo != null) ? fieldInfo.GetValue(obj) : null) ?? propertyInfo.GetValue(obj));
							string name = list[j];
							CustomUIProvider.GetFieldOrProperty(type, name, out fieldInfo, out propertyInfo);
							bool flag18 = fieldInfo == null && propertyInfo == null;
							if (flag18)
							{
								throw new Exception("CustomUI Set command selector doesn't match a markup property. Selector: " + customUICommand.Selector);
							}
							type = (((fieldInfo != null) ? fieldInfo.FieldType : null) ?? propertyInfo.PropertyType);
						}
						try
						{
							bool flag19 = fieldInfo != null;
							if (flag19)
							{
								fieldInfo.SetValue(obj, this.JsonToMarkupValue(type, jtoken));
							}
							else
							{
								propertyInfo.SetValue(obj, this.JsonToMarkupValue(type, jtoken));
							}
						}
						catch (Exception innerException)
						{
							throw new Exception("CustomUI Set command couldn't set value. Selector: " + customUICommand.Selector, innerException);
						}
					}
					else
					{
						bool flag20 = jtoken.Type != 1;
						if (flag20)
						{
							throw new Exception("CustomUI command data must be an object if no property was selected. Selector: " + customUICommand.Selector);
						}
						foreach (KeyValuePair<string, JToken> keyValuePair in ((JObject)jtoken))
						{
							FieldInfo fieldInfo2;
							PropertyInfo propertyInfo2;
							CustomUIProvider.GetFieldOrProperty(element.GetType(), keyValuePair.Key, out fieldInfo2, out propertyInfo2);
							MemberInfo element5 = (fieldInfo2 != null) ? fieldInfo2 : propertyInfo2;
							bool flag21 = element5.GetCustomAttribute<UIMarkupPropertyAttribute>() == null;
							if (flag21)
							{
								throw new Exception("CustomUI Set command selector doesn't match a markup property. Selector: " + customUICommand.Selector);
							}
							Type type2 = ((fieldInfo2 != null) ? fieldInfo2.FieldType : null) ?? propertyInfo2.PropertyType;
							object obj2 = ((fieldInfo2 != null) ? fieldInfo2.GetValue(element) : null) ?? propertyInfo2.GetValue(element);
							try
							{
								bool flag22 = fieldInfo2 != null;
								if (flag22)
								{
									fieldInfo2.SetValue(obj2, this.JsonToMarkupValue(fieldInfo2.FieldType, keyValuePair.Value));
								}
								else
								{
									propertyInfo2.SetValue(obj2, this.JsonToMarkupValue(propertyInfo2.PropertyType, keyValuePair.Value));
								}
							}
							catch (Exception innerException2)
							{
								throw new Exception("CustomUI Set command couldn't set value. Selector: " + customUICommand.Selector, innerException2);
							}
						}
					}
					break;
				}
				case 6:
				{
					bool flag23 = element == null;
					if (flag23)
					{
						throw new Exception("CustomUI Clear command needs a selected element");
					}
					element.Clear();
					break;
				}
				}
			}
			layer.Layout(null, true);
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x0009D700 File Offset: 0x0009B900
		public static void ResolveSelector(string selector, Element layer, out Element selectedElement, out List<string> selectedPropertyPath)
		{
			selectedElement = null;
			selectedPropertyPath = null;
			string[] array = selector.Split(new char[]
			{
				'.'
			}, 2);
			Element element = null;
			string[] array2 = array[0].Split(new char[]
			{
				' '
			});
			int i = 0;
			while (i < array2.Length)
			{
				string text = array2[i];
				bool flag = text[0] != '#';
				if (!flag)
				{
					string[] array3 = text.Split(new char[]
					{
						'['
					});
					element = (element ?? layer).Find<Element>(array3[0].Substring(1));
					bool flag2 = element == null || element.GetType().GetCustomAttribute<UIMarkupElementAttribute>() == null;
					if (!flag2)
					{
						bool flag3 = array3.Length > 1;
						if (flag3)
						{
							for (int j = 1; j < array3.Length; j++)
							{
								string text2 = array3[j];
								bool flag4 = text2.Length < 2 || text2[text2.Length - 1] != ']';
								if (flag4)
								{
									return;
								}
								int num;
								bool flag5 = !int.TryParse(text2.Substring(0, text2.Length - 1), NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture, out num) || num < 0 || num >= element.Children.Count;
								if (flag5)
								{
									return;
								}
								element = element.Children[num];
								bool flag6 = element == null || element.GetType().GetCustomAttribute<UIMarkupElementAttribute>() == null;
								if (flag6)
								{
									return;
								}
							}
						}
						i++;
						continue;
					}
				}
				return;
			}
			bool flag7 = element == null;
			if (flag7)
			{
				return;
			}
			List<string> list = new List<string>();
			bool flag8 = array.Length == 2;
			if (flag8)
			{
				foreach (string text3 in array[1].Split(new char[]
				{
					'.'
				}))
				{
					bool flag9 = text3.Length == 0;
					if (flag9)
					{
						return;
					}
					bool flag10 = text3.IndexOf(' ') != -1;
					if (flag10)
					{
						return;
					}
					list.Add(text3);
				}
			}
			selectedElement = element;
			bool flag11 = list.Count > 0;
			if (flag11)
			{
				selectedPropertyPath = list;
				return;
			}
		}

		// Token: 0x06003D6D RID: 15725 RVA: 0x0009D938 File Offset: 0x0009BB38
		public static bool TryGetPropertyValueAsJsonFromSelector(string selector, Element layer, out JToken value)
		{
			value = null;
			Element element;
			List<string> list;
			CustomUIProvider.ResolveSelector(selector, layer, out element, out list);
			bool flag = list == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				FieldInfo fieldInfo;
				PropertyInfo propertyInfo;
				CustomUIProvider.GetFieldOrProperty(element.GetType(), list[0], out fieldInfo, out propertyInfo);
				bool flag2 = fieldInfo == null && propertyInfo == null;
				if (flag2)
				{
					result = false;
				}
				else
				{
					MemberInfo element2 = (fieldInfo != null) ? fieldInfo : propertyInfo;
					bool flag3 = element2.GetCustomAttribute<UIMarkupPropertyAttribute>() == null;
					if (flag3)
					{
						result = false;
					}
					else
					{
						Type parentType = ((fieldInfo != null) ? fieldInfo.FieldType : null) ?? propertyInfo.PropertyType;
						object obj = element;
						for (int i = 1; i < list.Count; i++)
						{
							bool flag4 = obj == null;
							if (flag4)
							{
								return false;
							}
							obj = (((fieldInfo != null) ? fieldInfo.GetValue(obj) : null) ?? propertyInfo.GetValue(obj));
							string name = list[i];
							CustomUIProvider.GetFieldOrProperty(parentType, name, out fieldInfo, out propertyInfo);
							bool flag5 = fieldInfo == null && propertyInfo == null;
							if (flag5)
							{
								return false;
							}
							parentType = (((fieldInfo != null) ? fieldInfo.FieldType : null) ?? propertyInfo.PropertyType);
						}
						object value2;
						try
						{
							bool flag6 = fieldInfo != null;
							if (flag6)
							{
								value2 = fieldInfo.GetValue(obj);
							}
							else
							{
								value2 = propertyInfo.GetValue(obj);
							}
						}
						catch
						{
							return false;
						}
						result = CustomUIProvider.TrySerializeObjectAsJson(value2, out value);
					}
				}
			}
			return result;
		}

		// Token: 0x06003D6E RID: 15726 RVA: 0x0009DAD0 File Offset: 0x0009BCD0
		private static bool TrySerializeObjectAsJson(object obj, out JToken result)
		{
			result = null;
			bool flag = obj == null;
			bool result2;
			if (flag)
			{
				result = JValue.CreateNull();
				result2 = true;
			}
			else
			{
				Type type = obj.GetType();
				type = (Nullable.GetUnderlyingType(type) ?? type);
				bool flag2 = type == typeof(string);
				if (flag2)
				{
					result = JToken.FromObject((string)obj);
					result2 = true;
				}
				else
				{
					bool flag3 = type == typeof(char);
					if (flag3)
					{
						result = JToken.FromObject((char)obj);
						result2 = true;
					}
					else
					{
						bool flag4 = type == typeof(int);
						if (flag4)
						{
							result = JToken.FromObject((int)obj);
							result2 = true;
						}
						else
						{
							bool flag5 = type == typeof(float);
							if (flag5)
							{
								result = JToken.FromObject((float)obj);
								result2 = true;
							}
							else
							{
								bool flag6 = type == typeof(decimal);
								if (flag6)
								{
									result = JToken.FromObject((decimal)obj);
									result2 = true;
								}
								else
								{
									bool flag7 = type == typeof(bool);
									if (flag7)
									{
										result = JToken.FromObject((bool)obj);
										result2 = true;
									}
									else
									{
										bool isEnum = type.IsEnum;
										if (isEnum)
										{
											result = JToken.FromObject(obj.ToString());
											result2 = true;
										}
										else
										{
											bool flag8 = type == typeof(UInt32Color);
											if (flag8)
											{
												result = JToken.FromObject(((UInt32Color)obj).ToHexString(true));
												result2 = true;
											}
											else
											{
												bool isArray = type.IsArray;
												if (isArray)
												{
													JArray jarray = new JArray();
													foreach (object obj2 in ((Array)obj))
													{
														JToken jtoken;
														bool flag9 = !CustomUIProvider.TrySerializeObjectAsJson(obj2, out jtoken);
														if (flag9)
														{
															return false;
														}
														jarray.Add(jtoken);
													}
													result = jarray;
													result2 = true;
												}
												else
												{
													bool flag10 = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
													if (flag10)
													{
														JArray jarray2 = new JArray();
														foreach (object obj3 in ((IList)obj))
														{
															JToken jtoken2;
															bool flag11 = !CustomUIProvider.TrySerializeObjectAsJson(obj3, out jtoken2);
															if (flag11)
															{
																return false;
															}
															jarray2.Add(jtoken2);
														}
														result = jarray2;
														result2 = true;
													}
													else
													{
														result = new JObject();
														foreach (MemberInfo memberInfo in type.GetMembers(BindingFlags.Instance | BindingFlags.Public))
														{
															FieldInfo fieldInfo = memberInfo as FieldInfo;
															PropertyInfo propertyInfo = memberInfo as PropertyInfo;
															bool flag12 = fieldInfo == null && propertyInfo == null;
															if (!flag12)
															{
																object obj4 = (fieldInfo != null) ? fieldInfo.GetValue(obj) : propertyInfo.GetValue(obj);
																JToken jtoken3;
																bool flag13 = !CustomUIProvider.TrySerializeObjectAsJson(obj4, out jtoken3);
																if (flag13)
																{
																	return false;
																}
																result[memberInfo.Name] = jtoken3;
															}
														}
														result2 = true;
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
			return result2;
		}

		// Token: 0x06003D6F RID: 15727 RVA: 0x0009DE5C File Offset: 0x0009C05C
		private object JsonToMarkupValue(Type type, JToken jsonToken)
		{
			bool flag = jsonToken.Type == 10;
			if (!flag)
			{
				type = (Nullable.GetUnderlyingType(type) ?? type);
				switch (jsonToken.Type)
				{
				case 1:
				{
					JObject jobject = (JObject)jsonToken;
					JToken jtoken;
					JToken jtoken2;
					bool flag2 = jobject.TryGetValue("$Document", ref jtoken) && jobject.TryGetValue("@Value", ref jtoken2);
					if (flag2)
					{
						Document document;
						bool flag3 = !this.Interface.InGameCustomUIProvider.TryGetDocument((string)jtoken, out document);
						if (flag3)
						{
							throw new Exception(string.Format("Failed to find document {0}", jtoken));
						}
						object result;
						bool flag4 = !document.RootNode.TryResolveAs(new ResolutionContext(this), document.RootNode.Data.LocalNamedExpressions[(string)jtoken2], type, out result);
						if (flag4)
						{
							throw new Exception(string.Format("Failed to resolve expession for root named value {0}", jtoken2));
						}
						return result;
					}
					else
					{
						JToken jtoken3;
						bool flag5 = type == typeof(string) && jobject.TryGetValue("MessageId", ref jtoken3);
						if (flag5)
						{
							Dictionary<string, string> dictionary = null;
							JToken jtoken4;
							bool flag6 = jobject.TryGetValue("Params", ref jtoken4);
							if (flag6)
							{
								dictionary = new Dictionary<string, string>();
								foreach (KeyValuePair<string, JToken> keyValuePair in ((JObject)jtoken4))
								{
									dictionary.Add(keyValuePair.Key, (string)keyValuePair.Value);
								}
							}
							return this.GetText((string)jtoken3, dictionary, true);
						}
						bool flag7 = type == typeof(IList<Label.LabelSpan>);
						if (flag7)
						{
							return FormattedMessageConverter.GetLabelSpans(jobject.ToObject<FormattedMessage>(), this.Interface, default(SpanStyle), true);
						}
						object obj = Activator.CreateInstance(type);
						foreach (KeyValuePair<string, JToken> keyValuePair2 in jobject)
						{
							FieldInfo fieldInfo;
							PropertyInfo propertyInfo;
							CustomUIProvider.GetFieldOrProperty(type, keyValuePair2.Key, out fieldInfo, out propertyInfo);
							bool flag8 = fieldInfo == null && propertyInfo == null;
							if (flag8)
							{
								throw new Exception(string.Format("Property {0} does not exist on {1}", keyValuePair2.Key, type));
							}
							object value = this.JsonToMarkupValue(((fieldInfo != null) ? fieldInfo.FieldType : null) ?? propertyInfo.PropertyType, keyValuePair2.Value);
							bool flag9 = fieldInfo != null;
							if (flag9)
							{
								fieldInfo.SetValue(obj, value);
							}
							else
							{
								propertyInfo.SetValue(obj, value);
							}
						}
						return obj;
					}
					break;
				}
				case 2:
				{
					bool isGenericType = type.IsGenericType;
					if (isGenericType)
					{
						Type genericTypeDefinition = type.GetGenericTypeDefinition();
						bool flag10 = genericTypeDefinition == typeof(List<>);
						if (flag10)
						{
							IList list = (IList)Activator.CreateInstance(type);
							Type type2 = type.GenericTypeArguments[0];
							foreach (JToken jsonToken2 in ((JArray)jsonToken))
							{
								list.Add(this.JsonToMarkupValue(type2, jsonToken2));
							}
							return list;
						}
						bool flag11 = genericTypeDefinition == typeof(IReadOnlyList<>);
						if (flag11)
						{
							Type type3 = typeof(List<>).MakeGenericType(new Type[]
							{
								type.GetGenericArguments()[0]
							});
							IList list2 = (IList)Activator.CreateInstance(type3);
							Type type4 = type.GenericTypeArguments[0];
							foreach (JToken jsonToken3 in ((JArray)jsonToken))
							{
								list2.Add(this.JsonToMarkupValue(type4, jsonToken3));
							}
							return list2;
						}
					}
					bool isArray = type.IsArray;
					if (isArray)
					{
						JArray jarray = (JArray)jsonToken;
						Array array = (Array)Activator.CreateInstance(type, new object[]
						{
							jarray.Count
						});
						Type elementType = type.GetElementType();
						for (int i = 0; i < array.Length; i++)
						{
							array.SetValue(this.JsonToMarkupValue(elementType, jarray[i]), i);
						}
						return array;
					}
					break;
				}
				case 6:
				{
					bool flag12 = type == typeof(int);
					if (flag12)
					{
						return (int)jsonToken;
					}
					bool flag13 = type == typeof(float);
					if (flag13)
					{
						return (float)jsonToken;
					}
					bool flag14 = type == typeof(double);
					if (flag14)
					{
						return (double)jsonToken;
					}
					bool flag15 = type == typeof(decimal);
					if (flag15)
					{
						return (decimal)jsonToken;
					}
					break;
				}
				case 7:
				{
					bool flag16 = type == typeof(float);
					if (flag16)
					{
						return (float)jsonToken;
					}
					bool flag17 = type == typeof(double);
					if (flag17)
					{
						return (double)jsonToken;
					}
					bool flag18 = type == typeof(decimal);
					if (flag18)
					{
						return (decimal)jsonToken;
					}
					break;
				}
				case 8:
				{
					string text = (string)jsonToken;
					bool flag19 = type == typeof(string);
					if (flag19)
					{
						return text;
					}
					bool flag20 = type == typeof(char) && text.Length == 1;
					if (flag20)
					{
						return text[0];
					}
					bool flag21 = type == typeof(UIPath);
					if (flag21)
					{
						return new UIPath(text);
					}
					bool flag22 = type == typeof(UInt32Color);
					if (flag22)
					{
						bool flag23 = (text.Length == 7 || text.Length == 9) && text[0] == '#';
						if (flag23)
						{
							try
							{
								byte r = Convert.ToByte(text.Substring(1, 2), 16);
								byte g = Convert.ToByte(text.Substring(3, 2), 16);
								byte b = Convert.ToByte(text.Substring(5, 2), 16);
								byte a = (text.Length == 7) ? byte.MaxValue : Convert.ToByte(text.Substring(7, 2), 16);
								return UInt32Color.FromRGBA(r, g, b, a);
							}
							catch
							{
							}
						}
					}
					else
					{
						bool flag24 = type == typeof(PatchStyle);
						if (flag24)
						{
							bool flag25 = (text.Length == 7 || text.Length == 9) && text[0] == '#';
							if (flag25)
							{
								return new PatchStyle((UInt32Color)this.JsonToMarkupValue(typeof(UInt32Color), jsonToken));
							}
							return new PatchStyle(text);
						}
						else
						{
							bool isEnum = type.IsEnum;
							if (isEnum)
							{
								try
								{
									return Enum.Parse(type, text);
								}
								catch
								{
								}
							}
						}
					}
					break;
				}
				case 9:
				{
					bool flag26 = type == typeof(bool);
					if (flag26)
					{
						return (bool)jsonToken;
					}
					break;
				}
				}
				throw new Exception(string.Format("Failed to convert JSON value ({0}) to specified type ({1})", jsonToken.Type, type.Name));
			}
			return null;
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x0009E67C File Offset: 0x0009C87C
		public CustomUIProvider(Interface @interface)
		{
			this.Interface = @interface;
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x0009E6A3 File Offset: 0x0009C8A3
		protected override void DoDispose()
		{
			this.DisposeTextures();
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x0009E6AD File Offset: 0x0009C8AD
		public string FormatNumber(int value)
		{
			return this.Interface.FormatNumber(value);
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x0009E6BB File Offset: 0x0009C8BB
		public string FormatNumber(float value)
		{
			return this.Interface.FormatNumber(value);
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x0009E6C9 File Offset: 0x0009C8C9
		public string FormatNumber(double value)
		{
			return this.Interface.FormatNumber(value);
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x0009E6D7 File Offset: 0x0009C8D7
		public string FormatRelativeTime(DateTime time)
		{
			return this.Interface.FormatRelativeTime(time);
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x0009E6E5 File Offset: 0x0009C8E5
		public FontFamily GetFontFamily(string name)
		{
			return this.Interface.GetFontFamily(name);
		}

		// Token: 0x06003D77 RID: 15735 RVA: 0x0009E6F3 File Offset: 0x0009C8F3
		public string GetText(string key, Dictionary<string, string> parameters = null, bool returnFallback = true)
		{
			return this.Interface.GetServerText(key, parameters, returnFallback);
		}

		// Token: 0x06003D78 RID: 15736 RVA: 0x0009E704 File Offset: 0x0009C904
		public void LoadDocuments()
		{
			this._documentsLibrary.Clear();
			foreach (KeyValuePair<string, string> keyValuePair in this.Interface.App.InGame.Instance.HashesByServerAssetPath)
			{
				bool flag = !keyValuePair.Key.StartsWith("UI/Custom/") || !keyValuePair.Key.EndsWith(".ui");
				if (!flag)
				{
					string text = keyValuePair.Key.Substring("UI/Custom/".Length);
					Document value = DocumentParser.Parse(File.ReadAllText(AssetManager.GetAssetLocalPathUsingHash(keyValuePair.Value)), text);
					this._documentsLibrary.Add(text, value);
				}
			}
			foreach (KeyValuePair<string, Document> keyValuePair2 in this._documentsLibrary)
			{
				keyValuePair2.Value.ResolveProperties(this);
			}
		}

		// Token: 0x06003D79 RID: 15737 RVA: 0x0009E82C File Offset: 0x0009CA2C
		public void ClearDocuments()
		{
			this._documentsLibrary.Clear();
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x0009E83A File Offset: 0x0009CA3A
		public bool TryGetDocument(string path, out Document document)
		{
			return this._documentsLibrary.TryGetValue(path, out document);
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x0009E84C File Offset: 0x0009CA4C
		public void PlaySound(SoundStyle sound)
		{
			Engine engine = this.Interface.Engine;
			uint eventId;
			bool flag = !engine.Audio.ResourceManager.WwiseEventIds.TryGetValue(sound.SoundPath.Value, out eventId);
			if (flag)
			{
				CustomUIProvider.Logger.Warn("Unknown custom UI sound: {0}", sound.SoundPath.Value);
			}
			else
			{
				engine.Audio.PostEvent(eventId, AudioDevice.PlayerSoundObjectReference);
			}
		}

		// Token: 0x1700108B RID: 4235
		// (get) Token: 0x06003D7C RID: 15740 RVA: 0x0009E8C1 File Offset: 0x0009CAC1
		public Point TextureAtlasSize
		{
			get
			{
				Texture atlas = this._atlas;
				int x = (atlas != null) ? atlas.Width : 0;
				Texture atlas2 = this._atlas;
				return new Point(x, (atlas2 != null) ? atlas2.Height : 0);
			}
		}

		// Token: 0x1700108C RID: 4236
		// (get) Token: 0x06003D7D RID: 15741 RVA: 0x0009E8EC File Offset: 0x0009CAEC
		// (set) Token: 0x06003D7E RID: 15742 RVA: 0x0009E8F4 File Offset: 0x0009CAF4
		public TextureArea WhitePixel { get; private set; }

		// Token: 0x1700108D RID: 4237
		// (get) Token: 0x06003D7F RID: 15743 RVA: 0x0009E8FD File Offset: 0x0009CAFD
		// (set) Token: 0x06003D80 RID: 15744 RVA: 0x0009E905 File Offset: 0x0009CB05
		public TextureArea MissingTexture { get; private set; }

		// Token: 0x1700108E RID: 4238
		// (get) Token: 0x06003D81 RID: 15745 RVA: 0x0009E90E File Offset: 0x0009CB0E
		// (set) Token: 0x06003D82 RID: 15746 RVA: 0x0009E916 File Offset: 0x0009CB16
		public TexturePatch MissingTexturePatch { get; private set; }

		// Token: 0x06003D83 RID: 15747 RVA: 0x0009E920 File Offset: 0x0009CB20
		public void LoadTextures(bool use2x)
		{
			Texture atlas = this._atlas;
			if (atlas != null)
			{
				atlas.Dispose();
			}
			this._atlasTextureAreas.Clear();
			List<string> list = new List<string>();
			HashSet<string> hashSet = new HashSet<string>();
			ConcurrentDictionary<string, string> hashesByServerAssetPath = this.Interface.App.InGame.Instance.HashesByServerAssetPath;
			foreach (KeyValuePair<string, string> keyValuePair in hashesByServerAssetPath)
			{
				bool flag = !keyValuePair.Key.StartsWith("UI/Custom/") || !keyValuePair.Key.EndsWith(".png");
				if (!flag)
				{
					hashSet.Add(keyValuePair.Key);
				}
			}
			foreach (string text in hashSet)
			{
				bool flag2 = text.EndsWith("@2x.png");
				bool flag3 = !use2x && flag2;
				if (flag3)
				{
					string text2 = text.Replace("@2x.png", ".png");
					bool flag4 = hashSet.Contains(text2);
					if (flag4)
					{
						continue;
					}
				}
				else
				{
					bool flag5 = use2x && !flag2;
					if (flag5)
					{
						string text3 = text.Replace(".png", "@2x.png");
						bool flag6 = hashSet.Contains(text3);
						if (flag6)
						{
							continue;
						}
					}
				}
				list.Add(text);
			}
			int num = use2x ? 8192 : 4096;
			this._atlas = new Texture(Texture.TextureTypes.Texture2D);
			this._atlas.CreateTexture2D(num, num, null, 0, GL.LINEAR_MIPMAP_LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			List<Image> list2 = new List<Image>();
			list2.Add(BaseInterface.MakeWhitePixelImage("Special:WhitePixel"));
			list2.Add(BaseInterface.MakeMissingImage("Special:Missing"));
			foreach (string text4 in list)
			{
				string hash;
				hashesByServerAssetPath.TryGetValue(text4, ref hash);
				list2.Add(new Image(text4.Substring("UI/Custom/".Length), AssetManager.GetAssetUsingHash(hash, false)));
			}
			list2.Sort((Image a, Image b) => b.Height.CompareTo(a.Height));
			Dictionary<Image, Point> dictionary;
			byte[] atlasPixels = Image.Pack(num, list2, out dictionary, true, default(CancellationToken));
			this._atlas.UpdateTexture2DMipMaps(Texture.BuildMipmapPixels(atlasPixels, num, this._atlas.MipmapLevelCount));
			foreach (KeyValuePair<Image, Point> keyValuePair2 in dictionary)
			{
				Image key = keyValuePair2.Key;
				Point value = keyValuePair2.Value;
				string text5 = key.Name.Replace("\\", "/");
				int num2 = text5.EndsWith("@2x.png") ? 2 : 1;
				bool flag7 = num2 == 2;
				if (flag7)
				{
					text5 = text5.Replace("@2x.png", ".png");
				}
				this._atlasTextureAreas.Add(text5, new TextureArea(this._atlas, value.X, value.Y, key.Width, key.Height, num2));
			}
			this.WhitePixel = this.MakeTextureArea("Special:WhitePixel");
			this.MissingTexture = this.MakeTextureArea("Special:Missing");
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x0009ECF4 File Offset: 0x0009CEF4
		public void DisposeTextures()
		{
			Texture atlas = this._atlas;
			if (atlas != null)
			{
				atlas.Dispose();
			}
			this._atlasTextureAreas.Clear();
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x0009ED18 File Offset: 0x0009CF18
		public TextureArea MakeTextureArea(string path)
		{
			TextureArea textureArea;
			bool flag = this._atlasTextureAreas.TryGetValue(path, out textureArea);
			TextureArea result;
			if (flag)
			{
				result = textureArea.Clone();
			}
			else
			{
				result = this.MissingTexture;
			}
			return result;
		}

		// Token: 0x04001C97 RID: 7319
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04001C98 RID: 7320
		public readonly Interface Interface;

		// Token: 0x04001C99 RID: 7321
		private readonly Dictionary<string, Document> _documentsLibrary = new Dictionary<string, Document>();

		// Token: 0x04001C9A RID: 7322
		private Texture _atlas;

		// Token: 0x04001C9B RID: 7323
		private Dictionary<string, TextureArea> _atlasTextureAreas = new Dictionary<string, TextureArea>();
	}
}
