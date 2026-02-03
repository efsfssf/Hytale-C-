using System;
using System.Collections.Generic;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.InGame;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Protocol;

namespace HytaleClient.Interface.Messages
{
	// Token: 0x02000816 RID: 2070
	internal static class FormattedMessageConverter
	{
		// Token: 0x06003964 RID: 14692 RVA: 0x0007AF80 File Offset: 0x00079180
		public static string GetString(FormattedMessage message, IUIProvider provider)
		{
			string text = "";
			bool flag = message.MessageId != null;
			if (flag)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				bool flag2 = message.Params != null;
				if (flag2)
				{
					foreach (KeyValuePair<string, object> keyValuePair in message.Params)
					{
						dictionary[keyValuePair.Key] = keyValuePair.Value.ToString();
					}
				}
				bool flag3 = message.MessageParams != null;
				if (flag3)
				{
					foreach (KeyValuePair<string, FormattedMessage> keyValuePair2 in message.MessageParams)
					{
						dictionary[keyValuePair2.Key] = FormattedMessageConverter.GetString(keyValuePair2.Value, provider);
					}
				}
				text = provider.GetText(message.MessageId, dictionary, true);
			}
			else
			{
				bool flag4 = message.RawText != null;
				if (flag4)
				{
					text = message.RawText;
				}
			}
			text = text.Replace("\t", "        ");
			bool flag5 = message.Children != null;
			if (flag5)
			{
				foreach (FormattedMessage message2 in message.Children)
				{
					text += FormattedMessageConverter.GetString(message2, provider);
				}
			}
			return text;
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x0007B124 File Offset: 0x00079324
		public static List<Label.LabelSpan> GetLabelSpans(FormattedMessage message, IUIProvider uiProvider, SpanStyle style = default(SpanStyle), bool allowFormatting = true)
		{
			List<Label.LabelSpan> list = new List<Label.LabelSpan>();
			FormattedMessageConverter.AppendLabelSpans(message, list, style, uiProvider, allowFormatting);
			return list;
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x0007B148 File Offset: 0x00079348
		public static List<Label.LabelSpan> GetLabelSpansFromMarkup(string message, SpanStyle style)
		{
			List<Label.LabelSpan> list = new List<Label.LabelSpan>();
			FormattedMessageConverter.AppendLabelSpansFromMarkup(message, list, style);
			return list;
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x0007B16C File Offset: 0x0007936C
		public static void AppendLabelSpansFromMarkup(string text, List<Label.LabelSpan> textSpans, SpanStyle style)
		{
			SpanStyle spanStyle = style;
			string text2 = "";
			Stack<SpanStyle> stack = new Stack<SpanStyle>();
			TextParser textParser = new TextParser(text, null);
			while (!textParser.IsEOF())
			{
				bool flag = textParser.Data[textParser.Cursor] == '<';
				if (flag)
				{
					FormattedMessageConverter.AddSpan(textSpans, text2, spanStyle);
					text2 = "";
					int cursor = textParser.Cursor;
					bool flag2 = textParser.TryEat("<b>");
					if (flag2)
					{
						stack.Push(spanStyle);
						spanStyle.IsBold = true;
						spanStyle.LastTag = "b";
					}
					else
					{
						bool flag3 = textParser.TryEat("</b>");
						if (flag3)
						{
							bool flag4 = spanStyle.LastTag != "b";
							if (flag4)
							{
								break;
							}
							spanStyle = stack.Pop();
						}
						else
						{
							bool flag5 = textParser.TryEat("<i>");
							if (flag5)
							{
								stack.Push(spanStyle);
								spanStyle.IsItalics = true;
								spanStyle.LastTag = "i";
							}
							else
							{
								bool flag6 = textParser.TryEat("</i>");
								if (flag6)
								{
									bool flag7 = spanStyle.LastTag != "i";
									if (flag7)
									{
										break;
									}
									spanStyle = stack.Pop();
								}
								else
								{
									bool flag8 = textParser.TryEat("<u>");
									if (flag8)
									{
										stack.Push(spanStyle);
										spanStyle.IsUnderlined = true;
										spanStyle.LastTag = "u";
									}
									else
									{
										bool flag9 = textParser.TryEat("</u>");
										if (flag9)
										{
											bool flag10 = spanStyle.LastTag != "u";
											if (flag10)
											{
												break;
											}
											spanStyle = stack.Pop();
										}
										else
										{
											bool flag11 = textParser.TryEat("<color is=\"#");
											if (flag11)
											{
												string text3 = textParser.Data.Substring(textParser.Cursor, 6);
												textParser.Cursor += 6;
												bool flag12 = !textParser.TryEat("\">");
												if (flag12)
												{
													textParser.Cursor = cursor;
													break;
												}
												byte r = byte.MaxValue;
												byte g = byte.MaxValue;
												byte b = byte.MaxValue;
												try
												{
													r = Convert.ToByte(text3.Substring(0, 2), 16);
													g = Convert.ToByte(text3.Substring(2, 2), 16);
													b = Convert.ToByte(text3.Substring(4, 2), 16);
												}
												catch
												{
												}
												stack.Push(spanStyle);
												spanStyle.Color = new UInt32Color?(UInt32Color.FromRGBA(r, g, b, byte.MaxValue));
												spanStyle.LastTag = "color";
											}
											else
											{
												bool flag13 = textParser.TryEat("</color>");
												if (flag13)
												{
													bool flag14 = spanStyle.LastTag != "color";
													if (flag14)
													{
														break;
													}
													spanStyle = stack.Pop();
												}
												else
												{
													bool flag15 = textParser.TryEat("<a href=\"");
													if (flag15)
													{
														string text4 = "";
														for (;;)
														{
															bool flag16 = textParser.TryEat("\">");
															if (flag16)
															{
																break;
															}
															text4 += textParser.Data[textParser.Cursor].ToString();
															textParser.Cursor++;
														}
														stack.Push(spanStyle);
														spanStyle.Link = text4;
														spanStyle.LastTag = "a";
													}
													else
													{
														bool flag17 = textParser.TryEat("</a>");
														if (!flag17)
														{
															break;
														}
														bool flag18 = spanStyle.LastTag != "a";
														if (flag18)
														{
															break;
														}
														spanStyle = stack.Pop();
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
				else
				{
					text2 += textParser.Data[textParser.Cursor].ToString();
					textParser.Cursor++;
				}
			}
			bool flag19 = !textParser.IsEOF() || stack.Count > 0;
			if (flag19)
			{
				text2 += textParser.Data.Substring(textParser.Cursor, textParser.Data.Length - textParser.Cursor);
			}
			FormattedMessageConverter.AddSpan(textSpans, text2, spanStyle);
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x0007B598 File Offset: 0x00079798
		private static void AppendLabelSpans(FormattedMessage message, List<Label.LabelSpan> textSpans, SpanStyle style, IUIProvider uiProvider, bool allowFormatting = true)
		{
			bool flag = message.Link != null;
			if (flag)
			{
				style.Link = message.Link;
			}
			if (allowFormatting)
			{
				bool flag2 = message.Color != null && message.Color.StartsWith("#");
				if (flag2)
				{
					string color = message.Color;
					int length = color.Length;
					int num = length;
					if (num != 4)
					{
						if (num == 7)
						{
							try
							{
								byte r = Convert.ToByte(color.Substring(1, 2), 16);
								byte g = Convert.ToByte(color.Substring(3, 2), 16);
								byte b = Convert.ToByte(color.Substring(5, 2), 16);
								style.Color = new UInt32Color?(UInt32Color.FromRGBA(r, g, b, byte.MaxValue));
							}
							catch
							{
							}
						}
					}
					else
					{
						try
						{
							byte r2 = Convert.ToByte(color.Substring(1, 1) + color.Substring(1, 1), 16);
							byte g2 = Convert.ToByte(color.Substring(2, 1) + color.Substring(2, 1), 16);
							byte b2 = Convert.ToByte(color.Substring(3, 1) + color.Substring(3, 1), 16);
							style.Color = new UInt32Color?(UInt32Color.FromRGBA(r2, g2, b2, byte.MaxValue));
						}
						catch
						{
						}
					}
				}
				bool flag3 = message.Bold != null;
				if (flag3)
				{
					style.IsBold = message.Bold.Value;
				}
				bool flag4 = message.Italic != null;
				if (flag4)
				{
					style.IsItalics = message.Italic.Value;
				}
				bool flag5 = message.Underlined != null;
				if (flag5)
				{
					style.IsUnderlined = message.Underlined.Value;
				}
			}
			string text = null;
			bool flag6 = message.MessageId != null;
			if (flag6)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				bool flag7 = message.Params != null;
				if (flag7)
				{
					foreach (KeyValuePair<string, object> keyValuePair in message.Params)
					{
						dictionary[keyValuePair.Key] = keyValuePair.Value.ToString();
					}
				}
				bool flag8 = message.MessageParams != null;
				if (flag8)
				{
					foreach (KeyValuePair<string, FormattedMessage> keyValuePair2 in message.MessageParams)
					{
						dictionary[keyValuePair2.Key] = FormattedMessageConverter.GetString(keyValuePair2.Value, uiProvider);
					}
				}
				text = uiProvider.GetText(message.MessageId, dictionary, true);
			}
			else
			{
				bool flag9 = message.RawText != null;
				if (flag9)
				{
					text = message.RawText;
				}
			}
			bool flag10 = text != null;
			if (flag10)
			{
				text = text.Replace("\t", "        ");
				bool flag11 = allowFormatting && message.MarkupEnabled;
				if (flag11)
				{
					FormattedMessageConverter.AppendLabelSpansFromMarkup(text, textSpans, style);
				}
				else
				{
					ChatTagType chatTagType;
					bool flag12 = message.Params != null && message.Params.ContainsKey("tagType") && Enum.TryParse<ChatTagType>(message.Params["tagType"].ToString(), out chatTagType);
					if (flag12)
					{
						Interface @interface = uiProvider as Interface;
						bool flag13 = @interface != null;
						if (flag13)
						{
							SpanStyle style2 = style;
							ChatTagType chatTagType2 = chatTagType;
							ChatTagType chatTagType3 = chatTagType2;
							if (chatTagType3 != null)
							{
								FormattedMessageConverter.AddSpan(textSpans, string.Format("[Unrecognized tag type: {0}]", chatTagType), style2);
							}
							else
							{
								GameInstance instance = @interface.InGameView.InGame.Instance;
								object arg = message.Params["id"];
								ClientItemBase item = instance.ItemLibraryModule.GetItem(message.Params["id"].ToString());
								string text2 = @interface.GetText(string.Format("items.{0}.name", arg), null, true);
								ClientItemQuality[] itemQualities = instance.ServerSettings.ItemQualities;
								style2.Color = new UInt32Color?(itemQualities[item.QualityIndex].TextColor);
								FormattedMessageConverter.AddSpan(textSpans, "[" + text2 + "]", style2, message.Params);
							}
						}
						else
						{
							FormattedMessageConverter.AddSpan(textSpans, string.Format("[Unrecognized tag type: {0}]", chatTagType), style);
						}
					}
					else
					{
						FormattedMessageConverter.AddSpan(textSpans, text, style);
					}
				}
			}
			bool flag14 = message.Children != null;
			if (flag14)
			{
				foreach (FormattedMessage message2 in message.Children)
				{
					FormattedMessageConverter.AppendLabelSpans(message2, textSpans, style, uiProvider, true);
				}
			}
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x0007BA9C File Offset: 0x00079C9C
		private static void AddSpan(List<Label.LabelSpan> spans, string text, SpanStyle style)
		{
			FormattedMessageConverter.AddSpan(spans, text, style, null);
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x0007BAA8 File Offset: 0x00079CA8
		private static void AddSpan(List<Label.LabelSpan> spans, string text, SpanStyle style, Dictionary<string, object> parameters)
		{
			bool flag = text.Length == 0;
			if (!flag)
			{
				spans.Add(new Label.LabelSpan
				{
					Text = text,
					Color = style.Color,
					IsBold = style.IsBold,
					IsItalics = style.IsItalics,
					IsUnderlined = style.IsUnderlined,
					IsUppercase = style.IsUppercase,
					Link = style.Link,
					Params = parameters
				});
			}
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x0007BB28 File Offset: 0x00079D28
		public static string GetString(FormattedMessage message, Interface @interface)
		{
			bool flag = message.MessageId == null;
			string result;
			if (flag)
			{
				result = message.RawText;
			}
			else
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				bool flag2 = message.Params != null;
				if (flag2)
				{
					foreach (KeyValuePair<string, object> keyValuePair in message.Params)
					{
						dictionary[keyValuePair.Key] = keyValuePair.Value.ToString();
					}
				}
				bool flag3 = message.MessageParams != null;
				if (flag3)
				{
					foreach (KeyValuePair<string, FormattedMessage> keyValuePair2 in message.MessageParams)
					{
						dictionary[keyValuePair2.Key] = FormattedMessageConverter.GetString(keyValuePair2.Value, @interface);
					}
				}
				string text = @interface.GetText(message.MessageId, dictionary, true);
				bool flag4 = message.Children != null;
				if (flag4)
				{
					foreach (FormattedMessage message2 in message.Children)
					{
						text += FormattedMessageConverter.GetString(message2, @interface);
					}
				}
				result = text;
			}
			return result;
		}
	}
}
