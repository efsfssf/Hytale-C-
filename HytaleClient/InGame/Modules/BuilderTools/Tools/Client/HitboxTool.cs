using System;
using System.Collections.Generic;
using HytaleClient.AssetEditor.Interface.Config;
using HytaleClient.Data.Map;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json.Linq;
using SDL2;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools.Client
{
	// Token: 0x02000987 RID: 2439
	internal class HitboxTool : ClientTool
	{
		// Token: 0x17001278 RID: 4728
		// (get) Token: 0x06004D6C RID: 19820 RVA: 0x0014D0AD File Offset: 0x0014B2AD
		public override string ToolId
		{
			get
			{
				return "Hitbox";
			}
		}

		// Token: 0x06004D6D RID: 19821 RVA: 0x0014D0B4 File Offset: 0x0014B2B4
		public HitboxTool(GameInstance gameInstance) : base(gameInstance)
		{
			this._boxEditorGizmo = new BoxEditorGizmo(this._gameInstance.Engine.Graphics, delegate(BoundingBox box)
			{
				this._hitbox.Boxes[this._boxId] = box;
			});
			this._boxRenderer = new BoxRenderer(this._gameInstance.Engine.Graphics, this._gameInstance.Engine.Graphics.GPUProgramStore.BasicProgram);
			GraphicsDevice graphics = this._gameInstance.Engine.Graphics;
			this._boxColors = new Vector3[]
			{
				graphics.WhiteColor,
				graphics.RedColor,
				graphics.GreenColor,
				graphics.BlueColor,
				graphics.CyanColor,
				graphics.YellowColor,
				graphics.MagentaColor,
				graphics.BlackColor
			};
		}

		// Token: 0x06004D6E RID: 19822 RVA: 0x0014D1C7 File Offset: 0x0014B3C7
		protected override void DoDispose()
		{
			this._boxRenderer.Dispose();
			this._boxEditorGizmo.Dispose();
		}

		// Token: 0x06004D6F RID: 19823 RVA: 0x0014D1E4 File Offset: 0x0014B3E4
		public override bool NeedsDrawing()
		{
			return this._state > ToolState.None;
		}

		// Token: 0x06004D70 RID: 19824 RVA: 0x0014D200 File Offset: 0x0014B400
		public override void Draw(ref Matrix viewProjectionMatrix)
		{
			bool flag = this._state == ToolState.None;
			if (!flag)
			{
				GraphicsDevice graphics = this._gameInstance.Engine.Graphics;
				GLFunctions gl = graphics.GL;
				Vector3 cameraPosition = this._gameInstance.SceneRenderer.Data.CameraPosition;
				gl.DepthFunc(GL.ALWAYS);
				bool flag2 = this._state == ToolState.Hover;
				BlockHitbox blockHitbox;
				if (flag2)
				{
					int block = this._gameInstance.MapModule.GetBlock(this._blockPosition, int.MaxValue);
					ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
					blockHitbox = this._gameInstance.ServerSettings.BlockHitboxes[clientBlockType.HitboxType];
				}
				else
				{
					blockHitbox = this._hitbox;
				}
				HitDetection.RaycastHit targetBlockHit = this._gameInstance.InteractionModule.TargetBlockHit;
				Vector3 color = Vector3.One;
				for (int i = 0; i < blockHitbox.Boxes.Length; i++)
				{
					Vector3 vector = (this._state == ToolState.Hover) ? this._boxColors[0] : this._boxColors[i % this._boxColors.Length];
					bool flag3 = this._boxId == i;
					if (flag3)
					{
						color = vector;
					}
					float num = 0.1f;
					bool flag4 = this._state == ToolState.Editing && this._boxId == i;
					if (!flag4)
					{
						bool flag5 = targetBlockHit.BoxId == i || (this._state == ToolState.Editing && this._boxId == i);
						if (flag5)
						{
							num = 0.3f;
						}
						bool flag6 = this._state == ToolState.Hover || (this._state == ToolState.Selected && this._blockPosition != targetBlockHit.BlockOrigin);
						if (flag6)
						{
							num = 0.1f;
						}
						else
						{
							bool flag7 = this._state == ToolState.Editing && this._boxId != i;
							if (flag7)
							{
								num = 0.05f;
							}
						}
						this._boxRenderer.Draw(this._blockPosition - cameraPosition, blockHitbox.Boxes[i], viewProjectionMatrix, vector, num * 3f, vector, num);
					}
				}
				bool flag8 = this._state == ToolState.Editing || this._state == ToolState.Hover;
				if (flag8)
				{
					this._boxEditorGizmo.Draw(ref viewProjectionMatrix, -cameraPosition, color);
				}
				gl.DepthFunc((!graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
			}
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x0014D480 File Offset: 0x0014B680
		public override void Update(float deltaTime)
		{
			switch (this._state)
			{
			case ToolState.None:
			case ToolState.Hover:
			{
				bool hasFoundTargetBlock = this._gameInstance.InteractionModule.HasFoundTargetBlock;
				if (hasFoundTargetBlock)
				{
					this._blockPosition = this._gameInstance.InteractionModule.TargetBlockHit.BlockOrigin;
					this._state = ToolState.Hover;
				}
				else
				{
					this._state = ToolState.None;
				}
				break;
			}
			case ToolState.Selected:
			{
				bool flag = this._gameInstance.Input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_INSERT, false);
				if (flag)
				{
					BoundingBox[] array = new BoundingBox[this._hitbox.Boxes.Length + 1];
					for (int i = 0; i < this._hitbox.Boxes.Length; i++)
					{
						array[i] = this._hitbox.Boxes[i];
					}
					array[array.Length - 1] = this.VoxelBox;
					this._hitbox = new BlockHitbox(array);
					this.OnBoxUpdate();
					this._gameInstance.Chat.Log("Added new bounding box to the block hitbox.");
				}
				bool flag2 = this._gameInstance.Input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_DELETE, false);
				if (flag2)
				{
					bool flag3 = this._gameInstance.InteractionModule.HasFoundTargetBlock && this._gameInstance.InteractionModule.TargetBlockHit.BlockOrigin == this._blockPosition;
					if (flag3)
					{
						bool flag4 = this._hitbox.Boxes.Length < 2;
						if (flag4)
						{
							this._gameInstance.Chat.Log("Unable to remove box, at least one must be present in the hitbox.");
						}
						else
						{
							BoundingBox[] array2 = new BoundingBox[this._hitbox.Boxes.Length - 1];
							int num = 0;
							for (int j = 0; j < this._hitbox.Boxes.Length; j++)
							{
								bool flag5 = j == this._gameInstance.InteractionModule.TargetBlockHit.BoxId;
								if (!flag5)
								{
									array2[num] = this._hitbox.Boxes[j];
									num++;
								}
							}
							this._hitbox = new BlockHitbox(array2);
							this.OnBoxUpdate();
							this._gameInstance.Chat.Log("Bounding box removed.");
						}
					}
				}
				break;
			}
			case ToolState.Editing:
				this._boxEditorGizmo.Tick(this._gameInstance.CameraModule.GetLookRay(), this._gameInstance.Input.IsAltHeld());
				break;
			}
		}

		// Token: 0x06004D72 RID: 19826 RVA: 0x0014D71C File Offset: 0x0014B91C
		public override void OnInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = clickType == InteractionModule.ClickType.None;
			if (!flag)
			{
				Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
				switch (this._state)
				{
				case ToolState.Hover:
				{
					bool flag2 = interactionType == 0;
					if (flag2)
					{
						int block = this._gameInstance.MapModule.GetBlock(this._blockPosition, int.MaxValue);
						bool flag3 = block == int.MaxValue || block == 1;
						if (!flag3)
						{
							ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
							ClientBlockType clientBlockTypeFromName = this._gameInstance.MapModule.GetClientBlockTypeFromName(ClientBlockType.GetOriginalBlockName(clientBlockType.Name));
							string text = null;
							Dictionary<string, string> blockVariantData = ClientBlockType.GetBlockVariantData(clientBlockType.Name);
							if (blockVariantData != null)
							{
								blockVariantData.TryGetValue("State", out text);
							}
							this._hitboxType = clientBlockType.HitboxType;
							this._hitbox = this._gameInstance.ServerSettings.BlockHitboxes[clientBlockType.HitboxType].Clone();
							this._state = ToolState.Selected;
							string message = string.Concat(new string[]
							{
								"Block: ",
								clientBlockTypeFromName.Name,
								(clientBlockTypeFromName.Variants.Count == 0) ? "\n" : string.Format(" - {0} Variants\n", clientBlockTypeFromName.Variants.Count),
								(text == null) ? "" : ("State: " + text + "\n"),
								string.Format("Hitbox: {0} - Used by {1} other blocks", clientBlockTypeFromName.HitboxType, this.GetHitboxUsageCount(block))
							});
							this._gameInstance.Chat.Log(message);
						}
					}
					else
					{
						this._state = ToolState.None;
					}
					break;
				}
				case ToolState.Selected:
				{
					bool flag4 = interactionType == 0;
					if (flag4)
					{
						bool flag5 = !this._gameInstance.InteractionModule.HasFoundTargetBlock || this._blockPosition != this._gameInstance.InteractionModule.TargetBlockHit.BlockOrigin;
						if (!flag5)
						{
							this._boxId = this._gameInstance.InteractionModule.TargetBlockHit.BoxId;
							Vector3[] hitboxSnapValues = HitboxTool.GetHitboxSnapValues(this._hitbox, this._boxId);
							this._boxEditorGizmo.Show(this._blockPosition, this._hitbox.Boxes[this._boxId], hitboxSnapValues);
							this._boxEditorGizmo.Tick(lookRay, this._gameInstance.Input.IsAltHeld());
							this._boxEditorGizmo.OnInteract(interactionType, lookRay, this._gameInstance.Input.IsShiftHeld(), this._gameInstance.Input.IsAltHeld());
							this._state = ToolState.Editing;
						}
					}
					else
					{
						this._state = ToolState.None;
					}
					break;
				}
				case ToolState.Editing:
				{
					bool flag6 = interactionType == 0;
					if (flag6)
					{
						this._boxEditorGizmo.OnInteract(interactionType, lookRay, this._gameInstance.Input.IsShiftHeld(), this._gameInstance.Input.IsAltHeld());
						this.OnBoxUpdate();
					}
					else
					{
						this._boxEditorGizmo.ResetBox();
					}
					this._boxEditorGizmo.Hide();
					this._state = ToolState.Selected;
					break;
				}
				}
			}
		}

		// Token: 0x06004D73 RID: 19827 RVA: 0x0014DA68 File Offset: 0x0014BC68
		private void OnBoxUpdate()
		{
			BlockHitbox blockHitbox = this._hitbox.Clone();
			bool flag = !this._gameInstance.ServerSettings.BlockHitboxes[this._hitboxType].Equals(blockHitbox);
			if (flag)
			{
				int block = this._gameInstance.MapModule.GetBlock(this._blockPosition, int.MaxValue);
				this.UpdateHitboxAsset(block, blockHitbox);
				this._gameInstance.Chat.Log("Block hitbox updated!");
			}
		}

		// Token: 0x06004D74 RID: 19828 RVA: 0x0014DAE4 File Offset: 0x0014BCE4
		private void UpdateHitboxAsset(int blockId, BlockHitbox hitbox)
		{
			ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[blockId];
			string text = clientBlockType.Name;
			bool flag = clientBlockType.RotationPitch != null || clientBlockType.RotationYaw > 0;
			if (flag)
			{
				HitboxTool.RotateHitBox(clientBlockType, ref hitbox);
				text = ClientBlockType.GetOriginalBlockName(clientBlockType.Name);
				clientBlockType = this._gameInstance.MapModule.GetClientBlockTypeFromName(text);
			}
			int hitboxType = clientBlockType.HitboxType;
			bool flag2 = hitboxType == 0 || hitboxType == int.MinValue;
			if (flag2)
			{
				this._gameInstance.Chat.Error("Hitbox for block \"" + text + "\" can't be edited!");
			}
			else
			{
				JArray jarray = JArray.FromObject(hitbox.Boxes);
				JsonUpdateCommand[] array = new JsonUpdateCommand[1];
				int num = 0;
				JsonUpdateCommand jsonUpdateCommand = new JsonUpdateCommand();
				jsonUpdateCommand.Type = 0;
				jsonUpdateCommand.Path = PropertyPath.FromString("Boxes").Elements;
				JObject jobject = new JObject();
				jobject.Add("value", jarray);
				jsonUpdateCommand.Value = (sbyte[])ProtoHelper.SerializeBson(jobject);
				array[num] = jsonUpdateCommand;
				JsonUpdateCommand[] commands = array;
				this._gameInstance.Connection.SendPacket(new AssetEditorUpdateJsonAsset
				{
					Token = 0,
					AssetType = "BlockBoundingBoxes",
					AssetIndex = hitboxType,
					Commands = commands
				});
			}
		}

		// Token: 0x06004D75 RID: 19829 RVA: 0x0014DC24 File Offset: 0x0014BE24
		private int GetHitboxUsageCount(int blockId)
		{
			ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[blockId];
			string originalBlockName = ClientBlockType.GetOriginalBlockName(clientBlockType.Name);
			ClientBlockType clientBlockTypeFromName = this._gameInstance.MapModule.GetClientBlockTypeFromName(originalBlockName);
			int num = 0;
			foreach (ClientBlockType clientBlockType2 in this._gameInstance.MapModule.ClientBlockTypes)
			{
				int? num2 = (clientBlockType2 != null) ? new int?(clientBlockType2.HitboxType) : null;
				int hitboxType = clientBlockTypeFromName.HitboxType;
				bool flag = (num2.GetValueOrDefault() == hitboxType & num2 != null) && !clientBlockType2.Name.Contains('|'.ToString()) && clientBlockType2.Name != clientBlockTypeFromName.Name;
				if (flag)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06004D76 RID: 19830 RVA: 0x0014DD18 File Offset: 0x0014BF18
		private static Vector3[] GetHitboxSnapValues(BlockHitbox hitbox, int excludedBox)
		{
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < hitbox.Boxes.Length; i++)
			{
				bool flag = i == excludedBox;
				if (!flag)
				{
					list.Add(hitbox.Boxes[i].Min);
					list.Add(hitbox.Boxes[i].Max);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06004D77 RID: 19831 RVA: 0x0014DD8C File Offset: 0x0014BF8C
		private static void RotateHitBox(ClientBlockType blockType, ref BlockHitbox hitbox)
		{
			Rotation rotation = 0;
			switch (blockType.RotationYaw)
			{
			case 0:
				rotation = 0;
				break;
			case 1:
				rotation = 3;
				break;
			case 2:
				rotation = 2;
				break;
			case 3:
				rotation = 1;
				break;
			}
			Rotation rotation2 = 0;
			switch (blockType.RotationPitch)
			{
			case 0:
				rotation2 = 0;
				break;
			case 1:
				rotation2 = 3;
				break;
			case 2:
				rotation2 = 2;
				break;
			case 3:
				rotation2 = 1;
				break;
			}
			hitbox.Rotate(MathHelper.RotationToDegrees(rotation2), MathHelper.RotationToDegrees(rotation));
		}

		// Token: 0x040028AB RID: 10411
		private readonly BoxEditorGizmo _boxEditorGizmo;

		// Token: 0x040028AC RID: 10412
		private readonly BoxRenderer _boxRenderer;

		// Token: 0x040028AD RID: 10413
		private readonly Vector3[] _boxColors;

		// Token: 0x040028AE RID: 10414
		private ToolState _state = ToolState.None;

		// Token: 0x040028AF RID: 10415
		private int _blockIndex;

		// Token: 0x040028B0 RID: 10416
		private Vector3 _blockPosition;

		// Token: 0x040028B1 RID: 10417
		private BlockHitbox _hitbox;

		// Token: 0x040028B2 RID: 10418
		private int _hitboxType;

		// Token: 0x040028B3 RID: 10419
		private int _boxId;

		// Token: 0x040028B4 RID: 10420
		private readonly BoundingBox VoxelBox = new BoundingBox(Vector3.Zero, Vector3.One);
	}
}
