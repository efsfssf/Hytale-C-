using System;
using System.Collections.Generic;
using System.Reflection;
using HytaleClient.Core;
using Newtonsoft.Json;
using SDL2;

namespace HytaleClient.Data.UserSettings
{
	// Token: 0x02000AD4 RID: 2772
	internal class InputBindings
	{
		// Token: 0x06005774 RID: 22388 RVA: 0x001A7F20 File Offset: 0x001A6120
		public void ResetDefaults()
		{
			foreach (FieldInfo fieldInfo in base.GetType().GetFields())
			{
				bool flag = fieldInfo.FieldType == typeof(InputBinding);
				if (flag)
				{
					fieldInfo.SetValue(this, null);
				}
			}
			this.Setup();
		}

		// Token: 0x06005775 RID: 22389 RVA: 0x001A7F78 File Offset: 0x001A6178
		public void Setup()
		{
			bool flag = SDL.SDL_WasInit(32U) == 0U;
			if (flag)
			{
				throw new Exception("The SDL video subsystem must be initialized to access keyboard layout");
			}
			this.MoveForwards = (this.MoveForwards ?? InputBinding.FromScancode(SDL.SDL_Scancode.SDL_SCANCODE_W));
			this.MoveBackwards = (this.MoveBackwards ?? InputBinding.FromScancode(SDL.SDL_Scancode.SDL_SCANCODE_S));
			this.StrafeLeft = (this.StrafeLeft ?? InputBinding.FromScancode(SDL.SDL_Scancode.SDL_SCANCODE_A));
			this.StrafeRight = (this.StrafeRight ?? InputBinding.FromScancode(SDL.SDL_Scancode.SDL_SCANCODE_D));
			this.OpenMachinimaEditor = (this.OpenMachinimaEditor ?? InputBinding.FromScancode(SDL.SDL_Scancode.SDL_SCANCODE_N));
			this.OpenInventory = (this.OpenInventory ?? InputBinding.FromScancode(SDL.SDL_Scancode.SDL_SCANCODE_TAB));
			this.OpenAssetEditor = (this.OpenAssetEditor ?? InputBinding.FromScancode(SDL.SDL_Scancode.SDL_SCANCODE_H));
			this.OpenDevTools = (this.OpenDevTools ?? InputBinding.FromScancode(SDL.SDL_Scancode.SDL_SCANCODE_GRAVE));
			this.DropItem = (this.DropItem ?? new InputBinding(null));
			this.Jump = (this.Jump ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_SPACE));
			this.Crouch = (this.Crouch ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_LCTRL));
			this.ToggleCrouch = (this.ToggleCrouch ?? new InputBinding(null));
			this.Sprint = (this.Sprint ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_LSHIFT));
			this.ToggleSprint = (this.ToggleSprint ?? new InputBinding(null));
			this.Walk = (this.Walk ?? new InputBinding(null));
			this.ToggleWalk = (this.ToggleWalk ?? new InputBinding(null));
			this.FlyUp = (this.FlyUp ?? new InputBinding(this.Jump));
			this.FlyDown = (this.FlyDown ?? new InputBinding(this.Crouch));
			this.Chat = (this.Chat ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_RETURN));
			this.Command = (this.Command ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_SLASH));
			this.ShowPlayerList = (this.ShowPlayerList ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_p));
			this.ShowUtilitySlotSelector = (this.ShowUtilitySlotSelector ?? InputBinding.FromScancode(SDL.SDL_Scancode.SDL_SCANCODE_Z));
			this.ShowConsumableSlotSelector = (this.ShowConsumableSlotSelector ?? InputBinding.FromScancode(SDL.SDL_Scancode.SDL_SCANCODE_X));
			this.ToggleBuilderToolsLegend = (this.ToggleBuilderToolsLegend ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_l));
			this.OpenMap = (this.OpenMap ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_m));
			this.OpenToolsSettings = (this.OpenToolsSettings ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_b));
			this.SwitchCameraMode = (this.SwitchCameraMode ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_v));
			this.ActivateCameraRotation = (this.ActivateCameraRotation ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_c));
			this.ToggleProfiling = (this.ToggleProfiling ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_F7));
			this.SwitchHudVisibility = (this.SwitchHudVisibility ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_F8));
			this.ToggleFullscreen = (this.ToggleFullscreen ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_F11));
			this.TakeScreenshot = (this.TakeScreenshot ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_F12));
			this.DecreaseSpeedMultiplier = (this.DecreaseSpeedMultiplier ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_F1));
			this.IncreaseSpeedMultiplier = (this.IncreaseSpeedMultiplier ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_F2));
			this.ToggleCreativeCollision = (this.ToggleCreativeCollision ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_F3));
			this.ToggleFlyCamera = (this.ToggleFlyCamera ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_F4));
			this.ToggleFlyCameraControlTarget = (this.ToggleFlyCameraControlTarget ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_j));
			this.BlockInteractAction = (this.BlockInteractAction ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_f));
			this.PrimaryItemAction = (this.PrimaryItemAction ?? InputBinding.FromMouseButton(Input.MouseButton.SDL_BUTTON_LEFT));
			this.SecondaryItemAction = (this.SecondaryItemAction ?? InputBinding.FromMouseButton(Input.MouseButton.SDL_BUTTON_RIGHT));
			this.TertiaryItemAction = (this.TertiaryItemAction ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_k));
			this.Ability1ItemAction = (this.Ability1ItemAction ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_q));
			this.Ability2ItemAction = (this.Ability2ItemAction ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_e));
			this.Ability3ItemAction = (this.Ability3ItemAction ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_r));
			this.PickBlock = (this.PickBlock ?? InputBinding.FromMouseButton(Input.MouseButton.SDL_BUTTON_MIDDLE));
			this.HotbarSlot1 = (this.HotbarSlot1 ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_1));
			this.HotbarSlot2 = (this.HotbarSlot2 ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_2));
			this.HotbarSlot3 = (this.HotbarSlot3 ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_3));
			this.HotbarSlot4 = (this.HotbarSlot4 ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_4));
			this.HotbarSlot5 = (this.HotbarSlot5 ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_5));
			this.HotbarSlot6 = (this.HotbarSlot6 ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_6));
			this.HotbarSlot7 = (this.HotbarSlot7 ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_7));
			this.HotbarSlot8 = (this.HotbarSlot8 ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_8));
			this.HotbarSlot9 = (this.HotbarSlot9 ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_9));
			this.ToolPaintBrush = (this.ToolPaintBrush ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_KP_1));
			this.ToolSculptBrush = (this.ToolSculptBrush ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_KP_2));
			this.ToolSelectionTool = (this.ToolSelectionTool ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_KP_3));
			this.ToolPaste = (this.ToolPaste ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_KP_4));
			this.ToolLine = (this.ToolLine ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_KP_5));
			this.UndoItemAction = (this.UndoItemAction ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_z));
			this.RedoItemAction = (this.RedoItemAction ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_y));
			this.AddRemoveFavoriteMaterialItemAction = (this.AddRemoveFavoriteMaterialItemAction ?? new InputBinding(null));
			this.DismountAction = (this.DismountAction ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_f));
			this.NextRotationAxis = (this.NextRotationAxis ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_g));
			this.PreviousRotationAxis = (this.PreviousRotationAxis ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_b));
			this.TogglePreRotationMode = (this.TogglePreRotationMode ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_t));
			this.TogglePostRotationMode = (this.TogglePostRotationMode ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_y));
			this.AlternatePlaySculptBrushModeModifier = (this.AlternatePlaySculptBrushModeModifier ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_LSHIFT));
			this.NextBrushLockAxisOrPlane = (this.NextBrushLockAxisOrPlane ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_PERIOD));
			this.NextBrushLockMode = (this.NextBrushLockMode ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_COMMA));
			this.UsePaintModeForBrush = (this.UsePaintModeForBrush ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_u));
			this.SelectBlockFromSet = (this.SelectBlockFromSet ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_q));
			this.PastePreview = (this.PastePreview ?? InputBinding.FromKeycode(SDL.SDL_Keycode.SDLK_e));
			FieldInfo[] fields = base.GetType().GetFields();
			this.AllBindings = new List<InputBinding>();
			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo fieldInfo = fields[i];
				bool flag2 = fieldInfo.FieldType == typeof(InputBinding);
				if (flag2)
				{
					InputBinding inputBinding = (InputBinding)fieldInfo.GetValue(this);
					inputBinding.Id = i;
					this.AllBindings.Add(inputBinding);
				}
			}
		}

		// Token: 0x06005776 RID: 22390 RVA: 0x001A86D0 File Offset: 0x001A68D0
		public InputBinding GetHotbarSlot(int slot)
		{
			InputBinding result;
			switch (slot)
			{
			case 0:
				result = this.HotbarSlot1;
				break;
			case 1:
				result = this.HotbarSlot2;
				break;
			case 2:
				result = this.HotbarSlot3;
				break;
			case 3:
				result = this.HotbarSlot4;
				break;
			case 4:
				result = this.HotbarSlot5;
				break;
			case 5:
				result = this.HotbarSlot6;
				break;
			case 6:
				result = this.HotbarSlot7;
				break;
			case 7:
				result = this.HotbarSlot8;
				break;
			case 8:
				result = this.HotbarSlot9;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return result;
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x001A8768 File Offset: 0x001A6968
		public InputBindings Clone()
		{
			InputBindings inputBindings = new InputBindings();
			inputBindings.AllBindings = new List<InputBinding>();
			foreach (FieldInfo fieldInfo in base.GetType().GetFields())
			{
				bool flag = fieldInfo.FieldType == typeof(InputBinding);
				if (flag)
				{
					fieldInfo.SetValue(inputBindings, fieldInfo.GetValue(this));
					inputBindings.AllBindings.Add((InputBinding)fieldInfo.GetValue(this));
				}
			}
			return inputBindings;
		}

		// Token: 0x04003524 RID: 13604
		public InputBinding MoveForwards;

		// Token: 0x04003525 RID: 13605
		public InputBinding MoveBackwards;

		// Token: 0x04003526 RID: 13606
		public InputBinding StrafeLeft;

		// Token: 0x04003527 RID: 13607
		public InputBinding StrafeRight;

		// Token: 0x04003528 RID: 13608
		public InputBinding Jump;

		// Token: 0x04003529 RID: 13609
		public InputBinding Crouch;

		// Token: 0x0400352A RID: 13610
		public InputBinding ToggleCrouch;

		// Token: 0x0400352B RID: 13611
		public InputBinding Sprint;

		// Token: 0x0400352C RID: 13612
		public InputBinding ToggleSprint;

		// Token: 0x0400352D RID: 13613
		public InputBinding Walk;

		// Token: 0x0400352E RID: 13614
		public InputBinding ToggleWalk;

		// Token: 0x0400352F RID: 13615
		public InputBinding FlyUp;

		// Token: 0x04003530 RID: 13616
		public InputBinding FlyDown;

		// Token: 0x04003531 RID: 13617
		public InputBinding Chat;

		// Token: 0x04003532 RID: 13618
		public InputBinding Command;

		// Token: 0x04003533 RID: 13619
		public InputBinding ShowPlayerList;

		// Token: 0x04003534 RID: 13620
		public InputBinding ShowUtilitySlotSelector;

		// Token: 0x04003535 RID: 13621
		public InputBinding ShowConsumableSlotSelector;

		// Token: 0x04003536 RID: 13622
		public InputBinding ToggleBuilderToolsLegend;

		// Token: 0x04003537 RID: 13623
		public InputBinding OpenInventory;

		// Token: 0x04003538 RID: 13624
		public InputBinding OpenMachinimaEditor;

		// Token: 0x04003539 RID: 13625
		public InputBinding OpenMap;

		// Token: 0x0400353A RID: 13626
		public InputBinding OpenToolsSettings;

		// Token: 0x0400353B RID: 13627
		public InputBinding OpenAssetEditor;

		// Token: 0x0400353C RID: 13628
		public InputBinding OpenDevTools;

		// Token: 0x0400353D RID: 13629
		public InputBinding DropItem;

		// Token: 0x0400353E RID: 13630
		public InputBinding SwitchCameraMode;

		// Token: 0x0400353F RID: 13631
		public InputBinding ActivateCameraRotation;

		// Token: 0x04003540 RID: 13632
		public InputBinding ToggleProfiling;

		// Token: 0x04003541 RID: 13633
		public InputBinding SwitchHudVisibility;

		// Token: 0x04003542 RID: 13634
		public InputBinding ToggleFullscreen;

		// Token: 0x04003543 RID: 13635
		public InputBinding TakeScreenshot;

		// Token: 0x04003544 RID: 13636
		public InputBinding DecreaseSpeedMultiplier;

		// Token: 0x04003545 RID: 13637
		public InputBinding IncreaseSpeedMultiplier;

		// Token: 0x04003546 RID: 13638
		public InputBinding ToggleCreativeCollision;

		// Token: 0x04003547 RID: 13639
		public InputBinding ToggleFlyCamera;

		// Token: 0x04003548 RID: 13640
		public InputBinding ToggleFlyCameraControlTarget;

		// Token: 0x04003549 RID: 13641
		public InputBinding BlockInteractAction;

		// Token: 0x0400354A RID: 13642
		public InputBinding PrimaryItemAction;

		// Token: 0x0400354B RID: 13643
		public InputBinding SecondaryItemAction;

		// Token: 0x0400354C RID: 13644
		public InputBinding TertiaryItemAction;

		// Token: 0x0400354D RID: 13645
		public InputBinding Ability1ItemAction;

		// Token: 0x0400354E RID: 13646
		public InputBinding Ability2ItemAction;

		// Token: 0x0400354F RID: 13647
		public InputBinding Ability3ItemAction;

		// Token: 0x04003550 RID: 13648
		public InputBinding PickBlock;

		// Token: 0x04003551 RID: 13649
		public InputBinding UndoItemAction;

		// Token: 0x04003552 RID: 13650
		public InputBinding RedoItemAction;

		// Token: 0x04003553 RID: 13651
		public InputBinding AddRemoveFavoriteMaterialItemAction;

		// Token: 0x04003554 RID: 13652
		public InputBinding DismountAction;

		// Token: 0x04003555 RID: 13653
		public InputBinding HotbarSlot1;

		// Token: 0x04003556 RID: 13654
		public InputBinding HotbarSlot2;

		// Token: 0x04003557 RID: 13655
		public InputBinding HotbarSlot3;

		// Token: 0x04003558 RID: 13656
		public InputBinding HotbarSlot4;

		// Token: 0x04003559 RID: 13657
		public InputBinding HotbarSlot5;

		// Token: 0x0400355A RID: 13658
		public InputBinding HotbarSlot6;

		// Token: 0x0400355B RID: 13659
		public InputBinding HotbarSlot7;

		// Token: 0x0400355C RID: 13660
		public InputBinding HotbarSlot8;

		// Token: 0x0400355D RID: 13661
		public InputBinding HotbarSlot9;

		// Token: 0x0400355E RID: 13662
		public InputBinding ToolPaintBrush;

		// Token: 0x0400355F RID: 13663
		public InputBinding ToolSculptBrush;

		// Token: 0x04003560 RID: 13664
		public InputBinding ToolSelectionTool;

		// Token: 0x04003561 RID: 13665
		public InputBinding ToolPaste;

		// Token: 0x04003562 RID: 13666
		public InputBinding ToolLine;

		// Token: 0x04003563 RID: 13667
		public InputBinding TogglePreRotationMode;

		// Token: 0x04003564 RID: 13668
		public InputBinding TogglePostRotationMode;

		// Token: 0x04003565 RID: 13669
		public InputBinding NextRotationAxis;

		// Token: 0x04003566 RID: 13670
		public InputBinding PreviousRotationAxis;

		// Token: 0x04003567 RID: 13671
		public InputBinding AlternatePlaySculptBrushModeModifier;

		// Token: 0x04003568 RID: 13672
		public InputBinding NextBrushLockAxisOrPlane;

		// Token: 0x04003569 RID: 13673
		public InputBinding NextBrushLockMode;

		// Token: 0x0400356A RID: 13674
		public InputBinding UsePaintModeForBrush;

		// Token: 0x0400356B RID: 13675
		public InputBinding SelectBlockFromSet;

		// Token: 0x0400356C RID: 13676
		public InputBinding PastePreview;

		// Token: 0x0400356D RID: 13677
		[JsonIgnore]
		public List<InputBinding> AllBindings;
	}
}
