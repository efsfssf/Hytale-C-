using System;
using System.Collections.Generic;
using HytaleClient.Data.Map;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame
{
	// Token: 0x020008E6 RID: 2278
	internal class HitDetection
	{
		// Token: 0x0600436A RID: 17258 RVA: 0x000D4EEB File Offset: 0x000D30EB
		public HitDetection(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x000D4EFC File Offset: 0x000D30FC
		public bool Raycast(Vector3 origin, Vector3 direction, HitDetection.RaycastOptions options, out bool hasFoundTargetBlock, out HitDetection.RaycastHit blockHitData, out bool hasFoundTargetEntity, out HitDetection.EntityHitData entityHitData)
		{
			hasFoundTargetBlock = this._gameInstance.HitDetection.RaycastBlock(origin, direction, options, out blockHitData);
			hasFoundTargetEntity = this._gameInstance.HitDetection.RaycastEntity(origin, direction, hasFoundTargetBlock ? blockHitData.Distance : options.Distance, options.CheckOnlyTangibleEntities, out entityHitData);
			return hasFoundTargetBlock | hasFoundTargetEntity;
		}

		// Token: 0x0600436C RID: 17260 RVA: 0x000D4F60 File Offset: 0x000D3160
		public bool RaycastBlock(Vector3 origin, Vector3 direction, out HitDetection.RaycastHit raycastHit)
		{
			return this.RaycastBlock(origin, direction, HitDetection.DefaultRaycastOptions, out raycastHit);
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x000D4F80 File Offset: 0x000D3180
		public bool RaycastBlock(Vector3 origin, Vector3 direction, HitDetection.RaycastOptions options, out HitDetection.RaycastHit raycastHit)
		{
			int num = 0;
			HitDetection.BoxIntersection boxIntersection = new HitDetection.BoxIntersection(origin, Vector3.Zero);
			float num2 = options.Distance;
			HitDetection.RayBoxCollision rayBoxCollision = default(HitDetection.RayBoxCollision);
			bool flag = false;
			raycastHit = default(HitDetection.RaycastHit);
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			Chunk chunk = null;
			while (Vector3.Distance(origin, boxIntersection.Position) <= options.Distance && num < 5000)
			{
				bool flag2 = num != 0;
				if (flag2)
				{
					HitDetection.NextGridIntersection(boxIntersection.Position, direction, out boxIntersection);
				}
				num++;
				int num6 = (int)Math.Floor((double)boxIntersection.Position.X) - ((boxIntersection.Normal.X > 0f) ? 1 : 0);
				int num7 = (int)Math.Floor((double)boxIntersection.Position.Y) - ((boxIntersection.Normal.Y > 0f) ? 1 : 0);
				int num8 = (int)Math.Floor((double)boxIntersection.Position.Z) - ((boxIntersection.Normal.Z > 0f) ? 1 : 0);
				int num9 = num6;
				int num10 = num7;
				int num11 = num8;
				int num12 = num6 >> 5;
				int num13 = num7 >> 5;
				int num14 = num8 >> 5;
				bool flag3 = chunk == null || chunk.X != num12 || chunk.Y != num13 || chunk.Z != num14;
				if (flag3)
				{
					chunk = this._gameInstance.MapModule.GetChunk(num12, num13, num14);
				}
				bool flag4 = chunk == null;
				if (!flag4)
				{
					int block = chunk.Data.GetBlock(num6, num7, num8);
					bool flag5 = block == 0;
					if (flag5)
					{
						num3 = num6;
						num4 = num7;
						num5 = num8;
					}
					else
					{
						ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
						bool flag6 = clientBlockType.FillerX != 0 || clientBlockType.FillerY != 0 || clientBlockType.FillerZ != 0;
						if (flag6)
						{
							num6 -= clientBlockType.FillerX;
							num7 -= clientBlockType.FillerY;
							num8 -= clientBlockType.FillerZ;
							block = this._gameInstance.MapModule.GetBlock(num6, num7, num8, 1);
							clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
						}
						BlockType.Material collisionMaterial = clientBlockType.CollisionMaterial;
						bool flag7 = options.IgnoreFluids && collisionMaterial == 2;
						if (!flag7)
						{
							bool flag8 = options.IgnoreEmptyCollisionMaterial && collisionMaterial == 0;
							if (!flag8)
							{
								bool flag9;
								if (options.RequiredBlockTag != -2147483648)
								{
									Dictionary<int, int[]> tagIndexes = clientBlockType.TagIndexes;
									flag9 = (tagIndexes != null && !tagIndexes.ContainsKey(options.RequiredBlockTag));
								}
								else
								{
									flag9 = false;
								}
								bool flag10 = flag9;
								if (!flag10)
								{
									float num15 = options.Distance;
									BlockHitbox blockHitbox = this._gameInstance.ServerSettings.BlockHitboxes[clientBlockType.HitboxType];
									int boxId = 0;
									int i = 0;
									while (i < blockHitbox.Boxes.Length)
									{
										HitDetection.RayBoxCollision rayBoxCollision2;
										bool flag11 = HitDetection.CheckRayBoxCollision(blockHitbox.Boxes[i], (float)num6, (float)num7, (float)num8, boxIntersection.Position, direction, out rayBoxCollision2, false);
										if (flag11)
										{
											float num16 = Vector3.Distance(origin, rayBoxCollision2.Position);
											bool flag12 = num16 >= num15;
											if (!flag12)
											{
												num15 = num16;
												rayBoxCollision = rayBoxCollision2;
												boxId = i;
											}
										}
										IL_315:
										i++;
										continue;
										goto IL_315;
									}
									bool flag13 = num15 < num2;
									if (flag13)
									{
										Vector3 vector = new Vector3((float)num6, (float)num7, (float)num8);
										Vector3 blockPositionNoFiller = new Vector3((float)num9, (float)num10, (float)num11);
										num2 = num15;
										raycastHit = new HitDetection.RaycastHit(vector, blockPositionNoFiller, vector, origin, rayBoxCollision.Position, rayBoxCollision.Normal, boxIntersection.Normal, rayBoxCollision.TextureCoord, num2, block, clientBlockType.HitboxType, boxId);
										flag = true;
										break;
									}
								}
							}
						}
					}
				}
			}
			bool flag14 = options.ReturnEndpointBlock && !flag;
			if (flag14)
			{
				Vector3 vector2 = new Vector3((float)num3, (float)num4, (float)num5);
				Vector3 blockPositionNoFiller2 = new Vector3((float)num3, (float)num4, (float)num5);
				int block2 = this._gameInstance.MapModule.GetBlock(num3, num4, num5, 1);
				ClientBlockType clientBlockType2 = this._gameInstance.MapModule.ClientBlockTypes[block2];
				int boxId2 = 0;
				raycastHit = new HitDetection.RaycastHit(vector2, blockPositionNoFiller2, vector2, origin, rayBoxCollision.Position, rayBoxCollision.Normal, boxIntersection.Normal, rayBoxCollision.TextureCoord, options.Distance - 1f, block2, clientBlockType2.HitboxType, boxId2);
				flag = true;
			}
			bool checkOversizedBoxes = options.CheckOversizedBoxes;
			if (checkOversizedBoxes)
			{
				int num17 = 1;
				int num18 = (int)origin.X;
				int num19 = (int)origin.Y;
				int num20 = (int)origin.Z;
				for (int j = -num17; j <= num17; j++)
				{
					for (int k = -num17; k <= num17; k++)
					{
						for (int l = -num17; l <= num17; l++)
						{
							int num21 = j + num18;
							int num22 = k + num19;
							int num23 = l + num20;
							int num24 = num21 >> 5;
							int num25 = num22 >> 5;
							int num26 = num23 >> 5;
							bool flag15 = chunk == null || chunk.X != num24 || chunk.Y != num25 || chunk.Z != num26;
							if (flag15)
							{
								chunk = this._gameInstance.MapModule.GetChunk(num24, num25, num26);
							}
							bool flag16 = chunk == null;
							if (!flag16)
							{
								int block3 = chunk.Data.GetBlock(num21, num22, num23);
								bool flag17 = block3 == 0;
								if (!flag17)
								{
									ClientBlockType clientBlockType3 = this._gameInstance.MapModule.ClientBlockTypes[block3];
									bool flag18 = options.IgnoreFluids && clientBlockType3.CollisionMaterial == 2;
									if (!flag18)
									{
										bool flag19 = options.IgnoreEmptyCollisionMaterial && clientBlockType3.CollisionMaterial == 0;
										if (!flag19)
										{
											BlockHitbox blockHitbox2 = this._gameInstance.ServerSettings.BlockHitboxes[clientBlockType3.HitboxType];
											bool flag20 = blockHitbox2.BoundingBox.Min.X < 0f || blockHitbox2.BoundingBox.Min.Y < 0f || blockHitbox2.BoundingBox.Min.Z < 0f || blockHitbox2.BoundingBox.Max.X > 1f || blockHitbox2.BoundingBox.Max.Y > 1f || blockHitbox2.BoundingBox.Max.Z > 1f;
											bool flag21 = flag20;
											if (flag21)
											{
												int boxId3 = 0;
												float num15 = options.Distance;
												int num27 = num21 - clientBlockType3.FillerX;
												int num28 = num22 - clientBlockType3.FillerY;
												int num29 = num23 - clientBlockType3.FillerZ;
												int m = 0;
												while (m < blockHitbox2.Boxes.Length)
												{
													HitDetection.RayBoxCollision rayBoxCollision2;
													bool flag22 = HitDetection.CheckRayBoxCollision(blockHitbox2.Boxes[m], (float)num27, (float)num28, (float)num29, origin, direction, out rayBoxCollision2, false);
													if (flag22)
													{
														float num30 = Vector3.Distance(origin, rayBoxCollision2.Position);
														bool flag23 = num30 >= num15;
														if (!flag23)
														{
															num15 = num30;
															rayBoxCollision = rayBoxCollision2;
															boxId3 = m;
														}
													}
													IL_70B:
													m++;
													continue;
													goto IL_70B;
												}
												bool flag24 = num15 < num2;
												if (flag24)
												{
													Vector3 vector3 = new Vector3((float)Math.Floor((double)rayBoxCollision.Position.X), (float)Math.Floor((double)rayBoxCollision.Position.Y), (float)Math.Floor((double)rayBoxCollision.Position.Z));
													bool flag25 = rayBoxCollision.Normal.X == 1f;
													if (flag25)
													{
														vector3.X = (float)Math.Ceiling((double)rayBoxCollision.Position.X) - 1f;
													}
													else
													{
														bool flag26 = rayBoxCollision.Normal.Y == 1f;
														if (flag26)
														{
															vector3.Y = (float)Math.Ceiling((double)rayBoxCollision.Position.Y) - 1f;
														}
														else
														{
															bool flag27 = rayBoxCollision.Normal.Z == 1f;
															if (flag27)
															{
																vector3.Z = (float)Math.Ceiling((double)rayBoxCollision.Position.Z) - 1f;
															}
														}
													}
													num2 = num15;
													raycastHit = new HitDetection.RaycastHit(vector3, vector3, new Vector3((float)num27, (float)num28, (float)num29), origin, rayBoxCollision.Position, rayBoxCollision.Normal, boxIntersection.Normal, rayBoxCollision.TextureCoord, num2, block3, clientBlockType3.HitboxType, boxId3);
													flag = true;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x000D5854 File Offset: 0x000D3A54
		public bool RaycastEntity(Vector3 rayPosition, Vector3 rayDirection, float maxDistance, bool checkOnlyTangibleEntities, out HitDetection.EntityHitData entityHitData)
		{
			float num = maxDistance;
			Entity entity = null;
			HitDetection.RayBoxCollision rayBoxCollision = default(HitDetection.RayBoxCollision);
			Entity[] allEntities = this._gameInstance.EntityStoreModule.GetAllEntities();
			int entitiesCount = this._gameInstance.EntityStoreModule.GetEntitiesCount();
			for (int i = 0; i < entitiesCount; i++)
			{
				Entity entity2 = allEntities[i];
				bool flag = entity2.NetworkId == this._gameInstance.LocalPlayerNetworkId || (checkOnlyTangibleEntities && !entity2.IsTangible());
				if (!flag)
				{
					BoundingBox hitbox = entity2.Hitbox;
					hitbox.Translate(entity2.Position);
					HitDetection.RayBoxCollision rayBoxCollision2;
					bool flag2 = !HitDetection.CheckRayBoxCollision(hitbox, rayPosition, rayDirection, out rayBoxCollision2, false);
					if (!flag2)
					{
						float num2 = Vector3.Distance(rayPosition, rayBoxCollision2.Position);
						bool flag3 = num2 >= num;
						if (!flag3)
						{
							num = num2;
							entity = entity2;
							rayBoxCollision = rayBoxCollision2;
						}
					}
				}
			}
			entityHitData = new HitDetection.EntityHitData(entity, rayBoxCollision, num);
			return entity != null;
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x000D5958 File Offset: 0x000D3B58
		public static bool CheckRayBoxCollision(BoundingBox box, Vector3 position, Vector3 direction, out HitDetection.RayBoxCollision collision, bool checkReverse = false)
		{
			return HitDetection.CheckRayBoxCollision(box, 0f, 0f, 0f, position, direction, out collision, checkReverse);
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x000D5984 File Offset: 0x000D3B84
		public static bool CheckRayBoxCollision(BoundingBox box, float offsetX, float offsetY, float offsetZ, Vector3 position, Vector3 direction, out HitDetection.RayBoxCollision collision, bool checkReverse = false)
		{
			Vector3 zero = Vector3.Zero;
			Vector2 zero2 = Vector2.Zero;
			float num = box.Min.X + offsetX;
			float num2 = box.Max.X + offsetX;
			float num3 = box.Min.Y + offsetY;
			float num4 = box.Max.Y + offsetY;
			float num5 = box.Min.Z + offsetZ;
			float num6 = box.Max.Z + offsetZ;
			bool flag = direction.X >= 0f;
			if (flag)
			{
				float num7 = (num - position.X) / direction.X;
				bool flag2 = checkReverse || num7 >= 0f;
				if (flag2)
				{
					zero = new Vector3(position.X + num7 * direction.X, position.Y + num7 * direction.Y, position.Z + num7 * direction.Z);
					bool flag3 = zero.Y >= num3 && zero.Y <= num4 && zero.Z >= num5 && zero.Z <= num6;
					if (flag3)
					{
						zero2 = new Vector2(1f - (num6 - zero.Z) / (num6 - num5), 1f - (num4 - zero.Y) / (num4 - num3));
						collision = new HitDetection.RayBoxCollision(zero, Vector3.Left, zero2);
						return true;
					}
				}
			}
			else
			{
				float num7 = (position.X - num2) / -direction.X;
				bool flag4 = checkReverse || num7 >= 0f;
				if (flag4)
				{
					zero = new Vector3(position.X + num7 * direction.X, position.Y + num7 * direction.Y, position.Z + num7 * direction.Z);
					bool flag5 = zero.Y >= num3 && zero.Y <= num4 && zero.Z >= num5 && zero.Z <= num6;
					if (flag5)
					{
						zero2 = new Vector2((num6 - zero.Z) / (num6 - num5), 1f - (num4 - zero.Y) / (num4 - num3));
						collision = new HitDetection.RayBoxCollision(zero, Vector3.Right, zero2);
						return true;
					}
				}
			}
			bool flag6 = direction.Y >= 0f;
			if (flag6)
			{
				float num7 = (num3 - position.Y) / direction.Y;
				bool flag7 = checkReverse || num7 >= 0f;
				if (flag7)
				{
					zero = new Vector3(position.X + num7 * direction.X, position.Y + num7 * direction.Y, position.Z + num7 * direction.Z);
					bool flag8 = zero.X >= num && zero.X <= num2 && zero.Z >= num5 && zero.Z <= num6;
					if (flag8)
					{
						zero2 = new Vector2((num2 - zero.X) / (num2 - num), (num6 - zero.Z) / (num6 - num5));
						collision = new HitDetection.RayBoxCollision(zero, Vector3.Down, zero2);
						return true;
					}
				}
			}
			else
			{
				float num7 = (position.Y - num4) / -direction.Y;
				bool flag9 = checkReverse || num7 >= 0f;
				if (flag9)
				{
					zero = new Vector3(position.X + num7 * direction.X, position.Y + num7 * direction.Y, position.Z + num7 * direction.Z);
					bool flag10 = zero.X >= num && zero.X <= num2 && zero.Z >= num5 && zero.Z <= num6;
					if (flag10)
					{
						zero2 = new Vector2((num2 - zero.X) / (num2 - num), (num6 - zero.Z) / (num6 - num5));
						collision = new HitDetection.RayBoxCollision(zero, Vector3.Up, zero2);
						return true;
					}
				}
			}
			bool flag11 = direction.Z >= 0f;
			if (flag11)
			{
				float num7 = (num5 - position.Z) / direction.Z;
				bool flag12 = checkReverse || num7 >= 0f;
				if (flag12)
				{
					zero = new Vector3(position.X + num7 * direction.X, position.Y + num7 * direction.Y, position.Z + num7 * direction.Z);
					bool flag13 = zero.X >= num && zero.X <= num2 && zero.Y >= num3 && zero.Y <= num4;
					if (flag13)
					{
						zero2 = new Vector2((num2 - zero.X) / (num2 - num), 1f - (num4 - zero.Y) / (num4 - num3));
						collision = new HitDetection.RayBoxCollision(zero, Vector3.Forward, zero2);
						return true;
					}
				}
			}
			else
			{
				float num7 = (position.Z - num6) / -direction.Z;
				bool flag14 = checkReverse || num7 >= 0f;
				if (flag14)
				{
					zero = new Vector3(position.X + num7 * direction.X, position.Y + num7 * direction.Y, position.Z + num7 * direction.Z);
					bool flag15 = zero.X >= num && zero.X <= num2 && zero.Y >= num3 && zero.Y <= num4;
					if (flag15)
					{
						zero2 = new Vector2(1f - (num2 - zero.X) / (num2 - num), 1f - (num4 - zero.Y) / (num4 - num3));
						collision = new HitDetection.RayBoxCollision(zero, Vector3.Backward, zero2);
						return true;
					}
				}
			}
			collision = default(HitDetection.RayBoxCollision);
			return false;
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x000D5FC4 File Offset: 0x000D41C4
		private static void NextGridIntersection(Vector3 position, Vector3 direction, out HitDetection.BoxIntersection intersection)
		{
			double num3;
			double num2;
			double num = num2 = (num3 = 1.0);
			bool flag = direction.X > 0f;
			if (flag)
			{
				num2 = (Math.Floor((double)position.X) + 1.0 - (double)position.X) / (double)direction.X;
			}
			else
			{
				bool flag2 = direction.X < 0f;
				if (flag2)
				{
					num2 = (Math.Floor((double)(-(double)position.X)) + 1.0 + (double)position.X) / (double)(-(double)direction.X);
				}
			}
			bool flag3 = direction.Y > 0f;
			if (flag3)
			{
				num = (Math.Floor((double)position.Y) + 1.0 - (double)position.Y) / (double)direction.Y;
			}
			else
			{
				bool flag4 = direction.Y < 0f;
				if (flag4)
				{
					num = (Math.Floor((double)(-(double)position.Y)) + 1.0 + (double)position.Y) / (double)(-(double)direction.Y);
				}
			}
			bool flag5 = direction.Z > 0f;
			if (flag5)
			{
				num3 = (Math.Floor((double)position.Z) + 1.0 - (double)position.Z) / (double)direction.Z;
			}
			else
			{
				bool flag6 = direction.Z < 0f;
				if (flag6)
				{
					num3 = (Math.Floor((double)(-(double)position.Z)) + 1.0 + (double)position.Z) / (double)(-(double)direction.Z);
				}
			}
			double num4 = Math.Min(num2, Math.Min(num, num3));
			Vector3 zero = Vector3.Zero;
			bool flag7 = direction.X >= 0f && num4 == num2;
			if (flag7)
			{
				zero.X = -1f;
			}
			else
			{
				bool flag8 = direction.X < 0f && num4 == num2;
				if (flag8)
				{
					zero.X = 1f;
				}
				else
				{
					bool flag9 = direction.Y >= 0f && num4 == num;
					if (flag9)
					{
						zero.Y = -1f;
					}
					else
					{
						bool flag10 = direction.Y < 0f && num4 == num;
						if (flag10)
						{
							zero.Y = 1f;
						}
						else
						{
							bool flag11 = direction.Z >= 0f && num4 == num3;
							if (flag11)
							{
								zero.Z = -1f;
							}
							else
							{
								bool flag12 = direction.Z < 0f && num4 == num3;
								if (flag12)
								{
									zero.Z = 1f;
								}
							}
						}
					}
				}
			}
			Vector3 position2 = new Vector3((float)((double)position.X + num4 * (double)direction.X), (float)((double)position.Y + num4 * (double)direction.Y), (float)((double)position.Z + num4 * (double)direction.Z));
			intersection = new HitDetection.BoxIntersection(position2, zero);
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x000D62A0 File Offset: 0x000D44A0
		public bool CheckBlockCollision(BoundingBox box, Vector3 pos, Vector3 moveOffset, out HitDetection.CollisionHitData hitData)
		{
			int num = (int)Math.Floor((double)pos.X);
			int num2 = (int)Math.Floor((double)pos.Y);
			int num3 = (int)Math.Floor((double)pos.Z);
			int block = this._gameInstance.MapModule.GetBlock(num, num2, num3, 1);
			bool flag = block > 0;
			if (flag)
			{
				ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
				bool flag2 = clientBlockType.FillerX != 0 || clientBlockType.FillerY != 0 || clientBlockType.FillerZ != 0;
				if (flag2)
				{
					num -= clientBlockType.FillerX;
					num2 -= clientBlockType.FillerY;
					num3 -= clientBlockType.FillerZ;
					block = this._gameInstance.MapModule.GetBlock(num, num2, num3, 1);
					clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
				}
				BlockType.Material collisionMaterial = clientBlockType.CollisionMaterial;
				bool flag3 = collisionMaterial != null && collisionMaterial != 2;
				if (flag3)
				{
					BlockHitbox blockHitbox = this._gameInstance.ServerSettings.BlockHitboxes[clientBlockType.HitboxType];
					foreach (BoundingBox boxB in blockHitbox.Boxes)
					{
						bool flag4 = HitDetection.CheckBoxCollision(box, boxB, (float)num, (float)num2, (float)num3, moveOffset, out hitData);
						if (flag4)
						{
							return true;
						}
					}
				}
			}
			hitData = default(HitDetection.CollisionHitData);
			return false;
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x000D6410 File Offset: 0x000D4610
		public static bool CheckBoxCollision(BoundingBox boxA, BoundingBox boxB, float offsetX, float offsetY, float offsetZ, Vector3 moveOffset, out HitDetection.CollisionHitData hitData)
		{
			Vector3 zero = Vector3.Zero;
			Vector3 zero2 = Vector3.Zero;
			HitDetection.GetRangeOverlap(boxA.Min.X, boxA.Max.X, boxB.Min.X + offsetX, boxB.Max.X + offsetX, moveOffset.X, ref zero2.X, ref zero.X);
			HitDetection.GetRangeOverlap(boxA.Min.Y, boxA.Max.Y, boxB.Min.Y + offsetY, boxB.Max.Y + offsetY, moveOffset.Y, ref zero2.Y, ref zero.Y);
			HitDetection.GetRangeOverlap(boxA.Min.Z, boxA.Max.Z, boxB.Min.Z + offsetZ, boxB.Max.Z + offsetZ, moveOffset.Z, ref zero2.Z, ref zero.Z);
			hitData = new HitDetection.CollisionHitData(zero, zero2);
			return zero.X > 0f && zero.Y > 0f && zero.Z > 0f;
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x000D654C File Offset: 0x000D474C
		public static bool Get2DRayIntersection(Vector2 ray1Position, Vector2 ray1Direction, Vector2 ray2Position, Vector2 ray2Direction, out Vector2 intersection)
		{
			intersection = Vector2.Zero;
			float num = ray1Direction.Y / ray1Direction.X;
			float num2 = ray1Position.Y - num * ray1Position.X;
			float num3 = ray2Direction.Y / ray2Direction.X;
			float num4 = ray2Position.Y - num3 * ray2Position.X;
			bool flag = num - num3 == 0f;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				float num5 = (num4 - num2) / (num - num3);
				float y = num * num5 + num2;
				intersection = new Vector2(num5, y);
				result = true;
			}
			return result;
		}

		// Token: 0x06004375 RID: 17269 RVA: 0x000D65E0 File Offset: 0x000D47E0
		public static bool CheckRayPlaneIntersection(Vector3 planePoint, Vector3 planeNormal, Vector3 linePoint, Vector3 lineDirection, out Vector3 intersection, bool forwardOnly = false)
		{
			float num;
			bool flag = HitDetection.CheckRayPlaneDistance(planePoint, planeNormal, linePoint, lineDirection, out num);
			intersection = linePoint + lineDirection * num;
			return flag && (!forwardOnly || num > 0f);
		}

		// Token: 0x06004376 RID: 17270 RVA: 0x000D6628 File Offset: 0x000D4828
		public static bool CheckRayPlaneDistance(Vector3 planePoint, Vector3 planeNormal, Vector3 linePoint, Vector3 lineDirection, out float distance)
		{
			float num = Vector3.Dot(planeNormal, lineDirection);
			bool flag = num == 0f;
			bool result;
			if (flag)
			{
				distance = 0f;
				result = false;
			}
			else
			{
				distance = (Vector3.Dot(planeNormal, planePoint) - Vector3.Dot(planeNormal, linePoint)) / num;
				result = true;
			}
			return result;
		}

		// Token: 0x06004377 RID: 17271 RVA: 0x000D6670 File Offset: 0x000D4870
		public static float DistanceSquaredToPlanePointFromRayIntersection(Vector3 planePoint, Vector3 planeNormal, Vector3 rayPoint, Vector3 rayVector, out Vector3 intersection)
		{
			Vector3 vector = rayPoint - planePoint;
			float num = Vector3.Dot(vector, planeNormal);
			float num2 = Vector3.Dot(rayVector, planeNormal);
			bool flag = num2 == 0f;
			float result;
			if (flag)
			{
				intersection = Vector3.NaN;
				result = 0f;
			}
			else
			{
				float scaleFactor = num / num2;
				intersection = rayPoint - rayVector * scaleFactor;
				result = Vector3.DistanceSquared(intersection, planePoint);
			}
			return result;
		}

		// Token: 0x06004378 RID: 17272 RVA: 0x000D66E8 File Offset: 0x000D48E8
		private static void GetRangeOverlap(float minA, float maxA, float minB, float maxB, float offset, ref float limit, ref float overlap)
		{
			bool flag = (minA <= minB && maxA >= maxB) || (minA > minB && maxA < maxB) || (minA < maxB && minA > minB) || (maxA > minB && maxA < maxB);
			if (flag)
			{
				overlap = ((offset > 0f) ? (maxA - minB) : (maxB - minA));
				limit = ((offset > 0f) ? minB : maxB);
			}
		}

		// Token: 0x04002139 RID: 8505
		private const int MaxRaycastIterations = 5000;

		// Token: 0x0400213A RID: 8506
		public const float DefaultRaycastDistance = 128f;

		// Token: 0x0400213B RID: 8507
		private const int OversizeRaycastDistance = 1;

		// Token: 0x0400213C RID: 8508
		private static HitDetection.RaycastOptions DefaultRaycastOptions = new HitDetection.RaycastOptions();

		// Token: 0x0400213D RID: 8509
		private readonly GameInstance _gameInstance;

		// Token: 0x02000DAA RID: 3498
		public enum CollisionAxis
		{
			// Token: 0x04004358 RID: 17240
			X,
			// Token: 0x04004359 RID: 17241
			Y,
			// Token: 0x0400435A RID: 17242
			Z
		}

		// Token: 0x02000DAB RID: 3499
		public struct EntityHitData
		{
			// Token: 0x1700142C RID: 5164
			// (get) Token: 0x060065BE RID: 26046 RVA: 0x00212390 File Offset: 0x00210590
			// (set) Token: 0x060065BF RID: 26047 RVA: 0x00212398 File Offset: 0x00210598
			public Entity Entity { get; private set; }

			// Token: 0x1700142D RID: 5165
			// (get) Token: 0x060065C0 RID: 26048 RVA: 0x002123A1 File Offset: 0x002105A1
			// (set) Token: 0x060065C1 RID: 26049 RVA: 0x002123A9 File Offset: 0x002105A9
			public HitDetection.RayBoxCollision RayBoxCollision { get; private set; }

			// Token: 0x1700142E RID: 5166
			// (get) Token: 0x060065C2 RID: 26050 RVA: 0x002123B2 File Offset: 0x002105B2
			// (set) Token: 0x060065C3 RID: 26051 RVA: 0x002123BA File Offset: 0x002105BA
			public float ClosestDistance { get; private set; }

			// Token: 0x060065C4 RID: 26052 RVA: 0x002123C3 File Offset: 0x002105C3
			public EntityHitData(Entity entity, HitDetection.RayBoxCollision rayBoxCollision, float closestDistance)
			{
				this.Entity = entity;
				this.RayBoxCollision = rayBoxCollision;
				this.ClosestDistance = closestDistance;
			}
		}

		// Token: 0x02000DAC RID: 3500
		public struct CollisionHitData
		{
			// Token: 0x1700142F RID: 5167
			// (get) Token: 0x060065C5 RID: 26053 RVA: 0x002123DE File Offset: 0x002105DE
			// (set) Token: 0x060065C6 RID: 26054 RVA: 0x002123E6 File Offset: 0x002105E6
			public Vector3 Overlap { get; private set; }

			// Token: 0x17001430 RID: 5168
			// (get) Token: 0x060065C7 RID: 26055 RVA: 0x002123EF File Offset: 0x002105EF
			// (set) Token: 0x060065C8 RID: 26056 RVA: 0x002123F7 File Offset: 0x002105F7
			public Vector3 Limit { get; private set; }

			// Token: 0x060065C9 RID: 26057 RVA: 0x00212400 File Offset: 0x00210600
			public CollisionHitData(Vector3 overlap, Vector3 limit)
			{
				this.Overlap = overlap;
				this.Limit = limit;
				this.HitEntity = null;
			}

			// Token: 0x060065CA RID: 26058 RVA: 0x00212420 File Offset: 0x00210620
			public bool GetXCollideState()
			{
				return this.Overlap.X > 0f;
			}

			// Token: 0x060065CB RID: 26059 RVA: 0x00212444 File Offset: 0x00210644
			public bool GetYCollideState()
			{
				return this.Overlap.Y > 0f;
			}

			// Token: 0x060065CC RID: 26060 RVA: 0x00212468 File Offset: 0x00210668
			public bool GetZCollideState()
			{
				return this.Overlap.Z > 0f;
			}

			// Token: 0x060065CD RID: 26061 RVA: 0x0021248C File Offset: 0x0021068C
			public bool HasCollided()
			{
				return this.Overlap.X > 0f && this.Overlap.Y > 0f && this.Overlap.Z > 0f;
			}

			// Token: 0x060065CE RID: 26062 RVA: 0x002124D8 File Offset: 0x002106D8
			public override string ToString()
			{
				return string.Concat(new string[]
				{
					"{Overlap: ",
					this.Overlap.ToString(),
					", Limit: ",
					this.Limit.ToString(),
					"}"
				});
			}

			// Token: 0x04004360 RID: 17248
			public int? HitEntity;
		}

		// Token: 0x02000DAD RID: 3501
		public struct BoxIntersection
		{
			// Token: 0x17001431 RID: 5169
			// (get) Token: 0x060065CF RID: 26063 RVA: 0x0021253B File Offset: 0x0021073B
			// (set) Token: 0x060065D0 RID: 26064 RVA: 0x00212543 File Offset: 0x00210743
			public Vector3 Position { get; private set; }

			// Token: 0x17001432 RID: 5170
			// (get) Token: 0x060065D1 RID: 26065 RVA: 0x0021254C File Offset: 0x0021074C
			// (set) Token: 0x060065D2 RID: 26066 RVA: 0x00212554 File Offset: 0x00210754
			public Vector3 Normal { get; private set; }

			// Token: 0x060065D3 RID: 26067 RVA: 0x0021255D File Offset: 0x0021075D
			public BoxIntersection(Vector3 position, Vector3 normal)
			{
				this.Position = position;
				this.Normal = normal;
			}
		}

		// Token: 0x02000DAE RID: 3502
		public struct RayBoxCollision
		{
			// Token: 0x17001433 RID: 5171
			// (get) Token: 0x060065D4 RID: 26068 RVA: 0x00212570 File Offset: 0x00210770
			// (set) Token: 0x060065D5 RID: 26069 RVA: 0x00212578 File Offset: 0x00210778
			public Vector3 Position { get; private set; }

			// Token: 0x17001434 RID: 5172
			// (get) Token: 0x060065D6 RID: 26070 RVA: 0x00212581 File Offset: 0x00210781
			// (set) Token: 0x060065D7 RID: 26071 RVA: 0x00212589 File Offset: 0x00210789
			public Vector3 Normal { get; private set; }

			// Token: 0x17001435 RID: 5173
			// (get) Token: 0x060065D8 RID: 26072 RVA: 0x00212592 File Offset: 0x00210792
			// (set) Token: 0x060065D9 RID: 26073 RVA: 0x0021259A File Offset: 0x0021079A
			public Vector2 TextureCoord { get; private set; }

			// Token: 0x060065DA RID: 26074 RVA: 0x002125A3 File Offset: 0x002107A3
			public RayBoxCollision(Vector3 position, Vector3 normal, Vector2 textureCoord)
			{
				this.Position = position;
				this.Normal = normal;
				this.TextureCoord = textureCoord;
			}
		}

		// Token: 0x02000DAF RID: 3503
		public class RaycastOptions
		{
			// Token: 0x04004366 RID: 17254
			public float Distance = 128f;

			// Token: 0x04004367 RID: 17255
			public bool IgnoreFluids = false;

			// Token: 0x04004368 RID: 17256
			public bool IgnoreEmptyCollisionMaterial = false;

			// Token: 0x04004369 RID: 17257
			public bool CheckOversizedBoxes = false;

			// Token: 0x0400436A RID: 17258
			public bool CheckOnlyTangibleEntities = true;

			// Token: 0x0400436B RID: 17259
			public bool ReturnEndpointBlock = false;

			// Token: 0x0400436C RID: 17260
			public int RequiredBlockTag = int.MinValue;
		}

		// Token: 0x02000DB0 RID: 3504
		public struct RaycastHit
		{
			// Token: 0x17001436 RID: 5174
			// (get) Token: 0x060065DC RID: 26076 RVA: 0x0021260D File Offset: 0x0021080D
			// (set) Token: 0x060065DD RID: 26077 RVA: 0x00212615 File Offset: 0x00210815
			public Vector3 BlockPosition { get; private set; }

			// Token: 0x17001437 RID: 5175
			// (get) Token: 0x060065DE RID: 26078 RVA: 0x0021261E File Offset: 0x0021081E
			// (set) Token: 0x060065DF RID: 26079 RVA: 0x00212626 File Offset: 0x00210826
			public Vector3 BlockPositionNoFiller { get; private set; }

			// Token: 0x17001438 RID: 5176
			// (get) Token: 0x060065E0 RID: 26080 RVA: 0x0021262F File Offset: 0x0021082F
			// (set) Token: 0x060065E1 RID: 26081 RVA: 0x00212637 File Offset: 0x00210837
			public Vector3 BlockOrigin { get; private set; }

			// Token: 0x17001439 RID: 5177
			// (get) Token: 0x060065E2 RID: 26082 RVA: 0x00212640 File Offset: 0x00210840
			// (set) Token: 0x060065E3 RID: 26083 RVA: 0x00212648 File Offset: 0x00210848
			public Vector3 StartPosition { get; private set; }

			// Token: 0x1700143A RID: 5178
			// (get) Token: 0x060065E4 RID: 26084 RVA: 0x00212651 File Offset: 0x00210851
			// (set) Token: 0x060065E5 RID: 26085 RVA: 0x00212659 File Offset: 0x00210859
			public Vector3 HitPosition { get; private set; }

			// Token: 0x1700143B RID: 5179
			// (get) Token: 0x060065E6 RID: 26086 RVA: 0x00212662 File Offset: 0x00210862
			// (set) Token: 0x060065E7 RID: 26087 RVA: 0x0021266A File Offset: 0x0021086A
			public Vector3 Normal { get; private set; }

			// Token: 0x1700143C RID: 5180
			// (get) Token: 0x060065E8 RID: 26088 RVA: 0x00212673 File Offset: 0x00210873
			// (set) Token: 0x060065E9 RID: 26089 RVA: 0x0021267B File Offset: 0x0021087B
			public Vector3 BlockNormal { get; private set; }

			// Token: 0x1700143D RID: 5181
			// (get) Token: 0x060065EA RID: 26090 RVA: 0x00212684 File Offset: 0x00210884
			// (set) Token: 0x060065EB RID: 26091 RVA: 0x0021268C File Offset: 0x0021088C
			public Vector2 TextureCoord { get; private set; }

			// Token: 0x1700143E RID: 5182
			// (get) Token: 0x060065EC RID: 26092 RVA: 0x00212695 File Offset: 0x00210895
			// (set) Token: 0x060065ED RID: 26093 RVA: 0x0021269D File Offset: 0x0021089D
			public float Distance { get; private set; }

			// Token: 0x1700143F RID: 5183
			// (get) Token: 0x060065EE RID: 26094 RVA: 0x002126A6 File Offset: 0x002108A6
			// (set) Token: 0x060065EF RID: 26095 RVA: 0x002126AE File Offset: 0x002108AE
			public int BlockId { get; private set; }

			// Token: 0x17001440 RID: 5184
			// (get) Token: 0x060065F0 RID: 26096 RVA: 0x002126B7 File Offset: 0x002108B7
			// (set) Token: 0x060065F1 RID: 26097 RVA: 0x002126BF File Offset: 0x002108BF
			public int BlockHitboxId { get; private set; }

			// Token: 0x17001441 RID: 5185
			// (get) Token: 0x060065F2 RID: 26098 RVA: 0x002126C8 File Offset: 0x002108C8
			// (set) Token: 0x060065F3 RID: 26099 RVA: 0x002126D0 File Offset: 0x002108D0
			public int BoxId { get; private set; }

			// Token: 0x060065F4 RID: 26100 RVA: 0x002126DC File Offset: 0x002108DC
			public RaycastHit(Vector3 blockPosition, Vector3 blockPositionNoFiller, Vector3 blockOrigin, Vector3 startPosition, Vector3 hitPosition, Vector3 normal, Vector3 blockNormal, Vector2 textureCoord, float distance, int blockId, int blockHitboxId, int boxId)
			{
				this.BlockPosition = blockPosition;
				this.BlockPositionNoFiller = blockPositionNoFiller;
				this.BlockOrigin = blockOrigin;
				this.StartPosition = startPosition;
				this.HitPosition = hitPosition;
				this.Normal = normal;
				this.BlockNormal = blockNormal;
				this.TextureCoord = textureCoord;
				this.Distance = distance;
				this.BlockId = blockId;
				this.BlockHitboxId = blockHitboxId;
				this.BoxId = boxId;
			}
		}
	}
}
