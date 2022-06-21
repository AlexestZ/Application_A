using System.Collections;
using System.Collections.Generic;

namespace MyRabbitMQService.BL.Services
{
    public class UserCacheDictionary
    {
        public static IDictionary userDictionary = new Dictionary<int, User>();
    }
}
