using DynamicTree.SharedKernel.Interfaces;

namespace DynamicTree.Application.Services;

public class UtcDateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.UtcNow;
}