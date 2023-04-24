using AutoMapper;
using AutoMapper.QueryableExtensions;
using DynamicTree.Application.Features.User.Journal.Models;
using DynamicTree.Persistence;
using DynamicTree.SharedKernel.Extensions;
using DynamicTree.SharedKernel.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DynamicTree.Application.Features.User.Journal;

public class GetRangeHandler : IRequestHandler<GetRangeRequest, PagedList<JournalViewInfo>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private readonly IMapper _mapper;

    public GetRangeHandler(IDbContextFactory<ApplicationDbContext> dbContextFactory, IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<PagedList<JournalViewInfo>> Handle(GetRangeRequest request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await db.Set<Domain.Entities.Journal>()
            .ApplyQuery(_mapper.Map<GetRangeQuery>(request))
            .ProjectTo<JournalViewInfo>(_mapper.ConfigurationProvider)
            .ApplyPagingAsync(request.Skip, request.Take, cancellationToken);
    }
}