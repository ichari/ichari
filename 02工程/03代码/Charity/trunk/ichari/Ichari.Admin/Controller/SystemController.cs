using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Ichari.Uow;
using Ichari.Admin.ViewModel;
using Ichari.Model.Admin;
using Ichari.Common;
using Ichari.Common.Extensions;

namespace Ichari.Admin
{
    public class SystemController : BaseController
    {
        private IAdminUow _uow;

        public SystemController() { }

        public SystemController(IAdminUow uow)
        {
            _uow = uow;
        }
        public ActionResult Role(string roleName,int? pageIndex)
        {
            RoleViewModel vm = new RoleViewModel();
            if (pageIndex.HasValue)
                vm.PageIndex = pageIndex.Value;
            var list = _uow.SysRoleService.GetQueryList();
            if (!string.IsNullOrEmpty(roleName))
            {
                list = list.Where(t => t.RoleName.Contains(roleName));                
            }
            vm.RoleList = new Common.Helper.PageList<SysRole>(list.OrderBy(t => t.Id), vm.PageIndex, vm.PageCount);
            vm.SubMenuList = GetSubMenuList();

            return View(vm);
        }

        public ActionResult CreateRole(int? id)
        {
            RoleViewModel vm = new RoleViewModel();
            vm.SubMenuList = GetSubMenuList();

            vm.RoleModel = new SysRole();

            if (id.HasValue)
            {
                vm.RoleModel = _uow.SysRoleService.Get(t => t.Id == id.Value);
            }
            
            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.RoleModel.Id == 0)
                    _uow.SysRoleService.Add(model.RoleModel);
                else
                {
                    var old = _uow.SysRoleService.Get(t => t.Id == model.RoleModel.Id);
                    if (old != null)
                        old.RoleName = model.RoleModel.RoleName;
                }
                _uow.Commit();
            }
            return RedirectToAction("Role");
        }

        #region 用户管理
        public ActionResult User(string logonName, int? pageIndex)
        {
            var vm = new SysUserViewModel();
            if (pageIndex.HasValue)
                vm.PageIndex = pageIndex.Value;
            var list = _uow.SysUserService.GetQueryList();
            if (!string.IsNullOrEmpty(logonName))
            {
                list = list.Where(t => t.LogonName.Contains(logonName));
                
            }
            vm.SysUserList = new Common.Helper.PageList<SysUser>(list.OrderBy(t => t.Id), vm.PageIndex, vm.PageCount);

            vm.SubMenuList = GetSubMenuList();

            return View(vm);
        }

        public ActionResult EditUser(int? id)
        {
            var vm = new SysUserViewModel();
            vm.SubMenuList = GetSubMenuList();
            vm.UserModel = new SysUser();
            vm.UserModel.Password = WebUtils.GetAppSettingValue(StaticKey.AppDefaultUserPwd).ToMd5();
            vm.HaveRoles = new HashSet<int>();
            vm.RoleList = _uow.SysRoleService.GetQueryList().ToList();

            if (id.HasValue)
            {
                vm.UserModel = _uow.SysUserService.Get(t => t.Id == id.Value);
                vm.HaveRoles = _uow.RelUserRoleService.GetQueryList(t => t.UserId == id.Value).Select(t => t.RoleId).ToHashSet();
            }
            

            return View(vm);
        }
        [HttpPost]
        public ActionResult EditUser(SysUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.UserModel.Id == 0)
                {
                    model.UserModel.Password = WebUtils.GetAppSettingValue(StaticKey.AppDefaultUserPwd).ToMd5();
                    _uow.SysUserService.Add(model.UserModel);
                    _uow.Commit();
                    if (model.HaveRoles != null)
                    {
                        foreach (var item in model.HaveRoles)
                        {
                            _uow.RelUserRoleService.Add(new RelUserRole()
                            {
                                UserId = model.UserModel.Id,
                                RoleId = item
                            });
                        }
                    }
                }
                else
                {
                    var old = _uow.SysUserService.Get(t => t.Id == model.UserModel.Id);
                    if (old != null)
                    {
                        old.LogonName = model.UserModel.LogonName;
                        old.TrueName = model.UserModel.TrueName;
                        old.Phone = model.UserModel.Phone;
                        old.Email = model.UserModel.Email;
                        //删除角色
                        var urList = _uow.RelUserRoleService.GetQueryList(t => t.UserId == old.Id);
                        foreach (var item in urList)
                        {
                            _uow.RelUserRoleService.Delete(item);
                        }
                        //新加角色
                        if (model.HaveRoles != null)
                        {
                            foreach (var item in model.HaveRoles)
                            {
                                _uow.RelUserRoleService.Add(new RelUserRole()
                                {
                                    UserId = model.UserModel.Id,
                                    RoleId = item
                                });
                            }
                        }
                    }
                }
                _uow.Commit();
            }
            return RedirectToAction("User");
        }

        public void ResetPwd(int id)
        {
            var u = _uow.SysUserService.Get(t => t.Id == id);
            if (u == null)
            {
                TempData[Ichari.Admin.StaticKey.TempGlobalError] = "用户不存在";
            }
            u.Password = WebUtils.GetAppSettingValue(StaticKey.AppDefaultUserPwd).ToMd5();
            _uow.Commit();
            TempData[Ichari.Admin.StaticKey.TempGlobalInfo] = StaticKey.MsgOpSuccess;
            Response.Redirect("/system/user");
        }

        public ActionResult SetAction(int id)
        {
            var vm = new RoleViewModel();
            vm.RoleModel = _uow.SysRoleService.Get(t => t.Id == id);
            var allActions = _uow.ActionsService.GetList().ToList();
            var tree = BuildActionTree(allActions, null);
            vm.ActionTree = tree;
            return View(vm);
        }

        private IEnumerable<Actions> BuildActionTree(IEnumerable<Actions> all,int? pid)
        {
            var root = all.Where(t => t.ParentID == pid);
            foreach (var item in root) {
                item.SubNodes = item.SubNodes ?? new List<Actions>();
                item.SubNodes = all.Where(t => t.ParentID == item.ID).ToList();
                item.SubNodes.ForEach(t => BuildActionTree(all, t.ID));
            }
            return root;
        }
        #endregion
    }
}
