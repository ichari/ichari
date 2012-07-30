using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Ichari.Model.Admin;
using Ichari.IRepository;
using Ichari.Repository;

namespace Ichari.Service
{
    public class ActionsService : BaseService<Actions>, Ichari.IService.IActionsService
    {
        
        private IActionsRepository _actionsRepository;

        public ActionsService()
        {
            this._actionsRepository = new ActionsRepository();
            base._repository = this._actionsRepository;
        }



        public ActionsService(System.Data.Objects.ObjectContext context)
        {
            this._actionsRepository = new ActionsRepository(context);
            base._repository = this._actionsRepository;
        }

        #region Method

        public List<Actions> GetRoots()
        { 
            //得到所有菜单节点
            var nodes = GetQueryList( t => t.IsMenu == true).ToList();
            //得到所有根节点
            var roots = nodes.Where(t => t.ParentID == null).ToList();

            BuildTree(roots, nodes);

            return roots;
            
        }
        private void BuildTree(List<Actions> roots,List<Actions> allNodes)
        {
            if (roots == null)
                return;

            foreach (var item in roots)
            {
                var nodes = allNodes.Where(t => t.ParentID == item.ID);
                if(nodes.Count() > 0)
                    item.SubNodes = nodes.ToList();
                BuildTree(item.SubNodes, allNodes);
            }
        }
        #endregion
    }
}
