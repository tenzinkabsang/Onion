using System.Collections.Generic;

namespace SportsStore.Core.Contracts
{
    public interface ICategoryCore
    {
        IList<string> GetCategoryNames();
    }
}