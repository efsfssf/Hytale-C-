using System;

namespace HytaleClient.Interface.UI.Markup
{
	// Token: 0x02000848 RID: 2120
	public class TextParserSpan
	{
		// Token: 0x06003AFD RID: 15101 RVA: 0x0008A10C File Offset: 0x0008830C
		public TextParserSpan(int start, int end, TextParser parser)
		{
			this.Parser = parser;
			this.Start = start;
			this.End = end;
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x0008A12C File Offset: 0x0008832C
		public void GetContext(int linesOfContext, out int startLine, out int startColumn, out string before, out string inside, out string after)
		{
			this.Parser.GetCursorLocation(this.Start, out startLine, out startColumn);
			int num = this.Start - startColumn;
			bool flag = num > 0;
			if (flag)
			{
				for (int i = 0; i < linesOfContext; i++)
				{
					int num2 = this.Parser.Data.LastIndexOf("\n", num - 1);
					bool flag2 = num2 == -1;
					if (flag2)
					{
						break;
					}
					num = num2;
				}
			}
			int num3 = this.Parser.Data.IndexOf("\n", this.End);
			bool flag3 = num3 == -1;
			if (flag3)
			{
				num3 = this.Parser.Data.Length;
			}
			else
			{
				for (int j = 0; j < linesOfContext; j++)
				{
					int num4 = this.Parser.Data.IndexOf("\n", num3 + 1);
					bool flag4 = num4 == -1;
					if (flag4)
					{
						break;
					}
					num3 = num4;
				}
			}
			before = this.Parser.Data.Substring(num, this.Start - num);
			while (before.StartsWith("\n"))
			{
				before = before.Substring(1);
			}
			inside = this.Parser.Data.Substring(this.Start, this.End - this.Start);
			after = this.Parser.Data.Substring(this.End, num3 - this.End).TrimEnd(Array.Empty<char>());
		}

		// Token: 0x04001B2E RID: 6958
		public readonly TextParser Parser;

		// Token: 0x04001B2F RID: 6959
		public readonly int Start;

		// Token: 0x04001B30 RID: 6960
		public readonly int End;
	}
}
