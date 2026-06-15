using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Groups;

namespace SkipSmart.Application.Groups.CreateGroup;

internal sealed class CreateGroupCommandHandler : ICommandHandler<CreateGroupCommand, Guid> {
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGroupCommandHandler(IGroupRepository groupRepository, IUnitOfWork unitOfWork) {
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateGroupCommand request, CancellationToken cancellationToken) {
        var group = new Group(Guid.NewGuid(), request.GroupName, request.EdupageClassId);

        _groupRepository.Add(group);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return group.Id;
    }
}
