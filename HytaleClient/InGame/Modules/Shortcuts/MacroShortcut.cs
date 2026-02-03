using System;

namespace HytaleClient.InGame.Modules.Shortcuts
{
	// Token: 0x02000901 RID: 2305
	internal class MacroShortcut : Shortcut
	{
		// Token: 0x06004539 RID: 17721 RVA: 0x000F13EA File Offset: 0x000EF5EA
		public MacroShortcut(string name, string command) : base(name, command)
		{
			this._commands = MacroShortcut.ParseCommands(command);
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x000F1404 File Offset: 0x000EF604
		public string[] GetCommands()
		{
			return this._commands;
		}

		// Token: 0x0600453B RID: 17723 RVA: 0x000F141C File Offset: 0x000EF61C
		private static string[] ParseCommands(string command)
		{
			return command.Split(new char[]
			{
				';'
			});
		}

		// Token: 0x0600453C RID: 17724 RVA: 0x000F143F File Offset: 0x000EF63F
		public override string ToString()
		{
			return base.Name + " - " + base.Command;
		}

		// Token: 0x040022BC RID: 8892
		private const char CommandSeperator = ';';

		// Token: 0x040022BD RID: 8893
		private string[] _commands;
	}
}
