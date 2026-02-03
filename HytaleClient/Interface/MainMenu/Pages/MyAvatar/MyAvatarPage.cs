using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HytaleClient.Application;
using HytaleClient.Data.Characters;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.MainMenu.Pages.MyAvatar
{
	// Token: 0x02000822 RID: 2082
	internal class MyAvatarPage : InterfaceComponent
	{
		// Token: 0x060039EB RID: 14827 RVA: 0x00080530 File Offset: 0x0007E730
		private void Animate(float deltaTime)
		{
			bool flag = this.RenderCharacterPartPreviewCommandQueue.Count > 0;
			if (flag)
			{
				this.MainMenuView.MainMenu.RenderAssetPreviews(this.RenderCharacterPartPreviewCommandQueue.ToArray());
				this.RenderCharacterPartPreviewCommandQueue.Clear();
			}
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x0008057C File Offset: 0x0007E77C
		public void OnPreviewRendered(CharacterPartId id, Texture texture)
		{
			Texture texture2;
			bool flag = this._previewCache.TryGetValue(id.PartId, out texture2);
			if (flag)
			{
				texture2.Dispose();
			}
			this._previewCache[id.PartId] = texture;
			this._previewComponents[id.PartId].Texture = texture;
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x000805D4 File Offset: 0x0007E7D4
		public void UpdateTags()
		{
			this._tags.Entries = new List<DropdownBox.DropdownEntryInfo>();
			bool flag = this._activeTab == MyAvatarPage.MyAvatarPageTab.General || this._activeTab == MyAvatarPage.MyAvatarPageTab.Emotes;
			if (!flag)
			{
				PlayerSkinProperty property = this.GetProperty(this._activeTab);
				List<CharacterPart> parts = this.Interface.App.CharacterPartStore.GetParts(property);
				List<string> tags = this.Interface.App.CharacterPartStore.GetTags(parts);
				List<DropdownBox.DropdownEntryInfo> list = new List<DropdownBox.DropdownEntryInfo>();
				foreach (string text in tags)
				{
					list.Add(new DropdownBox.DropdownEntryInfo(text, text, false));
				}
				this._tags.Entries = list;
			}
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x000806B4 File Offset: 0x0007E8B4
		private void UpdatePartList()
		{
			this._parts.Clear();
			this._colorsContainer.Visible = (this._activeTab != MyAvatarPage.MyAvatarPageTab.Face);
			bool flag = this._activeTab == MyAvatarPage.MyAvatarPageTab.General || this._activeTab == MyAvatarPage.MyAvatarPageTab.Emotes;
			if (!flag)
			{
				PlayerSkinProperty property = this.GetProperty(this._activeTab);
				bool flag2 = this._lockedCharacterOptionsForRandomization.Contains(property);
				this._partRandomizationLock.Style = (flag2 ? this._randomizationLockedButtonStyle : this._randomizationUnlockedButtonStyle);
				this._partRandomizationLock.TooltipText = this.Desktop.Provider.GetText(flag2 ? "ui.myAvatar.unlockProperty" : "ui.myAvatar.lockProperty", null, true);
				this._matchHairColorsButton.Visible = (property == PlayerSkinProperty.Haircut || property == PlayerSkinProperty.FacialHair || property == PlayerSkinProperty.Eyebrows);
				List<CharacterPart> parts = this.Interface.App.CharacterPartStore.GetParts(property);
				List<CharacterPart> list = new List<CharacterPart>();
				string text = this._searchField.Value.Trim().ToLower();
				CharacterPartId selectedPartId = this.GetSelectedPartId(property);
				CharacterPart selectedPart = (selectedPartId == null) ? null : parts.Find((CharacterPart p) => p.Id == selectedPartId.PartId);
				foreach (CharacterPart characterPart in parts)
				{
					bool flag3 = text != "" && !characterPart.Name.ToLower().Contains(text);
					if (!flag3)
					{
						bool flag4 = this.SelectedTags.Count > 0;
						if (flag4)
						{
							foreach (string text2 in this.SelectedTags)
							{
								bool flag5 = characterPart.Tags != null && Enumerable.Contains<string>(characterPart.Tags, text2);
								if (flag5)
								{
									list.Add(characterPart);
									break;
								}
							}
						}
						else
						{
							list.Add(characterPart);
						}
					}
				}
				int num = list.Count;
				int num2 = 0;
				bool flag6 = this._activeTab != MyAvatarPage.MyAvatarPageTab.Eyes && this._activeTab != MyAvatarPage.MyAvatarPageTab.Face && text == "";
				if (flag6)
				{
					num++;
					num2 = 1;
				}
				int num3 = (int)MathHelper.Max((float)num, (float)(this._previewsPerRow * this._visiblePreviewRows));
				bool flag7 = num3 % this._previewsPerRow != 0;
				if (flag7)
				{
					num3 += this._previewsPerRow - num3 % this._previewsPerRow;
				}
				Group group = null;
				int num4 = num3 / this._previewsPerRow;
				bool flag8 = this._activeTab != MyAvatarPage.MyAvatarPageTab.Eyes && this._activeTab != MyAvatarPage.MyAvatarPageTab.Face && text == "";
				if (flag8)
				{
					group = new Group(this.Desktop, this._parts)
					{
						LayoutMode = LayoutMode.Left
					};
					Document document;
					this.Interface.TryGetDocument("MainMenu/MyAvatar/EmptyPart.ui", out document);
					UIFragment uifragment = document.Instantiate(this.Desktop, group);
					uifragment.Get<Button>("Button").Activating = delegate()
					{
						this.SelectPart(this.GetProperty(this._activeTab), null, null, false);
					};
					bool flag9 = selectedPartId == null;
					if (flag9)
					{
						Button button = uifragment.Get<Button>("Button");
						BaseButtonStyle<Button.ButtonStyleState> style = button.Style;
						BaseButtonStyle<Button.ButtonStyleState> style2 = button.Style;
						BaseButtonStyle<Button.ButtonStyleState> style3 = button.Style;
						Button.ButtonStyleState buttonStyleState = new Button.ButtonStyleState();
						buttonStyleState.Background = this._previewBackgroundSelected;
						Button.ButtonStyleState hovered = buttonStyleState;
						style3.Pressed = buttonStyleState;
						style.Default = (style2.Hovered = hovered);
						Element element = new Element(this.Desktop, uifragment.Get<Group>("Container"));
						element.Anchor = this._previewSelectedFrameSize;
						element.Background = this._previewFrameBackgroundSelected;
					}
					else
					{
						new Element(this.Desktop, uifragment.Get<Group>("Container")).Background = this._previewFrameBackground;
					}
				}
				for (int i = 0; i < num3; i++)
				{
					int num5 = i / this._previewsPerRow;
					int num6 = i - num2;
					bool flag10 = num6 < 0;
					if (!flag10)
					{
						bool flag11 = group == null;
						if (flag11)
						{
							group = new Group(this.Desktop, this._parts)
							{
								LayoutMode = LayoutMode.Left
							};
						}
						Group group2 = new Group(this.Desktop, group);
						Anchor anchor = new Anchor
						{
							Width = new int?(this._previewWidth),
							Height = new int?(this._previewHeight),
							Left = new int?((i % this._previewsPerRow == 0) ? 0 : this._previewMargin),
							Bottom = new int?((num5 == num4 - 1) ? 0 : this._previewMargin)
						};
						group2.Anchor = anchor;
						Group parent = group2;
						bool flag12 = num6 < list.Count;
						if (flag12)
						{
							CharacterPart part = list[num6];
							CharacterPartId previewId = this.GetCharacterPartIdForPreview(property, part, selectedPartId);
							PartPreviewComponent partPreviewComponent = new PartPreviewComponent(this, parent, num5);
							anchor = new Anchor
							{
								Width = new int?(this._previewWidth),
								Height = new int?(this._previewHeight)
							};
							partPreviewComponent.Anchor = anchor;
							partPreviewComponent.Style = this._assetButtonStyle;
							CharacterPartId selectedPartId3 = selectedPartId;
							partPreviewComponent.IsSelected = (((selectedPartId3 != null) ? selectedPartId3.PartId : null) == part.Id);
							partPreviewComponent.MaskTexturePath = new UIPath("MainMenu/MyAvatar/PartMask.png");
							partPreviewComponent.Activating = delegate()
							{
								bool isShiftKeyDown = this.Desktop.IsShiftKeyDown;
								if (isShiftKeyDown)
								{
									this.EditAsset(part.Id);
								}
								else
								{
									this.SelectPart(this.GetProperty(this._activeTab), part, previewId, this._matchHairColors);
								}
							};
							PartPreviewComponent partPreviewComponent2 = partPreviewComponent;
							CharacterPartId selectedPartId2 = selectedPartId;
							bool flag13 = ((selectedPartId2 != null) ? selectedPartId2.PartId : null) == part.Id;
							if (flag13)
							{
								Element element2 = new Element(this.Desktop, parent);
								element2.Anchor = this._previewSelectedFrameSize;
								element2.Background = this._previewFrameBackgroundSelected;
							}
							else
							{
								new Element(this.Desktop, parent).Background = this._previewFrameBackground;
							}
							bool updateRender = true;
							Texture texture;
							bool flag14 = this._previewCache.TryGetValue(part.Id, out texture);
							if (flag14)
							{
								partPreviewComponent2.Texture = texture;
								updateRender = false;
							}
							this._previewComponents[part.Id] = partPreviewComponent2;
							partPreviewComponent2.Setup(property, part, previewId, this._previewBackgroundColor, this._previewBackgroundColorHovered, updateRender);
						}
						bool flag15 = i % this._previewsPerRow == this._previewsPerRow - 1;
						if (flag15)
						{
							group = null;
						}
					}
				}
				bool flag16 = selectedPart != null;
				if (flag16)
				{
					List<KeyValuePair<string, string[]>> list2 = new List<KeyValuePair<string, string[]>>();
					CharacterPartStore characterPartStore = this.MainMenuView.Interface.App.CharacterPartStore;
					CharacterPartGradientSet characterPartGradientSet;
					bool flag17 = selectedPart.GradientSet != null && characterPartStore.GradientSets.TryGetValue(selectedPart.GradientSet, out characterPartGradientSet);
					if (flag17)
					{
						foreach (KeyValuePair<string, CharacterPartTintColor> keyValuePair in characterPartGradientSet.Gradients)
						{
							list2.Add(new KeyValuePair<string, string[]>(keyValuePair.Key, keyValuePair.Value.BaseColor));
						}
					}
					bool flag18 = selectedPart.Variants != null;
					if (flag18)
					{
						bool flag19 = selectedPart.Variants[selectedPartId.VariantId].Textures != null;
						if (flag19)
						{
							foreach (KeyValuePair<string, CharacterPartTexture> keyValuePair2 in selectedPart.Variants[selectedPartId.VariantId].Textures)
							{
								list2.Add(new KeyValuePair<string, string[]>(keyValuePair2.Key, keyValuePair2.Value.BaseColor));
							}
						}
					}
					else
					{
						bool flag20 = selectedPart.Textures != null;
						if (flag20)
						{
							foreach (KeyValuePair<string, CharacterPartTexture> keyValuePair3 in selectedPart.Textures)
							{
								list2.Add(new KeyValuePair<string, string[]>(keyValuePair3.Key, keyValuePair3.Value.BaseColor));
							}
						}
					}
					this.BuildColorSelection(this._colors, list2, selectedPartId.ColorId, delegate(string color)
					{
						this.SelectPart(this.GetProperty(this._activeTab), selectedPart, new CharacterPartId(selectedPart.Id, selectedPartId.VariantId, color), this._matchHairColors);
					});
					bool flag21 = selectedPart.Variants != null;
					if (flag21)
					{
						List<DropdownBox.DropdownEntryInfo> entries = Enumerable.ToList<DropdownBox.DropdownEntryInfo>(Enumerable.Select<KeyValuePair<string, CharacterPartVariant>, DropdownBox.DropdownEntryInfo>(selectedPart.Variants, (KeyValuePair<string, CharacterPartVariant> variant) => new DropdownBox.DropdownEntryInfo(this.Desktop.Provider.GetText("characterCreator.variants." + variant.Key, null, true), variant.Key, false)));
						this._variantsContainer.Visible = true;
						this._variants.Entries = entries;
						this._variants.Value = selectedPartId.VariantId;
						this._variants.ValueChanged = delegate()
						{
							string text3 = selectedPartId.ColorId;
							List<string> colorOptions = this.Interface.App.CharacterPartStore.GetColorOptions(selectedPart, this._variants.Value);
							bool flag22 = !colorOptions.Contains(text3);
							if (flag22)
							{
								text3 = Enumerable.First<string>(colorOptions);
							}
							this.SelectPart(this.GetProperty(this._activeTab), selectedPart, new CharacterPartId(selectedPart.Id, this._variants.Value, text3), this._matchHairColors);
						};
					}
					else
					{
						this._variantsContainer.Visible = false;
					}
				}
				else
				{
					this._colors.Clear();
					this._variantsContainer.Visible = false;
				}
				this.UpdatePartPreviewVisibilities();
			}
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x00081084 File Offset: 0x0007F284
		private void UpdatePartPreviewVisibilities()
		{
			int num = this.Desktop.ScaleRound((float)this._previewHeight) + this.Desktop.ScaleRound((float)this._previewMargin);
			int num2 = Math.Max((int)Math.Floor((double)this._parts.ScaledScrollOffset.Y / (double)num) - 1, 0);
			int num3 = Math.Min(num2 + this._visiblePreviewRows - 1 + 2, this._parts.Children.Count - 1);
			foreach (PartPreviewComponent partPreviewComponent in this._previewComponents.Values)
			{
				bool flag = partPreviewComponent.Row >= num2 && partPreviewComponent.Row <= num3;
				if (flag)
				{
					bool flag2 = !partPreviewComponent.IsInView;
					if (flag2)
					{
						partPreviewComponent.IsInView = true;
						partPreviewComponent.Update();
					}
				}
				else
				{
					bool isInView = partPreviewComponent.IsInView;
					if (isInView)
					{
						partPreviewComponent.IsInView = false;
					}
				}
			}
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x000811A8 File Offset: 0x0007F3A8
		private void UpdateBodyType()
		{
			ClientPlayerSkin editedSkin = this.Interface.App.MainMenu.EditedSkin;
			Enumerable.Last<Element>(this._bodyTypeMasculine.Children).Visible = (editedSkin.BodyType == CharacterBodyType.Masculine);
			this._bodyTypeMasculine.Style = ((editedSkin.BodyType == CharacterBodyType.Masculine) ? this._bodyTypeButtonSelectedStyle : this._bodyTypeButtonStyle);
			Enumerable.Last<Element>(this._bodyTypeFeminine.Children).Visible = (editedSkin.BodyType == CharacterBodyType.Feminine);
			this._bodyTypeFeminine.Style = ((editedSkin.BodyType == CharacterBodyType.Feminine) ? this._bodyTypeButtonSelectedStyle : this._bodyTypeButtonStyle);
		}

		// Token: 0x060039F1 RID: 14833 RVA: 0x00081250 File Offset: 0x0007F450
		private void UpdateSkinTones()
		{
			string skinTone2 = this.Interface.App.MainMenu.EditedSkin.SkinTone;
			this.BuildColorSelection(this._skinTones, Enumerable.ToList<KeyValuePair<string, string[]>>(Enumerable.Select<KeyValuePair<string, CharacterPartTintColor>, KeyValuePair<string, string[]>>(this.Interface.App.CharacterPartStore.GradientSets["Skin"].Gradients, (KeyValuePair<string, CharacterPartTintColor> tone) => new KeyValuePair<string, string[]>(tone.Key, tone.Value.BaseColor))), skinTone2, delegate(string skinTone)
			{
				this.MainMenuView.MainMenu.SetCharacterAsset(PlayerSkinProperty.SkinTone, new CharacterPartId(skinTone, null), true);
				this.OnCharacterChanged();
			});
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x000812E0 File Offset: 0x0007F4E0
		public MyAvatarPage(MainMenuView mainMenuView) : base(mainMenuView.Interface, null)
		{
			this.MainMenuView = mainMenuView;
			this._skinJsonPopup = new SkinJsonPopup(this);
			this._failedToSyncDialogSetup = new ModalDialog.DialogSetup
			{
				Title = "ui.myAvatar.failedToSync",
				Text = "ui.myAvatar.tryAgainLater",
				Cancellable = false
			};
		}

		// Token: 0x060039F3 RID: 14835 RVA: 0x00081388 File Offset: 0x0007F588
		protected override void OnMounted()
		{
			this.SetTab(MyAvatarPage.MyAvatarPageTab.General);
			this.UpdateElements();
			this.MainMenuView.ShowTopBar(true);
			this._undoButton.Disabled = true;
			this._undoButton.Find<Group>("Icon").Background = this._undoIconDisabled;
			this._redoButton.Disabled = true;
			this._redoButton.Find<Group>("Icon").Background = this._redoIconDisabled;
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x060039F4 RID: 14836 RVA: 0x00081418 File Offset: 0x0007F618
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
			foreach (Texture texture in this._previewCache.Values)
			{
				texture.Dispose();
			}
			this._previewCache.Clear();
			this.RenderCharacterPartPreviewCommandQueue.Clear();
			this.MainMenuView.MainMenu.ClearSkinEditHistory();
			bool flag = this.MainMenuView.MainMenu.HasUnsavedSkinChanges();
			if (flag)
			{
				this.MainMenuView.MainMenu.CancelCharacter();
			}
		}

		// Token: 0x060039F5 RID: 14837 RVA: 0x000814E0 File Offset: 0x0007F6E0
		public void OnFailedToSync(Exception exception)
		{
			this.Interface.ModalDialog.Setup(this._failedToSyncDialogSetup);
			this.Desktop.SetLayer(4, this.Interface.ModalDialog);
		}

		// Token: 0x060039F6 RID: 14838 RVA: 0x00081514 File Offset: 0x0007F714
		public void OnAssetsReloaded()
		{
			this._reloadButton.Text = "Reload Assets";
			this._reloadButton.Disabled = false;
			bool isMounted = this._reloadButton.IsMounted;
			if (isMounted)
			{
				this._reloadButton.Parent.Layout(null, true);
			}
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x0008156C File Offset: 0x0007F76C
		public void OnSetCanUndoRedoSelection(bool canUndo, bool canRedo)
		{
			this._undoButton.Find<Group>("Icon").Background = (canUndo ? this._undoIcon : this._undoIconDisabled);
			this._undoButton.Disabled = !canUndo;
			this._undoButton.Layout(null, true);
			this._redoButton.Find<Group>("Icon").Background = (canRedo ? this._redoIcon : this._redoIconDisabled);
			this._redoButton.Disabled = !canRedo;
			this._redoButton.Layout(null, true);
		}

		// Token: 0x060039F8 RID: 14840 RVA: 0x00081610 File Offset: 0x0007F810
		private void SetTab(MyAvatarPage.MyAvatarPageTab tab)
		{
			bool flag = tab != this._activeTab;
			if (flag)
			{
				foreach (Texture texture in this._previewCache.Values)
				{
					texture.Dispose();
				}
				this._previewCache.Clear();
				this.RenderCharacterPartPreviewCommandQueue.Clear();
			}
			this._activeTab = tab;
			foreach (KeyValuePair<MyAvatarPage.MyAvatarPageTab, Button> keyValuePair in this._tabButtons)
			{
				string texturePath = string.Format("MainMenu/MyAvatar/CategoryIcons/{0}.png", keyValuePair.Key);
				Button value = keyValuePair.Value;
				value.Style.Default = new Button.ButtonStyleState
				{
					Background = new PatchStyle(texturePath)
					{
						Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 127)
					}
				};
				value.Style.Hovered = new Button.ButtonStyleState
				{
					Background = new PatchStyle(texturePath)
					{
						Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 220)
					}
				};
				value.Style.Pressed = new Button.ButtonStyleState
				{
					Background = new PatchStyle(texturePath)
					{
						Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 200)
					}
				};
				value.Children[0].Visible = false;
			}
			this._tabButtons[tab].Style.Default = new Button.ButtonStyleState
			{
				Background = new PatchStyle(string.Format("MainMenu/MyAvatar/CategoryIcons/{0}Selected.png", tab))
			};
			this._tabButtons[tab].Style.Hovered = null;
			this._tabButtons[tab].Style.Pressed = null;
			this._tabButtons[tab].Children[0].Visible = true;
			this._searchField.Value = "";
			this.SelectedTags.Clear();
			this._categoryName.Text = this.Desktop.Provider.GetText(string.Format("ui.myAvatar.tabs.{0}", this._activeTab), null, true);
			this._basicAttributesContainer.Visible = (tab == MyAvatarPage.MyAvatarPageTab.General);
			this._emoteListContainer.Visible = (tab == MyAvatarPage.MyAvatarPageTab.Emotes);
			this._partListContainer.Visible = (tab != MyAvatarPage.MyAvatarPageTab.General && tab != MyAvatarPage.MyAvatarPageTab.Emotes);
			this._parts.SetScroll(new int?(0), new int?(0));
			this._emotes.SetScroll(new int?(0), new int?(0));
		}

		// Token: 0x060039F9 RID: 14841 RVA: 0x00081920 File Offset: 0x0007FB20
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("MainMenu/MyAvatar/MyAvatarPage.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._previewWidth = document.ResolveNamedValue<int>(this.Desktop.Provider, "PartPreviewWidth");
			this._previewHeight = document.ResolveNamedValue<int>(this.Desktop.Provider, "PartPreviewHeight");
			this._previewMargin = document.ResolveNamedValue<int>(this.Desktop.Provider, "PartPreviewMargin");
			this._previewBackgroundColor = document.ResolveNamedValue<UInt32Color>(this.Desktop.Provider, "PartPreviewBackgroundColor");
			this._previewBackgroundColorHovered = document.ResolveNamedValue<UInt32Color>(this.Desktop.Provider, "PartPreviewBackgroundColorHovered");
			this._previewFrameBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "PartPreviewFrameBackground");
			this._previewFrameBackgroundSelected = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "PartPreviewSelectedFrameBackground");
			this._previewBackgroundSelected = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "PartPreviewSelectedBackground");
			this._previewsPerRow = document.ResolveNamedValue<int>(this.Desktop.Provider, "PartPreviewsPerRow");
			this._previewSelectedFrameSize = document.ResolveNamedValue<Anchor>(this.Desktop.Provider, "PartPreviewSelectedFrameSize");
			this._visiblePreviewRows = document.ResolveNamedValue<int>(this.Desktop.Provider, "PartPreviewRows");
			this._assetButtonStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "AssetButtonStyle");
			this._emoteButtonSounds = document.ResolveNamedValue<ButtonSounds>(this.Desktop.Provider, "EmoteButtonSounds");
			this._bodyTypeButtonStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "BodyTypeButtonStyle");
			this._bodyTypeButtonSelectedStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "BodyTypeButtonSelectedStyle");
			this._randomizationLockedButtonStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "RandomizationLockedButtonStyle");
			this._randomizationUnlockedButtonStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "RandomizationUnlockedButtonStyle");
			this._matchHairColorsOnButtonStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "MatchHairColorsOnButtonStyle");
			this._matchHairColorsOffButtonStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "MatchHairColorsOffButtonStyle");
			this._lockedCharacterOptionsForRandomization.Clear();
			this._matchHairColors = true;
			this._bodyTypeRandomizationLock = uifragment.Get<Button>("BodyTypeRandomizationLock");
			this._bodyTypeRandomizationLock.Activating = delegate()
			{
				this.ToggleRandomizationLock(PlayerSkinProperty.BodyType, this._bodyTypeRandomizationLock);
			};
			this._skinToneRandomizationLock = uifragment.Get<Button>("SkinToneRandomizationLock");
			this._skinToneRandomizationLock.Activating = delegate()
			{
				this.ToggleRandomizationLock(PlayerSkinProperty.SkinTone, this._skinToneRandomizationLock);
			};
			this._partRandomizationLock = uifragment.Get<Button>("PartRandomizationLock");
			this._partRandomizationLock.Activating = delegate()
			{
				this.ToggleRandomizationLock(this.GetProperty(this._activeTab), this._partRandomizationLock);
			};
			this._matchHairColorsButton = uifragment.Get<Button>("MatchHairColorsButton");
			this._matchHairColorsButton.Activating = delegate()
			{
				this._matchHairColors = !this._matchHairColors;
				this._matchHairColorsButton.Style = (this._matchHairColors ? this._matchHairColorsOnButtonStyle : this._matchHairColorsOffButtonStyle);
				this._matchHairColorsButton.TooltipText = this.Desktop.Provider.GetText(this._matchHairColors ? "ui.myAvatar.synchronizeHairColor.disable" : "ui.myAvatar.synchronizeHairColor.enable", null, true);
				this._matchHairColorsButton.Layout(null, true);
			};
			this._colorsContainer = uifragment.Get<Group>("ColorsContainer");
			this._partListContainer = uifragment.Get<Group>("PartListContainer");
			this._emoteListContainer = uifragment.Get<Group>("EmoteListContainer");
			this._basicAttributesContainer = uifragment.Get<Group>("BasicAttributesContainer");
			this._undoIcon = document.ResolveNamedValue<PatchStyle>(this.Interface, "UndoIcon");
			this._undoIconDisabled = document.ResolveNamedValue<PatchStyle>(this.Interface, "UndoIconDisabled");
			this._redoIcon = document.ResolveNamedValue<PatchStyle>(this.Interface, "RedoIcon");
			this._redoIconDisabled = document.ResolveNamedValue<PatchStyle>(this.Interface, "RedoIconDisabled");
			this._undoButton = uifragment.Get<Button>("Undo");
			this._undoButton.Activating = delegate()
			{
				this.MainMenuView.MainMenu.UndoCharacterSkinChange();
			};
			this._redoButton = uifragment.Get<Button>("Redo");
			this._redoButton.Activating = delegate()
			{
				this.MainMenuView.MainMenu.RedoCharacterSkinChange();
			};
			this._getJsonButton = uifragment.Get<TextButton>("GetJson");
			this._getJsonButton.Activating = delegate()
			{
				this.Desktop.SetLayer(2, this._skinJsonPopup);
			};
			this._getJsonButton.Visible = true;
			this._reloadButton = uifragment.Get<TextButton>("Reload");
			this._reloadButton.Activating = delegate()
			{
				this._reloadButton.Text = "Reloading...";
				this._reloadButton.Disabled = true;
				this._reloadButton.Parent.Layout(null, true);
				this.MainMenuView.MainMenu.ReloadCharacterAssets();
			};
			this._reloadButton.Visible = true;
			this._bodyTypeMasculine = uifragment.Get<Button>("BodyTypeMasculine");
			this._bodyTypeMasculine.Activating = delegate()
			{
				this.SetProperty(PlayerSkinProperty.BodyType, "Masculine");
			};
			this._bodyTypeFeminine = uifragment.Get<Button>("BodyTypeFeminine");
			this._bodyTypeFeminine.Activating = delegate()
			{
				this.SetProperty(PlayerSkinProperty.BodyType, "Feminine");
			};
			uifragment.Get<TextButton>("ResetOptions").Activating = delegate()
			{
				this.MainMenuView.MainMenu.MakeEditedSkinNaked();
			};
			uifragment.Get<Button>("Randomize").Activating = delegate()
			{
				this.MainMenuView.MainMenu.RandomizeCharacter(this._lockedCharacterOptionsForRandomization);
			};
			uifragment.Get<TextButton>("DiscardChanges").Activating = delegate()
			{
				this.MainMenuView.MainMenu.CancelCharacter();
				this.MainMenuView.MainMenu.Open(AppMainMenu.MainMenuPage.Home);
			};
			uifragment.Get<TextButton>("SaveChanges").Activating = delegate()
			{
				this.MainMenuView.MainMenu.SaveCharacter();
				this.MainMenuView.MainMenu.Open(AppMainMenu.MainMenuPage.Home);
			};
			this._categoryName = uifragment.Get<Label>("CategoryName");
			this._parts = uifragment.Get<Group>("Parts");
			this._parts.Scrolled = new Action(this.UpdatePartPreviewVisibilities);
			this._searchField = uifragment.Get<TextField>("SearchField");
			this._searchField.ValueChanged = delegate()
			{
				this.UpdatePartList();
				base.Layout(null, true);
			};
			this._tabButtons.Clear();
			using (IEnumerator enumerator = Enum.GetValues(typeof(MyAvatarPage.MyAvatarPageTab)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MyAvatarPage.MyAvatarPageTab tab = (MyAvatarPage.MyAvatarPageTab)enumerator.Current;
					this._tabButtons[tab] = uifragment.Get<Group>("Tab" + tab.ToString()).Find<Button>("Button");
					this._tabButtons[tab].Activating = delegate()
					{
						this.SetTab(tab);
						this.UpdateElements();
						this.Layout(null, true);
					};
				}
			}
			this._colors = uifragment.Get<Group>("Colors");
			this._variantsContainer = uifragment.Get<Group>("VariantsContainer");
			this._variants = uifragment.Get<DropdownBox>("Variants");
			this._skinTones = uifragment.Get<Group>("SkinTones");
			this._emotes = uifragment.Get<Group>("Emotes");
			this._tags = uifragment.Get<DropdownBox>("Tags");
			this._tags.ValueChanged = delegate()
			{
				this.SelectedTags.Clear();
				foreach (string text in this._tags.SelectedValues)
				{
					this.SelectedTags.Add(text);
				}
				this.UpdatePartList();
				base.Layout(null, true);
			};
			this.SetTab(this._activeTab);
			this.BuildEmoteList();
			bool flag = this.MainMenuView.MainMenu.EditedSkin != null;
			if (flag)
			{
				this.UpdateElements();
			}
			this._skinJsonPopup.Build();
		}

		// Token: 0x060039FA RID: 14842 RVA: 0x0008202C File Offset: 0x0008022C
		private void UpdateElements()
		{
			bool flag = this._activeTab == MyAvatarPage.MyAvatarPageTab.General;
			if (flag)
			{
				this.UpdateSkinTones();
				this.UpdateBodyType();
			}
			else
			{
				this.UpdateTags();
				this.UpdatePartList();
			}
		}

		// Token: 0x060039FB RID: 14843 RVA: 0x0008206C File Offset: 0x0008026C
		private void ToggleRandomizationLock(PlayerSkinProperty property, Button button)
		{
			bool flag = !this._lockedCharacterOptionsForRandomization.Contains(property);
			if (flag)
			{
				this._lockedCharacterOptionsForRandomization.Add(property);
				button.Style = this._randomizationLockedButtonStyle;
				button.TooltipText = this.Desktop.Provider.GetText("ui.myAvatar.unlockProperty", null, true);
				button.Layout(null, true);
			}
			else
			{
				this._lockedCharacterOptionsForRandomization.Remove(property);
				button.Style = this._randomizationUnlockedButtonStyle;
				button.TooltipText = this.Desktop.Provider.GetText("ui.myAvatar.lockProperty", null, true);
				button.Layout(null, true);
			}
		}

		// Token: 0x060039FC RID: 14844 RVA: 0x00082124 File Offset: 0x00080324
		private void BuildColorSelection(Group container, List<KeyValuePair<string, string[]>> colors, string selectedColor, Action<string> onSelect)
		{
			container.Clear();
			Group group = null;
			Document document;
			this.Interface.TryGetDocument("MainMenu/MyAvatar/ColorOption.ui", out document);
			int num = document.ResolveNamedValue<int>(this.Desktop.Provider, "ColorsPerRow");
			int num2 = 0;
			using (List<KeyValuePair<string, string[]>>.Enumerator enumerator = colors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string[]> color = enumerator.Current;
					bool flag = group == null;
					if (flag)
					{
						group = new Group(this.Desktop, container)
						{
							LayoutMode = LayoutMode.Left
						};
					}
					UIFragment uifragment = document.Instantiate(this.Desktop, group);
					Button button = uifragment.Get<Button>("Button");
					button.Anchor.Left = new int?((num2 % num == 0) ? 0 : 8);
					button.Activating = delegate()
					{
						onSelect(color.Key);
					};
					Group parent = uifragment.Get<Group>("Colors");
					foreach (string text in color.Value)
					{
						Element element = new Element(this.Desktop, parent);
						element.FlexWeight = 1;
						element.Background = new PatchStyle(UInt32Color.FromHexString(text));
					}
					bool flag2 = selectedColor == color.Key;
					if (flag2)
					{
						uifragment.Get<Element>("SelectedHighlight").Visible = true;
					}
					bool flag3 = num2 % num == num - 1;
					if (flag3)
					{
						group = null;
					}
					num2++;
				}
			}
		}

		// Token: 0x060039FD RID: 14845 RVA: 0x000822EC File Offset: 0x000804EC
		public void OnCharacterChanged()
		{
			this.UpdateElements();
			base.Layout(null, true);
		}

		// Token: 0x060039FE RID: 14846 RVA: 0x00082314 File Offset: 0x00080514
		private void EditAsset(string assetId)
		{
			this.MainMenuView.MainMenu.OpenAssetIdInCosmeticEditor("Cosmetics." + this.GetProperty(this._activeTab).ToString(), assetId);
		}

		// Token: 0x060039FF RID: 14847 RVA: 0x00082358 File Offset: 0x00080558
		private void SetProperty(PlayerSkinProperty property, string val)
		{
			this.MainMenuView.MainMenu.SetCharacterAsset(property, CharacterPartId.FromString(val), true);
			this.OnCharacterChanged();
		}

		// Token: 0x06003A00 RID: 14848 RVA: 0x0008237C File Offset: 0x0008057C
		private PlayerSkinProperty GetProperty(MyAvatarPage.MyAvatarPageTab tab)
		{
			switch (tab)
			{
			case MyAvatarPage.MyAvatarPageTab.Face:
				return PlayerSkinProperty.Face;
			case MyAvatarPage.MyAvatarPageTab.FacialHair:
				return PlayerSkinProperty.FacialHair;
			case MyAvatarPage.MyAvatarPageTab.Eyebrows:
				return PlayerSkinProperty.Eyebrows;
			case MyAvatarPage.MyAvatarPageTab.Eyes:
				return PlayerSkinProperty.Eyes;
			case MyAvatarPage.MyAvatarPageTab.Haircut:
				return PlayerSkinProperty.Haircut;
			case MyAvatarPage.MyAvatarPageTab.Pants:
				return PlayerSkinProperty.Pants;
			case MyAvatarPage.MyAvatarPageTab.Overpants:
				return PlayerSkinProperty.Overpants;
			case MyAvatarPage.MyAvatarPageTab.Undertop:
				return PlayerSkinProperty.Undertop;
			case MyAvatarPage.MyAvatarPageTab.Overtop:
				return PlayerSkinProperty.Overtop;
			case MyAvatarPage.MyAvatarPageTab.Shoes:
				return PlayerSkinProperty.Shoes;
			case MyAvatarPage.MyAvatarPageTab.Gloves:
				return PlayerSkinProperty.Gloves;
			case MyAvatarPage.MyAvatarPageTab.HeadAccessory:
				return PlayerSkinProperty.HeadAccessory;
			case MyAvatarPage.MyAvatarPageTab.FaceAccessory:
				return PlayerSkinProperty.FaceAccessory;
			case MyAvatarPage.MyAvatarPageTab.EarAccessory:
				return PlayerSkinProperty.EarAccessory;
			}
			throw new Exception("No property for tab " + tab.ToString());
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x00082434 File Offset: 0x00080634
		private CharacterPartId GetSelectedPartId(PlayerSkinProperty property)
		{
			ClientPlayerSkin editedSkin = this.MainMenuView.MainMenu.EditedSkin;
			switch (property)
			{
			case PlayerSkinProperty.BodyType:
				return new CharacterPartId(editedSkin.BodyType.ToString(), null);
			case PlayerSkinProperty.SkinTone:
				return new CharacterPartId(editedSkin.SkinTone, null);
			case PlayerSkinProperty.Haircut:
				return editedSkin.Haircut;
			case PlayerSkinProperty.FacialHair:
				return editedSkin.FacialHair;
			case PlayerSkinProperty.Eyebrows:
				return editedSkin.Eyebrows;
			case PlayerSkinProperty.Eyes:
				return editedSkin.Eyes;
			case PlayerSkinProperty.Face:
				return new CharacterPartId(editedSkin.Face, editedSkin.SkinTone);
			case PlayerSkinProperty.Pants:
				return editedSkin.Pants;
			case PlayerSkinProperty.Overpants:
				return editedSkin.Overpants;
			case PlayerSkinProperty.Undertop:
				return editedSkin.Undertop;
			case PlayerSkinProperty.Overtop:
				return editedSkin.Overtop;
			case PlayerSkinProperty.Shoes:
				return editedSkin.Shoes;
			case PlayerSkinProperty.HeadAccessory:
				return editedSkin.HeadAccessory;
			case PlayerSkinProperty.FaceAccessory:
				return editedSkin.FaceAccessory;
			case PlayerSkinProperty.EarAccessory:
				return editedSkin.EarAccessory;
			case PlayerSkinProperty.Gloves:
				return editedSkin.Gloves;
			}
			return null;
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x00082568 File Offset: 0x00080768
		private CharacterPartId GetCharacterPartIdForPreview(PlayerSkinProperty property, CharacterPart part, CharacterPartId selectedPartId)
		{
			bool flag = selectedPartId != null && part.Id == selectedPartId.PartId;
			CharacterPartId result;
			if (flag)
			{
				result = selectedPartId;
			}
			else
			{
				CharacterPartStore characterPartStore = this.MainMenuView.Interface.App.CharacterPartStore;
				if (property - PlayerSkinProperty.Haircut > 2)
				{
					if (property == PlayerSkinProperty.Eyes)
					{
						bool flag2 = selectedPartId != null;
						if (flag2)
						{
							bool flag3 = part.Textures != null && part.Textures.ContainsKey(selectedPartId.ColorId);
							if (flag3)
							{
								return new CharacterPartId(part.Id, null, selectedPartId.ColorId);
							}
							bool flag4 = part.GradientSet != null;
							if (flag4)
							{
								Dictionary<string, CharacterPartTintColor>.KeyCollection keys = characterPartStore.GradientSets[part.GradientSet].Gradients.Keys;
								bool flag5 = Enumerable.Contains<string>(keys, selectedPartId.ColorId);
								if (flag5)
								{
									return new CharacterPartId(part.Id, null, selectedPartId.ColorId);
								}
							}
						}
					}
				}
				else
				{
					bool flag6 = part.GradientSet != null;
					if (flag6)
					{
						Dictionary<string, CharacterPartTintColor>.KeyCollection keys2 = characterPartStore.GradientSets[part.GradientSet].Gradients.Keys;
						bool flag7 = selectedPartId != null && Enumerable.Contains<string>(keys2, selectedPartId.ColorId);
						if (flag7)
						{
							string id = part.Id;
							Dictionary<string, CharacterPartVariant> variants = part.Variants;
							return new CharacterPartId(id, (variants != null) ? Enumerable.First<KeyValuePair<string, CharacterPartVariant>>(variants).Key : null, selectedPartId.ColorId);
						}
						ClientPlayerSkin editedSkin = this.Interface.App.MainMenu.EditedSkin;
						bool flag8 = editedSkin.Haircut != null;
						if (flag8)
						{
							selectedPartId = editedSkin.Haircut;
						}
						else
						{
							bool flag9 = editedSkin.FacialHair != null;
							if (flag9)
							{
								selectedPartId = editedSkin.FacialHair;
							}
							else
							{
								bool flag10 = editedSkin.Eyebrows != null;
								if (flag10)
								{
									selectedPartId = editedSkin.Eyebrows;
								}
							}
						}
						bool flag11 = selectedPartId != null && Enumerable.Contains<string>(keys2, selectedPartId.ColorId);
						if (flag11)
						{
							string id2 = part.Id;
							Dictionary<string, CharacterPartVariant> variants2 = part.Variants;
							return new CharacterPartId(id2, (variants2 != null) ? Enumerable.First<KeyValuePair<string, CharacterPartVariant>>(variants2).Key : null, selectedPartId.ColorId);
						}
					}
				}
				bool flag12 = part.GradientSet != null;
				if (flag12)
				{
					CharacterPartGradientSet characterPartGradientSet;
					bool flag13 = !characterPartStore.GradientSets.TryGetValue(part.GradientSet, out characterPartGradientSet);
					if (flag13)
					{
						throw new Exception(string.Format("Gradient set '{0}' in '{1}.{2}' does not exist", part.GradientSet, property, part.Id));
					}
					bool flag14 = part.Variants != null;
					if (flag14)
					{
						result = new CharacterPartId(part.Id, Enumerable.First<KeyValuePair<string, CharacterPartVariant>>(part.Variants).Key, Enumerable.First<KeyValuePair<string, CharacterPartTintColor>>(characterPartGradientSet.Gradients).Key);
					}
					else
					{
						result = new CharacterPartId(part.Id, Enumerable.First<KeyValuePair<string, CharacterPartTintColor>>(characterPartGradientSet.Gradients).Key);
					}
				}
				else
				{
					bool flag15 = part.Variants != null;
					if (flag15)
					{
						result = new CharacterPartId(part.Id, Enumerable.First<KeyValuePair<string, CharacterPartVariant>>(part.Variants).Key, Enumerable.First<KeyValuePair<string, CharacterPartTexture>>(Enumerable.First<KeyValuePair<string, CharacterPartVariant>>(part.Variants).Value.Textures).Key);
					}
					else
					{
						bool flag16 = part.Textures != null;
						if (!flag16)
						{
							throw new Exception("Part has no texture defined");
						}
						result = new CharacterPartId(part.Id, Enumerable.First<KeyValuePair<string, CharacterPartTexture>>(part.Textures).Key);
					}
				}
			}
			return result;
		}

		// Token: 0x06003A03 RID: 14851 RVA: 0x000828FC File Offset: 0x00080AFC
		private bool HasCharacterPartColor(CharacterPart part, string colorId, string variantId)
		{
			CharacterPartStore characterPartStore = this.MainMenuView.Interface.App.CharacterPartStore;
			bool flag = part == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = variantId == null;
				if (flag2)
				{
					bool flag3 = part.Textures != null;
					if (flag3)
					{
						return part.Textures.ContainsKey(colorId);
					}
				}
				else
				{
					CharacterPartVariant characterPartVariant;
					bool flag4 = part.Variants != null && part.Variants.TryGetValue(variantId, out characterPartVariant);
					if (flag4)
					{
						return characterPartVariant.Textures.ContainsKey(colorId);
					}
				}
				CharacterPartGradientSet characterPartGradientSet;
				bool flag5 = part.GradientSet != null && characterPartStore.GradientSets.TryGetValue(part.GradientSet, out characterPartGradientSet);
				result = (flag5 && characterPartGradientSet.Gradients.ContainsKey(colorId));
			}
			return result;
		}

		// Token: 0x06003A04 RID: 14852 RVA: 0x000829C4 File Offset: 0x00080BC4
		private void AttemptToMatchTexture(string textureId, PlayerSkinProperty property)
		{
			string matchingPartId = this.GetMatchingPartId(textureId, property);
			bool flag = matchingPartId != null;
			if (flag)
			{
				this.MainMenuView.MainMenu.SetCharacterAsset(property, CharacterPartId.FromString(matchingPartId), false);
			}
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x00082A00 File Offset: 0x00080C00
		private string GetMatchingPartId(string textureId, PlayerSkinProperty property)
		{
			CharacterPartId currentSelectedId = this.GetSelectedPartId(property);
			bool flag = currentSelectedId != null;
			if (flag)
			{
				CharacterPart characterPart = this.Interface.App.CharacterPartStore.GetParts(property).Find((CharacterPart p) => p.Id == currentSelectedId.PartId);
				bool flag2 = characterPart.Variants != null;
				if (flag2)
				{
					bool flag3 = this.HasCharacterPartColor(characterPart, textureId, currentSelectedId.VariantId);
					if (flag3)
					{
						return CharacterPartId.BuildString(characterPart.Id, currentSelectedId.VariantId, textureId);
					}
				}
				else
				{
					bool flag4 = this.HasCharacterPartColor(characterPart, textureId, null);
					if (flag4)
					{
						return CharacterPartId.BuildString(characterPart.Id, null, textureId);
					}
				}
			}
			return null;
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x00082ACC File Offset: 0x00080CCC
		private void SelectPart(PlayerSkinProperty property, CharacterPart part, CharacterPartId id, bool matchHairColors)
		{
			bool flag = part != null;
			if (flag)
			{
				Debug.Assert(id.ColorId != null);
				Debug.Assert((part.Variants == null && id.VariantId == null) || (part.Variants != null && id.VariantId != null));
				if (matchHairColors)
				{
					switch (property)
					{
					case PlayerSkinProperty.Haircut:
						this.AttemptToMatchTexture(id.ColorId, PlayerSkinProperty.Eyebrows);
						this.AttemptToMatchTexture(id.ColorId, PlayerSkinProperty.FacialHair);
						break;
					case PlayerSkinProperty.FacialHair:
						this.AttemptToMatchTexture(id.ColorId, PlayerSkinProperty.Eyebrows);
						this.AttemptToMatchTexture(id.ColorId, PlayerSkinProperty.Haircut);
						break;
					case PlayerSkinProperty.Eyebrows:
						this.AttemptToMatchTexture(id.ColorId, PlayerSkinProperty.FacialHair);
						this.AttemptToMatchTexture(id.ColorId, PlayerSkinProperty.Haircut);
						break;
					}
				}
			}
			this.MainMenuView.MainMenu.SetCharacterAsset(property, id, true);
			this.OnCharacterChanged();
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x00082BC0 File Offset: 0x00080DC0
		private void BuildEmoteList()
		{
			this._emotes.Clear();
			using (List<Emote>.Enumerator enumerator = this.Interface.App.CharacterPartStore.Emotes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Emote emote = enumerator.Current;
					TextButton textButton = new TextButton(this.Desktop, this._emotes);
					textButton.Text = emote.Name;
					textButton.Anchor = new Anchor
					{
						Bottom = new int?(8),
						Height = new int?(44)
					};
					textButton.Padding = new Padding
					{
						Horizontal = new int?(14)
					};
					textButton.Style = new TextButton.TextButtonStyle
					{
						Default = new TextButton.TextButtonStyleState
						{
							Background = new PatchStyle("Common/InputBox.png")
							{
								Border = 16
							},
							LabelStyle = new LabelStyle
							{
								VerticalAlignment = LabelStyle.LabelAlignment.Center
							}
						},
						Sounds = this._emoteButtonSounds
					};
					textButton.Activating = delegate()
					{
						this.MainMenuView.MainMenu.PlayCharacterEmote(emote.Id);
					};
				}
			}
		}

		// Token: 0x040019DB RID: 6619
		private int _previewWidth;

		// Token: 0x040019DC RID: 6620
		private int _previewHeight;

		// Token: 0x040019DD RID: 6621
		private int _previewMargin;

		// Token: 0x040019DE RID: 6622
		private int _previewsPerRow;

		// Token: 0x040019DF RID: 6623
		private int _visiblePreviewRows;

		// Token: 0x040019E0 RID: 6624
		private UInt32Color _previewBackgroundColor;

		// Token: 0x040019E1 RID: 6625
		private UInt32Color _previewBackgroundColorHovered;

		// Token: 0x040019E2 RID: 6626
		private PatchStyle _previewFrameBackground;

		// Token: 0x040019E3 RID: 6627
		private PatchStyle _previewFrameBackgroundSelected;

		// Token: 0x040019E4 RID: 6628
		private PatchStyle _previewBackgroundSelected;

		// Token: 0x040019E5 RID: 6629
		private Anchor _previewSelectedFrameSize;

		// Token: 0x040019E6 RID: 6630
		private Button.ButtonStyle _assetButtonStyle;

		// Token: 0x040019E7 RID: 6631
		private Group _colors;

		// Token: 0x040019E8 RID: 6632
		private Group _parts;

		// Token: 0x040019E9 RID: 6633
		private Group _variantsContainer;

		// Token: 0x040019EA RID: 6634
		private DropdownBox _variants;

		// Token: 0x040019EB RID: 6635
		private Label _categoryName;

		// Token: 0x040019EC RID: 6636
		private TextField _searchField;

		// Token: 0x040019ED RID: 6637
		private DropdownBox _tags;

		// Token: 0x040019EE RID: 6638
		public readonly HashSet<string> SelectedTags = new HashSet<string>();

		// Token: 0x040019EF RID: 6639
		public readonly List<AppMainMenu.RenderCharacterPartPreviewCommand> RenderCharacterPartPreviewCommandQueue = new List<AppMainMenu.RenderCharacterPartPreviewCommand>();

		// Token: 0x040019F0 RID: 6640
		private readonly Dictionary<string, PartPreviewComponent> _previewComponents = new Dictionary<string, PartPreviewComponent>();

		// Token: 0x040019F1 RID: 6641
		private readonly Dictionary<string, Texture> _previewCache = new Dictionary<string, Texture>();

		// Token: 0x040019F2 RID: 6642
		private Group _skinTones;

		// Token: 0x040019F3 RID: 6643
		private Button _bodyTypeMasculine;

		// Token: 0x040019F4 RID: 6644
		private Button _bodyTypeFeminine;

		// Token: 0x040019F5 RID: 6645
		private Button.ButtonStyle _bodyTypeButtonStyle;

		// Token: 0x040019F6 RID: 6646
		private Button.ButtonStyle _bodyTypeButtonSelectedStyle;

		// Token: 0x040019F7 RID: 6647
		public readonly MainMenuView MainMenuView;

		// Token: 0x040019F8 RID: 6648
		private MyAvatarPage.MyAvatarPageTab _activeTab = MyAvatarPage.MyAvatarPageTab.General;

		// Token: 0x040019F9 RID: 6649
		private readonly Dictionary<MyAvatarPage.MyAvatarPageTab, Button> _tabButtons = new Dictionary<MyAvatarPage.MyAvatarPageTab, Button>();

		// Token: 0x040019FA RID: 6650
		private readonly SkinJsonPopup _skinJsonPopup;

		// Token: 0x040019FB RID: 6651
		private Group _partListContainer;

		// Token: 0x040019FC RID: 6652
		private Group _emoteListContainer;

		// Token: 0x040019FD RID: 6653
		private Group _basicAttributesContainer;

		// Token: 0x040019FE RID: 6654
		private Group _colorsContainer;

		// Token: 0x040019FF RID: 6655
		private PatchStyle _undoIcon;

		// Token: 0x04001A00 RID: 6656
		private PatchStyle _undoIconDisabled;

		// Token: 0x04001A01 RID: 6657
		private PatchStyle _redoIcon;

		// Token: 0x04001A02 RID: 6658
		private PatchStyle _redoIconDisabled;

		// Token: 0x04001A03 RID: 6659
		private Button _undoButton;

		// Token: 0x04001A04 RID: 6660
		private Button _redoButton;

		// Token: 0x04001A05 RID: 6661
		private TextButton _getJsonButton;

		// Token: 0x04001A06 RID: 6662
		private TextButton _reloadButton;

		// Token: 0x04001A07 RID: 6663
		private readonly ModalDialog.DialogSetup _failedToSyncDialogSetup;

		// Token: 0x04001A08 RID: 6664
		private Button.ButtonStyle _randomizationLockedButtonStyle;

		// Token: 0x04001A09 RID: 6665
		private Button.ButtonStyle _randomizationUnlockedButtonStyle;

		// Token: 0x04001A0A RID: 6666
		private Button.ButtonStyle _matchHairColorsOnButtonStyle;

		// Token: 0x04001A0B RID: 6667
		private Button.ButtonStyle _matchHairColorsOffButtonStyle;

		// Token: 0x04001A0C RID: 6668
		private Button _bodyTypeRandomizationLock;

		// Token: 0x04001A0D RID: 6669
		private Button _skinToneRandomizationLock;

		// Token: 0x04001A0E RID: 6670
		private Button _matchHairColorsButton;

		// Token: 0x04001A0F RID: 6671
		private Button _partRandomizationLock;

		// Token: 0x04001A10 RID: 6672
		private HashSet<PlayerSkinProperty> _lockedCharacterOptionsForRandomization = new HashSet<PlayerSkinProperty>();

		// Token: 0x04001A11 RID: 6673
		private bool _matchHairColors = true;

		// Token: 0x04001A12 RID: 6674
		private Group _emotes;

		// Token: 0x04001A13 RID: 6675
		private ButtonSounds _emoteButtonSounds;

		// Token: 0x02000D03 RID: 3331
		private enum MyAvatarPageTab
		{
			// Token: 0x0400406F RID: 16495
			General,
			// Token: 0x04004070 RID: 16496
			Face,
			// Token: 0x04004071 RID: 16497
			Emotes,
			// Token: 0x04004072 RID: 16498
			FacialHair,
			// Token: 0x04004073 RID: 16499
			Eyebrows,
			// Token: 0x04004074 RID: 16500
			Eyes,
			// Token: 0x04004075 RID: 16501
			Haircut,
			// Token: 0x04004076 RID: 16502
			Pants,
			// Token: 0x04004077 RID: 16503
			Overpants,
			// Token: 0x04004078 RID: 16504
			Undertop,
			// Token: 0x04004079 RID: 16505
			Overtop,
			// Token: 0x0400407A RID: 16506
			Shoes,
			// Token: 0x0400407B RID: 16507
			Gloves,
			// Token: 0x0400407C RID: 16508
			HeadAccessory,
			// Token: 0x0400407D RID: 16509
			FaceAccessory,
			// Token: 0x0400407E RID: 16510
			EarAccessory
		}
	}
}
