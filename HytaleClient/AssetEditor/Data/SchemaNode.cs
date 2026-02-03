using System;
using System.Collections.Generic;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Data
{
	// Token: 0x02000BDB RID: 3035
	public class SchemaNode
	{
		// Token: 0x06005F96 RID: 24470 RVA: 0x001EE018 File Offset: 0x001EC218
		private Dictionary<string, SchemaNode> DeepCloneProperties()
		{
			Dictionary<string, SchemaNode> dictionary = new Dictionary<string, SchemaNode>();
			foreach (KeyValuePair<string, SchemaNode> keyValuePair in this.Properties)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value.Clone(false));
			}
			return dictionary;
		}

		// Token: 0x06005F97 RID: 24471 RVA: 0x001EE090 File Offset: 0x001EC290
		public SchemaNode Clone(bool deep = false)
		{
			SchemaNode schemaNode = new SchemaNode();
			schemaNode.Id = this.Id;
			schemaNode.Type = this.Type;
			schemaNode.Const = this.Const;
			schemaNode.Title = this.Title;
			schemaNode.Description = this.Description;
			schemaNode.DefaultValue = this.DefaultValue;
			schemaNode.SchemaReference = this.SchemaReference;
			schemaNode.IsHidden = this.IsHidden;
			schemaNode.SectionStart = this.SectionStart;
			schemaNode.InheritsProperty = this.InheritsProperty;
			schemaNode.IsParentProperty = this.IsParentProperty;
			schemaNode.RebuildCaches = this.RebuildCaches;
			schemaNode.RebuildCachesForChildProperties = this.RebuildCachesForChildProperties;
			schemaNode.Properties = ((this.Properties != null) ? (deep ? this.DeepCloneProperties() : new Dictionary<string, SchemaNode>(this.Properties)) : null);
			schemaNode.MergesProperties = this.MergesProperties;
			SchemaNode value;
			if (!deep)
			{
				value = this.Value;
			}
			else
			{
				SchemaNode value2 = this.Value;
				value = ((value2 != null) ? value2.Clone(false) : null);
			}
			schemaNode.Value = value;
			SchemaNode key;
			if (!deep)
			{
				key = this.Key;
			}
			else
			{
				SchemaNode key2 = this.Key;
				key = ((key2 != null) ? key2.Clone(false) : null);
			}
			schemaNode.Key = key;
			schemaNode.IsCollapsedByDefault = this.IsCollapsedByDefault;
			schemaNode.DisplayCompact = this.DisplayCompact;
			schemaNode.AllowEmptyObject = this.AllowEmptyObject;
			schemaNode.TypePropertyKey = this.TypePropertyKey;
			schemaNode.TypeSchemas = ((this.TypeSchemas != null) ? (deep ? SchemaNode.DeepCloneSchemaArray(this.TypeSchemas) : this.TypeSchemas) : null);
			schemaNode.HasParentProperty = this.HasParentProperty;
			schemaNode.Enum = this.Enum;
			schemaNode.EnumTitles = this.EnumTitles;
			schemaNode.EnumDescriptions = this.EnumDescriptions;
			schemaNode.DataSet = this.DataSet;
			schemaNode.MaxLength = this.MaxLength;
			schemaNode.Step = this.Step;
			schemaNode.Min = this.Min;
			schemaNode.Max = this.Max;
			schemaNode.Suffix = this.Suffix;
			schemaNode.MaxDecimalPlaces = this.MaxDecimalPlaces;
			schemaNode.AssetType = this.AssetType;
			schemaNode.AllowedDirectories = this.AllowedDirectories;
			schemaNode.AllowedFileExtensions = this.AllowedFileExtensions;
			schemaNode.ColorFormat = this.ColorFormat;
			return schemaNode;
		}

		// Token: 0x06005F98 RID: 24472 RVA: 0x001EE2CC File Offset: 0x001EC4CC
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}, {8}: {9}, {10}: {11}, {12}: {13}, {14}: {15}, {16}: {17}, {18}: {19}, {20}: {21}, {22}: {23}, {24}: {25}, {26}: {27}, {28}: {29}, {30}: {31}, {32}: {33}, {34}: {35}, {36}: {37}, {38}: {39}, {40}: {41}, {42}: {43}, {44}: {45}, {46}: {47}, {48}: {49}, {50}: {51}, {52}: {53}, {54}: {55}, {56}: {57}, {58}: {59}, {60}: {61}", new object[]
			{
				"Id",
				this.Id,
				"Type",
				this.Type,
				"Const",
				this.Const,
				"Title",
				this.Title,
				"Description",
				this.Description,
				"DefaultValue",
				this.DefaultValue,
				"SchemaReference",
				this.SchemaReference,
				"IsHidden",
				this.IsHidden,
				"Properties",
				this.Properties,
				"Value",
				this.Value,
				"Key",
				this.Key,
				"IsCollapsedByDefault",
				this.IsCollapsedByDefault,
				"DisplayCompact",
				this.DisplayCompact,
				"AllowEmptyObject",
				this.AllowEmptyObject,
				"TypePropertyKey",
				this.TypePropertyKey,
				"TypeSchemas",
				this.TypeSchemas,
				"HasParentProperty",
				this.HasParentProperty,
				"Enum",
				this.Enum,
				"EnumTitles",
				this.EnumTitles,
				"EnumDescriptions",
				this.EnumDescriptions,
				"DataSet",
				this.DataSet,
				"MaxLength",
				this.MaxLength,
				"Step",
				this.Step,
				"Min",
				this.Min,
				"Max",
				this.Max,
				"Suffix",
				this.Suffix,
				"MaxDecimalPlaces",
				this.MaxDecimalPlaces,
				"AssetType",
				this.AssetType,
				"AllowedFileExtensions",
				this.AllowedFileExtensions,
				"AllowedDirectories",
				this.AllowedDirectories,
				"ColorFormat",
				this.ColorFormat
			});
		}

		// Token: 0x06005F99 RID: 24473 RVA: 0x001EE570 File Offset: 0x001EC770
		private static SchemaNode[] DeepCloneSchemaArray(SchemaNode[] nodes)
		{
			SchemaNode[] array = new SchemaNode[nodes.Length];
			for (int i = 0; i < nodes.Length; i++)
			{
				array[i] = nodes[i].Clone(false);
			}
			return array;
		}

		// Token: 0x04003B87 RID: 15239
		public string Id;

		// Token: 0x04003B88 RID: 15240
		public SchemaNode.NodeType Type;

		// Token: 0x04003B89 RID: 15241
		public string Title;

		// Token: 0x04003B8A RID: 15242
		public string Description;

		// Token: 0x04003B8B RID: 15243
		public JToken DefaultValue;

		// Token: 0x04003B8C RID: 15244
		public JToken Const;

		// Token: 0x04003B8D RID: 15245
		public string SchemaReference;

		// Token: 0x04003B8E RID: 15246
		public bool IsHidden;

		// Token: 0x04003B8F RID: 15247
		public string SectionStart;

		// Token: 0x04003B90 RID: 15248
		public bool InheritsProperty;

		// Token: 0x04003B91 RID: 15249
		public bool IsParentProperty;

		// Token: 0x04003B92 RID: 15250
		public AssetEditorRebuildCaches RebuildCaches;

		// Token: 0x04003B93 RID: 15251
		public bool RebuildCachesForChildProperties;

		// Token: 0x04003B94 RID: 15252
		public Dictionary<string, SchemaNode> Properties;

		// Token: 0x04003B95 RID: 15253
		public bool MergesProperties;

		// Token: 0x04003B96 RID: 15254
		public SchemaNode Value;

		// Token: 0x04003B97 RID: 15255
		public SchemaNode Key;

		// Token: 0x04003B98 RID: 15256
		public bool IsCollapsedByDefault = true;

		// Token: 0x04003B99 RID: 15257
		public bool DisplayCompact;

		// Token: 0x04003B9A RID: 15258
		public bool AllowEmptyObject;

		// Token: 0x04003B9B RID: 15259
		public string TypePropertyKey;

		// Token: 0x04003B9C RID: 15260
		public SchemaNode[] TypeSchemas;

		// Token: 0x04003B9D RID: 15261
		public bool HasParentProperty;

		// Token: 0x04003B9E RID: 15262
		public string DefaultTypeSchema;

		// Token: 0x04003B9F RID: 15263
		public string[] Enum;

		// Token: 0x04003BA0 RID: 15264
		public string[] EnumTitles;

		// Token: 0x04003BA1 RID: 15265
		public string[] EnumDescriptions;

		// Token: 0x04003BA2 RID: 15266
		public string DataSet;

		// Token: 0x04003BA3 RID: 15267
		public int MaxLength;

		// Token: 0x04003BA4 RID: 15268
		public double? Step;

		// Token: 0x04003BA5 RID: 15269
		public double? Min;

		// Token: 0x04003BA6 RID: 15270
		public double? Max;

		// Token: 0x04003BA7 RID: 15271
		public string Suffix;

		// Token: 0x04003BA8 RID: 15272
		public int MaxDecimalPlaces;

		// Token: 0x04003BA9 RID: 15273
		public string AssetType;

		// Token: 0x04003BAA RID: 15274
		public string[] AllowedFileExtensions;

		// Token: 0x04003BAB RID: 15275
		public string[] AllowedDirectories;

		// Token: 0x04003BAC RID: 15276
		public bool IsUITexture;

		// Token: 0x04003BAD RID: 15277
		public ColorPicker.ColorFormat ColorFormat;

		// Token: 0x02000FE6 RID: 4070
		public enum NodeType
		{
			// Token: 0x04004C4E RID: 19534
			ReadOnly,
			// Token: 0x04004C4F RID: 19535
			Dropdown,
			// Token: 0x04004C50 RID: 19536
			AssetIdDropdown,
			// Token: 0x04004C51 RID: 19537
			AssetFileDropdown,
			// Token: 0x04004C52 RID: 19538
			AssetReferenceOrInline,
			// Token: 0x04004C53 RID: 19539
			Text,
			// Token: 0x04004C54 RID: 19540
			Number,
			// Token: 0x04004C55 RID: 19541
			ItemIcon,
			// Token: 0x04004C56 RID: 19542
			Checkbox,
			// Token: 0x04004C57 RID: 19543
			Color,
			// Token: 0x04004C58 RID: 19544
			Timeline,
			// Token: 0x04004C59 RID: 19545
			WeightedTimeline,
			// Token: 0x04004C5A RID: 19546
			List,
			// Token: 0x04004C5B RID: 19547
			Map,
			// Token: 0x04004C5C RID: 19548
			Object,
			// Token: 0x04004C5D RID: 19549
			Source
		}
	}
}
