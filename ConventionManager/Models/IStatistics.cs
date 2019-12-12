using System;
using System.Collections.Generic;

namespace ConventionManager.Models
{
    public interface IStatistics<T, E>
    {
        E GetRating(List<T> feedbacks);
    }
}
