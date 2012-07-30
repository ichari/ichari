using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model.Admin;

namespace Ichari.IService
{
    public interface IActionsService : IService<Actions>
    {
        /// <summary>
        /// 递归获取功能树节点
        /// </summary>
        /// <returns></returns>
        List<Actions> GetRoots();
    }
}
