using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Groups;

namespace SkipSmart.Application.Groups.DeleteGroup;

internal sealed class DeleteGroupCommandHandler : ICommandHandler<DeleteGroupCommand> {
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGroupCommandHandler(IGroupRepository groupRepository, IUnitOfWork unitOfWork) {
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteGroupCommand request, CancellationToken cancellationToken) {
        var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);

        if (group is null) {
            return Result.Failure(new Error("Group.NotFound", $"The group with Id {request.GroupId} was not found"));
        }

        _groupRepository.Remove(group);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
