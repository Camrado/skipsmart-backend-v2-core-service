using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Users.ChangeSubgroups;

public sealed record ChangeSubgroupsCommand(int NewLanguageSubgroup, int NewFacultySubgroup) : ICommand<Result>;