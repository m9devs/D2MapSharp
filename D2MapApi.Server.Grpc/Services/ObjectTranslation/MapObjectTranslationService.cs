using D2MapApi.Common.DataStructures;
using D2MapApi.Common.Enumerations.GameData;
using D2MapApi.Server.Grpc.Protos.Api.V1.Map;

using Microsoft.Extensions.Logging;

namespace D2MapApi.Server.Grpc.Services.ObjectTranslation;

internal class MapObjectTranslationService(ILogger<MapObjectTranslationService> i_logger)
{
    internal D2AreaMap ToD2AreaMap(G_D2AreaMap p_areaMapMessage)
    {
        var newMap = new D2AreaMap
                           {
                               Area = (D2Area)p_areaMapMessage.Area,
                               LevelOrigin = ToPoint2D(p_areaMapMessage.LevelOrigin),
                               Width = p_areaMapMessage.Width,
                               Height = p_areaMapMessage.Height,
                               CollisionData = ToCollisionData(p_areaMapMessage.CollisionData),
                               TombArea    = (D2Area)p_areaMapMessage.TombArea
                           };
        
        newMap.Npcs.AddRange(p_areaMapMessage.Npcs.Select(ToNpcData));
        newMap.Objects.AddRange(p_areaMapMessage.Objects.Select(ToObjectData));
        newMap.AccessibleAreas.AddRange(p_areaMapMessage.AccessibleAreas.Select(ToAccessibleArea));

        return newMap;
    }

    private Point2D ToPoint2D(G_D2AreaMap.Types.G_Point2D p_point2DMessage)
    {
        return new Point2D(p_point2DMessage.XPosition, p_point2DMessage.YPosition);
    }
    
    private CollisionData ToCollisionData(G_D2AreaMap.Types.G_CollisionData p_collisionDataMessage)
    {
        var newCollisionData = new CollisionData(p_collisionDataMessage.Width, p_collisionDataMessage.Height);
        
        for (var i = 0; i < p_collisionDataMessage.CollisionRows.Count; i++)
        {
            for (var j = 0; j < p_collisionDataMessage.CollisionRows[i].Blocks.Count; j++)
            {
                var blockId = p_collisionDataMessage.CollisionRows[i].Blocks[j];
                newCollisionData.Blocks[i,j] = (CollisionBlock)blockId;
            }
        }
        
        return newCollisionData;
    }

    private (D2Npc, D2NpcData) ToNpcData(G_D2AreaMap.Types.G_Npc p_npcMessage)
    {
        var npcId = (D2Npc)p_npcMessage.NpcId;
        return ( npcId, new D2NpcData(npcId, ToPoint2D(p_npcMessage.NpcPosition)) );
    }
    
    private (D2Object, D2ObjectData) ToObjectData(G_D2AreaMap.Types.G_Object p_objectMessage)
    {
        var objectId = (D2Object)p_objectMessage.ObjectId;
        return ( objectId, new D2ObjectData(objectId, p_objectMessage.Width, p_objectMessage.Height, p_objectMessage.HasCollision) );
    }

    private (D2Area, Point2D) ToAccessibleArea(G_D2AreaMap.Types.G_AccessibleArea p_accessibleAreaMessage)
    {
        var areaId = (D2Area)p_accessibleAreaMessage.AreaId;
        return ( areaId, ToPoint2D(p_accessibleAreaMessage.ExitPosition) );
    }

    internal G_D2AreaMap ToD2AreaMapMessage(D2AreaMap p_areaMap)
    {
        var newD2AreaMapMessage = new G_D2AreaMap
                                  {
                                      Area = (int)p_areaMap.Area,
                                      LevelOrigin = ToPoint2DMessage(p_areaMap.LevelOrigin),
                                      Width = p_areaMap.Width,
                                      Height = p_areaMap.Height,
                                      CollisionData = ToCollisionDataMessage(p_areaMap.CollisionData),
                                      TombArea = (int)p_areaMap.TombArea
                                  };
        
        newD2AreaMapMessage.Npcs.AddRange(p_areaMap.Npcs.Select(ToNpcMessage));
        newD2AreaMapMessage.Objects.AddRange(p_areaMap.Objects.Select(ToObjectMessage));
        newD2AreaMapMessage.AccessibleAreas.AddRange(p_areaMap.AccessibleAreas.Select(ToAccessibleAreaMessage));
        
        return newD2AreaMapMessage;
    }

    private G_D2AreaMap.Types.G_Point2D ToPoint2DMessage(Point2D p_point2D)
    {
        return new G_D2AreaMap.Types.G_Point2D
               {
                   XPosition = p_point2D.X,
                   YPosition = p_point2D.Y
               };
    }

    private G_D2AreaMap.Types.G_CollisionData ToCollisionDataMessage(CollisionData p_collisionData)
    {
        var collisionDataMessage = new G_D2AreaMap.Types.G_CollisionData
                                   {
                                       Width  = p_collisionData.Width,
                                       Height = p_collisionData.Height
                                   };

        for (var i = 0; i < p_collisionData.Height; i++)
        {
            var newRow = new G_D2AreaMap.Types.G_CollisionRow();
            
            for (var j = 0; j < p_collisionData.Width; j++)
            {
                newRow.Blocks.Add((int)p_collisionData.Blocks[j, i]);
            }
            collisionDataMessage.CollisionRows.Add(newRow);
        }

        return collisionDataMessage;
    }

    private G_D2AreaMap.Types.G_AccessibleArea ToAccessibleAreaMessage((D2Area Area, Point2D ExitPosition) p_accessibleArea)
    {
        return new G_D2AreaMap.Types.G_AccessibleArea
               {
                   AreaId       = (int)p_accessibleArea.Area,
                   ExitPosition = ToPoint2DMessage(p_accessibleArea.ExitPosition)
               };
    }

    private G_D2AreaMap.Types.G_Npc ToNpcMessage((D2Npc NpcId, D2NpcData NpcData) p_npcData)
    {
        return new G_D2AreaMap.Types.G_Npc
               {
                   NpcId       = (int)p_npcData.NpcId,
                   NpcPosition = ToPoint2DMessage(p_npcData.NpcData.Position)
               };
    }

    private G_D2AreaMap.Types.G_Object ToObjectMessage((D2Object ObjectId, D2ObjectData ObjectData) p_objectData)
    {
        return new G_D2AreaMap.Types.G_Object
               {
                   ObjectId     = (int)p_objectData.ObjectId,
                   Width        = p_objectData.ObjectData.Width,
                   Height       = p_objectData.ObjectData.Height,
                   HasCollision = p_objectData.ObjectData.HasCollision,
                   Position = ToPoint2DMessage(p_objectData.ObjectData.Position)
               };
    }
}