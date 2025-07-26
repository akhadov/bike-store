using Application.Abstractions.Messaging;

namespace Application.Staffs.SyncStaffs;

public sealed record SyncStaffsCommand(string FilePath) : ICommand;
