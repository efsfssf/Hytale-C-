using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.AssetEditor.Data;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BC6 RID: 3014
	internal class MapEditor : ValueEditor
	{
		// Token: 0x06005EA4 RID: 24228 RVA: 0x001E505C File Offset: 0x001E325C
		public MapEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent, schema, path, parentPropertyEditor, parentSchema, configEditor, value)
		{
			this._layoutMode = LayoutMode.Full;
		}

		// Token: 0x06005EA5 RID: 24229 RVA: 0x001E5090 File Offset: 0x001E3290
		protected override void Build()
		{
			ReorderableList reorderableList = this._reorderableList;
			if (reorderableList != null)
			{
				reorderableList.Clear();
			}
			this._properties.Clear();
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
			bool flag = base.Value == null || ((JObject)base.Value).Count == 0;
			if (flag)
			{
				this.BuildEmptyLabel();
			}
			else
			{
				foreach (KeyValuePair<string, JToken> keyValuePair in ((JObject)base.Value))
				{
					PropertyEditor propertyEditor = new PropertyEditor(this.Desktop, this._reorderableList, keyValuePair.Key, this.GetSchema(), this.Path.GetChild(keyValuePair.Key), this.Schema, this.ConfigEditor, this, null, false);
					propertyEditor.Build(keyValuePair.Value, false, this.IsDetachedEditor, this.CachesToRebuild);
					this._properties.Add(keyValuePair.Key, propertyEditor);
				}
			}
		}

		// Token: 0x06005EA6 RID: 24230 RVA: 0x001E520C File Offset: 0x001E340C
		private void BuildEmptyLabel()
		{
			Label label = new Label(this.Desktop, this._reorderableList);
			label.Text = this.Desktop.Provider.GetText("ui.assetEditor.mapEditor.empty", null, true);
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

		// Token: 0x06005EA7 RID: 24231 RVA: 0x001E52C6 File Offset: 0x001E34C6
		private SchemaNode GetSchema()
		{
			return this.ConfigEditor.AssetEditorOverlay.ResolveSchemaInCurrentContext(this.Schema.Value);
		}

		// Token: 0x06005EA8 RID: 24232 RVA: 0x001E52E4 File Offset: 0x001E34E4
		public override void SetValueRecursively(JToken value)
		{
			JToken value2 = base.Value;
			base.SetValueRecursively(value);
			bool flag = value2 == base.Value;
			if (!flag)
			{
				this._reorderableList.Clear();
				this._properties.Clear();
				this.Build();
			}
		}

		// Token: 0x06005EA9 RID: 24233 RVA: 0x001E5330 File Offset: 0x001E3530
		public void OnItemRemoved(string key)
		{
			PropertyEditor child = this._properties[key];
			this._properties.Remove(key);
			this._reorderableList.Remove(child);
			bool flag = this._properties.Count == 0;
			if (flag)
			{
				this.BuildEmptyLabel();
			}
		}

		// Token: 0x06005EAA RID: 24234 RVA: 0x001E5380 File Offset: 0x001E3580
		public void OnItemInserted(string key, JToken value)
		{
			bool flag = this._properties.Count == 0;
			if (flag)
			{
				this._reorderableList.Clear();
			}
			PropertyEditor propertyEditor = new PropertyEditor(this.Desktop, null, key, this.GetSchema(), this.Path.GetChild(key), this.Schema, this.ConfigEditor, this, null, false);
			propertyEditor.Build(value, false, this.IsDetachedEditor, this.CachesToRebuild);
			this._reorderableList.Add(propertyEditor, -1);
			this._properties.Add(key, propertyEditor);
			PropertyEditor parentPropertyEditor = this.ParentPropertyEditor;
			if (parentPropertyEditor != null)
			{
				parentPropertyEditor.UpdateAppearance();
			}
		}

		// Token: 0x06005EAB RID: 24235 RVA: 0x001E5420 File Offset: 0x001E3620
		private void OnListElementReordered(int sourceIndex, int targetIndex)
		{
			this.HandleMoveKey(((PropertyEditor)this._reorderableList.Children[targetIndex]).PropertyName, targetIndex, false);
		}

		// Token: 0x06005EAC RID: 24236 RVA: 0x001E5448 File Offset: 0x001E3648
		internal void HandleInsertKey(string key)
		{
			PropertyPath child = this.Path.GetChild(key);
			JToken defaultValue = SchemaParser.GetDefaultValue(this.ConfigEditor.AssetEditorOverlay.ResolveSchemaInCurrentContext(this.Schema.Value));
			JToken jtoken = (defaultValue != null) ? defaultValue.DeepClone() : null;
			bool flag = this.ParentPropertyEditor != null && this.ParentPropertyEditor.IsCollapsed;
			if (flag)
			{
				this.ParentPropertyEditor.SetCollapseState(false, true);
			}
			ConfigEditor configEditor = this.ConfigEditor;
			PropertyPath path = child;
			JToken value = jtoken;
			JToken previousValue = null;
			CacheRebuildInfo cachesToRebuild = this.CachesToRebuild;
			configEditor.OnChangeValue(path, value, previousValue, (cachesToRebuild != null) ? cachesToRebuild.Caches : null, false, false, false);
			this.ConfigEditor.Layout(null, true);
			bool isMounted = this._properties[key].IsMounted;
			if (isMounted)
			{
				this._properties[key].ValueEditor.Focus();
			}
		}

		// Token: 0x06005EAD RID: 24237 RVA: 0x001E5520 File Offset: 0x001E3720
		internal void HandleRenameKey(string currentKey, string newKey)
		{
			PropertyEditor propertyEditor = this._properties[currentKey];
			this._properties.Remove(currentKey);
			this._properties.Add(newKey, propertyEditor);
			propertyEditor.UpdatePathRecursively(newKey, this.Path.GetChild(newKey));
			propertyEditor.Layout(null, true);
			JToken jtoken = base.Value.DeepClone();
			JObject jobject = (JObject)base.Value;
			JToken jtoken2 = jobject[currentKey];
			JToken jtoken3 = Enumerable.LastOrDefault<JToken>(jtoken2.Parent.BeforeSelf());
			jobject.Remove(currentKey);
			bool flag = jtoken3 == null;
			if (flag)
			{
				jobject.AddFirst(new JProperty(newKey, jtoken2));
			}
			else
			{
				jtoken3.AddAfterSelf(new JProperty(newKey, jtoken2));
			}
			ConfigEditor configEditor = this.ConfigEditor;
			PropertyPath path = this.Path;
			JToken value = jobject;
			JToken previousValue = jtoken;
			CacheRebuildInfo cachesToRebuild = this.CachesToRebuild;
			configEditor.OnChangeValue(path, value, previousValue, (cachesToRebuild != null) ? cachesToRebuild.Caches : null, false, false, false);
		}

		// Token: 0x06005EAE RID: 24238 RVA: 0x001E5610 File Offset: 0x001E3810
		public void HandleMoveKey(string key, bool backwards)
		{
			PropertyEditor propertyEditor = this._properties[key];
			int num = backwards ? -1 : 1;
			int num2 = -1;
			for (int i = 0; i < this._reorderableList.Children.Count; i++)
			{
				bool flag = this._reorderableList.Children[i] == propertyEditor;
				if (flag)
				{
					num2 = i;
					break;
				}
			}
			this.HandleMoveKey(key, num2 + num, true);
		}

		// Token: 0x06005EAF RID: 24239 RVA: 0x001E5684 File Offset: 0x001E3884
		public void HandleMoveKey(string key, int targetIndex, bool reorderElement = true)
		{
			JToken jtoken = base.Value.DeepClone();
			JObject jobject = (JObject)base.Value;
			JToken jtoken2 = jobject[key];
			int num = -1;
			string text = null;
			foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
			{
				num++;
				bool flag = keyValuePair.Key == key;
				if (flag)
				{
					break;
				}
			}
			int num2 = 0;
			foreach (KeyValuePair<string, JToken> keyValuePair2 in jobject)
			{
				bool flag2 = num2 == targetIndex;
				if (flag2)
				{
					text = keyValuePair2.Key;
					break;
				}
				num2++;
			}
			jobject.Remove(key);
			bool flag3 = num > targetIndex;
			if (flag3)
			{
				jobject[text].Parent.AddBeforeSelf(new JProperty(key, jtoken2));
			}
			else
			{
				jobject[text].Parent.AddAfterSelf(new JProperty(key, jtoken2));
			}
			if (reorderElement)
			{
				this._reorderableList.Reorder(this._properties[key], targetIndex);
				this._reorderableList.Layout(null, true);
			}
			ConfigEditor configEditor = this.ConfigEditor;
			PropertyPath path = this.Path;
			JToken value = jobject;
			JToken previousValue = jtoken;
			CacheRebuildInfo cachesToRebuild = this.CachesToRebuild;
			configEditor.OnChangeValue(path, value, previousValue, (cachesToRebuild != null) ? cachesToRebuild.Caches : null, false, false, false);
		}

		// Token: 0x06005EB0 RID: 24240 RVA: 0x001E5818 File Offset: 0x001E3A18
		public override void UpdatePathRecursively(PropertyPath path)
		{
			base.UpdatePathRecursively(path);
			foreach (KeyValuePair<string, PropertyEditor> keyValuePair in this._properties)
			{
			}
			foreach (KeyValuePair<string, PropertyEditor> keyValuePair2 in this._properties)
			{
				keyValuePair2.Value.UpdatePathRecursively(keyValuePair2.Key, this.Path.GetChild(keyValuePair2.Key));
			}
		}

		// Token: 0x06005EB1 RID: 24241 RVA: 0x001E58D8 File Offset: 0x001E3AD8
		public bool HasItemWithKey(string key)
		{
			key = key.ToLowerInvariant();
			foreach (KeyValuePair<string, PropertyEditor> keyValuePair in this._properties)
			{
				bool flag = keyValuePair.Key.ToLowerInvariant() == key;
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005EB2 RID: 24242 RVA: 0x001E5950 File Offset: 0x001E3B50
		protected override bool ValidateType(JToken value)
		{
			return value.Type == 1;
		}

		// Token: 0x06005EB3 RID: 24243 RVA: 0x001E595C File Offset: 0x001E3B5C
		public void SetCollapseStateForAllItems(bool uncollapsed)
		{
			foreach (PropertyEditor propertyEditor in this._properties.Values)
			{
				propertyEditor.SetCollapseState(uncollapsed, false);
			}
			this.ConfigEditor.SetupDiagnostics(false, true);
			this.ConfigEditor.Layout(null, true);
		}

		// Token: 0x06005EB4 RID: 24244 RVA: 0x001E59E0 File Offset: 0x001E3BE0
		public override bool TryFindPropertyEditor(PropertyPath path, out PropertyEditor propertyEditor)
		{
			string key = path.Elements[this.Path.Elements.Length];
			bool flag = !this._properties.TryGetValue(key, out propertyEditor);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = path.Elements.Length > this.Path.Elements.Length + 1;
				result = (!flag2 || propertyEditor.ValueEditor.TryFindPropertyEditor(path, out propertyEditor));
			}
			return result;
		}

		// Token: 0x06005EB5 RID: 24245 RVA: 0x001E5A50 File Offset: 0x001E3C50
		public override void ClearDiagnosticsOfDescendants()
		{
			foreach (PropertyEditor propertyEditor in this._properties.Values)
			{
				propertyEditor.ClearDiagnostics(true);
				propertyEditor.ValueEditor.ClearDiagnosticsOfDescendants();
			}
		}

		// Token: 0x04003B11 RID: 15121
		private readonly Dictionary<string, PropertyEditor> _properties = new Dictionary<string, PropertyEditor>();

		// Token: 0x04003B12 RID: 15122
		private ReorderableList _reorderableList;
	}
}
