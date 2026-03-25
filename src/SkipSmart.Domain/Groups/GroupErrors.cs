using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Domain.Groups;

public static class GroupErrors {
    public static Error NotFound = new(
        "Group.NotFound",
        "The group with the specified identifier was not found");
}