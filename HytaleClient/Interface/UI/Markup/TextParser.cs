using System;
using System.Collections.Generic;
using System.Globalization;

namespace HytaleClient.Interface.UI.Markup
{
	// Token: 0x02000847 RID: 2119
	public class TextParser
	{
		// Token: 0x06003AEE RID: 15086 RVA: 0x00089A6C File Offset: 0x00087C6C
		protected int PushSpan()
		{
			this._spanStack.Push(this.Cursor);
			return this.Cursor;
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x00089A98 File Offset: 0x00087C98
		protected TextParserSpan PopSpan(int startCursor)
		{
			bool flag = startCursor != this._spanStack.Pop();
			if (flag)
			{
				throw new Exception("Invalid span stacking");
			}
			return new TextParserSpan(startCursor, this.Cursor, this);
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x00089AD7 File Offset: 0x00087CD7
		public TextParser(string data, string sourcePath)
		{
			this.Data = data.Replace("\r\n", "\n");
			this.SourcePath = sourcePath;
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x00089B09 File Offset: 0x00087D09
		public bool IsEOF()
		{
			return this.Cursor >= this.Data.Length;
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x00089B24 File Offset: 0x00087D24
		public void GetCursorLocation(int cursor, out int line, out int column)
		{
			line = 0;
			column = 0;
			for (int i = 0; i < cursor; i++)
			{
				bool flag = this.Data[i] == '\n';
				if (flag)
				{
					line++;
					column = 0;
				}
				else
				{
					column++;
				}
			}
		}

		// Token: 0x06003AF3 RID: 15091 RVA: 0x00089B74 File Offset: 0x00087D74
		public void GetCursorInfo(out int line, out int column, out string context)
		{
			this.GetCursorLocation(this.Cursor, out line, out column);
			int num = this.Cursor - column;
			int num2 = this.Data.IndexOf('\n', num);
			bool flag = num2 == -1;
			if (flag)
			{
				num2 = this.Data.Length;
			}
			context = this.Data.Substring(num, num2 - num);
		}

		// Token: 0x06003AF4 RID: 15092 RVA: 0x00089BCF File Offset: 0x00087DCF
		public bool Peek(char c)
		{
			return !this.IsEOF() && this.Data[this.Cursor] == c;
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x00089BF0 File Offset: 0x00087DF0
		public bool Peek(string text)
		{
			return this.Cursor + text.Length < this.Data.Length && this.Data.Substring(this.Cursor, text.Length) == text;
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x00089C2C File Offset: 0x00087E2C
		public void Eat(char c)
		{
			int startCursor = this.PushSpan();
			bool flag = this.IsEOF();
			if (flag)
			{
				throw new TextParser.TextParserException(string.Format("Expected {0}, found end of file", c), this.PopSpan(startCursor));
			}
			bool flag2 = this.Data[this.Cursor] != c;
			if (flag2)
			{
				throw new TextParser.TextParserException(string.Format("Expected {0}, found {1}", c, this.Data[this.Cursor]), this.PopSpan(startCursor));
			}
			this.Cursor++;
			this.PopSpan(startCursor);
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x00089CCC File Offset: 0x00087ECC
		public bool TryEat(char c)
		{
			bool flag = this.IsEOF() || this.Data[this.Cursor] != c;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.Cursor++;
				result = true;
			}
			return result;
		}

		// Token: 0x06003AF8 RID: 15096 RVA: 0x00089D18 File Offset: 0x00087F18
		public void Eat(string text)
		{
			int startCursor = this.PushSpan();
			bool flag = this.IsEOF();
			if (flag)
			{
				throw new TextParser.TextParserException("Expected " + text + ", found end of file", this.PopSpan(startCursor));
			}
			string text2 = this.Data.Substring(this.Cursor, text.Length);
			bool flag2 = text2 != text;
			if (flag2)
			{
				throw new TextParser.TextParserException("Expected " + text + ", found " + text2, this.PopSpan(startCursor));
			}
			this.Cursor += text.Length;
			this.PopSpan(startCursor);
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x00089DB0 File Offset: 0x00087FB0
		public bool TryEat(string text)
		{
			bool flag = this.Cursor + text.Length > this.Data.Length || this.Data.Substring(this.Cursor, text.Length) != text;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.Cursor += text.Length;
				result = true;
			}
			return result;
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x00089E18 File Offset: 0x00088018
		public void SkipUntil(char c)
		{
			int startCursor = this.PushSpan();
			for (;;)
			{
				bool flag = this.IsEOF();
				if (flag)
				{
					break;
				}
				bool flag2 = this.Data[this.Cursor] == c;
				if (flag2)
				{
					goto Block_2;
				}
				this.Cursor++;
			}
			throw new TextParser.TextParserException(string.Format("Encountered end of file while looking for {0}", c), this.PopSpan(startCursor));
			Block_2:
			this.PopSpan(startCursor);
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x00089E8C File Offset: 0x0008808C
		public int EatInteger()
		{
			int startCursor = this.PushSpan();
			bool flag = this.IsEOF();
			if (flag)
			{
				throw new TextParser.TextParserException("Expected integer, found end of file", this.PopSpan(startCursor));
			}
			string text = "";
			while (!this.IsEOF())
			{
				char c = this.Data[this.Cursor];
				bool flag2 = c >= '0' && c <= '9';
				if (flag2)
				{
					text += c.ToString();
					this.Cursor++;
				}
				else
				{
					bool flag3 = c == '-' && text.Length == 0;
					if (flag3)
					{
						text += c.ToString();
						this.Cursor++;
					}
					else
					{
						bool flag4 = text.Length == 0;
						if (flag4)
						{
							throw new TextParser.TextParserException(string.Format("Expected integer, found {0}", c), this.PopSpan(startCursor));
						}
						break;
					}
				}
			}
			this.PopSpan(startCursor);
			return int.Parse(text);
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x00089FA0 File Offset: 0x000881A0
		public decimal EatDecimal()
		{
			int startCursor = this.PushSpan();
			bool flag = this.IsEOF();
			if (flag)
			{
				throw new TextParser.TextParserException("Expected decimal, found end of file", this.PopSpan(startCursor));
			}
			bool flag2 = false;
			string text = "";
			while (!this.IsEOF())
			{
				char c = this.Data[this.Cursor];
				bool flag3 = c >= '0' && c <= '9';
				if (flag3)
				{
					text += c.ToString();
					this.Cursor++;
				}
				else
				{
					bool flag4 = c == '-' && text.Length == 0;
					if (flag4)
					{
						text += c.ToString();
						this.Cursor++;
					}
					else
					{
						bool flag5 = c == '.' && text.Length > 0 && !flag2;
						if (flag5)
						{
							text += c.ToString();
							flag2 = true;
							this.Cursor++;
						}
						else
						{
							bool flag6 = text.Length == 0 || text == "-";
							if (flag6)
							{
								throw new TextParser.TextParserException(string.Format("Expected decimal, found {0}", c), this.PopSpan(startCursor));
							}
							break;
						}
					}
				}
			}
			this.PopSpan(startCursor);
			return decimal.Parse(text, CultureInfo.InvariantCulture);
		}

		// Token: 0x04001B2A RID: 6954
		public readonly string Data;

		// Token: 0x04001B2B RID: 6955
		public int Cursor;

		// Token: 0x04001B2C RID: 6956
		protected readonly Stack<int> _spanStack = new Stack<int>();

		// Token: 0x04001B2D RID: 6957
		public readonly string SourcePath;

		// Token: 0x02000D21 RID: 3361
		public class TextParserException : Exception
		{
			// Token: 0x17001427 RID: 5159
			// (get) Token: 0x060064DD RID: 25821 RVA: 0x002101F0 File Offset: 0x0020E3F0
			public override string Message
			{
				get
				{
					int num;
					int num2;
					string text;
					string text2;
					string text3;
					this.Span.GetContext(3, out num, out num2, out text, out text2, out text3);
					return string.Format("Failed to parse file {0} ({1}:{2}) – {3}", new object[]
					{
						this.Span.Parser.SourcePath,
						num + 1,
						num2 + 1,
						this.RawMessage
					});
				}
			}

			// Token: 0x060064DE RID: 25822 RVA: 0x0021025D File Offset: 0x0020E45D
			public TextParserException(string message, TextParserSpan span) : base(message)
			{
				this.Span = span;
				this.RawMessage = message;
			}

			// Token: 0x060064DF RID: 25823 RVA: 0x00210276 File Offset: 0x0020E476
			public TextParserException(string message, TextParserSpan span, Exception inner) : base(message, inner)
			{
				this.Span = span;
				this.RawMessage = message;
			}

			// Token: 0x040040E4 RID: 16612
			public readonly TextParserSpan Span;

			// Token: 0x040040E5 RID: 16613
			public readonly string RawMessage;
		}
	}
}
