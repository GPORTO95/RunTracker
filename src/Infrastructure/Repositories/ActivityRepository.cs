using Infrastructure.Data;
using Modules.Training.Domain.Activies;

namespace Infrastructure.Repositories;

internal sealed class ActivityRepository(ApplicationDbContext context) : IActivityRepository
{
    public void Insert(Activity activity)
    {
        context.Activities.Add(activity);
    }
}
