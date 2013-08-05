using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BluEngine.Engine.GameObjects;

namespace BluEngine.Engine
{
    public class CollisionType
    {
        private short id;

        private List<short> collidingIDs = new List<short>();

        public short ID
        {
            get { return id; }
            set { id = value; }
        }

        public List<short> CollidingIDs
        {
            get { return collidingIDs; }
        }

        public CollisionType(short id)
        {
            this.id = id;

            if (!CollisionSimulator.CollisionTypes.ContainsKey(id))
                CollisionSimulator.CollisionTypes.Add(id, this);

        }
    }

    public static class CollisionSimulator
    {
        #region fields

        private static Dictionary<short, CollisionType> collisionTypes = new Dictionary<short, CollisionType>();
        private static Dictionary<short, List<CollisionBoxComponent>> collisionLists = new Dictionary<short, List<CollisionBoxComponent>>();

        #endregion

        #region Properties

        public static Dictionary<short, CollisionType> CollisionTypes
        {
            get { return collisionTypes; }
        }

        #endregion

        public static CollisionBoxComponent CheckForCollision(CollisionBoxComponent entity)
        {
            foreach (short colType in entity.CollisionType.CollidingIDs)
            {
                CollisionBoxComponent colbox = CheckForSpecificCollision(entity, colType);
                if (colbox != null)
                    return colbox;
            }

            return null;
        }

        public static CollisionBoxComponent CheckForSpecificCollision(CollisionBoxComponent entity, short collisionID)
        {
            foreach (CollisionBoxComponent colBox in collisionLists[collisionID])
            {
                if(entity.Intersects(colBox))
                    return colBox;
            }

            return null;
        }
    }
}
