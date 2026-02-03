using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HytaleClient.Data.EntityStats;
using HytaleClient.Data.Items;
using HytaleClient.Data.UserSettings;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Interface.InGame.Hud.Abilities;
using HytaleClient.Interface.InGame.Hud.StatusEffects;
using HytaleClient.Networking;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using SDL2;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008F3 RID: 2291
	internal class InventoryModule : Module
	{
		// Token: 0x170010FF RID: 4351
		// (get) Token: 0x06004410 RID: 17424 RVA: 0x000E40FF File Offset: 0x000E22FF
		// (set) Token: 0x06004411 RID: 17425 RVA: 0x000E4107 File Offset: 0x000E2307
		public int UtilityActiveSlot { get; private set; } = -1;

		// Token: 0x17001100 RID: 4352
		// (get) Token: 0x06004412 RID: 17426 RVA: 0x000E4110 File Offset: 0x000E2310
		// (set) Token: 0x06004413 RID: 17427 RVA: 0x000E4118 File Offset: 0x000E2318
		public int ConsumableActiveSlot { get; private set; } = -1;

		// Token: 0x17001101 RID: 4353
		// (get) Token: 0x06004414 RID: 17428 RVA: 0x000E4121 File Offset: 0x000E2321
		// (set) Token: 0x06004415 RID: 17429 RVA: 0x000E4129 File Offset: 0x000E2329
		public int ToolsActiveSlot { get; private set; } = -1;

		// Token: 0x06004416 RID: 17430 RVA: 0x000E4134 File Offset: 0x000E2334
		public InventoryModule(GameInstance gameInstance) : base(gameInstance)
		{
			this._gameInstance.App.Interface.RegisterForEvent<int>("game.selectActiveUtilitySlot", this._gameInstance, delegate(int slot)
			{
				this.SetActiveUtilitySlot(slot, true);
			});
			this._gameInstance.App.Interface.RegisterForEvent<int>("game.selectActiveConsumableSlot", this._gameInstance, delegate(int slot)
			{
				this.SetActiveConsumableSlot(slot, true, false);
			});
			this._gameInstance.App.Interface.RegisterForEvent<int>("game.useConsumableSlot", this._gameInstance, delegate(int slot)
			{
				this.SetActiveConsumableSlot(slot, true, true);
			});
		}

		// Token: 0x06004417 RID: 17431 RVA: 0x000E4208 File Offset: 0x000E2408
		protected override void DoDispose()
		{
			this._gameInstance.App.Interface.UnregisterFromEvent("game.selectActiveUtilitySlot");
			this._gameInstance.App.Interface.UnregisterFromEvent("game.selectActiveConsumableSlot");
			this._gameInstance.App.Interface.UnregisterFromEvent("game.useConsumableSlot");
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x000E4268 File Offset: 0x000E2468
		public void Update(float deltaTime)
		{
			InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
			bool flag = this._gameInstance.Input.IsBindingHeld(inputBindings.DropItem, false);
			if (flag)
			{
				this._dropBindingHeldTick += deltaTime;
				bool flag2 = this._dropBindingHeldTick >= 0.5f && !this._hasDroppedStack && this.GetHotbarItem(this.HotbarActiveSlot) != null;
				if (flag2)
				{
					this.DropItem(true);
					this._hasDroppedStack = true;
				}
			}
			else
			{
				bool flag3 = this._dropBindingHeldTick > 0f && !this._hasDroppedStack && this.GetHotbarItem(this.HotbarActiveSlot) != null;
				if (flag3)
				{
					this.DropItem(false);
				}
				this._hasDroppedStack = false;
				this._dropBindingHeldTick = 0f;
			}
			bool flag4 = this._gameInstance.GameMode == 1 && this._gameInstance.Input.IsShiftHeld();
			if (flag4)
			{
				for (sbyte b = 0; b < 10; b += 1)
				{
					bool flag5 = this._gameInstance.Input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_1 + (int)b, false);
					if (flag5)
					{
						this._gameInstance.Connection.SendPacket(new LoadHotbar(b));
						this._gameInstance.AudioModule.PlayLocalSoundEvent("UI_HOTBAR_UP");
						break;
					}
				}
			}
			bool flag6 = this._gameInstance.GameMode == 1 && this._gameInstance.Input.ConsumeBinding(this._gameInstance.App.Settings.InputBindings.SelectBlockFromSet, false);
			if (flag6)
			{
				this.TrySelectBlockFromSet();
			}
			bool flag7 = this._gameInstance.Input.IsAltHeld() && this._gameInstance.Input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_G, false);
			if (flag7)
			{
				this._gameInstance.SetGameMode((this._gameInstance.GameMode == 1) ? 0 : 1, true);
			}
		}

		// Token: 0x06004419 RID: 17433 RVA: 0x000E4466 File Offset: 0x000E2666
		private void TrySelectBlockFromSet()
		{
			this._gameInstance.Connection.SendPacket(new SwitchHotbarBlockSet());
		}

		// Token: 0x0600441A RID: 17434 RVA: 0x000E4480 File Offset: 0x000E2680
		private void DropItem(bool dropWholeStack)
		{
			bool flag = !this._gameInstance.InteractionModule.ApplyRules(null, new InteractionChainData(), 0, null);
			if (!flag)
			{
				bool flag2 = this.UsingToolsItem();
				if (!flag2)
				{
					bool flag3 = !this._gameInstance.InteractionModule.ApplyRules(null, new InteractionChainData(), 0, null);
					if (!flag3)
					{
						ClientItemStack hotbarItem = this.GetHotbarItem(this.HotbarActiveSlot);
						int num = dropWholeStack ? hotbarItem.Quantity : 1;
						Item item = new Item(hotbarItem.Id, num, hotbarItem.Durability, hotbarItem.MaxDurability, false, (sbyte[])ProtoHelper.SerializeBson(hotbarItem.Metadata));
						this._gameInstance.Connection.SendPacket(new DropItemStack(new InventoryPosition(-1, this.HotbarActiveSlot, item)));
						this.HotbarInventory[this.HotbarActiveSlot].Quantity -= num;
						bool flag4 = this.HotbarInventory[this.HotbarActiveSlot].Quantity == 0;
						if (flag4)
						{
							this.HotbarInventory[this.HotbarActiveSlot] = null;
							string newItemId = null;
							ClientItemStack utilityItem = this.GetUtilityItem(this.UtilityActiveSlot);
							this.ChangeCharacterItem(newItemId, (utilityItem != null) ? utilityItem.Id : null, ItemChangeType.Dropped);
						}
						this.UpdateAll();
					}
				}
			}
		}

		// Token: 0x0600441B RID: 17435 RVA: 0x000E45C0 File Offset: 0x000E27C0
		public void SetInventory(UpdatePlayerInventory inventory)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool flag = inventory.Storage != null;
			if (flag)
			{
				this._storageInventory = new ClientItemStack[(int)inventory.Storage.Capacity];
				foreach (KeyValuePair<int, Item> keyValuePair in inventory.Storage.Items)
				{
					this._storageInventory[keyValuePair.Key] = new ClientItemStack(keyValuePair.Value);
				}
			}
			bool flag2 = inventory.Armor != null;
			if (flag2)
			{
				this._armorInventory = new ClientItemStack[(int)inventory.Armor.Capacity];
				string[] array = new string[(int)inventory.Armor.Capacity];
				foreach (KeyValuePair<int, Item> keyValuePair2 in inventory.Armor.Items)
				{
					this._armorInventory[keyValuePair2.Key] = new ClientItemStack(keyValuePair2.Value);
					array[keyValuePair2.Key] = keyValuePair2.Value.ItemId;
				}
				PlayerEntity localPlayer = this._gameInstance.LocalPlayer;
				if (localPlayer != null)
				{
					localPlayer.SetCharacterModel(null, array);
				}
			}
			bool flag3 = inventory.Hotbar != null;
			if (flag3)
			{
				this.HotbarInventory = new ClientItemStack[(int)inventory.Hotbar.Capacity];
				foreach (KeyValuePair<int, Item> keyValuePair3 in inventory.Hotbar.Items)
				{
					this.HotbarInventory[keyValuePair3.Key] = new ClientItemStack(keyValuePair3.Value);
				}
				bool flag4 = this.HotbarActiveSlot >= (int)inventory.Hotbar.Capacity;
				if (flag4)
				{
					this.SetActiveHotbarSlot((int)(inventory.Hotbar.Capacity - 1), true);
				}
			}
			bool flag5 = inventory.Utility != null;
			if (flag5)
			{
				this.UtilityInventory = new ClientItemStack[(int)inventory.Utility.Capacity];
				foreach (KeyValuePair<int, Item> keyValuePair4 in inventory.Utility.Items)
				{
					this.UtilityInventory[keyValuePair4.Key] = new ClientItemStack(keyValuePair4.Value);
				}
			}
			bool flag6 = inventory.Consumable != null;
			if (flag6)
			{
				this.ConsumableInventory = new ClientItemStack[(int)inventory.Consumable.Capacity];
				foreach (KeyValuePair<int, Item> keyValuePair5 in inventory.Consumable.Items)
				{
					this.ConsumableInventory[keyValuePair5.Key] = new ClientItemStack(keyValuePair5.Value);
				}
			}
			bool flag7 = inventory.Tools != null;
			if (flag7)
			{
				this.ToolsInventory = new ClientItemStack[(int)inventory.Tools.Capacity];
				foreach (KeyValuePair<int, Item> keyValuePair6 in inventory.Tools.Items)
				{
					this.ToolsInventory[keyValuePair6.Key] = new ClientItemStack(keyValuePair6.Value);
				}
			}
			ClientItemStack activeItem = this.GetActiveItem();
			string newItemId = (activeItem != null) ? activeItem.Id : null;
			ClientItemStack utilityItem = this.GetUtilityItem(this.UtilityActiveSlot);
			string newSecondaryItemId = (utilityItem != null) ? utilityItem.Id : null;
			this.ChangeCharacterItem(newItemId, newSecondaryItemId, ItemChangeType.Other);
			this.UpdateAll();
			StatusEffectsHudComponent statusEffectsHudComponent = this._gameInstance.App.Interface.InGameView.StatusEffectsHudComponent;
			if (statusEffectsHudComponent != null)
			{
				statusEffectsHudComponent.UpdateTrinketsBuffs();
			}
			AbilitiesHudComponent abilitiesHudComponent = this._gameInstance.App.Interface.InGameView.AbilitiesHudComponent;
			if (abilitiesHudComponent != null)
			{
				abilitiesHudComponent.ShowOrHideHud();
			}
			this._gameInstance.App.Interface.TriggerEvent("inventory.setAutosortType", inventory.SortType_, null, null, null, null, null);
		}

		// Token: 0x0600441C RID: 17436 RVA: 0x000E4A34 File Offset: 0x000E2C34
		public void UpdateAll()
		{
			this._gameInstance.App.Interface.TriggerEvent("inventory.setAll", this._storageInventory, this._armorInventory, this.HotbarInventory, this.UtilityInventory, this.ConsumableInventory, this.ToolsInventory);
			this._gameInstance.App.Interface.InGameView.AbilitiesHudComponent.OnSignatureEnergyStatChanged(this._gameInstance.LocalPlayer.GetEntityStat(DefaultEntityStats.SignatureEnergy));
		}

		// Token: 0x0600441D RID: 17437 RVA: 0x000E4AB8 File Offset: 0x000E2CB8
		public ClientItemStack GetStorageItem(int slot)
		{
			ClientItemStack[] storageInventory = this._storageInventory;
			return (storageInventory != null) ? storageInventory[slot] : null;
		}

		// Token: 0x0600441E RID: 17438 RVA: 0x000E4ADC File Offset: 0x000E2CDC
		public ClientItemStack GetArmorItem(int slot)
		{
			ClientItemStack[] armorInventory = this._armorInventory;
			return (armorInventory != null) ? armorInventory[slot] : null;
		}

		// Token: 0x0600441F RID: 17439 RVA: 0x000E4B00 File Offset: 0x000E2D00
		public int GetActiveInventorySectionType()
		{
			return this.UsingToolsItem() ? -8 : -1;
		}

		// Token: 0x06004420 RID: 17440 RVA: 0x000E4B20 File Offset: 0x000E2D20
		public int GetActiveSlot()
		{
			return this.UsingToolsItem() ? this.ToolsActiveSlot : this.HotbarActiveSlot;
		}

		// Token: 0x06004421 RID: 17441 RVA: 0x000E4B48 File Offset: 0x000E2D48
		public ClientItemStack GetActiveItem()
		{
			bool flag = this.UsingToolsItem();
			ClientItemStack result;
			if (flag)
			{
				result = this.GetActiveToolsItem();
			}
			else
			{
				result = this.GetActiveHotbarItem();
			}
			return result;
		}

		// Token: 0x06004422 RID: 17442 RVA: 0x000E4B75 File Offset: 0x000E2D75
		public int GetActiveHotbarSlot()
		{
			return this.HotbarActiveSlot;
		}

		// Token: 0x06004423 RID: 17443 RVA: 0x000E4B7D File Offset: 0x000E2D7D
		public ClientItemStack GetActiveHotbarItem()
		{
			return this.GetHotbarItem(this.HotbarActiveSlot);
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x000E4B8C File Offset: 0x000E2D8C
		public ClientItemStack GetHotbarItem(int slot)
		{
			bool flag = this.HotbarInventory == null || slot == -1;
			ClientItemStack result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this.HotbarInventory[slot];
			}
			return result;
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x000E4BC0 File Offset: 0x000E2DC0
		public ClientItemStack GetUtilityItem(int slot)
		{
			bool flag = this.UtilityInventory == null || slot == -1;
			ClientItemStack result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this.UtilityInventory[slot];
			}
			return result;
		}

		// Token: 0x06004426 RID: 17446 RVA: 0x000E4BF4 File Offset: 0x000E2DF4
		public ClientItemStack GetConsumableItem(int slot)
		{
			bool flag = this.ConsumableInventory == null || slot == -1;
			ClientItemStack result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this.ConsumableInventory[slot];
			}
			return result;
		}

		// Token: 0x06004427 RID: 17447 RVA: 0x000E4C28 File Offset: 0x000E2E28
		public ClientItemStack[] GetToolItemStacks()
		{
			return this.ToolsInventory;
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x000E4C40 File Offset: 0x000E2E40
		public ClientItemStack GetToolItemStack(int id)
		{
			return this.ToolsInventory[id];
		}

		// Token: 0x06004429 RID: 17449 RVA: 0x000E4C5C File Offset: 0x000E2E5C
		public bool UsingToolsItem()
		{
			return this._usingToolsItem;
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x000E4C74 File Offset: 0x000E2E74
		public void SetUsingToolsItem(bool value)
		{
			BuilderToolAction builderToolAction = value ? 5 : 6;
			this._gameInstance.Connection.SendPacket(new BuilderToolGeneralAction(builderToolAction));
			this._usingToolsItem = value;
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x000E4CA8 File Offset: 0x000E2EA8
		public int GetActiveToolsSlot()
		{
			return this.ToolsActiveSlot;
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x000E4CB0 File Offset: 0x000E2EB0
		public ClientItemStack GetActiveToolsItem()
		{
			return this.GetToolsItem(this.ToolsActiveSlot);
		}

		// Token: 0x0600442D RID: 17453 RVA: 0x000E4CC0 File Offset: 0x000E2EC0
		public ClientItemStack GetToolsItem(int slot)
		{
			bool flag = this.ToolsInventory == null || slot == -1;
			ClientItemStack result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this.ToolsInventory[slot];
			}
			return result;
		}

		// Token: 0x0600442E RID: 17454 RVA: 0x000E4CF4 File Offset: 0x000E2EF4
		public void SetHotbarItem(int slot, ClientItemStack itemStack)
		{
			bool flag = this.HotbarInventory == null;
			if (!flag)
			{
				this.HotbarInventory[slot] = itemStack;
				this.UpdateAll();
			}
		}

		// Token: 0x0600442F RID: 17455 RVA: 0x000E4D21 File Offset: 0x000E2F21
		public int GetHotbarCapacity()
		{
			return this.HotbarInventory.Length;
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x000E4D2B File Offset: 0x000E2F2B
		public int GetStorageCapacity()
		{
			return this._storageInventory.Length;
		}

		// Token: 0x06004431 RID: 17457 RVA: 0x000E4D38 File Offset: 0x000E2F38
		public void ScrollHotbarSlot(bool positive)
		{
			bool flag = this.HotbarInventory == null || this._usingToolsItem;
			if (!flag)
			{
				int num = this.HotbarActiveSlot + (positive ? 1 : -1);
				bool flag2 = num < 0;
				if (flag2)
				{
					num = this.HotbarInventory.Length - 1;
				}
				else
				{
					bool flag3 = num >= this.HotbarInventory.Length;
					if (flag3)
					{
						num = 0;
					}
				}
				this.SetActiveHotbarSlot(num, true);
			}
		}

		// Token: 0x06004432 RID: 17458 RVA: 0x000E4DA0 File Offset: 0x000E2FA0
		public void SetActiveHotbarSlot(int slot, bool triggerInteraction = true)
		{
			bool flag = this.HotbarInventory == null;
			if (!flag)
			{
				bool flag2 = slot >= this.HotbarInventory.Length;
				if (!flag2)
				{
					if (triggerInteraction)
					{
						this._gameInstance.InteractionModule.ConsumeInteractionType(null, 15, new int?(slot));
					}
					else
					{
						bool usingToolsItem = this._usingToolsItem;
						if (usingToolsItem)
						{
							this.SetUsingToolsItem(false);
						}
						else
						{
							bool flag3 = slot == this.HotbarActiveSlot;
							if (flag3)
							{
								return;
							}
						}
						this.HotbarActiveSlot = slot;
						this._gameInstance.App.Interface.TriggerEvent("game.setActiveHotbarSlot", this.HotbarActiveSlot, null, null, null, null, null);
						bool flag4 = this._gameInstance.LocalPlayer != null;
						if (flag4)
						{
							ClientItemStack hotbarItem = this.GetHotbarItem(this.HotbarActiveSlot);
							string newItemId = (hotbarItem != null) ? hotbarItem.Id : null;
							ClientItemStack utilityItem = this.GetUtilityItem(this.UtilityActiveSlot);
							this.ChangeCharacterItem(newItemId, (utilityItem != null) ? utilityItem.Id : null, ItemChangeType.SlotChanged);
							this._gameInstance.LocalPlayer.ClearFirstPersonItemWiggle();
							this._gameInstance.LocalPlayer.FinishAction();
						}
					}
				}
			}
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x000E4ECC File Offset: 0x000E30CC
		public void SetActiveUtilitySlot(int slot, bool sendPacket = true)
		{
			bool flag = slot == this.UtilityActiveSlot;
			if (!flag)
			{
				bool flag2 = this.UtilityInventory != null && slot >= this.UtilityInventory.Length;
				if (!flag2)
				{
					this.UtilityActiveSlot = slot;
					this._gameInstance.App.Interface.TriggerEvent("game.setActiveUtilitySlot", this.UtilityActiveSlot, null, null, null, null, null);
					if (sendPacket)
					{
						this._gameInstance.Connection.SendPacket(new SetActiveSlot(-5, this.UtilityActiveSlot));
					}
					AbilitiesHudComponent abilitiesHudComponent = this._gameInstance.App.Interface.InGameView.AbilitiesHudComponent;
					if (abilitiesHudComponent != null)
					{
						abilitiesHudComponent.ShowOrHideHud();
					}
					bool flag3 = this._gameInstance.LocalPlayer != null;
					if (flag3)
					{
						ClientItemStack hotbarItem = this.GetHotbarItem(this.HotbarActiveSlot);
						string newItemId = (hotbarItem != null) ? hotbarItem.Id : null;
						ClientItemStack utilityItem = this.GetUtilityItem(this.UtilityActiveSlot);
						this.ChangeCharacterItem(newItemId, (utilityItem != null) ? utilityItem.Id : null, ItemChangeType.SlotChanged);
						this._gameInstance.LocalPlayer.ClearFirstPersonItemWiggle();
						this._gameInstance.LocalPlayer.FinishAction();
					}
				}
			}
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x000E4FF8 File Offset: 0x000E31F8
		public void SetActiveConsumableSlot(int slot, bool sendPacket = true, bool doInteraction = false)
		{
			bool flag = this.ConsumableInventory != null && slot >= this.ConsumableInventory.Length;
			if (!flag)
			{
				bool flag2 = slot != this.ConsumableActiveSlot;
				if (flag2)
				{
					this.ConsumableActiveSlot = slot;
					this._gameInstance.App.Interface.TriggerEvent("game.setActiveConsumableSlot", this.ConsumableActiveSlot, null, null, null, null, null);
					if (sendPacket)
					{
						this._gameInstance.Connection.SendPacket(new SetActiveSlot(-6, this.ConsumableActiveSlot));
					}
				}
				if (doInteraction)
				{
					ClientItemStack consumableItem = this.GetConsumableItem(this.ConsumableActiveSlot);
					this._gameInstance.LocalPlayer.ConsumableItem = this._gameInstance.ItemLibraryModule.GetItem((consumableItem != null) ? consumableItem.Id : null);
					this._gameInstance.LocalPlayer.SetCharacterItemConsumable();
					bool flag3 = !this._gameInstance.InteractionModule.StartChain(6, InteractionModule.ClickType.Single, new Action(this.<SetActiveConsumableSlot>g__OnCompletion|54_0));
					if (flag3)
					{
						this._gameInstance.LocalPlayer.RestoreCharacterItem();
					}
				}
			}
		}

		// Token: 0x06004435 RID: 17461 RVA: 0x000E5124 File Offset: 0x000E3324
		public void SetActiveToolsSlot(int slot, bool sendPacket = true, bool useTool = true)
		{
			bool flag = this.ToolsInventory == null;
			if (!flag)
			{
				bool flag2 = !this._usingToolsItem;
				if (flag2)
				{
					if (useTool)
					{
						this.SetUsingToolsItem(true);
					}
				}
				else
				{
					bool flag3 = slot == this.ToolsActiveSlot;
					if (flag3)
					{
						return;
					}
				}
				bool flag4 = slot >= this.ToolsInventory.Length;
				if (!flag4)
				{
					if (useTool)
					{
						this._gameInstance.App.Interface.InGameView.ClearSlotHighlight();
						this._gameInstance.App.Interface.TriggerEvent("game.setActiveHotbarSlot", -1, null, null, null, null, null);
						this._gameInstance.App.InGame.Instance.BuilderToolsModule.ClearConfiguringTool();
					}
					bool flag5 = this.ToolsActiveSlot != slot;
					if (flag5)
					{
						this.ToolsActiveSlot = slot;
						if (sendPacket)
						{
							this._gameInstance.Connection.SendPacket(new SetActiveSlot(-8, this.ToolsActiveSlot));
						}
					}
					this._gameInstance.App.Interface.TriggerEvent("game.setActiveToolsSlot", this.ToolsActiveSlot, null, null, null, null, null);
					ClientItemStack toolsItem = this.GetToolsItem(this.ToolsActiveSlot);
					this._gameInstance.BuilderToolsModule.TrySelectActiveTool(-8, slot, toolsItem);
					bool flag6 = this._gameInstance.LocalPlayer != null;
					if (flag6)
					{
						string newItemId = (toolsItem != null) ? toolsItem.Id : null;
						ClientItemStack utilityItem = this.GetUtilityItem(this.UtilityActiveSlot);
						this.ChangeCharacterItem(newItemId, (utilityItem != null) ? utilityItem.Id : null, ItemChangeType.SlotChanged);
						this._gameInstance.LocalPlayer.ClearFirstPersonItemWiggle();
						this._gameInstance.LocalPlayer.FinishAction();
					}
				}
			}
		}

		// Token: 0x06004436 RID: 17462 RVA: 0x000E52EC File Offset: 0x000E34EC
		private void ChangeCharacterItem(string newItemId, string newSecondaryItemId = null, ItemChangeType changeType = ItemChangeType.Other)
		{
			bool flag = this._gameInstance.LocalPlayer == null;
			if (!flag)
			{
				this._gameInstance.LocalPlayer.ChangeCharacterItem(newItemId, newSecondaryItemId);
				this._gameInstance.LocalPlayer.UpdateLight();
				bool flag2 = !this._gameInstance.BuilderToolsModule.TrySelectActiveTool() || this._gameInstance.GameMode != 1;
				if (flag2)
				{
					this._gameInstance.App.Interface.InGameView.ClearSlotHighlight();
				}
				this._gameInstance.App.Interface.InGameView.ToolsSettingsPage.OnPlayerCharacterItemChanged(changeType);
				this._gameInstance.App.Interface.InGameView.OnPlayerCharacterItemChanged(changeType);
			}
		}

		// Token: 0x06004437 RID: 17463 RVA: 0x000E53BC File Offset: 0x000E35BC
		public void AddAndSelectHotbarItem(string itemId)
		{
			int num = -1;
			for (int i = 0; i < this.HotbarInventory.Length; i++)
			{
				ClientItemStack hotbarItem = this.GetHotbarItem(i);
				bool flag = hotbarItem == null;
				if (flag)
				{
					bool flag2 = num == -1;
					if (flag2)
					{
						num = i;
					}
				}
				else
				{
					bool flag3 = hotbarItem.Id == itemId;
					if (flag3)
					{
						this.SetActiveHotbarSlot(i, true);
						return;
					}
				}
			}
			bool flag4 = num != -1;
			if (flag4)
			{
				this.SetActiveHotbarSlot(num, true);
			}
			else
			{
				num = this.HotbarActiveSlot;
			}
			for (int j = 0; j < this._storageInventory.Length; j++)
			{
				ClientItemStack storageItem = this.GetStorageItem(j);
				bool flag5 = storageItem != null && storageItem.Id == itemId;
				if (flag5)
				{
					ConnectionToServer connection = this._gameInstance.Connection;
					InventoryPosition inventoryPosition = new InventoryPosition(-2, j, storageItem.ToItemPacket(true));
					int num2 = -1;
					int num3 = num;
					ClientItemStack hotbarItem2 = this.GetHotbarItem(num);
					connection.SendPacket(new MoveItemStack(inventoryPosition, new InventoryPosition(num2, num3, (hotbarItem2 != null) ? hotbarItem2.ToItemPacket(true) : null)));
					return;
				}
			}
			bool flag6 = this._gameInstance.GameMode == 1;
			if (flag6)
			{
				ClientItemBase item = this._gameInstance.ItemLibraryModule.GetItem(itemId);
				bool flag7 = item != null;
				if (flag7)
				{
					double durability = item.Durability;
					this._gameInstance.Connection.SendPacket(new SetCreativeItem(new InventoryPosition(-1, num, new Item(itemId, 1, durability, durability, false, null)), false));
				}
				return;
			}
		}

		// Token: 0x0600443B RID: 17467 RVA: 0x000E5562 File Offset: 0x000E3762
		[CompilerGenerated]
		private void <SetActiveConsumableSlot>g__OnCompletion|54_0()
		{
			this._gameInstance.LocalPlayer.RestoreCharacterItem();
		}

		// Token: 0x04002193 RID: 8595
		public const float TimeToDropStack = 0.5f;

		// Token: 0x04002194 RID: 8596
		public const int InactiveSlotIndex = -1;

		// Token: 0x04002195 RID: 8597
		private ClientItemStack[] _storageInventory;

		// Token: 0x04002196 RID: 8598
		public ClientItemStack[] _armorInventory;

		// Token: 0x04002197 RID: 8599
		public ClientItemStack[] HotbarInventory;

		// Token: 0x04002198 RID: 8600
		public ClientItemStack[] UtilityInventory;

		// Token: 0x04002199 RID: 8601
		public ClientItemStack[] ConsumableInventory;

		// Token: 0x0400219A RID: 8602
		public ClientItemStack[] ToolsInventory;

		// Token: 0x0400219B RID: 8603
		public int HotbarActiveSlot = -1;

		// Token: 0x0400219F RID: 8607
		private float _dropBindingHeldTick = 0f;

		// Token: 0x040021A0 RID: 8608
		private bool _hasDroppedStack = false;

		// Token: 0x040021A1 RID: 8609
		private bool _usingToolsItem = false;
	}
}
