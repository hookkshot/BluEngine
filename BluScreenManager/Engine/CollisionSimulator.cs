using System.Collections.Generic;

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

            if (!CollisionSimulator.CollisionLists.ContainsKey(id))
            {
                CollisionSimulator.CollisionLists.Add(id, new List<CollisionBoxComponent>());
            }


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

        public static Dictionary<short, List<CollisionBoxComponent>> CollisionLists
        {
            get { return collisionLists; }
        }

        #endregion

        public static CollisionBoxComponent CheckForCollision(CollisionBoxComponent entity)
        {
            return CheckForCollision(entity, null);
        }

        public static CollisionBoxComponent CheckForSpecificCollision(CollisionBoxComponent entity, short collisionID)
        {
            return CheckForSpecificCollision(entity, collisionID, null);
        }

        public static CollisionBoxComponent CheckForCollision(CollisionBoxComponent entity, CollisionBoxComponent ignore)
        {
            foreach (short colType in entity.CollisionType.CollidingIDs)
            {
                CollisionBoxComponent colbox = CheckForSpecificCollision(entity, colType, ignore);
                if (colbox != null)
                    return colbox;
            }

            return null;
        }

        public static CollisionBoxComponent CheckForSpecificCollision(CollisionBoxComponent entity, short collisionID, CollisionBoxComponent ignore)
        {
            foreach (CollisionBoxComponent colBox in collisionLists[collisionID])
            {
                if (entity.Intersects(colBox) && colBox != ignore && colBox != entity)
                    return colBox;
            }

            return null;
        }
    }
}
