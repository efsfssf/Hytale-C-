using System;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BCF RID: 3023
	internal abstract class ValueEditor : Element
	{
		// Token: 0x170013DA RID: 5082
		// (get) Token: 0x06005F31 RID: 24369 RVA: 0x001EB56F File Offset: 0x001E976F
		// (set) Token: 0x06005F32 RID: 24370 RVA: 0x001EB577 File Offset: 0x001E9777
		public JToken Value { get; private set; }

		// Token: 0x06005F33 RID: 24371 RVA: 0x001EB580 File Offset: 0x001E9780
		protected ValueEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent)
		{
			this.Schema = schema;
			this._parentSchema = parentSchema;
			this.ConfigEditor = configEditor;
			this.Value = ((value != null && value.Type == 10) ? null : value);
			this.Path = path;
			this.ParentPropertyEditor = parentPropertyEditor;
		}

		// Token: 0x06005F34 RID: 24372
		protected abstract void Build();

		// Token: 0x06005F35 RID: 24373 RVA: 0x001EB5D9 File Offset: 0x001E97D9
		public void BuildEditor()
		{
			this.Build();
		}

		// Token: 0x06005F36 RID: 24374 RVA: 0x001EB5E2 File Offset: 0x001E97E2
		public void ValidateValue()
		{
			this.ValidateValue(this.Value);
		}

		// Token: 0x06005F37 RID: 24375 RVA: 0x001EB5F4 File Offset: 0x001E97F4
		public void ValidateValue(JToken value)
		{
			bool flag = value != null && !this.ValidateType(value);
			if (flag)
			{
				throw new ValueEditor.InvalidJsonTypeException(this.Path.ToString(), value, this.Schema.Type);
			}
		}

		// Token: 0x06005F38 RID: 24376 RVA: 0x001EB63C File Offset: 0x001E983C
		public virtual void PasteValue(string text)
		{
			JToken value = JsonUtils.ParseLenient(text);
			value = this.SanitizeValue(value);
			try
			{
				this.ValidateValue(value);
			}
			catch (Exception exception)
			{
				ValueEditor.Logger.Info(exception, "Failed to paste value because validation failed.");
				return;
			}
			try
			{
				this.HandleChangeValue(value, false, false, true);
			}
			catch (ValueEditor.InvalidJsonTypeException exception2)
			{
				ValueEditor.Logger.Info(exception2, "Failed to paste value because validation failed.");
				this.ConfigEditor.BuildPropertyEditors();
			}
			this.ConfigEditor.Layout(null, true);
		}

		// Token: 0x06005F39 RID: 24377 RVA: 0x001EB6E0 File Offset: 0x001E98E0
		protected virtual JToken SanitizeValue(JToken value)
		{
			return value;
		}

		// Token: 0x06005F3A RID: 24378 RVA: 0x001EB6E4 File Offset: 0x001E98E4
		protected void HandleChangeValue(JToken value, bool withheldCommand = false, bool confirmed = false, bool updateDisplayedValue = false)
		{
			ValueEditor.<>c__DisplayClass20_0 CS$<>8__locals1 = new ValueEditor.<>c__DisplayClass20_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.value = value;
			bool isDetachedEditor = this.IsDetachedEditor;
			if (isDetachedEditor)
			{
				this.Value = CS$<>8__locals1.value;
			}
			else
			{
				PropertyEditor parentPropertyEditor = this.ParentPropertyEditor;
				bool flag = parentPropertyEditor != null && parentPropertyEditor.IsSchemaTypeField && !confirmed;
				if (flag)
				{
					ValueEditor.<>c__DisplayClass20_1 CS$<>8__locals2 = new ValueEditor.<>c__DisplayClass20_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.parentPath = this.Path.GetParent();
					CS$<>8__locals2.parentValue = (JObject)this.ConfigEditor.GetValue(CS$<>8__locals2.parentPath);
					bool flag2 = CS$<>8__locals2.parentValue != null && CS$<>8__locals2.parentValue.Count != 0 && (CS$<>8__locals2.parentValue.Count > 1 || !CS$<>8__locals2.parentValue.ContainsKey(this._parentSchema.TypePropertyKey));
					if (flag2)
					{
						this.ConfigEditor.AssetEditorOverlay.ConfirmationModal.Open(this.Desktop.Provider.GetText("ui.assetEditor.changeConfirmationModal.title", null, true), this.Desktop.Provider.GetText("ui.assetEditor.changeConfirmationModal.text", null, true), new Action(CS$<>8__locals2.<HandleChangeValue>g__Confirm|0), new Action(this.UpdateDisplayedValue), null, null, false);
					}
					else
					{
						CS$<>8__locals2.<HandleChangeValue>g__Confirm|0();
					}
				}
				else
				{
					ConfigEditor configEditor = this.ConfigEditor;
					PropertyPath path = this.Path;
					JToken value2 = CS$<>8__locals1.value;
					JToken value3 = this.Value;
					JToken previousValue = (value3 != null) ? value3.DeepClone() : null;
					CacheRebuildInfo cachesToRebuild = this.CachesToRebuild;
					configEditor.OnChangeValue(path, value2, previousValue, (cachesToRebuild != null) ? cachesToRebuild.Caches : null, withheldCommand, false, updateDisplayedValue);
				}
			}
		}

		// Token: 0x06005F3B RID: 24379 RVA: 0x001EB878 File Offset: 0x001E9A78
		protected void SubmitUpdateCommand()
		{
			this.ConfigEditor.SubmitPendingUpdateCommands();
		}

		// Token: 0x06005F3C RID: 24380 RVA: 0x001EB886 File Offset: 0x001E9A86
		public virtual void Focus()
		{
		}

		// Token: 0x06005F3D RID: 24381 RVA: 0x001EB889 File Offset: 0x001E9A89
		protected internal virtual void UpdateDisplayedValue()
		{
		}

		// Token: 0x06005F3E RID: 24382 RVA: 0x001EB88C File Offset: 0x001E9A8C
		public virtual void SetValue(JToken value)
		{
			bool flag = value != null && value.Type == 10;
			if (flag)
			{
				value = null;
			}
			this.Value = value;
			PropertyEditor parentPropertyEditor = this.ParentPropertyEditor;
			if (parentPropertyEditor != null)
			{
				parentPropertyEditor.UpdateAppearance();
			}
		}

		// Token: 0x06005F3F RID: 24383 RVA: 0x001EB8CD File Offset: 0x001E9ACD
		public virtual void SetValueRecursively(JToken value)
		{
			this.SetValue(value);
		}

		// Token: 0x06005F40 RID: 24384 RVA: 0x001EB8D7 File Offset: 0x001E9AD7
		public virtual void UpdatePathRecursively(PropertyPath path)
		{
			this.Path = path;
		}

		// Token: 0x06005F41 RID: 24385 RVA: 0x001EB8E0 File Offset: 0x001E9AE0
		protected virtual bool ValidateType(JToken JToken)
		{
			return true;
		}

		// Token: 0x06005F42 RID: 24386 RVA: 0x001EB8E3 File Offset: 0x001E9AE3
		protected virtual bool IsValueEmptyOrDefault(JToken value)
		{
			return false;
		}

		// Token: 0x06005F43 RID: 24387 RVA: 0x001EB8E8 File Offset: 0x001E9AE8
		public virtual bool TryFindPropertyEditor(PropertyPath path, out PropertyEditor propertyEditor)
		{
			propertyEditor = null;
			return false;
		}

		// Token: 0x06005F44 RID: 24388 RVA: 0x001EB8FE File Offset: 0x001E9AFE
		public virtual void ClearDiagnosticsOfDescendants()
		{
		}

		// Token: 0x06005F45 RID: 24389 RVA: 0x001EB904 File Offset: 0x001E9B04
		public static ValueEditor CreateFromSchema(Element parent, SchemaNode schemaNode, PropertyPath path, PropertyEditor parentProperty, SchemaNode parentSchema, ConfigEditor configEditor, JToken value)
		{
			ValueEditor result;
			switch (schemaNode.Type)
			{
			case SchemaNode.NodeType.Dropdown:
				result = new DropdownEditor(parent.Desktop, parent, schemaNode, path, parentProperty, parentSchema, configEditor, value);
				break;
			case SchemaNode.NodeType.AssetIdDropdown:
				result = new AssetDropdownEditor(parent.Desktop, parent, schemaNode, path, parentProperty, parentSchema, configEditor, value);
				break;
			case SchemaNode.NodeType.AssetFileDropdown:
				result = new AssetFileSelectorEditor(parent.Desktop, parent, schemaNode, path, parentProperty, parentSchema, configEditor, value);
				break;
			case SchemaNode.NodeType.AssetReferenceOrInline:
				result = new AssetReferenceOrInlineEditor(parent.Desktop, parent, schemaNode, path, parentProperty, parentSchema, configEditor, value);
				break;
			case SchemaNode.NodeType.Text:
				result = new TextEditor(parent.Desktop, parent, schemaNode, path, parentProperty, parentSchema, configEditor, value);
				break;
			case SchemaNode.NodeType.Number:
				result = new NumberEditor(parent.Desktop, parent, schemaNode, path, parentProperty, parentSchema, configEditor, value);
				break;
			case SchemaNode.NodeType.ItemIcon:
				result = new IconEditor(parent.Desktop, parent, schemaNode, path, parentProperty, parentSchema, configEditor, value);
				break;
			case SchemaNode.NodeType.Checkbox:
				result = new CheckBoxEditor(parent.Desktop, parent, schemaNode, path, parentProperty, parentSchema, configEditor, value);
				break;
			case SchemaNode.NodeType.Color:
				result = new ColorEditor(parent.Desktop, parent, schemaNode, path, parentProperty, parentSchema, configEditor, value);
				break;
			case SchemaNode.NodeType.Timeline:
				result = new TimelineEditor(parent.Desktop, parent, schemaNode, path, parentProperty, parentSchema, configEditor, value);
				break;
			case SchemaNode.NodeType.WeightedTimeline:
				result = new WeightedTimelineEditor(parent.Desktop, parent, schemaNode, path, parentProperty, parentSchema, configEditor, value);
				break;
			case SchemaNode.NodeType.List:
				result = new ListEditor(parent.Desktop, parent, schemaNode, path, parentProperty, parentSchema, configEditor, value);
				break;
			case SchemaNode.NodeType.Map:
				result = new MapEditor(parent.Desktop, parent, schemaNode, path, parentProperty, parentSchema, configEditor, value);
				break;
			case SchemaNode.NodeType.Object:
				result = new ObjectEditor(parent.Desktop, parent, schemaNode, path, parentProperty, parentSchema, configEditor, value);
				break;
			case SchemaNode.NodeType.Source:
				result = new JsonEditor(parent.Desktop, parent, schemaNode, path, parentProperty, parentSchema, configEditor, value);
				break;
			default:
				result = new ReadOnlyEditor(parent.Desktop, parent, schemaNode, path, parentProperty, parentSchema, configEditor, value);
				break;
			}
			return result;
		}

		// Token: 0x04003B41 RID: 15169
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003B42 RID: 15170
		protected readonly SchemaNode _parentSchema;

		// Token: 0x04003B43 RID: 15171
		public readonly ConfigEditor ConfigEditor;

		// Token: 0x04003B44 RID: 15172
		public readonly SchemaNode Schema;

		// Token: 0x04003B45 RID: 15173
		public readonly PropertyEditor ParentPropertyEditor;

		// Token: 0x04003B46 RID: 15174
		public PropertyPath Path;

		// Token: 0x04003B48 RID: 15176
		public bool FilterCategory;

		// Token: 0x04003B49 RID: 15177
		public bool IsDetachedEditor;

		// Token: 0x04003B4A RID: 15178
		public CacheRebuildInfo CachesToRebuild;

		// Token: 0x02000FDC RID: 4060
		public class InvalidJsonTypeException : Exception
		{
			// Token: 0x060069E0 RID: 27104 RVA: 0x0021F630 File Offset: 0x0021D830
			public InvalidJsonTypeException(string path, JToken value, SchemaNode.NodeType type) : base(string.Format("Invalid JSON type at '{0}', got {1} for {2} editor field", path, value.Type, type))
			{
			}
		}

		// Token: 0x02000FDD RID: 4061
		public class InvalidValueException : Exception
		{
			// Token: 0x060069E1 RID: 27105 RVA: 0x0021F656 File Offset: 0x0021D856
			public InvalidValueException(string path, SchemaNode.NodeType type, Exception ex) : base(ex.Message)
			{
				this.DisplayMessage = string.Format("Invalid value for '{0}' with type {1}.", path, type);
			}

			// Token: 0x04004C32 RID: 19506
			public readonly string DisplayMessage;
		}
	}
}
