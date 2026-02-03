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
	// Token: 0x02000BCE RID: 3022
	internal class TimelineEditor : ValueEditor
	{
		// Token: 0x06005F21 RID: 24353 RVA: 0x001EA1AC File Offset: 0x001E83AC
		public TimelineEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent, schema, path, parentPropertyEditor, parentSchema, configEditor, value)
		{
			this._layoutMode = LayoutMode.Center;
			bool flag = base.IsMounted && !this._isEditorRegistered;
			if (flag)
			{
				this._isEditorRegistered = true;
				this.ConfigEditor.MountedTimelineEditors.Add(this);
			}
		}

		// Token: 0x06005F22 RID: 24354 RVA: 0x001EA230 File Offset: 0x001E8430
		protected override void Build()
		{
			SchemaNode schemaNode = this.ConfigEditor.AssetEditorOverlay.ResolveSchemaInCurrentContext(this.Schema.Value);
			bool flag = !schemaNode.Properties.ContainsKey("Hour");
			if (flag)
			{
				string str = "Timeline schema is invalid ";
				SchemaNode schemaNode2 = schemaNode;
				throw new Exception(str + ((schemaNode2 != null) ? schemaNode2.ToString() : null));
			}
			this._valueSchema = null;
			this._valuePropertyKey = null;
			this.Padding.Vertical = new int?(5);
			foreach (KeyValuePair<string, SchemaNode> keyValuePair in schemaNode.Properties)
			{
				bool flag2 = keyValuePair.Key == "Hour" || keyValuePair.Value.IsHidden;
				if (!flag2)
				{
					this._valueSchema = this.ConfigEditor.AssetEditorOverlay.ResolveSchemaInCurrentContext(keyValuePair.Value);
					this._valuePropertyKey = keyValuePair.Key;
					break;
				}
			}
			JToken[] array = new JToken[24];
			bool flag3 = base.Value != null;
			if (flag3)
			{
				JArray jarray = (JArray)base.Value;
				foreach (JToken jtoken in jarray)
				{
					bool flag4 = jtoken == null || jtoken.Type == 10;
					if (!flag4)
					{
						int num = (int)jtoken["Hour"];
						bool flag5 = num < 0 || num >= 24;
						if (!flag5)
						{
							array[num] = jtoken[this._valuePropertyKey];
						}
					}
				}
			}
			for (int i = 0; i < 24; i++)
			{
				Group group = new Group(this.Desktop, this)
				{
					Anchor = new Anchor
					{
						Height = new int?(24),
						Width = new int?(24),
						Horizontal = new int?(2)
					},
					LayoutMode = LayoutMode.Top,
					OutlineColor = UInt32Color.FromRGBA(125, 175, byte.MaxValue, byte.MaxValue)
				};
				this._containers[i] = group;
				bool flag6 = array[i] == null;
				if (flag6)
				{
					this.BuildPlaceholder(i);
				}
				else
				{
					SchemaNode.NodeType type = this._valueSchema.Type;
					SchemaNode.NodeType nodeType = type;
					if (nodeType != SchemaNode.NodeType.Number)
					{
						if (nodeType != SchemaNode.NodeType.Color)
						{
							throw new Exception(string.Format("TimelineEditor at {0} does not support value schema of type ", this.Path) + this._valueSchema.Type.ToString());
						}
						this.BuildColorPicker(array[i], i);
					}
					else
					{
						this.BuildNumberField(array[i], i);
					}
				}
			}
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.UpdateHighlightedHour(this.ConfigEditor.AssetEditorOverlay.WeatherDaytimeBar.CurrentHour);
			}
		}

		// Token: 0x06005F23 RID: 24355 RVA: 0x001EA554 File Offset: 0x001E8754
		protected override void OnMounted()
		{
			base.OnMounted();
			bool flag = this.ConfigEditor != null;
			if (flag)
			{
				this.UpdateHighlightedHour(this.ConfigEditor.AssetEditorOverlay.WeatherDaytimeBar.CurrentHour);
				bool flag2 = !this._isEditorRegistered;
				if (flag2)
				{
					this._isEditorRegistered = true;
					this.ConfigEditor.MountedTimelineEditors.Add(this);
				}
			}
		}

		// Token: 0x06005F24 RID: 24356 RVA: 0x001EA5BC File Offset: 0x001E87BC
		protected override void OnUnmounted()
		{
			base.OnUnmounted();
			bool flag = !this._isEditorRegistered;
			if (!flag)
			{
				this._isEditorRegistered = false;
				ConfigEditor configEditor = this.ConfigEditor;
				if (configEditor != null)
				{
					configEditor.MountedTimelineEditors.Remove(this);
				}
			}
		}

		// Token: 0x06005F25 RID: 24357 RVA: 0x001EA600 File Offset: 0x001E8800
		public void UpdateHighlightedHour(int hour)
		{
			for (int i = 0; i < 24; i++)
			{
				this._containers[i].OutlineSize = (float)((hour == i) ? 1 : 0);
				bool flag = this._colorPickers[i] != null;
				if (flag)
				{
					this._colorPickers[i].OutlineSize = (float)((hour == i) ? 1 : 0);
				}
			}
		}

		// Token: 0x06005F26 RID: 24358 RVA: 0x001EA660 File Offset: 0x001E8860
		private void BuildPlaceholder(int hour)
		{
			TextButton textButton = new TextButton(this.Desktop, this._containers[hour]);
			textButton.Text = "+";
			textButton.TooltipText = this.Desktop.Provider.GetText("ui.assetEditor.timelineEditor.insertAt", new Dictionary<string, string>
			{
				{
					"hour",
					this.Desktop.Provider.FormatNumber(hour)
				}
			}, true);
			textButton.TextTooltipStyle = new TextTooltipStyle
			{
				MaxWidth = new int?(140),
				LabelStyle = new LabelStyle
				{
					Wrap = true,
					FontSize = 13f
				}
			};
			textButton.Style = new TextButton.TextButtonStyle
			{
				Default = new TextButton.TextButtonStyleState
				{
					Background = new PatchStyle(UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 20)),
					LabelStyle = new LabelStyle
					{
						TextColor = UInt32Color.Transparent
					}
				},
				Hovered = new TextButton.TextButtonStyleState
				{
					Background = new PatchStyle(UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 40)),
					LabelStyle = new LabelStyle
					{
						Alignment = LabelStyle.LabelAlignment.Center,
						TextColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 70),
						RenderBold = true
					}
				}
			};
			textButton.OutlineColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 150);
			textButton.Anchor = new Anchor
			{
				Width = new int?(24),
				Height = new int?(24)
			};
			textButton.Activating = delegate()
			{
				this.InsertEntry(hour);
			};
		}

		// Token: 0x06005F27 RID: 24359 RVA: 0x001EA830 File Offset: 0x001E8A30
		private void BuildColorPicker(JToken jValue, int hour)
		{
			UInt32Color color = (this._valueSchema.ColorFormat == ColorPicker.ColorFormat.Rgba) ? UInt32Color.Transparent : UInt32Color.White;
			string text = (jValue != null) ? ((string)jValue).Trim() : "";
			bool flag = text == "" && this._valueSchema.DefaultValue != null;
			if (flag)
			{
				text = (string)this._valueSchema.DefaultValue;
			}
			UInt32Color uint32Color;
			bool flag2 = text != "" && this.TryParseColor(text, out uint32Color);
			if (flag2)
			{
				color = uint32Color;
			}
			this._colorPickers[hour] = new ColorPickerDropdownBox(this.Desktop, this._containers[hour])
			{
				Color = color,
				Format = this._valueSchema.ColorFormat,
				ResetTransparencyWhenChangingColor = true,
				DisplayTextField = true,
				TooltipText = this.Desktop.Provider.GetText("ui.assetEditor.timelineEditor.updateAt", new Dictionary<string, string>
				{
					{
						"hour",
						this.Desktop.Provider.FormatNumber(hour)
					}
				}, true),
				TextTooltipStyle = new TextTooltipStyle
				{
					MaxWidth = new int?(120),
					LabelStyle = new LabelStyle
					{
						Wrap = true,
						FontSize = 13f
					}
				},
				RightClicking = delegate()
				{
					this.RemoveEntry(hour);
				},
				ValueChanged = delegate
				{
					bool flag3 = this._valueSchema.ColorFormat == ColorPicker.ColorFormat.RgbShort;
					string text2;
					if (flag3)
					{
						text2 = this._colorPickers[hour].Color.ToShortHexString();
					}
					else
					{
						text2 = ColorUtils.FormatColor(this._colorPickers[hour].Color, (this._valueSchema.ColorFormat == ColorPicker.ColorFormat.Rgba) ? ColorUtils.ColorFormatType.HexAlpha : ColorUtils.ColorFormatType.Hex);
					}
					this.HandleTimeValueChanged(hour, JToken.FromObject(text2), false, false);
				},
				Style = this.ConfigEditor.ColorPickerDropdownBoxStyle,
				OutlineColor = UInt32Color.FromRGBA(125, 175, byte.MaxValue, byte.MaxValue),
				Anchor = new Anchor
				{
					Width = new int?(24),
					Height = new int?(24)
				}
			};
		}

		// Token: 0x06005F28 RID: 24360 RVA: 0x001EAA28 File Offset: 0x001E8C28
		private void BuildNumberField(JToken jValue, int hour)
		{
			decimal num = (this._valueSchema.DefaultValue != null) ? JsonUtils.ConvertToDecimal(this._valueSchema.DefaultValue) : 0m;
			NumberFieldFormat numberFieldFormat = new NumberFieldFormat
			{
				DefaultValue = num,
				MaxDecimalPlaces = this._valueSchema.MaxDecimalPlaces,
				Suffix = this._valueSchema.Suffix
			};
			bool flag = this._valueSchema.Min != null;
			if (flag)
			{
				numberFieldFormat.MinValue = JsonUtils.ConvertToDecimal(this._valueSchema.Min.Value);
			}
			bool flag2 = this._valueSchema.Max != null;
			if (flag2)
			{
				numberFieldFormat.MaxValue = JsonUtils.ConvertToDecimal(this._valueSchema.Max.Value);
			}
			bool flag3 = this._valueSchema.Step != null;
			if (flag3)
			{
				numberFieldFormat.Step = JsonUtils.ConvertToDecimal(this._valueSchema.Step.Value);
			}
			this._numberFields[hour] = new NumberField(this.Desktop, this._containers[hour])
			{
				Value = ((jValue != null) ? JsonUtils.ConvertToDecimal(jValue) : num),
				Format = numberFieldFormat,
				TooltipText = this.Desktop.Provider.GetText("ui.assetEditor.timelineEditor.updateAt", new Dictionary<string, string>
				{
					{
						"hour",
						this.Desktop.Provider.FormatNumber(hour)
					}
				}, true),
				TextTooltipStyle = new TextTooltipStyle
				{
					MaxWidth = new int?(120),
					LabelStyle = new LabelStyle
					{
						Wrap = true,
						FontSize = 13f
					}
				},
				Padding = new Padding
				{
					Left = new int?(3)
				},
				PlaceholderStyle = new InputFieldStyle
				{
					TextColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 100),
					FontSize = 11f
				},
				Style = new InputFieldStyle
				{
					FontSize = 11f
				},
				Blurred = delegate()
				{
					this.OnNumberFieldBlur(hour);
				},
				Validating = delegate()
				{
					this.OnNumberFieldValidate(hour);
				},
				RightClicking = delegate()
				{
					this.RemoveEntry(hour);
				},
				ValueChanged = delegate()
				{
					bool flag4 = this._valueSchema.MaxDecimalPlaces == 0;
					if (flag4)
					{
						int num2 = (int)this._numberFields[hour].Value;
						this.HandleTimeValueChanged(hour, num2, false, true);
					}
					else
					{
						decimal value = this._numberFields[hour].Value;
						this.HandleTimeValueChanged(hour, value, false, true);
					}
				},
				Decoration = new InputFieldDecorationStyle
				{
					Default = new InputFieldDecorationStyleState
					{
						Background = new PatchStyle(UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 20)),
						OutlineColor = new UInt32Color?(UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 150))
					},
					Focused = new InputFieldDecorationStyleState
					{
						OutlineSize = new int?(1),
						OutlineColor = new UInt32Color?(UInt32Color.FromRGBA(244, 188, 81, 153))
					}
				},
				Anchor = new Anchor
				{
					Width = new int?(24),
					Height = new int?(24)
				}
			};
		}

		// Token: 0x06005F29 RID: 24361 RVA: 0x001EAD58 File Offset: 0x001E8F58
		private bool TryParseColor(string value, out UInt32Color color)
		{
			switch (this._valueSchema.ColorFormat)
			{
			case ColorPicker.ColorFormat.Rgb:
			{
				ColorUtils.ColorFormatType colorFormatType;
				bool flag = ColorUtils.TryParseColor(value, out color, out colorFormatType);
				if (flag)
				{
					return true;
				}
				break;
			}
			case ColorPicker.ColorFormat.Rgba:
			{
				ColorUtils.ColorFormatType colorFormatType;
				bool flag2 = ColorUtils.TryParseColorAlpha(value, out color, out colorFormatType);
				if (flag2)
				{
					return true;
				}
				break;
			}
			case ColorPicker.ColorFormat.RgbShort:
			{
				bool flag3 = value.StartsWith("#") && value.Length == 4;
				if (flag3)
				{
					color = UInt32Color.FromShortHexString(value);
					return true;
				}
				break;
			}
			}
			color = UInt32Color.White;
			return false;
		}

		// Token: 0x06005F2A RID: 24362 RVA: 0x001EADF8 File Offset: 0x001E8FF8
		private void RemoveEntry(int hour)
		{
			this.HandleTimeValueChanged(hour, null, true, false);
			this._containers[hour].Clear();
			SchemaNode.NodeType type = this._valueSchema.Type;
			SchemaNode.NodeType nodeType = type;
			if (nodeType != SchemaNode.NodeType.Number)
			{
				if (nodeType == SchemaNode.NodeType.Color)
				{
					this._colorPickers[hour] = null;
				}
			}
			else
			{
				this._numberFields[hour] = null;
			}
			this.BuildPlaceholder(hour);
			base.Layout(null, true);
		}

		// Token: 0x06005F2B RID: 24363 RVA: 0x001EAE6C File Offset: 0x001E906C
		private void InsertEntry(int hour)
		{
			JToken jtoken = null;
			this._containers[hour].Clear();
			SchemaNode.NodeType type = this._valueSchema.Type;
			SchemaNode.NodeType nodeType = type;
			if (nodeType != SchemaNode.NodeType.Number)
			{
				if (nodeType == SchemaNode.NodeType.Color)
				{
					jtoken = ((this._valueSchema.DefaultValue != null) ? ((string)this._valueSchema.DefaultValue) : "#ffffff");
					this.BuildColorPicker(jtoken, hour);
					this._colorPickers[hour].Open();
				}
			}
			else
			{
				jtoken = ((this._valueSchema.DefaultValue != null) ? JsonUtils.ConvertToDecimal(this._valueSchema.DefaultValue) : 0m);
				this.BuildNumberField(jtoken, hour);
				this.Desktop.FocusElement(this._numberFields[hour], true);
			}
			this.HandleTimeValueChanged(hour, jtoken, false, false);
			base.Layout(null, true);
		}

		// Token: 0x06005F2C RID: 24364 RVA: 0x001EAF54 File Offset: 0x001E9154
		private void OnNumberFieldValidate(int hour)
		{
			bool syncPropertyChanges = this.ParentPropertyEditor.SyncPropertyChanges;
			if (syncPropertyChanges)
			{
				bool flag = this._valueSchema.MaxDecimalPlaces == 0;
				if (flag)
				{
					this.HandleTimeValueChanged(hour, (int)this._numberFields[hour].Value, false, false);
				}
				else
				{
					this.HandleTimeValueChanged(hour, this._numberFields[hour].Value, false, false);
				}
			}
			else
			{
				base.SubmitUpdateCommand();
			}
			this.Validate();
		}

		// Token: 0x06005F2D RID: 24365 RVA: 0x001EAFDC File Offset: 0x001E91DC
		private void OnNumberFieldBlur(int hour)
		{
			bool syncPropertyChanges = this.ParentPropertyEditor.SyncPropertyChanges;
			if (syncPropertyChanges)
			{
				bool flag = this._valueSchema.MaxDecimalPlaces == 0;
				if (flag)
				{
					this.HandleTimeValueChanged(hour, (int)this._numberFields[hour].Value, false, false);
				}
				else
				{
					this.HandleTimeValueChanged(hour, this._numberFields[hour].Value, false, false);
				}
			}
			else
			{
				base.SubmitUpdateCommand();
			}
		}

		// Token: 0x06005F2E RID: 24366 RVA: 0x001EB05C File Offset: 0x001E925C
		private void HandleTimeValueChanged(int targetHour, JToken targetValue, bool clear, bool withheldCommand)
		{
			JToken value = base.Value;
			JToken jtoken = (value != null) ? value.DeepClone() : null;
			JArray jarray = (JArray)base.Value;
			bool flag = this.ParentPropertyEditor.SyncPropertyChanges && !withheldCommand;
			if (flag)
			{
				jarray = new JArray();
				bool flag2 = !clear;
				if (flag2)
				{
					for (int i = 0; i < 24; i++)
					{
						JArray jarray2 = jarray;
						JObject jobject = new JObject();
						jobject.Add("Hour", i);
						jobject.Add(this._valuePropertyKey, targetValue);
						jarray2.Add(jobject);
					}
				}
			}
			else
			{
				int num = -1;
				bool flag3 = base.Value != null;
				if (flag3)
				{
					JArray jarray3 = (JArray)base.Value;
					for (int j = 0; j < jarray3.Count; j++)
					{
						JToken jtoken2 = jarray3[j];
						bool flag4 = jtoken2 == null || jtoken2.Type == 10;
						if (!flag4)
						{
							int num2 = (int)jtoken2["Hour"];
							bool flag5 = num2 != targetHour;
							if (!flag5)
							{
								num = j;
								break;
							}
						}
					}
				}
				bool flag6 = jarray == null;
				if (flag6)
				{
					jarray = new JArray();
				}
				bool flag7 = num == -1;
				if (flag7)
				{
					bool flag8 = !clear;
					if (flag8)
					{
						JArray jarray4 = jarray;
						JObject jobject2 = new JObject();
						jobject2.Add("Hour", targetHour);
						jobject2.Add(this._valuePropertyKey, targetValue);
						jarray4.Add(jobject2);
					}
				}
				else
				{
					bool flag9 = !clear;
					if (flag9)
					{
						jarray[num][this._valuePropertyKey] = targetValue;
					}
					else
					{
						jarray.RemoveAt(num);
					}
				}
			}
			bool flag10 = jarray.Count == 0;
			if (flag10)
			{
				jarray = null;
			}
			bool flag11 = base.Value != jarray;
			ConfigEditor configEditor = this.ConfigEditor;
			PropertyPath path = this.Path;
			JToken value2 = jarray;
			JToken previousValue = jtoken;
			CacheRebuildInfo cachesToRebuild = this.CachesToRebuild;
			configEditor.OnChangeValue(path, value2, previousValue, (cachesToRebuild != null) ? cachesToRebuild.Caches : null, withheldCommand, false, false);
			PropertyEditor parentPropertyEditor = this.ParentPropertyEditor;
			if (parentPropertyEditor != null)
			{
				parentPropertyEditor.UpdateAppearance();
			}
			bool flag12 = flag11;
			if (flag12)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06005F2F RID: 24367 RVA: 0x001EB28C File Offset: 0x001E948C
		public override void SetValueRecursively(JToken value)
		{
			bool flag = value == base.Value;
			if (!flag)
			{
				base.SetValueRecursively(value);
				JToken[] array = new JToken[24];
				bool flag2 = base.Value != null;
				if (flag2)
				{
					JArray jarray = (JArray)base.Value;
					foreach (JToken jtoken in jarray)
					{
						bool flag3 = jtoken == null || jtoken.Type == 10;
						if (!flag3)
						{
							int num = (int)jtoken["Hour"];
							bool flag4 = num < 0 || num >= 24;
							if (!flag4)
							{
								array[num] = jtoken[this._valuePropertyKey];
							}
						}
					}
				}
				for (int i = 0; i < 24; i++)
				{
					SchemaNode.NodeType type = this._valueSchema.Type;
					SchemaNode.NodeType nodeType = type;
					if (nodeType != SchemaNode.NodeType.Number)
					{
						if (nodeType != SchemaNode.NodeType.Color)
						{
							throw new Exception("TimelineEditor does not support value schema of type " + this._valueSchema.Type.ToString());
						}
						bool flag5 = this._colorPickers[i] != null;
						if (flag5)
						{
							bool flag6 = array[i] != null;
							if (flag6)
							{
								UInt32Color color;
								bool flag7 = this.TryParseColor((string)array[i], out color);
								if (flag7)
								{
									this._colorPickers[i].Color = color;
								}
								else
								{
									this._colorPickers[i].Color = ((this._valueSchema.ColorFormat == ColorPicker.ColorFormat.Rgba) ? UInt32Color.Transparent : UInt32Color.White);
								}
							}
							else
							{
								this._colorPickers[i].Parent.Remove(this._colorPickers[i]);
								this._colorPickers[i] = null;
								this.BuildPlaceholder(i);
							}
						}
						else
						{
							bool flag8 = array[i] != null;
							if (flag8)
							{
								this._containers[i].Clear();
								this.BuildColorPicker(array[i], i);
							}
						}
					}
					else
					{
						bool flag9 = this._numberFields[i] != null;
						if (flag9)
						{
							bool flag10 = array[i] != null;
							if (flag10)
							{
								this._numberFields[i].Value = JsonUtils.ConvertToDecimal(array[i]);
							}
							else
							{
								this._numberFields[i].Parent.Remove(this._numberFields[i]);
								this._numberFields[i] = null;
								this.BuildPlaceholder(i);
							}
						}
						else
						{
							bool flag11 = array[i] != null;
							if (flag11)
							{
								this._containers[i].Clear();
								this.BuildNumberField(array[i], i);
							}
						}
					}
				}
			}
		}

		// Token: 0x06005F30 RID: 24368 RVA: 0x001EB564 File Offset: 0x001E9764
		protected override bool ValidateType(JToken value)
		{
			return value.Type == 2;
		}

		// Token: 0x04003B3A RID: 15162
		private const string HourPropertyKey = "Hour";

		// Token: 0x04003B3B RID: 15163
		private ColorPickerDropdownBox[] _colorPickers = new ColorPickerDropdownBox[24];

		// Token: 0x04003B3C RID: 15164
		private NumberField[] _numberFields = new NumberField[24];

		// Token: 0x04003B3D RID: 15165
		private Group[] _containers = new Group[24];

		// Token: 0x04003B3E RID: 15166
		private string _valuePropertyKey;

		// Token: 0x04003B3F RID: 15167
		private SchemaNode _valueSchema;

		// Token: 0x04003B40 RID: 15168
		private bool _isEditorRegistered;
	}
}
