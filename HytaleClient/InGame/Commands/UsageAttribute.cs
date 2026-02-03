using System;

namespace HytaleClient.InGame.Commands
{
	// Token: 0x020008EA RID: 2282
	internal class UsageAttribute : Attribute
	{
		// Token: 0x06004380 RID: 17280 RVA: 0x000D683E File Offset: 0x000D4A3E
		public UsageAttribute(string command, params string[] usage)
		{
			this.Command = command;
			this.Usage = usage;
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x000D6858 File Offset: 0x000D4A58
		public override string ToString()
		{
			string text = "Usage:";
			bool flag = this.Usage.Length == 0;
			if (flag)
			{
				text = text + "\n  ." + this.Command;
			}
			else
			{
				foreach (string text2 in this.Usage)
				{
					text = string.Concat(new string[]
					{
						text,
						"\n  .",
						this.Command,
						" ",
						text2
					});
				}
			}
			return text;
		}

		// Token: 0x04002141 RID: 8513
		public readonly string Command;

		// Token: 0x04002142 RID: 8514
		public readonly string[] Usage;
	}
}
