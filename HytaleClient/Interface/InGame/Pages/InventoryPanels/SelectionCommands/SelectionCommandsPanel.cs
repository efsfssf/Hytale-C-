using System;
using System.Collections.Generic;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels.SelectionCommands
{
	// Token: 0x020008A8 RID: 2216
	internal class SelectionCommandsPanel : Panel
	{
		// Token: 0x170010BF RID: 4287
		// (get) Token: 0x06004032 RID: 16434 RVA: 0x000B821D File Offset: 0x000B641D
		// (set) Token: 0x06004033 RID: 16435 RVA: 0x000B8225 File Offset: 0x000B6425
		public Group Panel { get; private set; }

		// Token: 0x06004034 RID: 16436 RVA: 0x000B822E File Offset: 0x000B642E
		public SelectionCommandsPanel(InGameView inGameView, Element parent = null) : base(inGameView, parent)
		{
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x000B8268 File Offset: 0x000B6468
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/Inventory/BuilderTools/SelectionCommandsPanel.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this.Panel = uifragment.Get<Group>("Panel");
			TextButton textButton = uifragment.Get<TextButton>("ExecuteCommandButton");
			textButton.Activating = delegate()
			{
				this.ExecuteCommand();
			};
			this._body = uifragment.Get<Group>("Body");
			this._modesDropdown = uifragment.Get<DropdownBox>("ModesDropdown");
			List<DropdownBox.DropdownEntryInfo> list = new List<DropdownBox.DropdownEntryInfo>();
			foreach (string text in this.SelectionCommands)
			{
				list.Add(new DropdownBox.DropdownEntryInfo(text, text, false));
			}
			this._modesDropdown.Entries = list;
			this._modesDropdown.Value = this.SelectionCommands[0];
			this._modesDropdown.ValueChanged = delegate()
			{
				this.SelectCommand();
			};
			this.SelectCommand();
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x000B836C File Offset: 0x000B656C
		private void ExecuteCommand()
		{
			bool flag = this._currentCommand == null;
			if (!flag)
			{
				string chatCommand = this._currentCommand.GetChatCommand();
				this._inGameView.InGame.SendChatMessageOrExecuteCommand(chatCommand);
			}
		}

		// Token: 0x06004037 RID: 16439 RVA: 0x000B83A8 File Offset: 0x000B65A8
		private void SelectCommand()
		{
			this._body.Clear();
			this._currentCommand = this.GetSelectCommand();
			bool flag = this._currentCommand == null;
			if (!flag)
			{
				this._currentCommand.Build();
				this._body.Add(this._currentCommand, -1);
				base.Layout(null, true);
			}
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x000B8410 File Offset: 0x000B6610
		private BaseSelectionCommand GetSelectCommand()
		{
			bool flag = this._modesDropdown.Value == "SET";
			BaseSelectionCommand result;
			if (flag)
			{
				result = new SetCommand(this._inGameView, this.Desktop, null);
			}
			else
			{
				bool flag2 = this._modesDropdown.Value == "WALL";
				if (flag2)
				{
					result = new WallCommand(this._inGameView, this.Desktop, null);
				}
				else
				{
					bool flag3 = this._modesDropdown.Value == "FILL";
					if (flag3)
					{
						result = new FillCommand(this._inGameView, this.Desktop, null);
					}
					else
					{
						bool flag4 = this._modesDropdown.Value == "REPLACE";
						if (flag4)
						{
							result = new ReplaceCommand(this._inGameView, this.Desktop, null);
						}
						else
						{
							result = null;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04001E97 RID: 7831
		private readonly string[] SelectionCommands = new string[]
		{
			"SET",
			"WALL",
			"FILL",
			"REPLACE"
		};

		// Token: 0x04001E98 RID: 7832
		private DropdownBox _modesDropdown;

		// Token: 0x04001E99 RID: 7833
		private Group _body;

		// Token: 0x04001E9A RID: 7834
		public const string SelectionToolId = "EditorTool_PlaySelection";

		// Token: 0x04001E9B RID: 7835
		private BaseSelectionCommand _currentCommand;
	}
}
