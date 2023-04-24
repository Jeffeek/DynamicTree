namespace DynamicTree.Domain.Interfaces;

public interface IIdentityEntity<TKeyType> where TKeyType: struct
{
    public TKeyType Id { get; set; }
}