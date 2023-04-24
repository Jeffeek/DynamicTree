using AutoMapper;
using AutoMapper.EquivalencyExpression;
using DynamicTree.Application.Features.User.Journal;
using DynamicTree.Application.Features.User.Journal.Models;
using DynamicTree.Application.Features.User.Tree.Models;
using DynamicTree.Application.Features.User.Tree.Node;
using DynamicTree.Domain.Entities;

namespace DynamicTree.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TreeNode, TreeNodeInfo>();

        CreateMap<CreateRequest, TreeNode>()
            .EqualityComparison((src, dest) => dest.Id == 0 && dest.ParentNodeId == src.ParentNodeId)
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NodeName));

        CreateMap<RenameRequest, TreeNode>()
            .EqualityComparison((src, dest) => dest.Id == src.NodeId)
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NewNodeName));

        CreateMap<DeleteRequest, TreeNode>()
            .EqualityComparison((src, dest) => dest.Id == src.NodeId);

        CreateMap<Features.User.Tree.GetRequest, TreeNode>()
            .EqualityComparison((src, dest) => dest.Id == 0);

        CreateProjection<Journal, JournalViewInfo>();

        CreateProjection<Journal, JournalInfo>();

        CreateMap<GetRangeRequest, GetRangeQuery>();

        CreateMap<SharedKernel.Features.User.Journal.CreateRequest, Journal>()
            .EqualityComparison((src, dest) => dest.Id == 0);
    }
}