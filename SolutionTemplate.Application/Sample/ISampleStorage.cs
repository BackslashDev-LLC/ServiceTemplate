using SolutionTemplate.Domain.Sample;

namespace SolutionTemplate.Application.Sample
{
    public interface ISampleStorage
    {
        Task<List<Item>> GetValues(CancellationToken cancellationToken = default);
    }
}
