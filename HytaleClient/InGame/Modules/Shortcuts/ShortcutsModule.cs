using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using HytaleClient.Data.Items;
using HytaleClient.InGame.Commands;

namespace HytaleClient.InGame.Modules.Shortcuts
{
	// Token: 0x02000903 RID: 2307
	internal class ShortcutsModule : Module
	{
		// Token: 0x06004543 RID: 17731 RVA: 0x000F14DC File Offset: 0x000EF6DC
		public ShortcutsModule(GameInstance gameInstance) : base(gameInstance)
		{
			this._macros = this._gameInstance.App.Settings.ShortcutSettings.MacroShortcuts;
			this._keybinds = this._gameInstance.App.Settings.ShortcutSettings.KeybindShortcuts;
			foreach (string key in this._keybinds.Keys)
			{
				bool flag = this._keybinds[key] == null;
				if (flag)
				{
					this._keybinds.Remove(key);
				}
			}
			this._gameInstance.RegisterCommand("macro", new GameInstance.Command(this.MacroCommand));
			this._gameInstance.RegisterCommand("keybind", new GameInstance.Command(this.KeybindCommand));
		}

		// Token: 0x06004544 RID: 17732 RVA: 0x000F15E8 File Offset: 0x000EF7E8
		public void Update()
		{
			foreach (KeyValuePair<string, KeybindShortcut> keyValuePair in this._keybinds)
			{
				bool flag = keyValuePair.Value.IsActive(this._gameInstance.Input);
				if (flag)
				{
					bool flag2 = keyValuePair.Value.ConsumeKeybinds(this._gameInstance.Input);
					if (flag2)
					{
						this.ExecuteCommand(keyValuePair.Value.Command, null);
					}
				}
			}
			bool flag3 = this._commandQueue.Count > 0;
			if (flag3)
			{
				string command = this._commandQueue[0];
				this._commandQueue.RemoveAt(0);
				this._gameInstance.Chat.SendCommand(command, Array.Empty<object>());
			}
			this._executionCount = 0;
		}

