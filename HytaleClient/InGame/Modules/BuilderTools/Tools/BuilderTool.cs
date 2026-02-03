using System;
using System.Collections.Generic;
using HytaleClient.Data.Items;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools
{
	// Token: 0x0200097E RID: 2430
	internal class BuilderTool
	{
		// Token: 0x06004D1A RID: 19738 RVA: 0x0014A1C0 File Offset: 0x001483C0
		public BuilderTool(ItemBase.ItemBuilderToolData builderToolData)
		{
			this.BuilderToolData = builderToolData;
			this.ToolItem = builderToolData.Tools[0];
			this.Id = this.ToolItem.Id;
			this.IsBrushTool = this.ToolItem.IsBrush;
		}

		// Token: 0x06004D1B RID: 19739 RVA: 0x0014A20C File Offset: 0x0014840C
		public bool TryGetArg(string argId, out BuilderToolArg toolArg)
		{
			bool flag = this.ToolItem.Args.ContainsKey(argId);
			bool result;
			if (flag)
			{
				toolArg = this.ToolItem.Args[argId];
				result = true;
			}
			else
			{
				toolArg = null;
				result = false;
			}
			return result;
		}

		// Token: 0x06004D1C RID: 19740 RVA: 0x0014A250 File Offset: 0x00148450
		public string GetDefaultArgValue(string argId)
		{
			BuilderToolArg builderToolArg;
			bool flag = !this.TryGetArg(argId, out builderToolArg);
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				switch (builderToolArg.ArgType)
				{
				case 0:
					return builderToolArg.BoolArg.Default_.ToString();
				case 1:
					return builderToolArg.FloatArg.Default_.ToString();
				case 2:
					return builderToolArg.IntArg.Default_.ToString();
				case 3:
					return builderToolArg.StringArg.Default_;
				case 4:
					return builderToolArg.BlockArg.Default_;
				case 5:
					return builderToolArg.MaskArg.Default_;
				case 6:
					return builderToolArg.BrushShapeArg.Default_.ToString();
				case 7:
					return builderToolArg.BrushOriginArg.Default_.ToString();
				case 10:
					return builderToolArg.OptionArg.Default_;
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06004D1D RID: 19741 RVA: 0x0014A368 File Offset: 0x00148568
		public JObject GetDefaultArgData()
		{
			JObject jobject = new JObject();
			Dictionary<string, BuilderToolArg> args = this.ToolItem.Args;
			bool flag = args != null && args.Count > 0;
			if (flag)
			{
				foreach (KeyValuePair<string, BuilderToolArg> keyValuePair in this.ToolItem.Args)
				{
					jobject[keyValuePair.Key] = this.GetDefaultArgValue(keyValuePair.Key);
				}
			}
			return jobject;
		}

		// Token: 0x06004D1E RID: 19742 RVA: 0x0014A40C File Offset: 0x0014860C
		public bool TryGetItemArgValue(ref ClientItemStack item, string argId, out string argValue)
		{
			argValue = string.Empty;
			bool flag = item.Metadata == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				JToken jtoken = item.Metadata["ToolData"];
				BuilderToolArg builderToolArg;
				bool flag2 = jtoken == null || !this.TryGetArg(argId, out builderToolArg);
				if (flag2)
				{
					result = false;
				}
				else
				{
					argValue = (string)jtoken[argId];
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06004D1F RID: 19743 RVA: 0x0014A474 File Offset: 0x00148674
		public string GetItemArgValueOrDefault(ref ClientItemStack item, string argId)
		{
			BuilderToolArg builderToolArg;
			string text;
			return (!this.TryGetArg(argId, out builderToolArg)) ? null : (this.TryGetItemArgValue(ref item, argId, out text) ? text : this.GetDefaultArgValue(argId));
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x0014A4AC File Offset: 0x001486AC
		public Dictionary<string, string> GetItemToolArgs(ClientItemStack item)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, BuilderToolArg> args = this.ToolItem.Args;
			bool flag = args != null && args.Count > 0;
			if (flag)
			{
				foreach (KeyValuePair<string, BuilderToolArg> keyValuePair in this.ToolItem.Args)
				{
					string key = keyValuePair.Key;
					string text;
					string value = this.TryGetItemArgValue(ref item, key, out text) ? text : this.GetDefaultArgValue(key);
					dictionary.Add(key, value);
				}
			}
			return dictionary;
		}

		// Token: 0x06004D21 RID: 19745 RVA: 0x0014A560 File Offset: 0x00148760
		public string GetToolArgsLogText(ToolInstance toolInstance)
		{
			ClientItemStack itemStack = toolInstance.ItemStack;
			string text = this.IsBrushTool ? toolInstance.BrushData.ToString() : "";
			Dictionary<string, BuilderToolArg> args = this.ToolItem.Args;
			bool flag = args != null && args.Count > 0;
			if (flag)
			{
				text += "Tool Args\n";
				foreach (KeyValuePair<string, BuilderToolArg> keyValuePair in this.ToolItem.Args)
				{
					string key = keyValuePair.Key;
					string text3;
					string text2 = this.TryGetItemArgValue(ref itemStack, key, out text3) ? text3 : this.GetDefaultArgValue(key);
					text = string.Concat(new string[]
					{
						text,
						"  ",
						key,
						": ",
						text2,
						"\n"
					});
				}
			}
			string str = this.Id + " " + (this.IsBrushTool ? "Brush" : "Tool") + "\n";
			return str + ((text.Length == 0) ? "No Args\n" : text);
		}

		// Token: 0x06004D22 RID: 19746 RVA: 0x0014A6A8 File Offset: 0x001488A8
		public string GetFirstBlockArgId()
		{
			foreach (KeyValuePair<string, BuilderToolArg> keyValuePair in this.ToolItem.Args)
			{
				bool flag = keyValuePair.Value.ArgType == 4;
				if (flag)
				{
					return keyValuePair.Key;
				}
			}
			return null;
		}

		// Token: 0x06004D23 RID: 19747 RVA: 0x0014A724 File Offset: 0x00148924
		public static BuilderTool GetToolFromItemStack(GameInstance gameInstance, ClientItemStack itemStack)
		{
			bool flag = itemStack == null || gameInstance == null;
			BuilderTool result;
			if (flag)
			{
				result = null;
			}
			else
			{
				ClientItemBase item = gameInstance.ItemLibraryModule.GetItem(itemStack.Id);
				result = ((item != null) ? item.BuilderTool : null);
			}
			return result;
		}

		// Token: 0x06004D24 RID: 19748 RVA: 0x0014A768 File Offset: 0x00148968
		public static ClientItemBase[] GetBuilderToolItems(GameInstance gameInstance)
		{
			bool flag = gameInstance == null;
			ClientItemBase[] result;
			if (flag)
			{
				result = null;
			}
			else
			{
				List<ClientItemBase> list = new List<ClientItemBase>();
				Dictionary<string, ClientItemBase> items = gameInstance.ItemLibraryModule.GetItems();
				foreach (ClientItemBase clientItemBase in items.Values)
				{
					bool flag2 = clientItemBase.BuilderTool != null;
					if (flag2)
					{
						list.Add(clientItemBase);
					}
				}
				list.Sort((ClientItemBase a, ClientItemBase b) => a.BuilderTool.Id.CompareTo(b.BuilderTool.Id));
				result = list.ToArray();
			}
			return result;
		}

		// Token: 0x06004D25 RID: 19749 RVA: 0x0014A828 File Offset: 0x00148A28
		public override string ToString()
		{
			return this.Id + " " + (this.IsBrushTool ? "Brush" : "Tool");
		}

		// Token: 0x04002864 RID: 10340
		public const string BaseToolNamePrefix = "EditorTool_";

		// Token: 0x04002865 RID: 10341
		public const string ToolDataKey = "ToolData";

		// Token: 0x04002866 RID: 10342
		public const string BrushDataKey = "BrushData";

		// Token: 0x04002867 RID: 10343
		public readonly ItemBase.ItemBuilderToolData BuilderToolData;

		// Token: 0x04002868 RID: 10344
		public readonly BuilderToolItem ToolItem;

		// Token: 0x04002869 RID: 10345
		public readonly string Id;

		// Token: 0x0400286A RID: 10346
		public readonly bool IsBrushTool;
	}
}
