using System;
using System.Collections.Generic;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BD1 RID: 3025
	internal class WeightedTimelineEditor : ValueEditor
	{
		// Token: 0x170013DC RID: 5084
		// (get) Token: 0x06005F54 RID: 24404 RVA: 0x001EBF44 File Offset: 0x001EA144
		// (set) Token: 0x06005F55 RID: 24405 RVA: 0x001EBF4C File Offset: 0x001EA14C
		public SchemaNode IdSchema { get; private set; }

		// Token: 0x06005F56 RID: 24406 RVA: 0x001EBF58 File Offset: 0x001EA158
		public WeightedTimelineEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent, schema, path, parentPropertyEditor, parentSchema, configEditor, value)
		{
		}

		// Token: 0x06005F57 RID: 24407 RVA: 0x001EBF90 File Offset: 0x001EA190
		protected override void Build()
		{
			base.Clear();
			this._numberFields.Clear();
			this._entryGroups.Clear();
			this._layoutMode = LayoutMode.Top;
			SchemaNode schemaNode = this.ConfigEditor.AssetEditorOverlay.ResolveSchemaInCurrentContext(this.Schema.Value.Value);
			this._weightSchema = this.ConfigEditor.AssetEditorOverlay.ResolveSchemaInCurrentContext(schemaNode.Properties["Weight"]);
			foreach (KeyValuePair<string, SchemaNode> keyValuePair in schemaNode.Properties)
			{
				bool flag = keyValuePair.Key == "Weight" || keyValuePair.Value.IsHidden;
				if (!flag)
				{
					this.IdSchema = this.ConfigEditor.AssetEditorOverlay.ResolveSchemaInCurrentContext(keyValuePair.Value);
					this._idPropertyKey = keyValuePair.Key;
					break;
				}
			}
			JObject jobject = ((JObject)base.Value) ?? new JObject();
			Dictionary<string, decimal[]> dictionary = new Dictionary<string, decimal[]>();
			foreach (KeyValuePair<string, JToken> keyValuePair2 in jobject)
			{
				int num;
				bool flag2 = !int.TryParse(keyValuePair2.Key, out num) || num < 0 || num >= 24;
				if (!flag2)
				{
					bool flag3 = keyValuePair2.Value == null || keyValuePair2.Value.Type != 2;
					if (!flag3)
					{
						JArray jarray = (JArray)keyValuePair2.Value;
						foreach (JToken jtoken in jarray)
						{
							bool flag4 = jtoken == null || jtoken.Type != 1;
							if (!flag4)
							{
								JToken jtoken2 = jtoken[this._idPropertyKey];
								decimal[] array;
								bool flag5 = !dictionary.TryGetValue((string)jtoken2, out array);
								if (flag5)
								{
									array = (dictionary[(string)jtoken2] = new decimal[24]);
									decimal num2 = (this._weightSchema.DefaultValue != null) ? ((decimal)this._weightSchema.DefaultValue) : 0m;
									for (int i = 0; i < array.Length; i++)
									{
										array[i] = num2;
									}
								}
								array[num] = JsonUtils.ConvertToDecimal(jtoken["Weight"]);
							}
						}
					}
				}
			}
			foreach (KeyValuePair<string, decimal[]> keyValuePair3 in dictionary)
			{
				this.BuildEntryGroup(keyValuePair3.Key, keyValuePair3.Value);
			}
		}

		// Token: 0x06005F58 RID: 24408 RVA: 0x001EC2F0 File Offset: 0x001EA4F0
		private void BuildEntryGroup(string entryId, decimal[] weights)
		{
			Group group = new Group(this.Desktop, this)
			{
				LayoutMode = LayoutMode.Top,
				Anchor = new Anchor
				{
					Vertical = new int?(5)
				}
			};
			this._entryGroups.Add(entryId, group);
			Group group2 = new Group(this.Desktop, group);
			group2.LayoutMode = LayoutMode.Left;
			group2.Anchor = new Anchor
			{
				Height = new int?(26),
				Width = new int?(672)
			};
			group2.Padding.Horizontal = new int?(8);
			group2.Background = new PatchStyle(UInt32Color.FromRGBA(0, 0, 0, 40));
			Group parent = group2;
			Label label = new Label(this.Desktop, parent);
			label.Text = entryId;
			label.Style = new LabelStyle
			{
				FontSize = 13f,
				VerticalAlignment = LabelStyle.LabelAlignment.Center,
				RenderBold = true
			};
			label.FlexWeight = 1;
			label.Padding = new Padding
			{
				Bottom = new int?(3)
			};
			Button button = new Button(this.Desktop, parent);
			button.Anchor = new Anchor
			{
				Width = new int?(16),
				Height = new int?(16)
			};
			button.Style = new Button.ButtonStyle
			{
				Default = new Button.ButtonStyleState
				{
					Background = new PatchStyle(PropertyLabel.IconRemove)
					{
						Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 38)
					}
				},
				Hovered = new Button.ButtonStyleState
				{
					Background = new PatchStyle(PropertyLabel.IconRemove)
					{
						Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 120)
					}
				},
				Pressed = new Button.ButtonStyleState
				{
					Background = new PatchStyle(PropertyLabel.IconRemove)
					{
						Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 100)
					}
				}
			};
			button.Activating = delegate()
			{
				this.HandleRemoveEntry(entryId);
			};
			Group parent2 = new Group(this.Desktop, group)
			{
				LayoutMode = LayoutMode.Center
			};
			this._numberFields[entryId] = new NumberField[24];
			for (int i = 0; i < weights.Length; i++)
			{
				Group container = new Group(this.Desktop, parent2)
				{
					Anchor = new Anchor
					{
						Height = new int?(24),
						Width = new int?(24),
						Horizontal = new int?(2)
					},
					LayoutMode = LayoutMode.Top
				};
				this._numberFields[entryId][i] = this.BuildNumberField(container, this._weightSchema, weights[i], entryId, i);
			}
		}

		// Token: 0x06005F59 RID: 24409 RVA: 0x001EC5F4 File Offset: 0x001EA7F4
		private NumberField BuildNumberField(Group container, SchemaNode valueSchema, JToken jValue, string id, int hour)
		{
			decimal num = (valueSchema.DefaultValue != null) ? JsonUtils.ConvertToDecimal(valueSchema.DefaultValue) : 0m;
			NumberFieldFormat numberFieldFormat = new NumberFieldFormat
			{
				DefaultValue = num,
				MaxDecimalPlaces = valueSchema.MaxDecimalPlaces,
				Suffix = valueSchema.Suffix
			};
			bool flag = valueSchema.Min != null;
			if (flag)
			{
				numberFieldFormat.MinValue = JsonUtils.ConvertToDecimal(valueSchema.Min.Value);
			}
			bool flag2 = valueSchema.Max != null;
			if (flag2)
			{
				numberFieldFormat.MaxValue = JsonUtils.ConvertToDecimal(valueSchema.Max.Value);
			}
			bool flag3 = valueSchema.Step != null;
			if (flag3)
			{
				numberFieldFormat.Step = JsonUtils.ConvertToDecimal(valueSchema.Step.Value);
			}
			return new NumberField(this.Desktop, container)
			{
				Value = ((jValue != null) ? JsonUtils.ConvertToDecimal(jValue) : num),
				Format = numberFieldFormat,
				Padding = new Padding
				{
					Left = new int?(3)
				},
				PlaceholderStyle = new InputFieldStyle
				{
					TextColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 100),
					FontSize = 12f
				},
				Style = new InputFieldStyle
				{
					FontSize = 12f
				},
				Blurred = delegate()
				{
					this.OnNumberFieldBlur(id, hour);
				},
				Validating = new Action(this.OnNumberFieldValidate),
				ValueChanged = delegate()
				{
					bool flag4 = valueSchema.MaxDecimalPlaces == 0;
					if (flag4)
					{
						int value = (int)this._numberFields[id][hour].Value;
						this.HandleWeightValueChanged(hour, id, value, true);
					}
					else
					{
						decimal value2 = this._numberFields[id][hour].Value;
						this.HandleWeightValueChanged(hour, id, value2, true);
					}
				},
				Decoration = new InputFieldDecorationStyle
				{
					Default = new InputFieldDecorationStyleState
					{
						Background = new PatchStyle(UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 20))
					},
					Focused = new InputFieldDecorationStyleState
					{
						OutlineSize = new int?(1),
						OutlineColor = new UInt32Color?(UInt32Color.FromRGBA(205, 240, 252, 206))
					}
				},
				Anchor = new Anchor
				{
					Width = new int?(24),
					Height = new int?(24)
				}
			};
		}

		// Token: 0x06005F5A RID: 24410 RVA: 0x001EC874 File Offset: 0x001EAA74
		public void HandleInsertEntry(string entryId)
		{
			JToken value = base.Value;
			JToken jtoken = (value != null) ? value.DeepClone() : null;
			JObject jobject = base.Value as JObject;
			bool flag = jobject != null;
			if (flag)
			{
				foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
				{
					JArray jarray = (JArray)keyValuePair.Value;
					JObject jobject2 = new JObject();
					jobject2.Add(this._idPropertyKey, entryId);
					jobject2.Add("Weight", (this._weightSchema.DefaultValue != null) ? JsonUtils.ConvertToDecimal(this._weightSchema.DefaultValue) : 0m);
					jarray.Add(jobject2);
				}
			}
			decimal[] array = new decimal[24];
			decimal num = (this._weightSchema.DefaultValue != null) ? JsonUtils.ConvertToDecimal(this._weightSchema.DefaultValue) : 0m;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = num;
			}
			this.BuildEntryGroup(entryId, array);
			ConfigEditor configEditor = this.ConfigEditor;
			PropertyPath path = this.Path;
			JToken value2 = base.Value;
			JToken previousValue = jtoken;
			CacheRebuildInfo cachesToRebuild = this.CachesToRebuild;
			configEditor.OnChangeValue(path, value2, previousValue, (cachesToRebuild != null) ? cachesToRebuild.Caches : null, false, false, false);
			this.ConfigEditor.Layout(null, true);
		}

		// Token: 0x06005F5B RID: 24411 RVA: 0x001EC9EC File Offset: 0x001EABEC
		public bool HasEntryId(string entryId)
		{
			return this._entryGroups.ContainsKey(entryId);
		}

		// Token: 0x06005F5C RID: 24412 RVA: 0x001EC9FC File Offset: 0x001EABFC
		private void HandleRemoveEntry(string entryId)
		{
			Group group = this._entryGroups[entryId];
			group.Parent.Remove(group);
			this._entryGroups.Remove(entryId);
			JToken value = base.Value;
			JToken jtoken = (value != null) ? value.DeepClone() : null;
			foreach (KeyValuePair<string, JToken> keyValuePair in ((JObject)base.Value))
			{
				JArray jarray = (JArray)keyValuePair.Value;
				foreach (JToken jtoken2 in jarray)
				{
					bool flag = (string)jtoken2[this._idPropertyKey] != entryId;
					if (!flag)
					{
						jtoken2.Remove();
						break;
					}
				}
			}
			ConfigEditor configEditor = this.ConfigEditor;
			PropertyPath path = this.Path;
			JToken value2 = base.Value;
			JToken previousValue = jtoken;
			CacheRebuildInfo cachesToRebuild = this.CachesToRebuild;
			configEditor.OnChangeValue(path, value2, previousValue, (cachesToRebuild != null) ? cachesToRebuild.Caches : null, false, false, false);
			this.ConfigEditor.Layout(null, true);
		}

		// Token: 0x06005F5D RID: 24413 RVA: 0x001ECB40 File Offset: 0x001EAD40
		private void HandleWeightValueChanged(int targetHour, string targetId, decimal weight, bool withheldCommand)
		{
			JToken value = base.Value;
			JToken jtoken = (value != null) ? value.DeepClone() : null;
			JToken jtoken2 = base.Value ?? new JObject();
			for (int i = 0; i < 24; i++)
			{
				bool flag = !this.ParentPropertyEditor.SyncPropertyChanges && i != targetHour;
				if (!flag)
				{
					bool flag2 = this.ParentPropertyEditor.SyncPropertyChanges && i != targetHour;
					if (flag2)
					{
						this._numberFields[targetId][i].Value = weight;
					}
					JArray jarray = (JArray)jtoken2[i.ToString()];
					bool flag3 = jarray == null;
					if (flag3)
					{
						jarray = (jtoken2[targetHour.ToString()] = new JArray());
					}
					bool flag4 = false;
					foreach (JToken jtoken3 in jarray)
					{
						bool flag5 = (string)jtoken3[this._idPropertyKey] == targetId;
						if (flag5)
						{
							jtoken3["Weight"] = weight;
							flag4 = true;
							break;
						}
					}
					bool flag6 = !flag4;
					if (flag6)
					{
						JArray jarray2 = jarray;
						JObject jobject = new JObject();
						jobject.Add(this._idPropertyKey, targetId);
						jobject.Add("Weight", weight);
						jarray2.Add(jobject);
					}
				}
			}
			bool flag7 = base.Value != jtoken2;
			ConfigEditor configEditor = this.ConfigEditor;
			PropertyPath path = this.Path;
			JToken value2 = jtoken2;
			JToken previousValue = jtoken;
			CacheRebuildInfo cachesToRebuild = this.CachesToRebuild;
			configEditor.OnChangeValue(path, value2, previousValue, (cachesToRebuild != null) ? cachesToRebuild.Caches : null, withheldCommand, false, false);
			bool flag8 = flag7;
			if (flag8)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06005F5E RID: 24414 RVA: 0x001ECD24 File Offset: 0x001EAF24
		private void OnNumberFieldValidate()
		{
			this.Validate();
			base.SubmitUpdateCommand();
		}

		// Token: 0x06005F5F RID: 24415 RVA: 0x001ECD38 File Offset: 0x001EAF38
		private void OnNumberFieldBlur(string id, int hour)
		{
			bool syncPropertyChanges = this.ParentPropertyEditor.SyncPropertyChanges;
			if (syncPropertyChanges)
			{
				bool flag = this._weightSchema.MaxDecimalPlaces == 0;
				if (flag)
				{
					this.HandleWeightValueChanged(hour, id, (int)this._numberFields[id][hour].Value, false);
				}
				else
				{
					this.HandleWeightValueChanged(hour, id, this._numberFields[id][hour].Value, false);
				}
			}
			else
			{
				base.SubmitUpdateCommand();
			}
		}

		// Token: 0x06005F60 RID: 24416 RVA: 0x001ECDBE File Offset: 0x001EAFBE
		protected override bool ValidateType(JToken value)
		{
			return value.Type == 1;
		}

		// Token: 0x06005F61 RID: 24417 RVA: 0x001ECDCC File Offset: 0x001EAFCC
		public override void SetValueRecursively(JToken value)
		{
			bool flag = value == base.Value;
			if (!flag)
			{
				base.SetValueRecursively(value);
				this.Build();
			}
		}

		// Token: 0x04003B50 RID: 15184
		private const int HoursPerDay = 24;

		// Token: 0x04003B51 RID: 15185
		private const string WeightPropertyKey = "Weight";

		// Token: 0x04003B52 RID: 15186
		private Dictionary<string, NumberField[]> _numberFields = new Dictionary<string, NumberField[]>();

		// Token: 0x04003B53 RID: 15187
		private Dictionary<string, Group> _entryGroups = new Dictionary<string, Group>();

		// Token: 0x04003B55 RID: 15189
		private string _idPropertyKey;

		// Token: 0x04003B56 RID: 15190
		private SchemaNode _weightSchema;
	}
}
