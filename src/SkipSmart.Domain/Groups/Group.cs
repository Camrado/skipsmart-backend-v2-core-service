using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Domain.Groups;

public class Group : Entity {
    public string GroupName { get; private set; }
    public int EdupageClassId { get; private set; }
    
    public Group(Guid id, string groupName, int edupageClassId) : base(id) {
        GroupName = groupName;
        EdupageClassId = edupageClassId;
    }
    
    private Group() {
    }

    public void Update(string groupName, int edupageClassId) {
        GroupName = groupName;
        EdupageClassId = edupageClassId;
    }
}