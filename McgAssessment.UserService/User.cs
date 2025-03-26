namespace McgAssessment.UserService;

public abstract class User
{
    protected User(string userId, UserQueryScope scope, UserPermissions permissions)
    {
        _userId = userId;
        _scope = scope;
        _permissions = permissions;
    }
    
    private string _userId;
    private UserQueryScope _scope;
    private UserPermissions _permissions;

    public virtual string UserId
    {
        get => _userId; 
        protected set => _userId = value;
    }
    public virtual UserQueryScope Scope 
    {
        get => _scope; 
        protected set => _scope = value;
    }
    public virtual UserPermissions Permissions
    {
        get => _permissions; 
        protected set => _permissions = value;
    }
}