using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Domain.Groups;

public class Group : Entity {
    public GroupName GroupName { get; private set; }
    public int EdupageClassId { get; private set; }
    
    public Group(Guid id, GroupName groupName, int edupageClassId) : base(id) {
        GroupName = groupName;
        EdupageClassId = edupageClassId;
    }
}