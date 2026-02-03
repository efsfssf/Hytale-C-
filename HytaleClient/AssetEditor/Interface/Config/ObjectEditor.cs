using System;
using System.Collections.Generic;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BC8 RID: 3016
	internal class ObjectEditor : ValueEditor
	{
		// Token: 0x06005EC4 RID: 24260 RVA: 0x001E6354 File Offset: 0x001E4554
		public ObjectEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent, schema, path, parentPropertyEditor, parentSchema, configEditor, value)
		{
			this._layoutMode = (this.Schema.DisplayCompact ? LayoutMode.Left : LayoutMode.Top);
		}

		// Token: 0x06005EC5 RID: 24261 RVA: 0x001E6398 File Offset: 0x001E4598
		protected override void Build()
		{
			this.Build(this.Schema);
		}

		// Token: 0x06005EC6 RID: 24262 RVA: 0x001E63A8 File Offset: 0x001E45A8
		protected void Build(SchemaNode schema)
		{
			string text = (!this.IsDetachedEditor) ? this.ConfigEditor.SearchQuery : "";
			bool flag = schema.Properties != null;
			if (flag)
			{
				bool flag2 = false;
				foreach (KeyValuePair<string, SchemaNode> keyValuePair in schema.Properties)
				{
					bool isHidden = keyValuePair.Value.IsHidden;
					if (!isHidden)
					{
						PropertyPath child = this.Path.GetChild(keyValuePair.Key);
						bool filterCategory = false;
						bool flag3 = this.FilterCategory && text == "";
						if (flag3)
						{
							bool flag4 = !flag2;
							if (flag4)
							{
								bool flag5 = keyValuePair.Value.SectionStart != null && this.ConfigEditor.State.ActiveCategory != null && this.ConfigEditor.State.ActiveCategory.Equals(child);
								if (flag5)
								{
									flag2 = true;
								}
								else
								{
									bool flag6 = this.ConfigEditor.State.ActiveCategory.Value.IsDescendantOf(child);
									if (!flag6)
									{
										continue;
									}
									filterCategory = true;
								}
							}
							else
							{
								bool flag7 = keyValuePair.Value.SectionStart != null;
								if (flag7)
								{
									break;
								}
							}
						}
						string text2 = keyValuePair.Value.Title ?? JsonUtils.GetTitleFromKey(keyValuePair.Key);
						bool flag8 = text != "" && !keyValuePair.Key.ToLower().Contains(text) && (keyValuePair.Value.Title == null || !keyValuePair.Value.Title.ToLower().Contains(text)) && !text2.ToLower().Contains(text);
						if (!flag8)
						{
							JToken value = base.Value;
							JToken jtoken = (value != null) ? value[keyValuePair.Key] : null;
							bool flag9 = !this.ConfigEditor.DisplayUnsetProperties && (jtoken == null || jtoken.Type == 10);
							if (!flag9)
							{
								SchemaNode schemaNode = this.ConfigEditor.AssetEditorOverlay.ResolveSchemaInCurrentContext(keyValuePair.Value);
								PropertyEditor propertyEditor = this._properties[keyValuePair.Key] = new PropertyEditor(this.Desktop, this, keyValuePair.Key, schemaNode, child, this.Schema, this.ConfigEditor, this, text2, false);
								CacheRebuildInfo cacheRebuildInfo = null;
								bool flag10 = schemaNode.RebuildCaches != null;
								if (flag10)
								{
									cacheRebuildInfo = new CacheRebuildInfo(schemaNode.RebuildCaches, schemaNode.RebuildCachesForChildProperties);
								}
								else
								{
									bool flag11 = this.CachesToRebuild != null && this.CachesToRebuild.AppliesToChildProperties;
									if (flag11)
									{
										cacheRebuildInfo = this.CachesToRebuild;
									}
								}
								propertyEditor.Build(jtoken, filterCategory, this.IsDetachedEditor, cacheRebuildInfo);
							}
						}
					}
				}
			}
			bool flag12 = schema.TypePropertyKey != null;
			if (flag12)
			{
				JToken value2 = base.Value;
				JToken value3 = (value2 != null) ? value2[schema.TypePropertyKey] : null;
				SchemaNode schemaNode2 = schema;
				bool flag13 = this.ConfigEditor.AssetEditorOverlay.TryResolveTypeSchemaInCurrentContext(base.Value, ref schemaNode2);
				if (flag13)
				{
					bool flag14 = false;
					foreach (KeyValuePair<string, SchemaNode> keyValuePair2 in schemaNode2.Properties)
					{
						bool isHidden2 = keyValuePair2.Value.IsHidden;
						if (!isHidden2)
						{
							PropertyPath child2 = this.Path.GetChild(keyValuePair2.Key);
							bool filterCategory2 = false;
							bool flag15 = this.FilterCategory && text == "";
							if (flag15)
							{
								bool flag16 = !flag14;
								if (flag16)
								{
									bool flag17 = keyValuePair2.Value.SectionStart != null && this.ConfigEditor.State.ActiveCategory.Value.Equals(child2);
									if (flag17)
									{
										flag14 = true;
									}
									else
									{
										bool flag18 = this.ConfigEditor.State.ActiveCategory.Value.IsDescendantOf(child2);
										if (!flag18)
										{
											continue;
										}
										filterCategory2 = true;
									}
								}
								else
								{
									bool flag19 = keyValuePair2.Value.SectionStart != null;
									if (flag19)
									{
										break;
									}
								}
							}
							bool flag20 = text != "" && !keyValuePair2.Key.ToLower().Contains(text) && (keyValuePair2.Value.Title == null || !keyValuePair2.Value.Title.ToLower().Contains(text));
							if (!flag20)
							{
								JToken value4 = base.Value;
								JToken jtoken2 = (value4 != null) ? value4[keyValuePair2.Key] : null;
								bool flag21 = !this.ConfigEditor.DisplayUnsetProperties && (jtoken2 == null || jtoken2.Type == 10);
								if (!flag21)
								{
									SchemaNode schema2 = this.ConfigEditor.AssetEditorOverlay.ResolveSchemaInCurrentContext((keyValuePair2.Key == schema.TypePropertyKey) ? schema.Value : keyValuePair2.Value);
									PropertyEditor propertyEditor2 = this._properties[keyValuePair2.Key] = new PropertyEditor(this.Desktop, this, keyValuePair2.Key, schema2, child2, schema, this.ConfigEditor, this, keyValuePair2.Value.Title ?? JsonUtils.GetTitleFromKey(keyValuePair2.Key), false);
									propertyEditor2.IsSchemaTypeField = (keyValuePair2.Key == schema.TypePropertyKey);
									CacheRebuildInfo cacheRebuildInfo2 = null;
									bool flag22 = schema.RebuildCaches != null;
									if (flag22)
									{
										cacheRebuildInfo2 = new CacheRebuildInfo(schema.RebuildCaches, schema.RebuildCachesForChildProperties);
									}
									else
									{
										bool flag23 = this.CachesToRebuild != null && this.CachesToRebuild.AppliesToChildProperties;
										if (flag23)
										{
											cacheRebuildInfo2 = this.CachesToRebuild;
										}
									}
									propertyEditor2.Build(jtoken2, filterCategory2, this.IsDetachedEditor, cacheRebuildInfo2);
								}
							}
						}
					}
				}
				else
				{
					bool flag24 = schema.TypeSchemas.Length != 0;
					if (flag24)
					{
						SchemaNode schema3 = this.ConfigEditor.AssetEditorOverlay.ResolveSchemaInCurrentContext(schema.Value);
						PropertyPath child3 = this.Path.GetChild(schema.TypePropertyKey);
						PropertyEditor propertyEditor3 = this._properties[schema.TypePropertyKey] = new PropertyEditor(this.Desktop, this, schema.TypePropertyKey, schema3, child3, schema, this.ConfigEditor, this, schema.Value.Title, false);
						propertyEditor3.IsSchemaTypeField = true;
						propertyEditor3.Build(value3, false, this.IsDetachedEditor, null);
						bool hasParentProperty = schema.HasParentProperty;
						if (hasParentProperty)
						{
							SchemaNode schemaNode3 = this.ConfigEditor.AssetEditorOverlay.ResolveSchemaInCurrentContext(schema.TypeSchemas[0]);
							SchemaNode schemaNode4 = schemaNode3.Properties["Parent"];
							PropertyPath child4 = this.Path.GetChild("Parent");
							PropertyEditor propertyEditor4 = this._properties["Parent"] = new PropertyEditor(this.Desktop, this, "Parent", this.ConfigEditor.AssetEditorOverlay.ResolveSchemaInCurrentContext(schemaNode4), child4, schema, this.ConfigEditor, this, schemaNode4.Title, false);
							PropertyEditor propertyEditor5 = propertyEditor4;
							JToken value5 = base.Value;
							propertyEditor5.Build((value5 != null) ? value5["Parent"] : null, false, this.IsDetachedEditor, null);
						}
					}
				}
			}
		}

		// Token: 0x06005EC7 RID: 24263 RVA: 0x001E6B6C File Offset: 0x001E4D6C
		public override void SetValueRecursively(JToken value)
		{
			base.SetValueRecursively(value);
			bool flag = base.Value == null;
			if (flag)
			{
				foreach (PropertyEditor propertyEditor in this._properties.Values)
				{
					propertyEditor.ValueEditor.SetValueRecursively(null);
					propertyEditor.ValueEditor.UpdateDisplayedValue();
				}
			}
			else
			{
				base.Clear();
				this._properties.Clear();
				this.Build();
			}
		}

		// Token: 0x06005EC8 RID: 24264 RVA: 0x001E6C0C File Offset: 0x001E4E0C
		public override void UpdatePathRecursively(PropertyPath path)
		{
			base.UpdatePathRecursively(path);
			foreach (KeyValuePair<string, PropertyEditor> keyValuePair in this._properties)
			{
				keyValuePair.Value.UpdatePathRecursively(keyValuePair.Key, path.GetChild(keyValuePair.Key));
			}
		}

		// Token: 0x06005EC9 RID: 24265 RVA: 0x001E6C84 File Offset: 0x001E4E84
		protected override bool ValidateType(JToken value)
		{
			return value.Type == 1;
		}

		// Token: 0x06005ECA RID: 24266 RVA: 0x001E6C90 File Offset: 0x001E4E90
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

		// Token: 0x06005ECB RID: 24267 RVA: 0x001E6D00 File Offset: 0x001E4F00
		public override void ClearDiagnosticsOfDescendants()
		{
			foreach (PropertyEditor propertyEditor in this._properties.Values)
			{
				propertyEditor.ClearDiagnostics(true);
				propertyEditor.ValueEditor.ClearDiagnosticsOfDescendants();
			}
		}

		// Token: 0x04003B15 RID: 15125
		protected readonly IDictionary<string, PropertyEditor> _properties = new Dictionary<string, PropertyEditor>();
	}
}
