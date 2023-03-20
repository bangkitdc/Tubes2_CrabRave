using System.Collections.Generic;

namespace CrabCrave.Core.SearchAlgorithm
{
    public abstract class SearchAlgorithm<T>
    {
        protected Map map;
        protected List<T> path;

        public SearchAlgorithm(Map m)
        {
            map = m;
            path = new List<T>();
        }

        public abstract List<T> Search();
    }
}
