using Application.Abstractions.Messaging;

namespace Application.Bronze.Staffs.SyncStaffs;

public sealed record SyncStaffsCommand(string FilePath) : ICommand;
