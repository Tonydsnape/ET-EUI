namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_Success = 0;

        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误
        
        // 110000以下的错误请看ErrorCore.cs
        
        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        // 200001以上不抛异常
        
        public const int ERR_NetWorkError = 200002; // 网络错误
        public const int ERR_LoginInfoIsNull = 200003; // 登录信息为空
        public const int ERR_AccountNameFromError = 200004; // 账号或密码错误
        public const int ERR_PasswordFormError = 200005; // 密码错误
        public const int ERR_AccountTypeIsBlackList = 200006; // 账号类型是黑名单
        public const int ERR_LoginPasswordError = 200007; // 账号格式错误
        public const int ERR_RequestRepeatedly = 200008; // 请求重复
        public const int ERR_AccountTokenError = 200009; // 账号Token错误
        public const int ERR_CreateRoleNameIsNull = 200010; // 创建角色名字为空
        public const int ERR_CreateRoleNameIsExist = 200011; // 创建角色名字已经存在
        
    }
}