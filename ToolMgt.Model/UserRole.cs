//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToolMgt.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserRole : ObservableObject
    {
    	private int _Id; //private字段
        public int Id { get => _Id; set => Set(ref _Id, value); }
    
    	private int _UserId; //private字段
        public int UserId { get => _UserId; set => Set(ref _UserId, value); }
    
    	private int _RoleId; //private字段
        public int RoleId { get => _RoleId; set => Set(ref _RoleId, value); }
    
    	private System.DateTime _CreateDateTime; //private字段
        public System.DateTime CreateDateTime { get => _CreateDateTime; set => Set(ref _CreateDateTime, value); }
    
    	private bool _IsDeleted; //private字段
        public bool IsDeleted { get => _IsDeleted; set => Set(ref _IsDeleted, value); }
    
    }
}
