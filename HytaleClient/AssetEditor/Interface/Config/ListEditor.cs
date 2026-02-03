using System;
using System.Collections.Generic;
using HytaleClient.AssetEditor.Data;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BC5 RID: 3013
	internal class ListEditor : ValueEditor
	{
		// Token: 0x06005E94 RID: 24212 RVA: 0x001E46C8 File Offset: 0x001E28C8
		public ListEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent, schema, path, parentPropertyEditor, parentSchema, configEditor, value)
		{
			this._layoutMode = LayoutMode.Full;
		}

		// Token: 0x06005E95 RID: 24213 RVA: 0x001E46FC File Offset: 0x001E28FC
		private SchemaNode GetSchema()
		{
			return this.ConfigEditor.AssetEditorOverlay.ResolveSchemaInCurrentContext(this.Schema.Value);
		}

		// Token: 0x06005E96 RID: 24214 RVA: 0x001E471C File Offset: 0x001E291C
		protected override void Build()
		{
			this._reorderableList = new ReorderableList(this.Desktop, this)
			{
				ElementReordered = new Action<int, int>(this.OnListElementReordered),
				DropIndicatorAnchor = new Anchor
				{
					Height = new int?(2),
					Horizontal = new int?(-2)
				},
				DropIndicatorBackground = new PatchStyle(UInt32Color.FromRGBA(0, 0, 0, 200))
			};
			bool flag = base.Value == null || ((JArray)base.Value).Count == 0;
			if (flag)
			{
				this.BuildEmptyLabel();
			}
			else
			{
				JArray jarray = (JArray)base.Value;
				for (int i = 0; i < jarray.Count; i++)
				{
					PropertyPath child = this.Path.GetChild(i.ToString());
					PropertyEditor propertyEditor = new PropertyEditor(this.Desktop, this._reorderableList, i.ToString(), this.GetSchema(), child, this.Schema, this.ConfigEditor, this, null, false);
					propertyEditor.Build(jarray[i], false, false, this.CachesToRebuild);
					this._items.Add(propertyEditor);
				}
			}
		}

		// Token: 0x06005E97 RID: 24215 RVA: 0x001E4850 File Offset: 0x001E2A50
		private void BuildEmptyLabel()
		{
			Label label = new Label(this.Desktop, this._reorderableList);
			label.Text = this.Desktop.Provider.GetText("ui.assetEditor.listEditor.empty", null, true);
			label.Style = new LabelStyle
			{
				FontSize = 12f,
				RenderItalics = true,
				TextColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 140),
				Alignment = LabelStyle.LabelAlignment.Center
			};
			label.Anchor = new Anchor
			{
				Height = new int?(32)
			};
			label.Padding = new Padding
			{
				Horizontal = new int?(8)
			};
		}

		// Token: 0x06005E98 RID: 24216 RVA: 0x001E490C File Offset: 0x001E2B0C
		public void HandleInsertItem(int index = -1)
		{
			bool flag = index < 0;
			if (flag)
			{
				index = this._items.Count;
			}
			PropertyPath child = this.Path.GetChild(index.ToString());
			JToken defaultValue = SchemaParser.GetDefaultValue(this.ConfigEditor.AssetEditorOverlay.ResolveSchemaInCurrentContext(this.Schema.Value));
			JToken jtoken = (defaultValue != null) ? defaultValue.DeepClone() : null;
			bool flag2 = this.ParentPropertyEditor != null && this.ParentPropertyEditor.IsCollapsed;
			if (flag2)
			{
				this.ParentPropertyEditor.SetCollapseState(false, true);
			}
			ConfigEditor configEditor = this.ConfigEditor;
			PropertyPath path = child;
			JToken value = jtoken;
			JToken previousValue = null;
			CacheRebuildInfo cachesToRebuild = this.CachesToRebuild;
			configEditor.OnChangeValue(path, value, previousValue, (cachesToRebuild != null) ? cachesToRebuild.Caches : null, false, true, false);
			this.ConfigEditor.Layout(null, true);
			bool isMounted = this._items[index].IsMounted;
			if (isMounted)
			{
				this._items[index].ValueEditor.Focus();
			}
		}

		// Token: 0x06005E99 RID: 24217 RVA: 0x001E4A00 File Offset: 0x001E2C00
		public override void SetValueRecursively(JToken value)
		{
			JToken value2 = base.Value;
			base.SetValueRecursively(value);
			bool flag = value2 == base.Value;
			if (!flag)
			{
				this._reorderableList.Clear();
				this._items.Clear();
				this.Build();
			}
		}

		// Token: 0x06005E9A RID: 24218 RVA: 0x001E4A4C File Offset: 0x001E2C4C
		public void OnItemRemoved(int index)
		{
			PropertyEditor child = this._items[index];
			this._items.RemoveAt(index);
			this._reorderableList.Remove(child);
			this.UpdatePathRecursively(this.Path);
			bool flag = this._items.Count == 0;
			if (flag)
			{
				this.BuildEmptyLabel();
			}
		}

		// Token: 0x06005E9B RID: 24219 RVA: 0x001E4AAC File Offset: 0x001E2CAC
		public void OnItemInserted(JToken value, int index)
		{
			bool flag = this._items.Count == 0;
			if (flag)
			{
				this._reorderableList.Clear();
			}
			PropertyEditor propertyEditor = new PropertyEditor(this.Desktop, null, index.ToString(), this.GetSchema(), this.Path.GetChild(index.ToString()), this.Schema, this.ConfigEditor, this, null, false);
			propertyEditor.Build(value, false, this.IsDetachedEditor, this.CachesToRebuild);
			this._items.Insert(index, propertyEditor);
			this.UpdatePathRecursively(this.Path);
			this._reorderableList.Add(propertyEditor, index);
			PropertyEditor parentPropertyEditor = this.ParentPropertyEditor;
			if (parentPropertyEditor != null)
			{
				parentPropertyEditor.UpdateAppearance();
			}
		}

		// Token: 0x06005E9C RID: 24220 RVA: 0x001E4B65 File Offset: 0x001E2D65
		public void OnListElementReordered(int sourceIndex, int targetIndex)
		{
			this.HandleMoveItem(sourceIndex, targetIndex, false);
		}

		// Token: 0x06005E9D RID: 24221 RVA: 0x001E4B74 File Offset: 0x001E2D74
		public void HandleMoveItem(int sourceIndex, int targetIndex, bool reorderElement = true)
		{
			PropertyEditor propertyEditor = this._items[sourceIndex];
			this._items.RemoveAt(sourceIndex);
			this._items.Insert(targetIndex, propertyEditor);
			if (reorderElement)
			{
				this._reorderableList.Reorder(propertyEditor, targetIndex);
			}
			JArray jarray = (JArray)base.Value.DeepClone();
			JToken jtoken = jarray[sourceIndex];
			jarray.RemoveAt(sourceIndex);
			jarray.Insert(targetIndex, jtoken);
			Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
			bool flag = sourceIndex > targetIndex;
			if (flag)
			{
				PropertyPath child = this.Path.GetChild(sourceIndex.ToString());
				bool value;
				bool flag2 = this.ConfigEditor.State.UncollapsedProperties.TryGetValue(child, out value);
				if (flag2)
				{
					this.ConfigEditor.State.UncollapsedProperties.Remove(child);
					dictionary[targetIndex] = value;
				}
				for (int i = targetIndex; i < sourceIndex; i++)
				{
					PropertyPath child2 = this.Path.GetChild(i.ToString());
					bool value2;
					bool flag3 = this.ConfigEditor.State.UncollapsedProperties.TryGetValue(child2, out value2);
					if (flag3)
					{
						this.ConfigEditor.State.UncollapsedProperties.Remove(child2);
						dictionary[i + 1] = value2;
					}
				}
			}
			else
			{
				PropertyPath child3 = this.Path.GetChild(sourceIndex.ToString());
				bool value3;
				bool flag4 = this.ConfigEditor.State.UncollapsedProperties.TryGetValue(child3, out value3);
				if (flag4)
				{
					this.ConfigEditor.State.UncollapsedProperties.Remove(child3);
					dictionary[targetIndex] = value3;
				}
				for (int j = sourceIndex + 1; j < targetIndex + 1; j++)
				{
					PropertyPath child4 = this.Path.GetChild(j.ToString());
					bool value4;
					bool flag5 = this.ConfigEditor.State.UncollapsedProperties.TryGetValue(child4, out value4);
					if (flag5)
					{
						this.ConfigEditor.State.UncollapsedProperties.Remove(child4);
						dictionary[j - 1] = value4;
					}
				}
			}
			foreach (KeyValuePair<int, bool> keyValuePair in dictionary)
			{
				this.ConfigEditor.State.UncollapsedProperties.Add(this.Path.GetChild(keyValuePair.Key.ToString()), keyValuePair.Value);
			}
			ConfigEditor configEditor = this.ConfigEditor;
			PropertyPath path = this.Path;
			JToken value5 = jarray;
			JToken value6 = base.Value;
			CacheRebuildInfo cachesToRebuild = this.CachesToRebuild;
			configEditor.OnChangeValue(path, value5, value6, (cachesToRebuild != null) ? cachesToRebuild.Caches : null, false, false, false);
			this.ConfigEditor.Layout(null, true);
		}

		// Token: 0x06005E9E RID: 24222 RVA: 0x001E4E5C File Offset: 0x001E305C
		public override void UpdatePathRecursively(PropertyPath path)
		{
			base.UpdatePathRecursively(path);
			for (int i = 0; i < this._items.Count; i++)
			{
				this._items[i].UpdatePathRecursively(i.ToString(), this.Path.GetChild(i.ToString()));
			}
		}

		// Token: 0x06005E9F RID: 24223 RVA: 0x001E4EB9 File Offset: 0x001E30B9
		public int Count()
		{
			return this._items.Count;
		}

		// Token: 0x06005EA0 RID: 24224 RVA: 0x001E4EC6 File Offset: 0x001E30C6
		protected override bool ValidateType(JToken value)
		{
			return value.Type == 2;
		}

		// Token: 0x06005EA1 RID: 24225 RVA: 0x001E4ED4 File Offset: 0x001E30D4
		public void SetCollapseStateForAllItems(bool uncollapsed)
		{
			foreach (PropertyEditor propertyEditor in this._items)
			{
				propertyEditor.SetCollapseState(uncollapsed, false);
			}
			this.ConfigEditor.SetupDiagnostics(false, true);
			this.ConfigEditor.Layout(null, true);
		}

		// Token: 0x06005EA2 RID: 24226 RVA: 0x001E4F54 File Offset: 0x001E3154
		public override bool TryFindPropertyEditor(PropertyPath path, out PropertyEditor propertyEditor)
		{
			string s = path.Elements[this.Path.Elements.Length];
			int num;
			bool flag = !int.TryParse(s, out num) || num < 0 || num >= this._items.Count;
			bool result;
			if (flag)
			{
				propertyEditor = null;
				result = false;
			}
			else
			{
				bool flag2 = path.Elements.Length > this.Path.Elements.Length + 1;
				if (flag2)
				{
					result = this._items[num].ValueEditor.TryFindPropertyEditor(path, out propertyEditor);
				}
				else
				{
					propertyEditor = this._items[num];
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005EA3 RID: 24227 RVA: 0x001E4FF4 File Offset: 0x001E31F4
		public override void ClearDiagnosticsOfDescendants()
		{
			foreach (PropertyEditor propertyEditor in this._items)
			{
				propertyEditor.ClearDiagnostics(true);
				propertyEditor.ValueEditor.ClearDiagnosticsOfDescendants();
			}
		}

		// Token: 0x04003B0F RID: 15119
		private readonly List<PropertyEditor> _items = new List<PropertyEditor>();

		// Token: 0x04003B10 RID: 15120
		private ReorderableList _reorderableList;
	}
}
