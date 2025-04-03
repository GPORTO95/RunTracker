using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Workouts.Remove;

internal sealed class WorkoutRemovedIntegrationEventHandler : INotificationHandler<WorkoutRemovedIntegrationEvent>
{
    public async Task Handle(WorkoutRemovedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        await Task.Delay(5000, cancellationToken);
    }
}
