using DataBlocks.DataAccess;
using Microsoft.AspNetCore.Identity;
using ScheMigrator.Migrations;

namespace UserBlocks.DataModels.Identity;

[ScheModel]
public class ApplicationUserRole : IdentityUserRole<long>, IModel
{
    [ScheKey("id")]
    public long Id { get; set; }
    
    [ScheData("user_id")]
    public override long UserId { get; set; }
    
    [ScheData("role_id")]
    public override long RoleId { get; set; }
    
    [ScheData("deleted")]
    public bool Deleted { get; set; }
    
    [ScheData("created")]
    public DateTime Created { get; set; }
    
    [ScheData("modified")]
    public DateTime Modified { get; set; }
    
    public long ID {
        get => Id;
        set => Id = value;
    }
}