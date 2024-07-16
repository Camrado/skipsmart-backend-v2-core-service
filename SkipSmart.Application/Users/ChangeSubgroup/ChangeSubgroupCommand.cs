using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Users.ChangeSubgroup;

public sealed record ChangeSubgroupCommand(int NewSubgroup) : ICommand<Result>;