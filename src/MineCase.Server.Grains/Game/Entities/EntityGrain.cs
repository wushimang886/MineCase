﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MineCase.Engine;
using MineCase.Server.Components;
using MineCase.Server.Game.Entities.Components;
using MineCase.Server.Network.Play;
using MineCase.Server.Persistence;
using MineCase.Server.Persistence.Components;
using MineCase.Server.World;
using MineCase.World;
using Orleans;
using Orleans.Concurrency;

namespace MineCase.Server.Game.Entities
{
    [PersistTableName("entity")]
    [Reentrant]
    internal abstract class EntityGrain : PersistableDependencyObject, IEntity
    {
        public Guid UUID => this.GetPrimaryKey();

        public uint EntityId => GetValue(EntityIdComponent.EntityIdProperty);

        public EntityWorldPos Position => GetValue(EntityWorldPositionComponent.EntityWorldPositionProperty);

        public IWorld World => GetValue(WorldComponent.WorldProperty);

        public float Pitch => GetValue(EntityLookComponent.PitchProperty);

        public float Yaw => GetValue(EntityLookComponent.YawProperty);

        protected override async Task InitializeComponents()
        {
            await SetComponent(new IsEnabledComponent());
            await SetComponent(new EntityIdComponent());
            await SetComponent(new WorldComponent());
            await SetComponent(new EntityWorldPositionComponent());
            await SetComponent(new EntityLookComponent());
            await SetComponent(new AddressByPartitionKeyComponent());
            await SetComponent(new ChunkEventBroadcastComponent());
            await SetComponent(new GameTickComponent());
            await SetComponent(new ChunkAccessorComponent());
            await SetComponent(new AutoSaveStateComponent(AutoSaveStateComponent.PerMinute));
        }

        Task<uint> IEntity.GetEntityId() =>
            Task.FromResult(EntityId);

        Task<EntityWorldPos> IEntity.GetPosition() =>
            Task.FromResult(Position);

        Task<IWorld> IEntity.GetWorld() =>
            Task.FromResult(World);

        Task<float> IEntity.GetYaw() =>
            Task.FromResult(Yaw);
    }
}
