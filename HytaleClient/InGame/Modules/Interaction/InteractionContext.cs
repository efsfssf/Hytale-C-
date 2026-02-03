using System;
using System.Collections.Generic;
using HytaleClient.Data.ClientInteraction;
using HytaleClient.Data.Items;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Interaction
{
	// Token: 0x02000939 RID: 2361
	internal class InteractionContext
	{
		// Token: 0x170011A9 RID: 4521
		// (get) Token: 0x06004890 RID: 18576 RVA: 0x00118A73 File Offset: 0x00116C73
		// (set) Token: 0x06004891 RID: 18577 RVA: 0x00118A7B File Offset: 0x00116C7B
		public string OriginalItemType { get; private set; }

		// Token: 0x170011AA RID: 4522
		// (get) Token: 0x06004892 RID: 18578 RVA: 0x00118A84 File Offset: 0x00116C84
		public bool AllowSkipChainOnClick
		{
			get
			{
				return this.Chain.SkipChainOnClick;
			}
		}

		// Token: 0x170011AB RID: 4523
		// (get) Token: 0x06004893 RID: 18579 RVA: 0x00118A91 File Offset: 0x00116C91
		// (set) Token: 0x06004894 RID: 18580 RVA: 0x00118A9E File Offset: 0x00116C9E
		public int OperationCounter
		{
			get
			{
				return this.Chain.OperationCounter;
			}
			set
			{
				this.Chain.OperationCounter = value;
			}
		}

		// Token: 0x170011AC RID: 4524
		// (get) Token: 0x06004895 RID: 18581 RVA: 0x00118AAC File Offset: 0x00116CAC
		// (set) Token: 0x06004896 RID: 18582 RVA: 0x00118AB4 File Offset: 0x00116CB4
		public InteractionSyncData State { get; private set; }

		// Token: 0x170011AD RID: 4525
		// (get) Token: 0x06004897 RID: 18583 RVA: 0x00118ABD File Offset: 0x00116CBD
		// (set) Token: 0x06004898 RID: 18584 RVA: 0x00118AC5 File Offset: 0x00116CC5
		public InteractionMetaStore InstanceStore { get; private set; }

		// Token: 0x170011AE RID: 4526
		// (get) Token: 0x06004899 RID: 18585 RVA: 0x00118ACE File Offset: 0x00116CCE
		// (set) Token: 0x0600489A RID: 18586 RVA: 0x00118AD6 File Offset: 0x00116CD6
		public InteractionSyncData ServerData { get; private set; }

		// Token: 0x170011AF RID: 4527
		// (get) Token: 0x0600489B RID: 18587 RVA: 0x00118ADF File Offset: 0x00116CDF
		// (set) Token: 0x0600489C RID: 18588 RVA: 0x00118AE7 File Offset: 0x00116CE7
		public InteractionChain Chain { get; private set; }

		// Token: 0x170011B0 RID: 4528
		// (get) Token: 0x0600489D RID: 18589 RVA: 0x00118AF0 File Offset: 0x00116CF0
		// (set) Token: 0x0600489E RID: 18590 RVA: 0x00118AF8 File Offset: 0x00116CF8
		public InteractionEntry Entry { get; private set; }

		// Token: 0x170011B1 RID: 4529
		// (get) Token: 0x0600489F RID: 18591 RVA: 0x00118B01 File Offset: 0x00116D01
		// (set) Token: 0x060048A0 RID: 18592 RVA: 0x00118B09 File Offset: 0x00116D09
		public GameInstance GameInstance { get; private set; }

		// Token: 0x170011B2 RID: 4530
		// (get) Token: 0x060048A1 RID: 18593 RVA: 0x00118B12 File Offset: 0x00116D12
		// (set) Token: 0x060048A2 RID: 18594 RVA: 0x00118B1A File Offset: 0x00116D1A
		public Entity Entity { get; private set; }

		// Token: 0x060048A3 RID: 18595 RVA: 0x00118B24 File Offset: 0x00116D24
		private InteractionContext(Entity runningForEntity, InventorySectionType heldItemSectionId, ClientItemStack[] heldItemContainer, int heldItemSlot, ClientItemStack heldItem)
		{
			this.Entity = runningForEntity;
			this.HeldItemSlot = heldItemSlot;
			this.HeldItem = heldItem;
			this.HeldItemContainer = heldItemContainer;
			this.HeldItemSectionId = heldItemSectionId;
			this.OriginalItemType = ((heldItem != null) ? heldItem.Id : null);
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x00118B80 File Offset: 0x00116D80
		public InteractionChain Fork(InteractionContext context, int rootInteractionId)
		{
			return this.Fork(this.Chain.Type, context, rootInteractionId);
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x00118BA8 File Offset: 0x00116DA8
		public InteractionChain Fork(InteractionType type, InteractionContext context, int rootInteractionId)
		{
			return this.Fork(type, context, rootInteractionId, this.Entry.NextForkId(), false);
		}

		// Token: 0x060048A6 RID: 18598 RVA: 0x00118BD0 File Offset: 0x00116DD0
		public InteractionChain ForkPredicted(InteractionChainData data, InteractionType type, InteractionContext context, int rootInteractionId)
		{
			return this.Fork(data, type, context, rootInteractionId, this.Entry.NextPredictedForkId(), true);
		}

		// Token: 0x060048A7 RID: 18599 RVA: 0x00118BFC File Offset: 0x00116DFC
		public InteractionChain ForkPredicted(InteractionType type, InteractionContext context, int rootInteractionId)
		{
			return this.Fork(type, context, rootInteractionId, this.Entry.NextPredictedForkId(), true);
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x00118C24 File Offset: 0x00116E24
		private InteractionChain Fork(InteractionType type, InteractionContext context, int rootInteractionId, int subIndex, bool matchServer)
		{
			InteractionChainData data = new InteractionChainData(this.Chain.ChainData);
			return this.Fork(data, type, context, rootInteractionId, subIndex, matchServer);
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x00118C58 File Offset: 0x00116E58
		private InteractionChain Fork(InteractionChainData data, InteractionType type, InteractionContext context, int rootInteractionId, int subIndex, bool matchServer)
		{
			bool flag = context.MetaStore.TargetSlot == null;
			if (flag)
			{
				context.MetaStore.TargetSlot = this.MetaStore.TargetSlot;
			}
			bool flag2 = context.MetaStore.TargetSlot != null;
			if (flag2)
			{
				data.TargetSlot = context.MetaStore.TargetSlot.Value;
			}
			bool flag3 = context.MetaStore.HitLocation != null;
			if (flag3)
			{
				Vector4 value = context.MetaStore.HitLocation.Value;
				data.HitLocation = new Vector3f(value.X, value.Y, value.Z);
			}
			bool flag4 = context.MetaStore.HitDetail != null;
			if (flag4)
			{
				data.HitDetail = context.MetaStore.HitDetail;
			}
			bool flag5 = context.MetaStore.TargetBlock != null;
			if (flag5)
			{
				data.BlockPosition_ = context.MetaStore.TargetBlock;
			}
			bool flag6 = context.MetaStore.TargetEntity != null;
			if (flag6)
			{
				data.EntityId = context.MetaStore.TargetEntity.NetworkId;
			}
			ClientRootInteraction.Operation operation = this.GameInstance.InteractionModule.RootInteractions[this.Entry.State.RootInteraction].Operations[this.Entry.State.OperationCounter];
			ClientRootInteraction.InteractionWrapper interactionWrapper;
			bool flag7;
			if (matchServer)
			{
				interactionWrapper = (operation as ClientRootInteraction.InteractionWrapper);
				flag7 = (interactionWrapper != null);
			}
			else
			{
				flag7 = false;
			}
			bool flag8 = flag7;
			if (flag8)
			{
				ClientInteraction interaction = interactionWrapper.GetInteraction(this.GameInstance.InteractionModule);
				foreach (KeyValuePair<ulong, InteractionChain> keyValuePair in this.Chain.ForkedChains)
				{
					InteractionChain value2 = keyValuePair.Value;
					bool flag9 = value2.BaseForkedChainId == null;
					if (!flag9)
					{
						int entryIndex = value2.BaseForkedChainId.EntryIndex;
						bool flag10 = entryIndex != this.Entry.Index;
						if (!flag10)
						{
							InteractionChain interactionChain = interaction.MapForkChain(this, data);
							bool flag11 = interactionChain != null;
							if (flag11)
							{
								return interactionChain;
							}
						}
					}
				}
			}
			int chainId = this.Chain.ChainId;
			ForkedChainId forkedChainId = this.Chain.ForkedChainId;
			ForkedChainId forkedChainId2 = new ForkedChainId(this.Entry.Index, subIndex, null);
			bool flag12 = forkedChainId != null;
			if (flag12)
			{
				ForkedChainId forkedChainId3;
				forkedChainId = (forkedChainId3 = new ForkedChainId(forkedChainId));
				while (forkedChainId3.ForkedId != null)
				{
					forkedChainId3 = forkedChainId3.ForkedId;
				}
				forkedChainId3.ForkedId = forkedChainId2;
			}
			else
			{
				forkedChainId = forkedChainId2;
			}
			ClientRootInteraction rootInteraction = this.GameInstance.InteractionModule.RootInteractions[rootInteractionId];
			int hotbarActiveSlot = this.GameInstance.InventoryModule.HotbarActiveSlot;
			ClientItemStack activeHotbarItem = this.GameInstance.InventoryModule.GetActiveHotbarItem();
			InteractionChain interactionChain2 = new InteractionChain(forkedChainId, forkedChainId2, type, context, data, rootInteraction, hotbarActiveSlot, activeHotbarItem, null);
			interactionChain2.Time.Start();
			interactionChain2.ChainId = chainId;
			interactionChain2.Predicted = true;
			interactionChain2.SkipChainOnClick = this.AllowSkipChainOnClick;
			InteractionChain.TempChain tempChain;
			bool flag13 = this.Chain.RemoveTempForkedChain(forkedChainId2, out tempChain);
			if (flag13)
			{
				interactionChain2.CopyTempFrom(tempChain);
			}
			this.Chain.PutForkedChain(forkedChainId2, interactionChain2);
			return interactionChain2;
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x00118FBC File Offset: 0x001171BC
		public InteractionContext Duplicate()
		{
			InteractionContext interactionContext = new InteractionContext(this.Entity, this.HeldItemSectionId, this.HeldItemContainer, this.HeldItemSlot, this.HeldItem);
			interactionContext.MetaStore.CopyFrom(this.MetaStore);
			return interactionContext;
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x00119005 File Offset: 0x00117205
		public void Jump(ClientRootInteraction.Label label)
		{
			this.Chain.OperationCounter = label.Index;
		}

		// Token: 0x060048AC RID: 18604 RVA: 0x00119019 File Offset: 0x00117219
		public void Execute(ClientRootInteraction nextInteraction)
		{
			this.Chain.PushRoot(nextInteraction);
		}

		// Token: 0x060048AD RID: 18605 RVA: 0x0011902C File Offset: 0x0011722C
		public void SetTimeShift(float shift)
		{
			this.Chain.TimeShift = shift;
			bool flag = this.Chain.ForkedChainId == null;
			if (flag)
			{
				this.GameInstance.InteractionModule.SetGlobalTimeShift(this.Chain.Type, shift);
			}
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x00119078 File Offset: 0x00117278
		internal void InitEntry(InteractionChain chain, InteractionEntry entry, GameInstance gameInstance)
		{
			this.Chain = chain;
			this.Entry = entry;
			this.GameInstance = gameInstance;
			this.State = entry.State;
			this.ServerData = entry.ServerState;
			this.InstanceStore = entry.InteractionMetaStore;
			this.Labels = null;
			RootInteractionSettings rootInteractionSettings;
			this.Chain.SkipChainOnClick |= (this.Chain.RootInteraction.RootInteraction.Settings.TryGetValue(this.GameInstance.GameMode, out rootInteractionSettings) && rootInteractionSettings.AllowSkipChainOnClick);
		}

		// Token: 0x060048AF RID: 18607 RVA: 0x00119110 File Offset: 0x00117310
		internal void DeinitEntry(InteractionChain chain, InteractionEntry entry, GameInstance gameInstance)
		{
			this.State = null;
			this.ServerData = null;
			this.InstanceStore = null;
			this.Chain = null;
			this.Entry = null;
			this.GameInstance = null;
			this.Labels = null;
		}

		// Token: 0x060048B0 RID: 18608 RVA: 0x0011914C File Offset: 0x0011734C
		internal bool GetRootInteractionId(GameInstance gameInstance, InteractionType type, out int id)
		{
			InteractionSource interactionSource = this.Entity as InteractionSource;
			bool flag = interactionSource != null;
			if (flag)
			{
				bool flag2 = interactionSource.TryGetInteractionId(type, out id);
				if (flag2)
				{
					return true;
				}
			}
			id = int.MinValue;
			bool flag3 = this.OriginalItemType == null;
			bool result;
			if (flag3)
			{
				result = gameInstance.ServerSettings.UnarmedInteractions.TryGetValue(type, out id);
			}
			else
			{
				ClientItemBase item = gameInstance.ItemLibraryModule.GetItem(this.OriginalItemType);
				bool? flag4;
				if (item == null)
				{
					flag4 = null;
				}
				else
				{
					Dictionary<InteractionType, int> interactions = item.Interactions;
					flag4 = ((interactions != null) ? new bool?(interactions.TryGetValue(type, out id)) : null);
				}
				bool? flag5 = flag4;
				result = flag5.GetValueOrDefault();
			}
			return result;
		}

		// Token: 0x060048B1 RID: 18609 RVA: 0x00119204 File Offset: 0x00117404
		public static InteractionContext ForProxy(Entity runningForEntity, InventoryModule inventoryModule, InteractionType type)
		{
			return new InteractionContext(runningForEntity, InventorySectionType.Hotbar, inventoryModule.HotbarInventory, inventoryModule.HotbarActiveSlot, inventoryModule.GetHotbarItem(inventoryModule.HotbarActiveSlot));
		}

		// Token: 0x060048B2 RID: 18610 RVA: 0x00119238 File Offset: 0x00117438
		public static InteractionContext ForInteraction(GameInstance gameInstance, InventoryModule inventoryModule, InteractionType type, int? equipSlot = null)
		{
			switch (type)
			{
			case 0:
			case 2:
			case 3:
			case 4:
			case 5:
			case 8:
			{
				bool flag = inventoryModule.UsingToolsItem();
				if (flag)
				{
					return new InteractionContext(gameInstance.LocalPlayer, InventorySectionType.Tools, inventoryModule.ToolsInventory, inventoryModule.ToolsActiveSlot, inventoryModule.GetToolsItem(inventoryModule.ToolsActiveSlot));
				}
				bool flag2 = inventoryModule.GetHotbarItem(inventoryModule.HotbarActiveSlot) == null && inventoryModule.GetUtilityItem(inventoryModule.UtilityActiveSlot) != null;
				if (flag2)
				{
					return new InteractionContext(gameInstance.LocalPlayer, InventorySectionType.Utility, inventoryModule.UtilityInventory, inventoryModule.UtilityActiveSlot, inventoryModule.GetUtilityItem(inventoryModule.UtilityActiveSlot));
				}
				return new InteractionContext(gameInstance.LocalPlayer, InventorySectionType.Hotbar, inventoryModule.HotbarInventory, inventoryModule.HotbarActiveSlot, inventoryModule.GetHotbarItem(inventoryModule.HotbarActiveSlot));
			}
			case 1:
			{
				bool flag3 = inventoryModule.UsingToolsItem();
				if (flag3)
				{
					return new InteractionContext(gameInstance.LocalPlayer, InventorySectionType.Tools, inventoryModule.ToolsInventory, inventoryModule.ToolsActiveSlot, inventoryModule.GetToolsItem(inventoryModule.ToolsActiveSlot));
				}
				ClientItemStack hotbarItem = inventoryModule.GetHotbarItem(inventoryModule.HotbarActiveSlot);
				ClientItemStack utilityItem = inventoryModule.GetUtilityItem(inventoryModule.UtilityActiveSlot);
				bool flag4 = hotbarItem != null;
				if (flag4)
				{
					ClientItemBase item = gameInstance.ItemLibraryModule.GetItem(hotbarItem.Id);
					bool? flag5;
					if (item == null)
					{
						flag5 = null;
					}
					else
					{
						ItemBase.ItemUtility utility = item.Utility;
						flag5 = ((utility != null) ? new bool?(utility.Compatible) : null);
					}
					bool? flag6 = flag5;
					bool valueOrDefault = flag6.GetValueOrDefault();
					bool flag7 = valueOrDefault && utilityItem != null;
					if (flag7)
					{
						return new InteractionContext(gameInstance.LocalPlayer, InventorySectionType.Utility, inventoryModule.UtilityInventory, inventoryModule.UtilityActiveSlot, inventoryModule.GetUtilityItem(inventoryModule.UtilityActiveSlot));
					}
				}
				else
				{
					bool flag8 = utilityItem != null;
					if (flag8)
					{
						return new InteractionContext(gameInstance.LocalPlayer, InventorySectionType.Utility, inventoryModule.UtilityInventory, inventoryModule.UtilityActiveSlot, inventoryModule.GetUtilityItem(inventoryModule.UtilityActiveSlot));
					}
				}
				break;
			}
			case 6:
				return new InteractionContext(gameInstance.LocalPlayer, InventorySectionType.Consumable, inventoryModule.ConsumableInventory, inventoryModule.ConsumableActiveSlot, inventoryModule.GetConsumableItem(inventoryModule.ConsumableActiveSlot));
			case 7:
				break;
			default:
				switch (type)
				{
				case 22:
					return new InteractionContext(gameInstance.LocalPlayer, InventorySectionType.Hotbar, inventoryModule.HotbarInventory, inventoryModule.HotbarActiveSlot, inventoryModule.GetHotbarItem(inventoryModule.HotbarActiveSlot));
				case 23:
					return new InteractionContext(gameInstance.LocalPlayer, InventorySectionType.Utility, inventoryModule.UtilityInventory, inventoryModule.UtilityActiveSlot, inventoryModule.GetUtilityItem(inventoryModule.UtilityActiveSlot));
				case 24:
				{
					bool flag9 = equipSlot == null;
					if (flag9)
					{
						throw new ArgumentException("Equipped interaction type requires a slot set");
					}
					return new InteractionContext(gameInstance.LocalPlayer, InventorySectionType.Armor, inventoryModule._armorInventory, equipSlot.Value, inventoryModule.GetArmorItem(equipSlot.Value));
				}
				}
				break;
			}
			return new InteractionContext(gameInstance.LocalPlayer, InventorySectionType.Hotbar, inventoryModule.HotbarInventory, inventoryModule.HotbarActiveSlot, inventoryModule.GetHotbarItem(inventoryModule.HotbarActiveSlot));
		}

		// Token: 0x060048B3 RID: 18611 RVA: 0x00119564 File Offset: 0x00117764
		public static InteractionContext ForInteraction(Entity entity, InteractionType type)
		{
			ClientItemBase clientItemBase = entity.PrimaryItem;
			switch (type)
			{
			case 0:
			case 2:
			case 3:
			case 4:
			case 5:
				clientItemBase = (clientItemBase ?? entity.SecondaryItem);
				break;
			case 1:
				clientItemBase = (entity.SecondaryItem ?? clientItemBase);
				break;
			case 6:
				clientItemBase = (entity.ConsumableItem ?? clientItemBase);
				break;
			default:
				if (type != 22)
				{
					if (type == 23)
					{
						clientItemBase = entity.SecondaryItem;
					}
				}
				break;
			}
			ClientItemStack heldItem = (clientItemBase != null) ? new ClientItemStack(clientItemBase.Id, 1) : null;
			return new InteractionContext(entity, InventorySectionType.Hotbar, null, 0, heldItem);
		}

		// Token: 0x040024B3 RID: 9395
		public readonly InventorySectionType HeldItemSectionId;

		// Token: 0x040024B4 RID: 9396
		public readonly ClientItemStack[] HeldItemContainer;

		// Token: 0x040024B5 RID: 9397
		public readonly int HeldItemSlot;

		// Token: 0x040024B6 RID: 9398
		public ClientItemStack HeldItem;

		// Token: 0x040024B8 RID: 9400
		public readonly ContextMetaStore MetaStore = new ContextMetaStore();

		// Token: 0x040024BF RID: 9407
		public ClientRootInteraction.Label[] Labels;
	}
}
