#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace LBPUnion.ProjectLighthouse.Types.Activity;

public static class GroupListExtensions
{
    public static LevelGroup GetOrCreateLevelGroup(this List<LevelGroup> levelGroups, int slotId)
    {
        LevelGroup? levelGroup = levelGroups.FirstOrDefault(l => l.SlotId == slotId);
        if (levelGroup == null)
        {
            levelGroup = new LevelGroup
            {
                SlotId = slotId,
                UserGroups = new List<UserGroup>(),
            };

            levelGroups.Add(levelGroup);
        }

        return levelGroup;
    }

    public static UserGroup GetOrCreateUserGroup(this List<UserGroup> userGroups, User user)
    {
        UserGroup? userGroup = userGroups.FirstOrDefault(u => u.User.UserId == user.UserId);
        if (userGroup == null)
        {
            userGroup = new UserGroup
            {
                User = user,
                Events = new List<IEvent>(),
            };

            userGroups.Add(userGroup);
        }

        return userGroup;
    }

    public static UserGroup GetOrCreateUserGroup(this LevelGroup levelGroup, User user)
    {
        UserGroup? userGroup = levelGroup.UserGroups.FirstOrDefault(u => u.User == user);
        if (userGroup == null)
        {
            userGroup = new UserGroup
            {
                User = user,
                Events = new List<IEvent>(),
            };

            levelGroup.UserGroups.Add(userGroup);
        }

        return userGroup;
    }
}