		// Token: 0x06004545 RID: 17733 RVA: 0x000F16D8 File Offset: 0x000EF8D8
		[Usage("macro", new string[]
		{
			"[name] OR ..[name]",
			"add [name] [command]",
			"remove [name]",
			"clear",
			"list"
		})]
		[Description("Manage macros")]
		public void MacroCommand(string[] args)
		{
			bool flag = args.Length == 0;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "add"))
			{
				if (!(a == "remove"))
				{
					if (!(a == "clear"))
					{
						if (!(a == "list"))
						{
							this.ExecuteMacro(args);
						}
						else
						{
							bool flag2 = args.Length != 1;
							if (flag2)
							{
								throw new InvalidCommandUsage();
							}
							this._gameInstance.Chat.Log("Saved Macros:");
							bool flag3 = this._macros.Count > 0;
							if (flag3)
							{
								foreach (KeyValuePair<string, MacroShortcut> keyValuePair in this._macros)
								{
									this._gameInstance.Chat.Log(keyValuePair.Value.ToString());
								}
							}
							else
							{
								this._gameInstance.Chat.Log("None");
							}
						}
					}
					else
					{
						bool flag4 = args.Length != 1;
						if (flag4)
						{
							throw new InvalidCommandUsage();
						}
						this._macros.Clear();
						this._gameInstance.Chat.Log("Cleared all macros.");
					}
				}
				else
				{
					bool flag5 = args.Length != 2;
					if (flag5)
					{
						throw new InvalidCommandUsage();
					}
					string text2 = args[1];
					bool flag6 = !this._macros.ContainsKey(text2);
					if (flag6)
					{
						this._gameInstance.Chat.Error("Unable to find macro '" + text2 + "'");
					}
					else
					{
						this._macros.Remove(text2);
						this._gameInstance.Chat.Log("Removed macro shortcut '" + text2 + "'");
					}
				}
			}
			else
			{
				bool flag7 = args.Length < 3;
				if (flag7)
				{
					throw new InvalidCommandUsage();
				}
				string text3 = ShortcutsModule.ParseCommandText(args, 2).Trim(new char[]
				{
					';'
				});
				bool flag8 = text3.Length == 0;
				if (flag8)
				{
					throw new InvalidCommandUsage();
				}
				string name = args[1];
				this.AddMacro(name, text3);
			}
		}

		// Token: 0x06004546 RID: 17734 RVA: 0x000F1920 File Offset: 0x000EFB20
		[Usage("keybind", new string[]
		{
			"add [keys] [command]",
			"remove [keys]",
			"clear",
			"list"
		})]
		[Description("Modify keybinds")]
		public void KeybindCommand(string[] args)
		{
			bool flag = args.Length == 0;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "add"))
			{
				if (!(a == "remove"))
				{
					if (!(a == "clear"))
					{
						if (!(a == "list"))
						{
							throw new InvalidCommandUsage();
						}
						bool flag2 = args.Length != 1;
						if (flag2)
						{
							throw new InvalidCommandUsage();
						}
						this._gameInstance.Chat.Log("Saved Keybinds:");
						bool flag3 = this._keybinds.Count > 0;
						if (flag3)
						{
							foreach (KeyValuePair<string, KeybindShortcut> keyValuePair in this._keybinds)
							{
								this._gameInstance.Chat.Log(keyValuePair.Value.ToString());
							}
						}
						else
						{
							this._gameInstance.Chat.Log("None");
						}
					}
					else
					{
						bool flag4 = args.Length != 1;
						if (flag4)
						{
							throw new InvalidCommandUsage();
						}
						this._keybinds.Clear();
						this._gameInstance.Chat.Log("Cleared all keybinds.");
					}
				}
				else
				{
					bool flag5 = args.Length != 2;
					if (flag5)
					{
						throw new InvalidCommandUsage();
					}
					string text2 = string.Join("", new string[]
					{
						Regex.Replace(args[1], "\\s+", "")
					});
					string str = "";
					try
					{
						str = KeybindShortcut.FixKeyList(text2);
					}
					catch (Exception ex)
					{
						this._gameInstance.Chat.Error("Unable to find matching key with name '" + ex.Message + "'");
						return;
					}
					bool flag6 = !this._keybinds.ContainsKey(text2);
					if (flag6)
					{
						this._gameInstance.Chat.Error("Unable to find keybind with keys [ " + str + " ]");
					}
					else
					{
						this._keybinds.Remove(text2);
						this._gameInstance.Chat.Log("Removed keybind shortcut for keys [ " + str + " ]");
					}
				}
			}
			else
			{
				bool flag7 = args.Length < 3;
				if (flag7)
				{
					throw new InvalidCommandUsage();
				}
				string text3 = ShortcutsModule.ParseCommandText(args, 2);
				bool flag8 = text3.Length == 0;
				if (flag8)
				{
					throw new InvalidCommandUsage();
				}
				string text4 = string.Join("", new string[]
				{
					Regex.Replace(args[1], "\\s+", "")
				});
				KeybindShortcut value;
				try
				{
					value = new KeybindShortcut(text4, text3);
				}
				catch (Exception ex2)
				{
					this._gameInstance.Chat.Error("Unable to find matching key with name '" + ex2.Message + "'");
					return;
				}
				bool flag9 = this._keybinds.ContainsKey(text4);
				if (flag9)
				{
					this._gameInstance.Chat.Error("A keybind already exists for the keys [ " + text4 + " ]");
				}
				else
				{
					this._keybinds.Add(text4, value);
					this._gameInstance.Chat.Log(string.Concat(new string[]
					{
						"Keybind [ ",
						text4,
						" ] set to command '",
						text3,
						"'"
					}));
				}
			}
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x000F1CB4 File Offset: 0x000EFEB4
		public void AddMacro(string name, string command)
		{
			bool flag = this._macros.ContainsKey(name);
			if (flag)
			{
				this._gameInstance.Chat.Error("A macro already exists with the name '" + name + "'");
			}
			else
			{
				this._macros.Add(name, new MacroShortcut(name, command));
				this._gameInstance.Chat.Log(string.Concat(new string[]
				{
					"Macro '",
					name,
					"' successfully set to command '",
					command,
					"'"
				}));
			}
		}

		// Token: 0x06004548 RID: 17736 RVA: 0x000F1D48 File Offset: 0x000EFF48
		public void ExecuteMacro(string[] args)
		{
			MacroShortcut macroShortcut;
			bool flag = !this._macros.TryGetValue(args[0], out macroShortcut);
			if (flag)
			{
				this._gameInstance.Chat.Error("Unable to find macro with name: " + args[0]);
			}
			else
			{
				bool flag2 = this._executionCount >= 50;
				if (flag2)
				{
					this._gameInstance.Chat.Error("Shortcut execution stopped after " + 50.ToString() + " cycles!");
					this._gameInstance.Chat.Error("This may be because you have an infinite loop in the commands run. Please check shortcuts and try again.");
				}
				else
				{
					this._executionCount++;
					string[] commands = macroShortcut.GetCommands();
					string[] args2 = Enumerable.ToArray<string>(Enumerable.Skip<string>(args, 1));
					for (int i = 0; i < commands.Length; i++)
					{
						this.ExecuteCommand(commands[i], args2);
					}
				}
			}
		}

		// Token: 0x06004549 RID: 17737 RVA: 0x000F1E30 File Offset: 0x000F0030
		private void ExecuteCommand(string command, string[] args = null)
		{
			command = command.Trim();
			bool flag = !this.ReplaceCommandArgs(ref command, args);
			if (!flag)
			{
				bool flag2 = command.StartsWith(".");
				if (flag2)
				{
					this._gameInstance.ExecuteCommand(command);
				}
				else
				{
					this._commandQueue.Add(command);
				}
			}
		}

		// Token: 0x0600454A RID: 17738 RVA: 0x000F1E84 File Offset: 0x000F0084
		private bool ReplaceCommandArgs(ref string command, string[] args)
		{
			bool flag = command.IndexOf("%x") > -1 || command.IndexOf("%y") > -1 || command.IndexOf("%z") > -1;
			if (flag)
			{
				ShortcutsModule.ReplacePositionArg(ref command, "%x", (int)this._gameInstance.LocalPlayer.Position.X);
				ShortcutsModule.ReplacePositionArg(ref command, "%y", (int)this._gameInstance.LocalPlayer.Position.Y);
				ShortcutsModule.ReplacePositionArg(ref command, "%z", (int)this._gameInstance.LocalPlayer.Position.Z);
			}
			bool flag2 = command.IndexOf("%chunkx") > -1 || command.IndexOf("%chunky") > -1 || command.IndexOf("%chunkz") > -1;
			if (flag2)
			{
				ShortcutsModule.ReplacePositionArg(ref command, "%chunkx", (int)this._gameInstance.LocalPlayer.Position.X >> 5);
				ShortcutsModule.ReplacePositionArg(ref command, "%chunky", (int)this._gameInstance.LocalPlayer.Position.Y >> 5);
				ShortcutsModule.ReplacePositionArg(ref command, "%chunkz", (int)this._gameInstance.LocalPlayer.Position.Z >> 5);
			}
			bool flag3 = command.IndexOf("%hitx") > -1 || command.IndexOf("%hity") > -1 || command.IndexOf("%hitz") > -1 || command.IndexOf("%hitblock") > -1;
			if (flag3)
			{
				bool hasFoundTargetBlock = this._gameInstance.InteractionModule.HasFoundTargetBlock;
				if (!hasFoundTargetBlock)
				{
					this._gameInstance.Chat.Error("Unable to replace all parameters - no blocks within range.");
					return false;
				}
				HitDetection.RaycastHit targetBlockHit = this._gameInstance.InteractionModule.TargetBlockHit;
				int num = (int)Math.Floor((double)targetBlockHit.BlockPosition.X);
				int num2 = (int)Math.Floor((double)targetBlockHit.BlockPosition.Y);
				int num3 = (int)Math.Floor((double)targetBlockHit.BlockPosition.Z);
				ShortcutsModule.ReplacePositionArg(ref command, "%hitx", num);
				ShortcutsModule.ReplacePositionArg(ref command, "%hity", num2);
				ShortcutsModule.ReplacePositionArg(ref command, "%hitz", num3);
				int block = this._gameInstance.MapModule.GetBlock(num, num2, num3, int.MaxValue);
				string name = this._gameInstance.MapModule.ClientBlockTypes[block].Name;
				ShortcutsModule.ReplaceParameterArg(ref command, "%hitblock", name);
			}
			bool flag4 = command.IndexOf("%activeitem") > -1;
			if (flag4)
			{
				ClientItemStack activeItem = this._gameInstance.InventoryModule.GetActiveItem();
				string val = (activeItem == null) ? "Empty" : activeItem.Id.Split(new char[]
				{
					'.'
				})[1];
				ShortcutsModule.ReplaceParameterArg(ref command, "%activeitem", val);
			}
			ShortcutsModule.ReplaceParameterArg(ref command, "%name", this._gameInstance.LocalPlayer.Name);
			bool flag5 = args != null;
			if (flag5)
			{
				for (int i = 0; i < args.Length; i++)
				{
					ShortcutsModule.ReplaceParameterArg(ref command, "%" + (i + 1).ToString(), args[i]);
				}
			}
			return true;
		}

		// Token: 0x0600454B RID: 17739 RVA: 0x000F21DC File Offset: 0x000F03DC
		private static void ReplacePositionArg(ref string command, string arg, int val)
		{
			int num = command.IndexOf(arg);
			bool flag = num > -1;
			if (flag)
			{
				int length = arg.Length;
				int num2 = command.IndexOf(' ', num);
				int num3 = (num2 - num - length > 0 && num2 > -1) ? int.Parse(command.Substring(num + length, num2 - num - length), CultureInfo.InvariantCulture) : 0;
				int num4 = num3 + val;
				command = command.Substring(0, num) + num4.ToString() + ((num2 > -1) ? command.Substring(num2) : "");
			}
		}

		// Token: 0x0600454C RID: 17740 RVA: 0x000F2269 File Offset: 0x000F0469
		private static void ReplaceParameterArg(ref string command, string arg, string val)
		{
			command = command.Replace(arg, val);
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x000F2278 File Offset: 0x000F0478
		private static string ParseCommandText(string[] args, int startIndex)
		{
			return string.Join(" ", Enumerable.Skip<string>(args, startIndex));
		}

		// Token: 0x040022C0 RID: 8896
		private readonly Dictionary<string, MacroShortcut> _macros;

		// Token: 0x040022C1 RID: 8897
		private readonly Dictionary<string, KeybindShortcut> _keybinds;

		// Token: 0x040022C2 RID: 8898
		private List<string> _commandQueue = new List<string>();

		// Token: 0x040022C3 RID: 8899
		private const int MaxTickExecution = 50;

		// Token: 0x040022C4 RID: 8900
		private int _executionCount = 0;
	}
}
