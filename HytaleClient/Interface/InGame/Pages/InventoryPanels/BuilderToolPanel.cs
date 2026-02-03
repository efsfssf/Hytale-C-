using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using HytaleClient.Data.Items;
using HytaleClient.InGame.Modules.BuilderTools.Tools;
using HytaleClient.InGame.Modules.BuilderTools.Tools.Brush;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x02000895 RID: 2197
	internal class BuilderToolPanel : Panel
	{
		// Token: 0x170010B2 RID: 4274
		// (get) Token: 0x06003F21 RID: 16161 RVA: 0x000AE502 File Offset: 0x000AC702
		// (set) Token: 0x06003F22 RID: 16162 RVA: 0x000AE50A File Offset: 0x000AC70A
		public Group Panel { get; private set; }

		// Token: 0x06003F23 RID: 16163 RVA: 0x000AE514 File Offset: 0x000AC714
		public BuilderToolPanel(InGameView inGameView, Element parent = null) : base(inGameView, parent)
		{
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x000AE700 File Offset: 0x000AC900
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/Inventory/BuilderTools/BuilderToolPanel.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this.Panel = uifragment.Get<Group>("Panel");
			this.Interface.TryGetDocument("InGame/Pages/Inventory/BuilderTools/Input/BlockSelector.ui", out this._blockSelectorDoc);
			this.Interface.TryGetDocument("InGame/Pages/Inventory/BuilderTools/Input/Checkbox.ui", out this._checkboxDoc);
			this.Interface.TryGetDocument("InGame/Pages/Inventory/BuilderTools/Input/Dropdown.ui", out this._dropdownDoc);
			this.Interface.TryGetDocument("InGame/Pages/Inventory/BuilderTools/Input/Slider.ui", out this._sliderDoc);
			this.Interface.TryGetDocument("InGame/Pages/Inventory/BuilderTools/Input/Number.ui", out this._numberInputDoc);
			this.Interface.TryGetDocument("InGame/Pages/Inventory/BuilderTools/Input/Text.ui", out this._textInputDoc);
			this.Interface.TryGetDocument("InGame/Pages/Inventory/BuilderTools/Input/MultilineText.ui", out this._multilineTextInputDoc);
			this._titleName = uifragment.Get<Label>("NameLabel");
			this._body = uifragment.Get<Group>("Body");
			this._body.KeepScrollPosition = true;
			this._infoLabel = uifragment.Get<Label>("InfoLabel");
			this._selectedMaterialContainer = uifragment.Get<Group>("SelectedMaterial");
			this._selectedMaterial = this.CreateBlockSelectorField(this._selectedMaterialContainer, null, null, 1);
			this._selectedMaterial.BindInput("Material", delegate(string argId, string value)
			{
				this._inGameView.InGame.Instance.AudioModule.PlayLocalSoundEvent(string.IsNullOrEmpty(value) ? "CREATE_EYEDROP_UNSELECT" : "CREATE_EYEDROP_SELECT");
				this.BrushArgValueChanged(argId, value);
			});
			this._generalSettingsTab = uifragment.Get<Button>("GeneralSettingsTab");
			this._generalSettingsTab.Activating = delegate()
			{
				bool visible = this._generalSettingsContainer.Visible;
				if (!visible)
				{
					this._generalSettingsContainer.Visible = true;
					this._maskSettingsContainer.Visible = false;
					this._footer.Visible = false;
					base.Layout(null, true);
				}
			};
			this._generalSettingsTabStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "GeneralSettingsTabStyle");
			this._generalSettingsTabActivatedStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "GeneralSettingsTabActivatedStyle");
			this._generalSettingsContainer = uifragment.Get<Group>("GeneralSettingsContainer");
			this._maskSettingsTabContainer = uifragment.Get<Group>("MaskSettingsTabContainer");
			this._maskSettingsTab = this._maskSettingsTabContainer.Find<Button>("MaskSettingsTab");
			this._maskSettingsTab.Activating = delegate()
			{
				bool visible = this._maskSettingsContainer.Visible;
				if (!visible)
				{
					this._generalSettingsContainer.Visible = false;
					this._maskSettingsContainer.Visible = true;
					this._maskSettingsBlockSelectorsContainer.Visible = !this._useCustomMaskCommandEntry.Value;
					this._maskSettingsCommandsContainer.Visible = this._useCustomMaskCommandEntry.Value;
					this._footer.Visible = true;
					base.Layout(null, true);
				}
			};
			this._maskSettingsTabStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "MaskSettingsTabStyle");
			this._maskSettingsTabActivatedStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "MaskSettingsTabActivatedStyle");
			this._maskSettingsContainer = uifragment.Get<Group>("MaskSettingsContainer");
			this._maskSettingsContainer.Visible = false;
			this._maskSettingsBlockSelectorsContainer = uifragment.Get<Group>("MaskSettingsBlockSelectorsContainer");
			this._maskSettingsCommandsContainer = uifragment.Get<Group>("MaskSettingsCommandsContainer");
			this._maskCommands = this.CreateMultilineTextField(this._maskSettingsCommandsContainer, null, "", 3);
			this._maskCommands.BindInput("MaskCommands", new Action<string, string>(this.BrushArgValueChanged));
			this._footer = uifragment.Get<Group>("Footer");
			this._footer.Visible = false;
			this._useCustomMaskCommandEntry = uifragment.Get<CheckBox>("UseCustomMaskCommandEntryCheckbox");
			this._useCustomMaskCommandEntry.ValueChanged = delegate()
			{
				bool value = this._useCustomMaskCommandEntry.Value;
				this._maskSettingsBlockSelectorsContainer.Visible = !value;
				this._maskSettingsCommandsContainer.Visible = value;
				base.Layout(null, true);
				this.BrushArgValueChanged("UseMaskCommands", value.ToString());
			};
			this.Refresh(false);
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x000AEA18 File Offset: 0x000ACC18
		protected override void ApplyStyles()
		{
			this._generalSettingsTab.Style = (this._generalSettingsContainer.IsMounted ? this._generalSettingsTabActivatedStyle : this._generalSettingsTabStyle);
			this._maskSettingsTab.Style = (this._maskSettingsContainer.IsMounted ? this._maskSettingsTabActivatedStyle : this._maskSettingsTabStyle);
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x000AEA72 File Offset: 0x000ACC72
		protected override void OnMounted()
		{
			this.Refresh(true);
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x000AEA7C File Offset: 0x000ACC7C
		private string ApplyEmptyFilter(string value, bool withEmpty)
		{
			bool flag = this.HasEmptyBlock(value);
			bool flag2 = withEmpty && !flag;
			string result;
			if (flag2)
			{
				result = this.AppendEmptyBlock(value);
			}
			else
			{
				bool flag3 = !withEmpty && flag;
				if (flag3)
				{
					result = this.RemoveEmptyBlock(value);
				}
				else
				{
					result = value;
				}
			}
			return result;
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x000AEAC3 File Offset: 0x000ACCC3
		private bool HasEmptyBlock(string value)
		{
			return Enumerable.Contains<string>(value.Split(new char[]
			{
				','
			}), "Empty");
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x000AEAE0 File Offset: 0x000ACCE0
		private string AppendEmptyBlock(string value)
		{
			return string.IsNullOrEmpty(value) ? "Empty" : string.Join(",", new string[]
			{
				value,
				"Empty"
			});
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x000AEB10 File Offset: 0x000ACD10
		private string RemoveEmptyBlock(string value)
		{
			return string.Join(",", Enumerable.Where<string>(value.Split(new char[]
			{
				','
			}), (string item) => item != "Empty"));
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x000AEB5C File Offset: 0x000ACD5C
		private void BrushMaskArgValueChanged(string argId, string value, bool withEmpty)
		{
			this._inGameView.InGame.Instance.AudioModule.PlayLocalSoundEvent("CREATE_MASK_ADD");
			this.BrushArgValueChanged(argId, this.ApplyEmptyFilter(value, withEmpty));
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x000AEB8F File Offset: 0x000ACD8F
		private void BrushArgValueChanged(string argId, string value)
		{
			this.ArgValueChanged(1, argId, value);
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x000AEB9B File Offset: 0x000ACD9B
		private void ToolArgValueChanged(string argId, string value)
		{
			this.ArgValueChanged(0, argId, value);
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x000AEBA7 File Offset: 0x000ACDA7
		private void ArgValueChanged(BuilderToolArgGroup argGroup, string argId, string value)
		{
			this.Interface.TriggerEventFromInterface("builderTools.argValueChange", argGroup, argId, value);
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x000AEBC4 File Offset: 0x000ACDC4
		public void ConfiguringToolChange(ToolInstance toolInstance)
		{
			ClientItemStack item = (toolInstance != null) ? toolInstance.ItemStack : null;
			this.ConfiguringToolChange(item);
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x000AEBE8 File Offset: 0x000ACDE8
		public void ConfiguringToolChange(ClientItemStack item)
		{
			bool flag = this._selectedTool == item;
			if (!flag)
			{
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					this.Update(item, true);
				}
				this._selectedTool = item;
			}
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x000AEC1F File Offset: 0x000ACE1F
		public void Refresh(bool doLayout = true)
		{
			this.Update(this._selectedTool, doLayout);
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x000AEC2F File Offset: 0x000ACE2F
		private string GetBrushArgLabelText(string key)
		{
			return this.Desktop.Provider.GetText("builderTools.brush.args." + key + ".name", null, true);
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x000AEC54 File Offset: 0x000ACE54
		private List<DropdownBox.DropdownEntryInfo> GetDropdownEntries<T>(string langKey)
		{
			return Enumerable.ToList<DropdownBox.DropdownEntryInfo>(Enumerable.Select<T, DropdownBox.DropdownEntryInfo>(Enumerable.Cast<T>(Enum.GetValues(typeof(T))), delegate(T type)
			{
				string text = type.ToString();
				return new DropdownBox.DropdownEntryInfo(this.Desktop.Provider.GetText(langKey + "." + text, null, true), text, false);
			}));
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x000AECA4 File Offset: 0x000ACEA4
		private List<DropdownBox.DropdownEntryInfo> GetBrushArgOptions(string key)
		{
			List<DropdownBox.DropdownEntryInfo> result;
			if (!(key == "Shape"))
			{
				if (!(key == "Origin"))
				{
					if (!(key == "RotationAxis") && !(key == "MirrorAxis"))
					{
						if (!(key == "RotationAngle"))
						{
							result = null;
						}
						else
						{
							result = this.GetDropdownEntries<Rotation>("builderTools.brush.rotation");
						}
					}
					else
					{
						result = this.GetDropdownEntries<BrushAxis>("builderTools.brush.axis");
					}
				}
				else
				{
					result = this.GetDropdownEntries<BrushOrigin>("builderTools.brush.origin");
				}
			}
			else
			{
				result = this.GetDropdownEntries<BrushShape>("builderTools.brush.shape");
			}
			return result;
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x000AED38 File Offset: 0x000ACF38
		private T GetBrushDataValue<T>(BuilderToolItem.BuilderToolBrushData data, string key)
		{
			return (T)((object)data.GetType().GetField(key).GetValue(data));
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x000AED64 File Offset: 0x000ACF64
		private void DisplayInfo(string key, bool doLayout)
		{
			this._infoLabel.Visible = true;
			this._infoLabel.Text = this.Desktop.Provider.GetText(key, null, true);
			this._generalSettingsContainer.Visible = true;
			this._maskSettingsContainer.Visible = false;
			this._footer.Visible = false;
			if (doLayout)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x000AEDDC File Offset: 0x000ACFDC
		private void Update(ClientItemStack item = null, bool doLayout = true)
		{
			BuilderToolPanel.<>c__DisplayClass54_0 CS$<>8__locals1 = new BuilderToolPanel.<>c__DisplayClass54_0();
			CS$<>8__locals1.<>4__this = this;
			this._titleName.Text = this.Desktop.Provider.GetText("ui.builderTools.name", null, true);
			this._selectedMaterialContainer.Visible = false;
			this._maskSettingsTabContainer.Visible = false;
			this._body.Visible = false;
			this._footer.Visible = false;
			this._generalSettingsContainer.Clear();
			this._maskSettingsBlockSelectorsContainer.Clear();
			bool flag;
			if (item != null)
			{
				ClientItemBase clientItemBase = this._inGameView.Items[item.Id];
				flag = (((clientItemBase != null) ? clientItemBase.BuilderTool : null) == null);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.DisplayInfo("ui.builderTools.selectATool", doLayout);
			}
			else
			{
				CS$<>8__locals1.tool = this._inGameView.Items[item.Id].BuilderTool;
				ClientItemBase clientItemBase2;
				this._inGameView.Items.TryGetValue(item.Id, out clientItemBase2);
				this._titleName.Text = this.Desktop.Provider.GetText("builderTools.tools." + CS$<>8__locals1.tool.Id + ".name", null, true);
				bool flag3 = !CS$<>8__locals1.tool.ToolItem.IsBrush && CS$<>8__locals1.tool.ToolItem.Args.Count == 0;
				if (flag3)
				{
					this.DisplayInfo("ui.builderTools.noToolSettings", doLayout);
				}
				else
				{
					this._infoLabel.Visible = false;
					this._body.Visible = true;
					bool isBrush = CS$<>8__locals1.tool.ToolItem.IsBrush;
					if (isBrush)
					{
						this._selectedMaterialContainer.Visible = true;
						this._maskSettingsTabContainer.Visible = true;
						Dictionary<string, string> dictionary = new BrushData(item, CS$<>8__locals1.tool, null).ToArgValues();
						this._selectedMaterial.Label.Text = this.GetBrushArgLabelText("Material");
						this._selectedMaterial.Input.Capacity = 7;
						this._selectedMaterial.Input.Value = dictionary["Material"];
						this._useCustomMaskCommandEntry.Value = bool.Parse(dictionary["UseMaskCommands"]);
						this._maskCommands.Label.Text = this.GetBrushArgLabelText("MaskCommands");
						this._maskCommands.Input.Value = dictionary["MaskCommands"];
						foreach (BuilderToolPanel.BrushSetting brushSetting in this._brushGeneralSettings)
						{
							BuilderToolPanel.<>c__DisplayClass54_1 CS$<>8__locals2 = new BuilderToolPanel.<>c__DisplayClass54_1();
							CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
							BuilderToolArgType type = brushSetting.Type;
							CS$<>8__locals2.key = brushSetting.Key;
							string brushArgLabelText = this.GetBrushArgLabelText(CS$<>8__locals2.key);
							string value = dictionary[CS$<>8__locals2.key];
							BuilderToolPanel.<>c__DisplayClass54_2 CS$<>8__locals3 = new BuilderToolPanel.<>c__DisplayClass54_2();
							CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
							switch (type)
							{
							case 0:
							{
								BuilderToolPanel.BuilderToolField<CheckBox, bool> builderToolField = this.CreateCheckBoxField(this._generalSettingsContainer, brushArgLabelText, value);
								builderToolField.BindInput(CS$<>8__locals3.CS$<>8__locals2.key, new Action<string, string>(this.BrushArgValueChanged));
								break;
							}
							case 1:
							{
								BuilderToolFloatArg brushDataValue = this.GetBrushDataValue<BuilderToolFloatArg>(CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.tool.ToolItem.BrushData, CS$<>8__locals3.CS$<>8__locals2.key);
								BuilderToolPanel.BuilderToolField<NumberField, decimal> builderToolField2 = this.CreateNumberField(this._generalSettingsContainer, brushArgLabelText, value, brushDataValue.Min, brushDataValue.Max);
								builderToolField2.BindInput(CS$<>8__locals3.CS$<>8__locals2.key, new Action<string, string>(this.BrushArgValueChanged));
								break;
							}
							case 2:
							{
								BuilderToolIntArg brushDataValue2 = this.GetBrushDataValue<BuilderToolIntArg>(CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.tool.ToolItem.BrushData, CS$<>8__locals3.CS$<>8__locals2.key);
								CS$<>8__locals3.intField = this.CreateSliderField(this._generalSettingsContainer, brushArgLabelText, value, brushDataValue2.Min, brushDataValue2.Max);
								CS$<>8__locals3.intField.Input.SliderMouseButtonReleased = new Action(CS$<>8__locals3.<Update>g__SliderCallback|0);
								CS$<>8__locals3.intField.Input.NumberFieldBlurred = new Action(CS$<>8__locals3.<Update>g__SliderCallback|0);
								break;
							}
							case 3:
							{
								BuilderToolPanel.BuilderToolField<TextField, string> builderToolField3 = this.CreateTextField(this._generalSettingsContainer, brushArgLabelText, value);
								builderToolField3.BindInput(CS$<>8__locals3.CS$<>8__locals2.key, new Action<string, string>(this.BrushArgValueChanged));
								break;
							}
							case 4:
							case 5:
							{
								BuilderToolPanel.BuilderToolBlockSelectorField builderToolBlockSelectorField = this.CreateBlockSelectorField(this._generalSettingsContainer, brushArgLabelText, value, 1);
								builderToolBlockSelectorField.BindInput(CS$<>8__locals3.CS$<>8__locals2.key, new Action<string, string>(this.BrushArgValueChanged));
								break;
							}
							case 10:
							{
								BuilderToolPanel.BuilderToolField<DropdownBox, string> builderToolField4 = this.CreateDropdownField(this._generalSettingsContainer, brushArgLabelText, value, this.GetBrushArgOptions(CS$<>8__locals3.CS$<>8__locals2.key));
								builderToolField4.BindInput(CS$<>8__locals3.CS$<>8__locals2.key, new Action<string, string>(this.BrushArgValueChanged));
								break;
							}
							}
						}
						foreach (BuilderToolPanel.BrushSetting brushSetting2 in this._brushMaskSettings)
						{
							string key = brushSetting2.Key;
							BuilderToolPanel.BuilderToolBlockSelectorField field = this.CreateBlockSelectorField(this._maskSettingsBlockSelectorsContainer, this.GetBrushArgLabelText(key), dictionary[key], 7);
							field.LabelPrefix.Text = brushSetting2.LabelPrefix;
							field.BindInput(key, delegate(string k, string v)
							{
								CS$<>8__locals1.<>4__this.BrushMaskArgValueChanged(k, v, field.EmptyFilterCheckbox.Value);
							});
							field.EmptyFilter.Visible = true;
							field.EmptyFilterCheckbox.Value = this.HasEmptyBlock(dictionary[key]);
							field.EmptyFilterCheckbox.ValueChanged = delegate()
							{
								CS$<>8__locals1.<>4__this.BrushArgValueChanged(key, CS$<>8__locals1.<>4__this.ApplyEmptyFilter(field.Input.Value, field.EmptyFilterCheckbox.Value));
							};
						}
						this._maskSettingsBlockSelectorsContainer.Visible = !this._useCustomMaskCommandEntry.Value;
						this._maskSettingsCommandsContainer.Visible = this._useCustomMaskCommandEntry.Value;
						this._footer.Visible = this._maskSettingsContainer.Visible;
					}
					else
					{
						this._generalSettingsContainer.Visible = true;
						this._maskSettingsContainer.Visible = false;
						this._footer.Visible = false;
					}
					bool flag4 = CS$<>8__locals1.tool.ToolItem.Args.Count > 0;
					if (flag4)
					{
						JToken jtoken = null;
						JObject metadata = item.Metadata;
						bool? flag5 = (metadata != null) ? new bool?(metadata.TryGetValue("ToolData", ref jtoken)) : null;
						bool flag8;
						if (flag5 != null)
						{
							bool? flag6 = flag5;
							bool flag7 = false;
							flag8 = (flag6.GetValueOrDefault() == flag7 & flag6 != null);
						}
						else
						{
							flag8 = true;
						}
						bool flag9 = flag8;
						if (flag9)
						{
							jtoken = new JObject();
						}
						JToken jtoken2 = CS$<>8__locals1.tool.GetDefaultArgData().DeepClone();
						foreach (KeyValuePair<string, BuilderToolArg> keyValuePair in ImmutableSortedDictionary.ToImmutableSortedDictionary<string, BuilderToolArg>(CS$<>8__locals1.tool.ToolItem.Args))
						{
							BuilderToolPanel.<>c__DisplayClass54_4 CS$<>8__locals5 = new BuilderToolPanel.<>c__DisplayClass54_4();
							CS$<>8__locals5.CS$<>8__locals4 = CS$<>8__locals1;
							BuilderToolArgType argType = keyValuePair.Value.ArgType;
							CS$<>8__locals5.key = keyValuePair.Key;
							string text = this.Desktop.Provider.GetText(string.Concat(new string[]
							{
								"builderTools.tools.",
								CS$<>8__locals5.CS$<>8__locals4.tool.Id,
								".args.",
								CS$<>8__locals5.key,
								".name"
							}), null, true);
							string value2 = (jtoken[keyValuePair.Key] ?? jtoken2[keyValuePair.Key]).ToString();
							BuilderToolPanel.<>c__DisplayClass54_5 CS$<>8__locals6 = new BuilderToolPanel.<>c__DisplayClass54_5();
							CS$<>8__locals6.CS$<>8__locals5 = CS$<>8__locals5;
							switch (argType)
							{
							case 0:
							{
								BuilderToolPanel.BuilderToolField<CheckBox, bool> builderToolField5 = this.CreateCheckBoxField(this._generalSettingsContainer, text, value2);
								builderToolField5.BindInput(CS$<>8__locals6.CS$<>8__locals5.key, new Action<string, string>(this.ToolArgValueChanged));
								break;
							}
							case 1:
							{
								BuilderToolPanel.BuilderToolField<NumberField, decimal> builderToolField6 = this.CreateNumberField(this._generalSettingsContainer, text, value2, keyValuePair.Value.FloatArg.Min, keyValuePair.Value.FloatArg.Max);
								builderToolField6.BindInput(CS$<>8__locals6.CS$<>8__locals5.key, new Action<string, string>(this.ToolArgValueChanged));
								break;
							}
							case 2:
								CS$<>8__locals6.sliderField = this.CreateSliderField(this._generalSettingsContainer, text, value2, keyValuePair.Value.IntArg.Min, keyValuePair.Value.IntArg.Max);
								CS$<>8__locals6.sliderField.Input.SliderMouseButtonReleased = new Action(CS$<>8__locals6.<Update>g__SliderCallback|3);
								CS$<>8__locals6.sliderField.Input.NumberFieldBlurred = new Action(CS$<>8__locals6.<Update>g__SliderCallback|3);
								break;
							case 3:
							{
								BuilderToolPanel.BuilderToolField<TextField, string> builderToolField7 = this.CreateTextField(this._generalSettingsContainer, text, value2);
								builderToolField7.BindInput(CS$<>8__locals6.CS$<>8__locals5.key, new Action<string, string>(this.ToolArgValueChanged));
								break;
							}
							case 4:
							case 5:
							{
								BuilderToolPanel.BuilderToolBlockSelectorField builderToolBlockSelectorField2 = this.CreateBlockSelectorField(this._generalSettingsContainer, text, value2, 1);
								builderToolBlockSelectorField2.BindInput(CS$<>8__locals6.CS$<>8__locals5.key, new Action<string, string>(this.ToolArgValueChanged));
								break;
							}
							case 10:
							{
								List<DropdownBox.DropdownEntryInfo> options = Enumerable.ToList<DropdownBox.DropdownEntryInfo>(Enumerable.Select<string, DropdownBox.DropdownEntryInfo>(CS$<>8__locals6.CS$<>8__locals5.CS$<>8__locals4.tool.ToolItem.Args[CS$<>8__locals6.CS$<>8__locals5.key].OptionArg.Options, delegate(string val)
								{
									string text2 = CS$<>8__locals6.CS$<>8__locals5.CS$<>8__locals4.<>4__this.Desktop.Provider.GetText(string.Concat(new string[]
									{
										"builderTools.tools.",
										CS$<>8__locals6.CS$<>8__locals5.CS$<>8__locals4.tool.Id,
										".args.",
										CS$<>8__locals6.CS$<>8__locals5.key,
										".values.",
										val
									}), null, true);
									return new DropdownBox.DropdownEntryInfo(text2, val, false);
								}));
								BuilderToolPanel.BuilderToolField<DropdownBox, string> builderToolField8 = this.CreateDropdownField(this._generalSettingsContainer, text, value2, options);
								builderToolField8.BindInput(CS$<>8__locals6.CS$<>8__locals5.key, new Action<string, string>(this.ToolArgValueChanged));
								break;
							}
							}
						}
					}
					if (doLayout)
					{
						base.Layout(null, true);
					}
				}
			}
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x000AF8F0 File Offset: 0x000ADAF0
		private BuilderToolPanel.BuilderToolBlockSelectorField CreateBlockSelectorField(Group container, string label, string value, int capacity = 1)
		{
			UIFragment fragment = this._blockSelectorDoc.Instantiate(this.Desktop, container);
			return new BuilderToolPanel.BuilderToolBlockSelectorField(this._inGameView, fragment, "BlockSelector")
			{
				Label = 
				{
					Text = label
				},
				Input = 
				{
					Capacity = capacity
				},
				Input = 
				{
					Value = value
				}
			};
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x000AF950 File Offset: 0x000ADB50
		private BuilderToolPanel.BuilderToolField<CheckBox, bool> CreateCheckBoxField(Group container, string label, string value)
		{
			UIFragment fragment = this._checkboxDoc.Instantiate(this.Desktop, container);
			bool flag;
			return new BuilderToolPanel.BuilderToolField<CheckBox, bool>(fragment, "Checkbox")
			{
				Label = 
				{
					Text = label
				},
				Input = 
				{
					Value = (bool.TryParse(value, out flag) && flag)
				}
			};
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x000AF9A8 File Offset: 0x000ADBA8
		private BuilderToolPanel.BuilderToolField<DropdownBox, string> CreateDropdownField(Group container, string label, string value, List<DropdownBox.DropdownEntryInfo> options)
		{
			UIFragment fragment = this._dropdownDoc.Instantiate(this.Desktop, container);
			return new BuilderToolPanel.BuilderToolField<DropdownBox, string>(fragment, "Dropdown")
			{
				Label = 
				{
					Text = label
				},
				Input = 
				{
					Entries = options
				},
				Input = 
				{
					Value = value
				}
			};
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x000AFA04 File Offset: 0x000ADC04
		private BuilderToolPanel.BuilderToolField<SliderNumberField, int> CreateSliderField(Group container, string label, string value, int min, int max)
		{
			UIFragment fragment = this._sliderDoc.Instantiate(this.Desktop, container);
			return new BuilderToolPanel.BuilderToolField<SliderNumberField, int>(fragment, "Slider")
			{
				Label = 
				{
					Text = label
				},
				Input = 
				{
					Value = int.Parse(value)
				},
				Input = 
				{
					Min = min
				},
				Input = 
				{
					Max = max
				}
			};
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x000AFA74 File Offset: 0x000ADC74
		private BuilderToolPanel.BuilderToolField<NumberField, decimal> CreateNumberField(Group container, string label, string value, float min, float max)
		{
			UIFragment fragment = this._numberInputDoc.Instantiate(this.Desktop, container);
			return new BuilderToolPanel.BuilderToolField<NumberField, decimal>(fragment, "NumberField")
			{
				Label = 
				{
					Text = label
				},
				Input = 
				{
					Value = decimal.Parse(value)
				},
				Input = 
				{
					Format = 
					{
						MinValue = Convert.ToDecimal(min)
					}
				},
				Input = 
				{
					Format = 
					{
						MaxValue = Convert.ToDecimal(max)
					}
				}
			};
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x000AFAF4 File Offset: 0x000ADCF4
		private BuilderToolPanel.BuilderToolField<TextField, string> CreateTextField(Group container, string label, string value)
		{
			UIFragment fragment = this._textInputDoc.Instantiate(this.Desktop, container);
			return new BuilderToolPanel.BuilderToolField<TextField, string>(fragment, "TextField")
			{
				Label = 
				{
					Text = label
				},
				Input = 
				{
					Value = value
				}
			};
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x000AFB40 File Offset: 0x000ADD40
		private BuilderToolPanel.BuilderToolField<MultilineTextField, string> CreateMultilineTextField(Group container, string label, string value, int maxLines = 3)
		{
			UIFragment fragment = this._multilineTextInputDoc.Instantiate(this.Desktop, container);
			return new BuilderToolPanel.BuilderToolField<MultilineTextField, string>(fragment, "MultilineTextField")
			{
				Label = 
				{
					Text = label
				},
				Input = 
				{
					Value = value
				},
				Input = 
				{
					MaxLines = maxLines
				}
			};
		}

		// Token: 0x04001DD8 RID: 7640
		private ClientItemStack _selectedTool;

		// Token: 0x04001DD9 RID: 7641
		private Document _blockSelectorDoc;

		// Token: 0x04001DDA RID: 7642
		private Document _checkboxDoc;

		// Token: 0x04001DDB RID: 7643
		private Document _dropdownDoc;

		// Token: 0x04001DDC RID: 7644
		private Document _sliderDoc;

		// Token: 0x04001DDD RID: 7645
		private Document _numberInputDoc;

		// Token: 0x04001DDE RID: 7646
		private Document _textInputDoc;

		// Token: 0x04001DDF RID: 7647
		private Document _multilineTextInputDoc;

		// Token: 0x04001DE0 RID: 7648
		private Label _titleName;

		// Token: 0x04001DE1 RID: 7649
		private Group _body;

		// Token: 0x04001DE2 RID: 7650
		private Label _infoLabel;

		// Token: 0x04001DE3 RID: 7651
		private Group _selectedMaterialContainer;

		// Token: 0x04001DE4 RID: 7652
		private BuilderToolPanel.BuilderToolBlockSelectorField _selectedMaterial;

		// Token: 0x04001DE5 RID: 7653
		private Button _generalSettingsTab;

		// Token: 0x04001DE6 RID: 7654
		private Button.ButtonStyle _generalSettingsTabStyle;

		// Token: 0x04001DE7 RID: 7655
		private Button.ButtonStyle _generalSettingsTabActivatedStyle;

		// Token: 0x04001DE8 RID: 7656
		private Group _generalSettingsContainer;

		// Token: 0x04001DE9 RID: 7657
		private Group _maskSettingsTabContainer;

		// Token: 0x04001DEA RID: 7658
		private Button _maskSettingsTab;

		// Token: 0x04001DEB RID: 7659
		private Button.ButtonStyle _maskSettingsTabStyle;

		// Token: 0x04001DEC RID: 7660
		private Button.ButtonStyle _maskSettingsTabActivatedStyle;

		// Token: 0x04001DED RID: 7661
		private Group _maskSettingsContainer;

		// Token: 0x04001DEE RID: 7662
		private Group _maskSettingsBlockSelectorsContainer;

		// Token: 0x04001DEF RID: 7663
		private Group _maskSettingsCommandsContainer;

		// Token: 0x04001DF0 RID: 7664
		private BuilderToolPanel.BuilderToolField<MultilineTextField, string> _maskCommands;

		// Token: 0x04001DF1 RID: 7665
		private Group _footer;

		// Token: 0x04001DF2 RID: 7666
		private CheckBox _useCustomMaskCommandEntry;

		// Token: 0x04001DF3 RID: 7667
		private readonly List<BuilderToolPanel.BrushSetting> _brushGeneralSettings = new List<BuilderToolPanel.BrushSetting>
		{
			new BuilderToolPanel.BrushSetting
			{
				Key = "Width",
				Type = 2
			},
			new BuilderToolPanel.BrushSetting
			{
				Key = "Height",
				Type = 2
			},
			new BuilderToolPanel.BrushSetting
			{
				Key = "Shape",
				Type = 10
			},
			new BuilderToolPanel.BrushSetting
			{
				Key = "Origin",
				Type = 10
			},
			new BuilderToolPanel.BrushSetting
			{
				Key = "MirrorAxis",
				Type = 10
			},
			new BuilderToolPanel.BrushSetting
			{
				Key = "Thickness",
				Type = 2
			}
		};

		// Token: 0x04001DF4 RID: 7668
		private readonly List<BuilderToolPanel.BrushSetting> _brushMaskSettings = new List<BuilderToolPanel.BrushSetting>
		{
			new BuilderToolPanel.BrushSetting
			{
				Key = "Mask"
			},
			new BuilderToolPanel.BrushSetting
			{
				Key = "MaskAbove",
				LabelPrefix = "> "
			},
			new BuilderToolPanel.BrushSetting
			{
				Key = "MaskNot",
				LabelPrefix = "! "
			},
			new BuilderToolPanel.BrushSetting
			{
				Key = "MaskBelow",
				LabelPrefix = "< "
			},
			new BuilderToolPanel.BrushSetting
			{
				Key = "MaskAdjacent",
				LabelPrefix = "~ "
			},
			new BuilderToolPanel.BrushSetting
			{
				Key = "MaskNeighbor",
				LabelPrefix = "^ "
			}
		};

		// Token: 0x02000D66 RID: 3430
		private struct BrushSetting
		{
			// Token: 0x040041D7 RID: 16855
			public string Key;

			// Token: 0x040041D8 RID: 16856
			public BuilderToolArgType Type;

			// Token: 0x040041D9 RID: 16857
			public string LabelPrefix;
		}

		// Token: 0x02000D67 RID: 3431
		private class BuilderToolField<TInput, TValue> where TInput : InputElement<TValue>
		{
			// Token: 0x06006546 RID: 25926 RVA: 0x002112CC File Offset: 0x0020F4CC
			public BuilderToolField(UIFragment fragment, string inputName)
			{
				this.LabelPrefix = fragment.Get<Label>("LabelPrefix");
				this.Label = fragment.Get<Label>("Label");
				this.LabelSuffix = fragment.Get<Label>("LabelSuffix");
				this.Input = fragment.Get<TInput>(inputName);
			}

			// Token: 0x06006547 RID: 25927 RVA: 0x00211324 File Offset: 0x0020F524
			public void BindInput(string key, Action<string, string> valueChanged)
			{
				this.Input.ValueChanged = delegate()
				{
					Action<string, string> valueChanged2 = valueChanged;
					string key2 = key;
					TValue value = this.Input.Value;
					valueChanged2(key2, value.ToString());
				};
			}

			// Token: 0x040041DA RID: 16858
			public readonly Label LabelPrefix;

			// Token: 0x040041DB RID: 16859
			public readonly Label Label;

			// Token: 0x040041DC RID: 16860
			public readonly Label LabelSuffix;

			// Token: 0x040041DD RID: 16861
			public readonly TInput Input;
		}

		// Token: 0x02000D68 RID: 3432
		private class BuilderToolBlockSelectorField : BuilderToolPanel.BuilderToolField<BlockSelector, string>
		{
			// Token: 0x06006548 RID: 25928 RVA: 0x0021136C File Offset: 0x0020F56C
			public BuilderToolBlockSelectorField(InGameView inGameView, UIFragment fragment, string inputName) : base(fragment, inputName)
			{
				BuilderToolPanel.BuilderToolBlockSelectorField <>4__this = this;
				this.EmptyFilter = fragment.Get<Group>("EmptyFilter");
				this.EmptyFilterCheckbox = fragment.Get<CheckBox>("EmptyFilterCheckbox");
				fragment.Get<Button>("ActionClear").Activating = delegate()
				{
					inGameView.InGame.Instance.AudioModule.PlayLocalSoundEvent("UI_CLEAR");
					<>4__this.Input.Reset();
				};
			}

			// Token: 0x040041DE RID: 16862
			public readonly Group EmptyFilter;

			// Token: 0x040041DF RID: 16863
			public readonly CheckBox EmptyFilterCheckbox;
		}
	}
}
