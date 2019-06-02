using BTC.Core.Base.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BTC.Business.Managers
{
    public sealed class MemoryCacheManager : BaseMemoryCacheManger
    {
        private static readonly MemoryCacheManager instance = new MemoryCacheManager();
        static MemoryCacheManager()
        {
        }
        private MemoryCacheManager()
        {
        }
        public static MemoryCacheManager Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
