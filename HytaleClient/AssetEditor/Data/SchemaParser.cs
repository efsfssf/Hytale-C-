using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Data
{
	// Token: 0x02000BDC RID: 3036
	internal class SchemaParser
	{
		// Token: 0x170013E1 RID: 5089
		// (get) Token: 0x06005F9A RID: 24474 RVA: 0x001EE5AC File Offset: 0x001EC7AC
		public string CurrentPath
		{
			get
			{
				string text = "";
				while (this._keyStack.Count > 0)
				{
					string str = this._keyStack.Pop();
					bool flag = text != "";
					if (flag)
					{
						str += ".";
					}
					text = str + text;
				}
				return text;
			}
		}

		// Token: 0x06005F9B RID: 24475 RVA: 0x001EE60C File Offset: 0x001EC80C
		public SchemaParser(bool collectKeyStack)
		{
			this._collectKeyStack = collectKeyStack;
		}

		// Token: 0x06005F9C RID: 24476 RVA: 0x001EE628 File Offset: 0x001EC828
		public SchemaNode Parse(JObject json, Dictionary<string, SchemaNode> definitions)
		{
			SchemaNode schemaNode = new SchemaNode();
			bool flag = json.ContainsKey("$id");
			if (flag)
			{
				schemaNode.Id = (string)json["$id"];
			}
			bool flag2 = json.ContainsKey("definitions");
			if (flag2)
			{
				foreach (KeyValuePair<string, JToken> keyValuePair in ((JObject)json["definitions"]))
				{
					bool collectKeyStack = this._collectKeyStack;
					if (collectKeyStack)
					{
						this.PushKey("[Definition=" + keyValuePair.Key + "]");
					}
					SchemaNode schemaNode2 = this.ParseNode((JObject)keyValuePair.Value, definitions);
					bool collectKeyStack2 = this._collectKeyStack;
					if (collectKeyStack2)
					{
						this.PopKey();
					}
					definitions.Add("#/definitions/" + keyValuePair.Key, schemaNode2);
					bool flag3 = schemaNode2.Id != null;
					if (flag3)
					{
						definitions.Add("#" + schemaNode2.Id, schemaNode2);
					}
				}
			}
			this.SetupMeta(schemaNode, json);
			this.TrySetupSchema(schemaNode, json, definitions);
			Debug.Assert(this._keyStack.Count == 0);
			return schemaNode;
		}

		// Token: 0x06005F9D RID: 24477 RVA: 0x001EE788 File Offset: 0x001EC988
		private SchemaNode ParseNode(JObject json, Dictionary<string, SchemaNode> definitions)
		{
			SchemaNode schemaNode = new SchemaNode();
			this.SetupMeta(schemaNode, json);
			this.TrySetupSchema(schemaNode, json, definitions);
			return schemaNode;
		}

		// Token: 0x06005F9E RID: 24478 RVA: 0x001EE7B4 File Offset: 0x001EC9B4
		private bool TrySetupSchema(SchemaNode node, JObject json, Dictionary<string, SchemaNode> definitions)
		{
			bool flag = json.ContainsKey("$ref");
			bool result;
			if (flag)
			{
				node.SchemaReference = (string)json["$ref"];
				result = true;
			}
			else
			{
				bool flag2 = json.ContainsKey("const");
				if (flag2)
				{
					node.Const = json["const"];
					result = true;
				}
				else
				{
					JObject jobject = (JObject)json["hytale"];
					bool flag3 = jobject != null && jobject.ContainsKey("type");
					if (flag3)
					{
						string text = (string)jobject["type"];
						string a = text;
						if (a == "Color")
						{
							node.Type = SchemaNode.NodeType.Color;
							node.ColorFormat = ColorPicker.ColorFormat.Rgb;
							return true;
						}
						if (a == "ColorAlpha")
						{
							node.Type = SchemaNode.NodeType.Color;
							node.ColorFormat = ColorPicker.ColorFormat.Rgba;
							return true;
						}
						if (a == "ColorShort")
						{
							node.Type = SchemaNode.NodeType.Color;
							node.ColorFormat = ColorPicker.ColorFormat.RgbShort;
							return true;
						}
					}
					bool flag4 = json.ContainsKey("anyOf");
					if (flag4)
					{
						bool flag5 = json.ContainsKey("hytaleAssetRef");
						if (flag5)
						{
							this.SetupAssetReferenceOrInline(node, json, definitions);
							result = true;
						}
						else
						{
							bool flag6 = json.ContainsKey("hytaleSchemaTypeField");
							if (flag6)
							{
								this.SetupSchemaTypeField(node, json, definitions);
								result = true;
							}
							else
							{
								result = this.TrySetupFromAnyOf(node, (JArray)json["anyOf"], definitions);
							}
						}
					}
					else
					{
						bool flag7 = json.ContainsKey("type");
						if (flag7)
						{
							bool flag8 = json["type"].Type == 2;
							if (flag8)
							{
								foreach (JToken jtoken in ((JArray)json["type"]))
								{
									string text2 = (string)jtoken;
									bool flag9 = text2 == "null";
									if (!flag9)
									{
										bool flag10 = this.TrySetupNodeFromType(node, text2, json, definitions);
										if (flag10)
										{
											return true;
										}
									}
								}
							}
							else
							{
								bool flag11 = this.TrySetupNodeFromType(node, (string)json["type"], json, definitions);
								if (flag11)
								{
									return true;
								}
							}
						}
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06005F9F RID: 24479 RVA: 0x001EEA1C File Offset: 0x001ECC1C
		private void SetupMeta(SchemaNode node, JObject json)
		{
			bool flag = json.ContainsKey("default");
			if (flag)
			{
				node.DefaultValue = json["default"];
			}
			bool flag2 = json.ContainsKey("description");
			if (flag2)
			{
				node.Description = (string)json["description"];
			}
			JObject jobject = (JObject)json["hytale"];
			bool flag3 = jobject != null;
			if (flag3)
			{
				bool flag4 = jobject.ContainsKey("uiPropertyTitle");
				if (flag4)
				{
					node.Title = (string)jobject["uiPropertyTitle"];
				}
				bool flag5 = jobject.ContainsKey("uiSectionStart");
				if (flag5)
				{
					node.SectionStart = (string)jobject["uiSectionStart"];
				}
				bool flag6 = jobject.ContainsKey("uiCollapsedByDefault");
				if (flag6)
				{
					node.IsCollapsedByDefault = (bool)jobject["uiCollapsedByDefault"];
				}
				bool flag7 = jobject.ContainsKey("inheritsProperty");
				if (flag7)
				{
					node.InheritsProperty = (bool)jobject["inheritsProperty"];
				}
				bool flag8 = jobject.ContainsKey("mergesProperties");
				if (flag8)
				{
					node.MergesProperties = (bool)jobject["mergesProperties"];
				}
				bool flag9 = jobject.ContainsKey("allowEmptyObject");
				if (flag9)
				{
					node.AllowEmptyObject = (bool)jobject["allowEmptyObject"];
				}
				bool flag10 = jobject.ContainsKey("uiDisplayMode");
				if (flag10)
				{
					string text = (string)jobject["uiDisplayMode"];
					string a = text;
					if (!(a == "Hidden"))
					{
						if (a == "Compact")
						{
							node.DisplayCompact = true;
						}
					}
					else
					{
						node.IsHidden = true;
					}
				}
				bool flag11 = jobject.ContainsKey("uiRebuildCaches");
				if (flag11)
				{
					JArray jarray = (JArray)jobject["uiRebuildCaches"];
					node.RebuildCaches = new AssetEditorRebuildCaches();
					foreach (JToken jtoken in jarray)
					{
						string text2 = (string)jtoken;
						string a2 = text2;
						if (!(a2 == "BlockTextures"))
						{
							if (!(a2 == "Models"))
							{
								if (!(a2 == "ModelTextures"))
								{
									if (!(a2 == "MapGeometry"))
									{
										if (a2 == "ItemIcons")
										{
											node.RebuildCaches.ItemIcons = true;
										}
									}
									else
									{
										node.RebuildCaches.MapGeometry = true;
									}
								}
								else
								{
									node.RebuildCaches.ModelTextures = true;
								}
							}
							else
							{
								node.RebuildCaches.Models = true;
							}
						}
						else
						{
							node.RebuildCaches.BlockTextures = true;
						}
					}
					node.RebuildCachesForChildProperties = (bool)jobject["uiRebuildCachesForChildProperties"];
				}
			}
		}

		// Token: 0x06005FA0 RID: 24480 RVA: 0x001EECFC File Offset: 0x001ECEFC
		private bool TrySetupNodeFromType(SchemaNode node, string type, JObject json, Dictionary<string, SchemaNode> definitions)
		{
			bool result;
			if (!(type == "string"))
			{
				if (!(type == "integer") && !(type == "number"))
				{
					if (!(type == "boolean"))
					{
						if (!(type == "object"))
						{
							if (!(type == "array"))
							{
								result = false;
							}
							else
							{
								this.SetupArray(node, json, definitions);
								result = true;
							}
						}
						else
						{
							this.SetupObject(node, json, definitions);
							result = true;
						}
					}
					else
					{
						node.Type = SchemaNode.NodeType.Checkbox;
						node.DefaultValue = false;
						result = true;
					}
				}
				else
				{
					this.SetupNumber(node, json, type);
					result = true;
				}
			}
			else
			{
				this.SetupString(node, json);
				result = true;
			}
			return result;
		}

		// Token: 0x06005FA1 RID: 24481 RVA: 0x001EEDB8 File Offset: 0x001ECFB8
		private bool TrySetupFromAnyOf(SchemaNode node, JArray anyOf, Dictionary<string, SchemaNode> definitions)
		{
			for (int i = 0; i < anyOf.Count; i++)
			{
				JToken jtoken = anyOf[i];
				JObject json = (JObject)jtoken;
				bool collectKeyStack = this._collectKeyStack;
				if (collectKeyStack)
				{
					this.PushKey("[AnyOf=" + i.ToString() + "]");
				}
				bool flag = this.TrySetupSchema(node, json, definitions);
				if (flag)
				{
					bool collectKeyStack2 = this._collectKeyStack;
					if (collectKeyStack2)
					{
						this.PopKey();
					}
					this.SetupMeta(node, json);
					return true;
				}
				bool collectKeyStack3 = this._collectKeyStack;
				if (collectKeyStack3)
				{
					this.PopKey();
				}
			}
			return false;
		}

		// Token: 0x06005FA2 RID: 24482 RVA: 0x001EEE68 File Offset: 0x001ED068
		private void SetupNumber(SchemaNode node, JObject json, string type)
		{
			SchemaParser.<>c__DisplayClass21_0 CS$<>8__locals1;
			CS$<>8__locals1.json = json;
			node.Type = SchemaNode.NodeType.Number;
			node.MaxDecimalPlaces = ((type == "number") ? 3 : 0);
			double num = (node.MaxDecimalPlaces == 0) ? 1.0 : 0.001;
			bool flag = SchemaParser.<SetupNumber>g__HasDecimal|21_0("minimum", ref CS$<>8__locals1);
			if (flag)
			{
				node.Min = new double?((double)CS$<>8__locals1.json["minimum"]);
			}
			bool flag2 = SchemaParser.<SetupNumber>g__HasDecimal|21_0("exclusiveMinimum", ref CS$<>8__locals1);
			if (flag2)
			{
				node.Min = new double?((double)CS$<>8__locals1.json["exclusiveMinimum"] + num);
			}
			bool flag3 = SchemaParser.<SetupNumber>g__HasDecimal|21_0("maximum", ref CS$<>8__locals1);
			if (flag3)
			{
				node.Max = new double?((double)CS$<>8__locals1.json["maximum"]);
			}
			bool flag4 = SchemaParser.<SetupNumber>g__HasDecimal|21_0("exclusiveMaximum", ref CS$<>8__locals1);
			if (flag4)
			{
				node.Max = new double?((double)CS$<>8__locals1.json["exclusiveMaximum"] - num);
			}
			JToken jtoken = CS$<>8__locals1.json["hytale"];
			JObject jobject = (JObject)((jtoken != null) ? jtoken["uiEditorComponent"] : null);
			bool flag5 = jobject != null && (string)jobject["component"] == SchemaNode.NodeType.Number.ToString();
			if (flag5)
			{
				bool flag6 = jobject.ContainsKey("maxDecimalPlaces");
				if (flag6)
				{
					node.MaxDecimalPlaces = (int)jobject["maxDecimalPlaces"];
				}
				bool flag7 = jobject.ContainsKey("suffix");
				if (flag7)
				{
					node.Suffix = (string)jobject["suffix"];
				}
				bool flag8 = jobject.ContainsKey("step");
				if (flag8)
				{
					node.Step = new double?((double)jobject["step"]);
				}
			}
		}

		// Token: 0x06005FA3 RID: 24483 RVA: 0x001EF060 File Offset: 0x001ED260
		private void SetupString(SchemaNode node, JObject obj)
		{
			JToken jtoken = obj["hytale"];
			JObject jobject = (JObject)((jtoken != null) ? jtoken["uiEditorComponent"] : null);
			bool flag = obj.ContainsKey("oneOf");
			if (flag)
			{
				node.Type = SchemaNode.NodeType.Dropdown;
				JArray jarray = (JArray)obj["oneOf"];
				node.Enum = new string[jarray.Count];
				node.EnumTitles = new string[jarray.Count];
				node.EnumDescriptions = new string[jarray.Count];
				for (int i = 0; i < node.Enum.Length; i++)
				{
					JObject jobject2 = (JObject)jarray[i];
					node.Enum[i] = (string)jobject2["const"];
					bool flag2 = jobject2.ContainsKey("title");
					if (flag2)
					{
						node.EnumTitles[i] = (string)jobject2["title"];
					}
					bool flag3 = jobject2.ContainsKey("description");
					if (flag3)
					{
						node.EnumDescriptions[i] = (string)jobject2["description"];
					}
				}
			}
			else
			{
				bool flag4 = obj.ContainsKey("enum");
				if (flag4)
				{
					node.Type = SchemaNode.NodeType.Dropdown;
					node.Enum = Enumerable.ToArray<string>(Enumerable.Select<JToken, string>((JArray)obj["enum"], (JToken a) => a.ToString()));
					node.EnumTitles = new string[node.Enum.Length];
					node.EnumDescriptions = new string[node.Enum.Length];
				}
				else
				{
					bool flag5 = jobject != null && (string)jobject["component"] == SchemaNode.NodeType.Dropdown.ToString();
					if (flag5)
					{
						node.Type = SchemaNode.NodeType.Dropdown;
						node.DataSet = (string)jobject["dataSet"];
					}
					else
					{
						bool flag6 = jobject != null && (string)jobject["component"] == SchemaNode.NodeType.Text.ToString();
						if (flag6)
						{
							node.Type = SchemaNode.NodeType.Text;
							node.DataSet = (string)jobject["dataSet"];
						}
						else
						{
							bool flag7 = obj.ContainsKey("hytaleAssetRef");
							if (flag7)
							{
								node.Type = SchemaNode.NodeType.AssetIdDropdown;
								node.AssetType = (string)obj["hytaleAssetRef"];
							}
							else
							{
								bool flag8 = obj.ContainsKey("hytaleParent");
								if (flag8)
								{
									node.Type = SchemaNode.NodeType.AssetIdDropdown;
									node.AssetType = (string)obj["hytaleParent"]["type"];
									node.IsParentProperty = true;
								}
								else
								{
									bool flag9 = obj.ContainsKey("hytaleCommonAsset");
									if (flag9)
									{
										node.Type = SchemaNode.NodeType.AssetFileDropdown;
										JObject jobject3 = (JObject)obj["hytaleCommonAsset"];
										bool flag10 = jobject3.ContainsKey("requiredRoots");
										if (flag10)
										{
											JArray jarray2 = (JArray)jobject3["requiredRoots"];
											node.AllowedDirectories = new string[jarray2.Count];
											for (int j = 0; j < jarray2.Count; j++)
											{
												string text = (string)jarray2[j];
												bool flag11 = !text.StartsWith("/");
												if (flag11)
												{
													text = "/" + text;
												}
												bool flag12 = !text.EndsWith("/");
												if (flag12)
												{
													text += "/";
												}
												node.AllowedDirectories[j] = text;
											}
										}
										bool flag13 = jobject3.ContainsKey("requiredExtension");
										if (flag13)
										{
											node.AllowedFileExtensions = new string[]
											{
												(string)jobject3["requiredExtension"]
											};
										}
										bool flag14 = jobject3.ContainsKey("isUIAsset");
										if (flag14)
										{
											node.IsUITexture = (bool)jobject3["isUIAsset"];
										}
										bool flag15 = jobject != null && (string)jobject["component"] == SchemaNode.NodeType.ItemIcon.ToString();
										if (flag15)
										{
											node.Type = SchemaNode.NodeType.ItemIcon;
										}
									}
									else
									{
										node.Type = SchemaNode.NodeType.Text;
									}
								}
							}
						}
					}
				}
			}
			bool flag16 = obj.ContainsKey("maxLength");
			if (flag16)
			{
				node.MaxLength = (int)obj["maxLength"];
			}
		}

		// Token: 0x06005FA4 RID: 24484 RVA: 0x001EF4F4 File Offset: 0x001ED6F4
		private void SetupArray(SchemaNode node, JObject obj, Dictionary<string, SchemaNode> definitions)
		{
			bool flag = obj["items"].Type == 2;
			if (flag)
			{
				node.Value = this.ParseNode((JObject)obj["items"][0], definitions);
			}
			else
			{
				node.Value = this.ParseNode((JObject)obj["items"], definitions);
			}
			node.Type = SchemaNode.NodeType.List;
			bool flag2 = node.Value.DefaultValue == null;
			if (flag2)
			{
				node.Value.DefaultValue = SchemaParser.GetDefaultValue(node.Value);
			}
			JToken jtoken = obj["hytale"];
			JObject jobject = (JObject)((jtoken != null) ? jtoken["uiEditorComponent"] : null);
			bool flag3 = jobject != null && (string)jobject["component"] == SchemaNode.NodeType.Timeline.ToString();
			if (flag3)
			{
				node.Type = SchemaNode.NodeType.Timeline;
			}
		}

		// Token: 0x06005FA5 RID: 24485 RVA: 0x001EF5F4 File Offset: 0x001ED7F4
		private void SetupObject(SchemaNode node, JObject obj, Dictionary<string, SchemaNode> definitions)
		{
			bool flag = obj.ContainsKey("additionalProperties") && obj["additionalProperties"].Type == 1;
			if (flag)
			{
				node.Type = SchemaNode.NodeType.Map;
				node.Value = this.ParseNode((JObject)obj["additionalProperties"], definitions);
				bool flag2 = node.Value.DefaultValue == null;
				if (flag2)
				{
					node.Value.DefaultValue = SchemaParser.GetDefaultValue(node.Value);
				}
				bool flag3 = obj.ContainsKey("propertyNames");
				if (flag3)
				{
					JObject jobject = (JObject)obj["propertyNames"];
					jobject["type"] = "string";
					node.Key = this.ParseNode(jobject, definitions);
				}
				else
				{
					node.Key = new SchemaNode
					{
						Type = SchemaNode.NodeType.Text
					};
				}
				JToken jtoken = obj["hytale"];
				JObject jobject2 = (JObject)((jtoken != null) ? jtoken["uiEditorComponent"] : null);
				bool flag4 = jobject2 != null && (string)jobject2["component"] == SchemaNode.NodeType.WeightedTimeline.ToString();
				if (flag4)
				{
					node.Type = SchemaNode.NodeType.WeightedTimeline;
				}
			}
			else
			{
				bool flag5 = (!obj.ContainsKey("additionalProperties") || !(bool)obj["additionalProperties"]) && !obj.ContainsKey("properties");
				if (flag5)
				{
					node.Type = SchemaNode.NodeType.Source;
				}
				else
				{
					node.Type = SchemaNode.NodeType.Object;
					node.Properties = new Dictionary<string, SchemaNode>();
					bool flag6 = obj.ContainsKey("properties");
					if (flag6)
					{
						foreach (KeyValuePair<string, JToken> keyValuePair in Extensions.Value<JObject>(obj["properties"]))
						{
							JObject json = (JObject)keyValuePair.Value;
							bool collectKeyStack = this._collectKeyStack;
							if (collectKeyStack)
							{
								this.PushKey("[Prop=" + keyValuePair.Key + "]");
							}
							SchemaNode value = this.ParseNode(json, definitions);
							bool collectKeyStack2 = this._collectKeyStack;
							if (collectKeyStack2)
							{
								this.PopKey();
							}
							node.Properties.Add(keyValuePair.Key, value);
						}
					}
				}
			}
		}

		// Token: 0x06005FA6 RID: 24486 RVA: 0x001EF868 File Offset: 0x001EDA68
		private void SetupAssetReferenceOrInline(SchemaNode node, JObject obj, Dictionary<string, SchemaNode> definitions)
		{
			node.AssetType = (string)obj["hytaleAssetRef"];
			node.Type = SchemaNode.NodeType.AssetReferenceOrInline;
			foreach (JToken jtoken in obj["anyOf"])
			{
				bool flag = (string)jtoken["type"] == "string";
				if (!flag)
				{
					node.Value = this.ParseNode((JObject)jtoken, definitions);
					break;
				}
			}
		}

		// Token: 0x06005FA7 RID: 24487 RVA: 0x001EF90C File Offset: 0x001EDB0C
		private void SetupSchemaTypeField(SchemaNode node, JObject obj, Dictionary<string, SchemaNode> definitions)
		{
			node.Type = SchemaNode.NodeType.Object;
			JObject jobject = (JObject)obj["hytaleSchemaTypeField"];
			JArray jarray = (JArray)jobject["values"];
			JArray jarray2 = (JArray)obj["anyOf"];
			node.TypePropertyKey = (string)jobject["property"];
			node.Value = new SchemaNode
			{
				Type = SchemaNode.NodeType.Dropdown,
				Enum = new string[jarray2.Count],
				EnumTitles = new string[jarray2.Count],
				EnumDescriptions = new string[jarray2.Count]
			};
			node.TypeSchemas = new SchemaNode[jarray2.Count];
			for (int i = 0; i < jarray2.Count; i++)
			{
				bool collectKeyStack = this._collectKeyStack;
				if (collectKeyStack)
				{
					this.PushKey("[Type=" + i.ToString() + "]");
				}
				node.TypeSchemas[i] = this.ParseNode((JObject)jarray2[i], definitions);
				bool collectKeyStack2 = this._collectKeyStack;
				if (collectKeyStack2)
				{
					this.PopKey();
				}
				node.Value.Enum[i] = (string)jarray[i];
			}
			bool flag = jobject.ContainsKey("hasParentProperty");
			if (flag)
			{
				node.HasParentProperty = (bool)jobject["hasParentProperty"];
			}
			bool flag2 = jobject.ContainsKey("defaultValue");
			if (flag2)
			{
				node.DefaultTypeSchema = (string)jobject["defaultValue"];
			}
		}

		// Token: 0x06005FA8 RID: 24488 RVA: 0x001EFAA9 File Offset: 0x001EDCA9
		private void PushKey(string key)
		{
			this._keyStack.Push(key);
		}

		// Token: 0x06005FA9 RID: 24489 RVA: 0x001EFAB8 File Offset: 0x001EDCB8
		private void PopKey()
		{
			this._keyStack.Pop();
		}

		// Token: 0x06005FAA RID: 24490 RVA: 0x001EFAC8 File Offset: 0x001EDCC8
		public static JToken GetDefaultValue(SchemaNode node)
		{
			bool flag = node.DefaultValue != null;
			JToken result;
			if (flag)
			{
				result = node.DefaultValue;
			}
			else
			{
				switch (node.Type)
				{
				case SchemaNode.NodeType.Dropdown:
				{
					bool flag2 = node.Enum == null || node.Enum.Length == 0;
					if (flag2)
					{
						return "";
					}
					return node.Enum[0];
				}
				case SchemaNode.NodeType.Text:
					return "";
				case SchemaNode.NodeType.Number:
					return 0;
				case SchemaNode.NodeType.Checkbox:
					return false;
				case SchemaNode.NodeType.Color:
				{
					ColorPicker.ColorFormat colorFormat = node.ColorFormat;
					ColorPicker.ColorFormat colorFormat2 = colorFormat;
					if (colorFormat2 == ColorPicker.ColorFormat.Rgba)
					{
						return "rgba(#ffffff, 1)";
					}
					if (colorFormat2 != ColorPicker.ColorFormat.RgbShort)
					{
						return "#ffffff";
					}
					return "#fffff";
				}
				case SchemaNode.NodeType.Timeline:
				case SchemaNode.NodeType.List:
					return new JArray();
				case SchemaNode.NodeType.WeightedTimeline:
				case SchemaNode.NodeType.Map:
				case SchemaNode.NodeType.Object:
				case SchemaNode.NodeType.Source:
					return new JObject();
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06005FAB RID: 24491 RVA: 0x001EFBF1 File Offset: 0x001EDDF1
		[CompilerGenerated]
		internal static bool <SetupNumber>g__HasDecimal|21_0(string key, ref SchemaParser.<>c__DisplayClass21_0 A_1)
		{
			JToken jtoken = A_1.json[key];
			bool result;
			if (jtoken == null || jtoken.Type != 6)
			{
				JToken jtoken2 = A_1.json[key];
				result = (jtoken2 != null && jtoken2.Type == 7);
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x04003BAE RID: 15278
		private const string TypeNumber = "number";

		// Token: 0x04003BAF RID: 15279
		private const string TypeInt = "integer";

		// Token: 0x04003BB0 RID: 15280
		private const string TypeString = "string";

		// Token: 0x04003BB1 RID: 15281
		private const string TypeNull = "null";

		// Token: 0x04003BB2 RID: 15282
		private const string TypeObject = "object";

		// Token: 0x04003BB3 RID: 15283
		private const string TypeArray = "array";

		// Token: 0x04003BB4 RID: 15284
		private const string TypeBoolean = "boolean";

		// Token: 0x04003BB5 RID: 15285
		private const string CustomTypeColor = "Color";

		// Token: 0x04003BB6 RID: 15286
		private const string CustomTypeColorAlpha = "ColorAlpha";

		// Token: 0x04003BB7 RID: 15287
		private const string CustomTypeColorShort = "ColorShort";

		// Token: 0x04003BB8 RID: 15288
		private readonly Stack<string> _keyStack = new Stack<string>();

		// Token: 0x04003BB9 RID: 15289
		private readonly bool _collectKeyStack;
	}
}
