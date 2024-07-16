﻿using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Users.ChangeGroup;

public sealed record ChangeGroupCommand(Guid NewGroupId) : ICommand<Result>;