namespace ET.Account
{
    public enum AccountType
    {
        General = 0,    //普通账户
        
        BlackList = 1,  //黑名单账户
    }
    
    public class Account: Entity, IAwake
    {
        public string AccountName;  //账户名
        
        public string Password;     //密码
        
        public long CreateTime;     //创建时间
        
        public int AccountType;     //账户类型
    }
}