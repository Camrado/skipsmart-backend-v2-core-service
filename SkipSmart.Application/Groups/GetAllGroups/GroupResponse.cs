namespace SkipSmart.Application.Groups.GetAllGroups;

public sealed class GroupResponse {
    public Guid Id { get; set; }
    
    public string GroupName { get; set; }
    
    public int EdupageClassId { get; set; }
}