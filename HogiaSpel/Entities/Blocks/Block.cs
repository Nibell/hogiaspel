﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HogiaSpel.Enums;
using HogiaSpel.CollisionDetection;
using HogiaSpel.Extensions;

namespace HogiaSpel.Entities.Blocks
{
    public class Block : AbstractEntity, IBlock
    {
        public override void Initialize(Vector2 position)
        {
            Id = Guid.NewGuid();
            Active = true;
            Speed = 0;
            BaseSpeed = 0;
            TopSpeed = 0;
            Acceleration = 0;
            CurrentAccelerationDirection = DirectionEnum.NoDirection;

            var sprites = Sprites.Instance;
            SpriteHandler = new SpriteHandler(position);
            SpriteHandler.InitializeAnimation(SpriteKeys.Block.Stand, sprites.GetSprite(SpriteKeys.Block.Stand), 64, 64, 1, 80, Color.White, 1f, true);
            SpriteHandler.Initialize(SpriteKeys.Block.Stand);

            var grid = CollisionGrid.Instance;
            CollisionCellPositions = grid.UpdateCellPosition(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteHandler.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            var grid = CollisionGrid.Instance;
            CollisionCellPositions = grid.UpdateCellPosition(this);

            SpriteHandler.Update(gameTime);
        }

        public override void CheckCollision(GameTime gameTime)
        {
            var grid = CollisionGrid.Instance;
            foreach (var entity in grid.GetEntitiesWithinCell(CollisionCellPositions))
            {
                if (Id != entity.Id)
                {
                    if (entity is PlayerAvatar)
                    {
                        if (Rectangle.Intersects(entity.Rectangle))
                        {
                            var collisionDepth = Rectangle.GetIntersectionDirection(entity.Rectangle);
                            MoveRight(collisionDepth.X);
                        }
                    }
                }
            }
        }
    }
}
