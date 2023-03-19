using System.Collections.Generic;
using CrabCrave.Core;

namespace CrabCrave.Core.SearchAlgorithm
{
    public abstract class SearchAlgorithm<T>
    {
        //private Map<T> map;
        private List<T> path;

        public SearchAlgorithm(Map m)
        {
            //map = m;
            //path = new List<T>();
        }

        public abstract List<T> Search();
    }
}
