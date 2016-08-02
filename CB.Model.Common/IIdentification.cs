namespace CB.Model.Common
{
    public interface IIdentification<TId>
    {
        TId Id { get; set; }
    }
}