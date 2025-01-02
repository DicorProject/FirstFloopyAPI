using Floppy.Domain.Entities;

namespace Floppy.Domain.Interfaces
{
    public interface IMetaTagRepository
    {
        Task<Metatag> GetMetagDetailsBypageName(string pagename);
        Task<List<Metatag>> FetchListOfMetatags();
    }
}
